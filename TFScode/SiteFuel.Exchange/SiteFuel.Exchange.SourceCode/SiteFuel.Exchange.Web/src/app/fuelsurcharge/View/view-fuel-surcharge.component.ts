import { ChangeDetectorRef, Component, OnInit, ViewChild } from '@angular/core';
import { FormBuilder, FormGroup, Validators, FormControl } from '@angular/forms';
import { FuelSurchargeService } from '../services/fuelsurcharge.service';
import { FuelSurchargeGridModel, FuelSurchargeInputModel, FuelSurchargeIndexViewModel } from '../models/CreateFuelSurcharge';
import { Subject } from 'rxjs';
import { IDropdownSettings } from 'ng-multiselect-dropdown';
import { MyLocalStorage } from 'src/app/my.localstorage';
import { DropdownItem, DropdownItemExt } from 'src/app/statelist.service';
import { RegionService } from 'src/app/company-addresses/region/service/region.service';
import { ViewFuelSurchargePricingdetailsComponent } from './view-fuel-surcharge-pricingdetails/view-fuel-surcharge-pricingdetails.component';
import { Declarations } from 'src/app/declarations.module';
import { ViewHistoricalPriceComponent } from './view-historical-price/view-historical-price.component';
import { Router } from '@angular/router';
import { SourceRegionInputModel } from 'src/app/app.enum';
import { DataTableDirective } from 'angular-datatables';

@Component({
  selector: 'app-view-fuel-surcharge',
  templateUrl: './view-fuel-surcharge.component.html',
  styleUrls: ['./view-fuel-surcharge.component.css']
})
export class ViewFuelSurchargeComponent implements OnInit {

    public IsLoading: boolean = false;
    public viewFuelSurchargeForm: FormGroup;
    dtOptions: any = {};
    dtTrigger: Subject<any> = new Subject();
    public SinlgeselectSettingsById = {};
    public MultiselectSettingsById: IDropdownSettings;
    public MultiSelectSettingsByGroup = {};
    public CounrtyId: any;

    public TableTypeList: DropdownItem[];
    public CustomerList: DropdownItem[];
    public CarrierList: DropdownItem[];
    public SourceRegionList: DropdownItem[];
    public TerminalsAndBulkPlantList: DropdownItemExt[] = [];
    public SelectedTerminalsAndBulkPlants: DropdownItemExt[] = [];

    public IsCustomerSelected = false;
    public IsMasterSelected = false;
    public IsCarrierSelected = false;
    public IsSourceRegionSelected = false;
    public Fsmodel: FuelSurchargeIndexViewModel;
    @ViewChild(DataTableDirective) datatableElement: DataTableDirective;

    public FuelSurchargeList: FuelSurchargeGridModel[] = [];
    public FuelSurchargeInput: FuelSurchargeInputModel;
    constructor(private fb: FormBuilder, private regionService: RegionService, private fuelsurchargeService: FuelSurchargeService,private router:Router,private cdr:ChangeDetectorRef) {
        
    }
    minDate = new Date();
    maxDate = new Date();
    public popoverTitle: string = 'Archive Confirmation';
    public popoverMessage: string = 'Do you want to archive?';
    public cancelClicked: boolean = false;
    public confirmClicked: boolean = false;

    @ViewChild(ViewFuelSurchargePricingdetailsComponent) fuelSurchageComponent: ViewFuelSurchargePricingdetailsComponent;
    @ViewChild(ViewHistoricalPriceComponent) historicalPriceComponent: ViewHistoricalPriceComponent;

    ngOnInit() {
        this.maxDate.setFullYear(this.maxDate.getFullYear() + 20);
        this.minDate.setFullYear(this.minDate.getFullYear() - 20);
        this.CounrtyId = MyLocalStorage.getData("countryIdForDashboard");
        this.SinlgeselectSettingsById = {
            singleSelection: true,
            closeDropDownOnSelection: true,
            idField: 'Id',
            textField: 'Name',
            enableCheckAll: false,
            selectAllText: 'Select All',
            unSelectAllText: 'UnSelect All',
            itemsShowLimit: 1,
            allowSearchFilter: true
        };
        this.MultiselectSettingsById = {
            singleSelection: false,
            idField: 'Id',
            textField: 'Name',
            selectAllText: 'Select All',
            unSelectAllText: 'UnSelect All',
            itemsShowLimit: 5,
            allowSearchFilter: true
        };

        this.MultiSelectSettingsByGroup = {
            singleSelection: false,
            text: "Select",
            selectAllText: 'Select All',
            unSelectAllText: 'UnSelect All',
            searchPlaceholderText: 'Search',
            primaryKey: "Id",
            labelKey: "Name",
            enableSearchFilter: true,
            badgeShowLimit: 5,
            groupBy: "Code"
        };

        this.getTableTypes();
        this.viewFuelSurchargeForm = this.createForm();
        this.initializeFuelSurchargeDatatableGrid();
    }
    ngOnDestroy() {
        this.rerender_destroy();
    }
    ngAfterViewInit() {
        this.getFuelSurchargeGridDetails();
    }

    private createForm() {
        if (this.Fsmodel == undefined || this.Fsmodel == null) {
            this.Fsmodel = new FuelSurchargeIndexViewModel();
        }

        return this.fb.group({
            TableTypes: new FormControl(this.Fsmodel.TableTypes, [Validators.required]),
            Customers: new FormControl(this.Fsmodel.Customers),
            Carriers: new FormControl(this.Fsmodel.Carriers),
            SourceRegions: new FormControl(this.Fsmodel.SourceRegions),
            TerminalsAndBulkPlants: new FormControl(this.SelectedTerminalsAndBulkPlants),
            fromDate: [''],
            toDate: [''],
            isArchived: false
        });
    }

    getFuelSurchargePricingDetails(fuelSurchargeIndexId: number) {
        this.fuelSurchageComponent.getFuelSurchargePricingDetails(fuelSurchargeIndexId);
    }

    getHistoricalPriceDetails(fuelSurchargeIndexId: number) {
        this.historicalPriceComponent.getHistoricalPriceDetails(fuelSurchargeIndexId);
    }

    archiveFuelSurchargeTable(fuelSurchargeIndexId: number) {
        this.IsLoading = true;
        this.fuelsurchargeService.archiveFuelSurchargeTable(fuelSurchargeIndexId)
            .subscribe((response: any) => {
                this.IsLoading = false;
                //this.serviceResponse = response;
                if (response.StatusCode == 0) {
                    Declarations.msgsuccess('Selected fuel surcharge table archived successfully', undefined, undefined);
                    this.filterGrid();
                }
            });
    }

    public onTableTypeSelect(item: any): void {
        this.IsMasterSelected = false;
        this.IsCustomerSelected = false;
        this.IsCarrierSelected = false;
        this.viewFuelSurchargeForm.get('Carriers').patchValue([]);
        this.viewFuelSurchargeForm.get('Customers').patchValue([]);
        switch (item.Id) {
            case 1: //master
                this.IsMasterSelected = true;
                break;
            case 2: // customer
                this.IsCustomerSelected = true;
                this.getSupplierCustomers();
                this.getCarriers();
                break;
            case 3: //carrier
                this.IsCarrierSelected = true;
                this.getCarriers();
                this.getSupplierCustomers();
                break;
        }
        this.viewFuelSurchargeForm.get('SourceRegions').patchValue([]);
        this.getSourceRegions(item.Id);
    }

    public onCarriersSelect(item: any): void {
        this.onCarriersOrCustomersChange();
    }

    public onCarriersDeSelect(item: any): void {
        this.viewFuelSurchargeForm.get('SourceRegions').patchValue([]);
        this.onCarriersOrCustomersChange();
    }

    public onCustomersSelect(item: any): void {
        this.onCarriersOrCustomersChange();
    }

    public onCustomersDeSelect(item: any): void {
        this.viewFuelSurchargeForm.get('SourceRegions').patchValue([]);
        this.onCarriersOrCustomersChange();
    }

    public onCarriersOrCustomersChange() {
        var selectedTableType = this.viewFuelSurchargeForm.get('TableTypes').value as DropdownItem[];
        this.getSourceRegions(selectedTableType[0].Id.toString());
    }

    private getTableTypes(): void {
        this.fuelsurchargeService.getTableTypes().subscribe(async (data) => {
            this.TableTypeList = await data;
        });
    }

    private getCarriers(): void {
        this.IsLoading = true;
        this.regionService.getCarriers().subscribe(async (carriers: DropdownItem[]) => {
                this.CarrierList = await carriers;
                this.SourceRegionList = null; 
                this.IsLoading = false;
            });
    }

    private getSupplierCustomers(): void {
        this.IsLoading = true;
        this.fuelsurchargeService.getSupplierCustomers().subscribe(async (data) => {
            this.CustomerList = await data;
            this.IsLoading = false;
        });
    }

    private getSourceRegions(tableType: string): void {;
        let customerIds: number[] = [];
        let carrierIds: number[] = [];

        let selectedCustomers = this.viewFuelSurchargeForm.get('Customers').value as DropdownItem[];
        if (selectedCustomers != null && selectedCustomers != undefined && selectedCustomers.length > 0) {
            customerIds = selectedCustomers.map(s => s.Id);
        }

        let selectedCarriers = this.viewFuelSurchargeForm.get('Carriers').value as DropdownItem[];
        if (selectedCarriers != null && selectedCarriers != undefined && selectedCarriers.length > 0) {
            carrierIds = selectedCarriers.map(s => s.Id);
        }

        var sourceRegionInput = new SourceRegionInputModel();
        sourceRegionInput.TableType = tableType;
        sourceRegionInput.CustomerId = customerIds;
        sourceRegionInput.CarrierId = carrierIds;
        this.IsLoading = true
        this.fuelsurchargeService.getSourceRegions(sourceRegionInput).subscribe(data => {
            this.SourceRegionList = data;
            this.IsLoading = false;
        });
    }

    private getTerminalsBulkPlant(): void {
        this.IsLoading = true;
        var selectedSourceRegions = this.viewFuelSurchargeForm.get('SourceRegions').value as DropdownItem[];
        if (selectedSourceRegions != undefined && selectedSourceRegions != null) {
            this.fuelsurchargeService.getTerminalsAndBulkPlants(selectedSourceRegions.map(s => s.Id).join(',')).subscribe(async (data) => {
                this.TerminalsAndBulkPlantList = await (data);
                this.IsLoading = false;
            });
        }
    }

    public onSourceRegionsDeSelect(item: any): void {
        this.IsSourceRegionSelected = this.Fsmodel.SourceRegions.length > 0;
    }

    public onSourceRegionsDeSelectAll(item: any): void {
        this.IsSourceRegionSelected = false;
    }

    public onSourceRegionsSelect(item: any): void {
        this.getTerminalsBulkPlant();
        this.IsSourceRegionSelected = this.Fsmodel.SourceRegions.length > 0;
    }

    filterGrid() {
        $("#fuel-surcharge-grid-datatable").DataTable().clear().destroy();
        this.getFuelSurchargeGridDetails();
    }

    clearFilter() {
        this.clearForm();
        this.getFuelSurchargeGridDetails();
        this.CustomerList = [];
        this.CarrierList = [];
        this.SourceRegionList = [];
    }

    clearForm() {
        this.viewFuelSurchargeForm.reset();
        $("#fuel-surcharge-grid-datatable").DataTable().clear().destroy();
    }

    getFuelSurchargeGridDetails() {;
        var input = new FuelSurchargeInputModel();

        var selectedTableTypes = this.viewFuelSurchargeForm.get('TableTypes').value as DropdownItem[];
        var selectedCustomers = this.viewFuelSurchargeForm.get('Customers').value as DropdownItem[];
        var selectedCarriers = this.viewFuelSurchargeForm.get('Carriers').value as DropdownItem[];
        var selectedSourceRegions = this.viewFuelSurchargeForm.get('SourceRegions').value as DropdownItem[];
        var selectedTerminalAndBulkPlants = this.viewFuelSurchargeForm.get('TerminalsAndBulkPlants').value as DropdownItemExt[];

        input.StartDate = this.viewFuelSurchargeForm.controls.fromDate.value;
        input.EndDate = this.viewFuelSurchargeForm.controls.toDate.value;
        input.IsArchived = this.viewFuelSurchargeForm.controls.isArchived.value;

        if (selectedTableTypes != null && selectedTableTypes != undefined && selectedTableTypes.length > 0) {
            var tableTypeIds = selectedTableTypes.map(s => s.Id);
            input.TableTypeIds = tableTypeIds.join(',');
        }

        if (selectedCustomers != null && selectedCustomers != undefined && selectedCustomers.length > 0) {
            var customerIds = selectedCustomers.map(s => s.Id);
            input.CustomerIds = customerIds.join(',');
        }

        if (selectedCarriers != null && selectedCarriers != undefined && selectedCarriers.length > 0) {
            var carrierIds = selectedCarriers.map(s => s.Id);
            input.CarrierIds = carrierIds.join(',');
        }

        if (selectedSourceRegions != null && selectedSourceRegions != undefined && selectedSourceRegions.length > 0) {
            var sourceRegionIds = selectedSourceRegions.map(s => s.Id);
            input.SourceRegionIds = sourceRegionIds.join(',');
        }

        if (selectedTerminalAndBulkPlants != null && selectedTerminalAndBulkPlants != undefined && selectedTerminalAndBulkPlants.length > 0) {
            var selectedTerminalIds = selectedTerminalAndBulkPlants.filter(c => c.Code == "Terminals");
            var terminalIds = selectedTerminalIds.map(s => s.Id);
            input.TerminalIds = terminalIds.join(',');

            var selectedBulkPlants = selectedTerminalAndBulkPlants.filter(c => c.Code == "BulkPlants");
            var bulkPlantIds = selectedBulkPlants.map(s => s.Id);
            input.BulkPlantIds = bulkPlantIds.join(',');
        }
        this.IsLoading = true
        this.cdr.detectChanges()
        this.fuelsurchargeService.getFuelSurchargeGridDetails(input).subscribe(data => {
            this.IsLoading = false;
            if(data && data.length>0){
                this.FuelSurchargeList = data as FuelSurchargeGridModel[];
            }
            this.dtTrigger.next();
           
        });
    }

    rerender_destroy(): void {
        if ((this.datatableElement && this.datatableElement.dtInstance)) {
            this.datatableElement.dtInstance.then((dtInstance: DataTables.Api) => {
                dtInstance.destroy();
            });
        }
    }
    rerender_trigger(): void {
        if ((this.datatableElement && this.datatableElement.dtInstance)) {
            this.datatableElement.dtInstance.then((dtInstance: DataTables.Api) => {
                this.dtTrigger.next();
            });
        }
    }

    setfromDate($event) {
        this.viewFuelSurchargeForm.controls.fromDate.setValue($event);
    }

    settoDate($event) {
        this.viewFuelSurchargeForm.controls.toDate.setValue($event);
    }


    public viewFuelSurcharge(fsurcharId: number, mode: string) {
        let operation = { FsurcharId: fsurcharId, Mode: mode };
        this.fuelsurchargeService.onSelectedFuelSurchargeId.next(JSON.stringify(operation));
        this.fuelsurchargeService.onSelectedTabChanged.next(1);
        //this.router.navigate(['/Supplier/FuelSurcharge/CreateNew/' + fsurcharId]);

    }

    initializeFuelSurchargeDatatableGrid() {
        let exportColumns = { columns: ':visible' };
        this.dtOptions = {
            dom: '<"html5buttons"B>lTfgitp',
            buttons: [
                { extend: 'colvis' },
                { extend: 'copy', exportOptions: exportColumns },
                { extend: 'csv', title: 'Fuel Surcharge', exportOptions: exportColumns },
                { extend: 'pdf', title: 'Fuel Surcharge', orientation: 'landscape', exportOptions: exportColumns },
                { extend: 'print', exportOptions: exportColumns }
            ],
            pagingType: 'first_last_numbers',
            pageLength: 10,
            lengthMenu: [[10, 25, 50, 100, -1], [10, 25, 50, 100, "All"]]
        };
        
    }
}
