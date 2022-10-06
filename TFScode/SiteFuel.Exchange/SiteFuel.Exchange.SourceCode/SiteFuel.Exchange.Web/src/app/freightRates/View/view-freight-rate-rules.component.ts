import { Component, OnInit, Input, ViewChild, AfterViewInit, OnDestroy, ChangeDetectorRef } from '@angular/core';
import { MyLocalStorage } from 'src/app/my.localstorage'
import { FreightRateRulesService } from 'src/app/freightRates/Services/freight-rate-rules.service'
import { Subject, forkJoin, merge } from 'rxjs';
import { FormBuilder, FormGroup, Validators, FormControl, FormArray, AbstractControl, ValidationErrors, ValidatorFn } from '@angular/forms';
import { IDropdownSettings } from 'ng-multiselect-dropdown';
import { RegionService } from 'src/app/company-addresses/region/service/region.service';
import { DropdownItem, DropdownItemExt } from 'src/app/statelist.service';
import { Declarations } from 'src/app/declarations.module';
import { HttpClient } from '@angular/common/http';
import { FreightRateGridModel, FreightRateInputModel, FreightRateModel } from '../Models/viewFreightRateRules';
import { FuelSurchargeService } from 'src/app/fuelsurcharge/services/fuelsurcharge.service';
import { SourceRegionInputModel } from 'src/app/app.enum';
import { DataTableDirective } from 'angular-datatables';

@Component({
    selector: 'app-view-freight-rate-rules',
    templateUrl: './view-freight-rate-rules.component.html',
    styleUrls: ['./view-freight-rate-rules.component.css']
})
export class ViewFreightRateRules implements OnInit{

    @ViewChild(DataTableDirective) datatableElement: DataTableDirective;
    public IsLoading: boolean = false;
    public viewFreightRateForm: FormGroup;
    dtOptions: any = {};
    dtTrigger: Subject<any> = new Subject();
    public SinlgeselectSettingsById = {};
    public MultiselectSettingsById: IDropdownSettings;
    public MultiSelectSettingsByGroup = {};
    public CounrtyId: any;
    
    public FreightRateRuleTypeList: DropdownItem[];
    public TableTypeList: DropdownItem[];
    public CustomerList: DropdownItem[];
    public CarrierList: DropdownItem[];
    public SourceRegionList: DropdownItem[];
    public TerminalsAndBulkPlantList: DropdownItemExt[] = [];

    public IsCustomerSelected = false;
    public IsMasterSelected = false;
    public IsCarrierSelected = false;
    public IsSourceRegionSelected = false;
    public Frmodel: FreightRateModel;
    public FreightRateList: FreightRateGridModel[] = [];
    public FreightRateInput: FreightRateInputModel;
    constructor(private fb: FormBuilder, private freightRateRulesService: FreightRateRulesService,
        private regionService: RegionService, private fuelsurchargeService: FuelSurchargeService, private cdr: ChangeDetectorRef) { }

    minDate = new Date();
    maxDate = new Date();
    public popoverTitle: string = 'Archive Confirmation';
    public popoverMessage: string = 'Do you want to archive?';
    public cancelClicked: boolean = false;
    public confirmClicked: boolean = false;
        
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
        this.getFreightRateRuleTypes();
        this.viewFreightRateForm = this.createForm();
        this.initializeFreightRateDatatableGrid();
    }
    ngOnDestroy() {
        this.datatableInventoryRerender();
    }
    ngAfterViewInit() {
        this.getFreightRateGridDetails();
    }



    public viewFreightRateRule(ruleId: number, freightRateRuleType: number, mode: string) {
        let operation = { RuleId: ruleId, FreightRateRuleType: freightRateRuleType, Mode: mode };
        this.freightRateRulesService.onSelectedFreightRateRuleId.next(JSON.stringify(operation));
        this.freightRateRulesService.onSelectedTabChanged.next(1);
    }

    private createForm() {
        if (this.Frmodel == undefined || this.Frmodel == null) {
            this.Frmodel = new FreightRateModel();
        }

        return this.fb.group({
            FreightRateRuleTypes : new FormControl(this.Frmodel.FreightRateRuleTypes, [Validators.required]),
            TableTypes: new FormControl(this.Frmodel.TableTypes, [Validators.required]),
            Customers: new FormControl(this.Frmodel.Customers),
            Carriers: new FormControl(this.Frmodel.Carriers),
            SourceRegions: new FormControl(this.Frmodel.SourceRegions),
            TerminalsAndBulkPlants: new FormControl(this.Frmodel.TerminalsAndBulkPlants),
            fromDate: [''],
            toDate: [''],
            isArchived: false
        });
    }

    archiveFreightRate(freightRateId: number) {
        this.IsLoading = true;
        this.freightRateRulesService.archiveFreightRate(freightRateId)
            .subscribe((response: any) => {
                if (response.StatusCode == 0) {
                    Declarations.msgsuccess('Selected Freight Rate archived successfully', undefined, undefined);
                    this.filterGrid();
                } 
                this.IsLoading = false;
            });
    }

    public onTableTypeSelect(item: any): void {
        this.IsMasterSelected = false;
        this.IsCustomerSelected = false;
        this.IsCarrierSelected = false;
        this.viewFreightRateForm.get('Carriers').patchValue([]);
        this.viewFreightRateForm.get('Customers').patchValue([]);
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
                this.getSupplierCustomers();
                this.getCarriers();
                break;
        }
        this.viewFreightRateForm.get('SourceRegions').patchValue([]);
        this.getSourceRegions(item.Id);
    }

    public onCarriersSelect(item: any): void {
        this.onCarriersOrCustomersChange();
    }

    public onCarriersDeSelect(item: any): void {
        this.viewFreightRateForm.get('SourceRegions').patchValue([]);
        this.onCarriersOrCustomersChange();
    }

    public onCustomersSelect(item: any): void {
        this.onCarriersOrCustomersChange();
    }

    public onCustomersDeSelect(item: any): void {
        this.viewFreightRateForm.get('SourceRegions').patchValue([]);
        this.onCarriersOrCustomersChange();
    }

    public onCarriersOrCustomersChange() {
        var selectedTableType = this.viewFreightRateForm.get('TableTypes').value as DropdownItem[];
        this.getSourceRegions(selectedTableType[0].Id.toString());
    }

    private getTableTypes(): void {
        this.fuelsurchargeService.getTableTypes().subscribe(async (data) => {
            this.TableTypeList = await data;
        });
    }

    private getFreightRateRuleTypes(): void {
        this.freightRateRulesService.getFreightRateRuleTypes().subscribe(async (data) => {
            this.FreightRateRuleTypeList = await data;
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

    private getSourceRegions(tableType: string): void {
        
        let customerIds: number[] = [];
        let carrierIds: number[] = [];

        let selectedCustomers = this.viewFreightRateForm.get('Customers').value as DropdownItem[];
        if (selectedCustomers != null && selectedCustomers != undefined && selectedCustomers.length > 0) {
            customerIds = selectedCustomers.map(s => s.Id);
        }

        let selectedCarriers = this.viewFreightRateForm.get('Carriers').value as DropdownItem[];
        if (selectedCarriers != null && selectedCarriers != undefined && selectedCarriers.length > 0) {
            carrierIds = selectedCarriers.map(s => s.Id);
        }

        var sourceRegionInput = new SourceRegionInputModel();
        sourceRegionInput.TableType = tableType;
        sourceRegionInput.CustomerId = customerIds;
        sourceRegionInput.CarrierId = carrierIds;
        this.IsLoading = true;
        this.fuelsurchargeService.getSourceRegions(sourceRegionInput).subscribe(data => {
            this.SourceRegionList = data;
            this.IsLoading = false;
        });
    }

    private getTerminalsBulkPlant(): void {
        this.IsLoading = true;
        var selectedSourceRegions = this.viewFreightRateForm.get('SourceRegions').value as DropdownItem[];
        if (selectedSourceRegions != undefined && selectedSourceRegions != null) {
            this.fuelsurchargeService.getTerminalsAndBulkPlants(selectedSourceRegions.map(s => s.Id).join(',')).subscribe(async (data) => {
                this.TerminalsAndBulkPlantList = await (data);
                this.IsLoading = false;
            });
        }
    }

    public onSourceRegionsDeSelect(item: any): void {
        this.IsSourceRegionSelected = this.Frmodel.SourceRegions.length > 0;
    }

    public onSourceRegionsDeSelectAll(item: any): void {
        this.IsSourceRegionSelected = false;
    }

    public onSourceRegionsSelect(item: any): void {
        this.getTerminalsBulkPlant();
        this.IsSourceRegionSelected = this.Frmodel.SourceRegions.length > 0;
    }

    filterGrid() {
        $("#freight-rate-grid-datatable").DataTable().clear().destroy();
        this.getFreightRateGridDetails();
    }

    clearFilter() {
        this.clearForm();
        this.getFreightRateGridDetails();
        this.CustomerList = [];
        this.CarrierList = [];
        this.SourceRegionList = [];
    }

    clearForm() {
        this.viewFreightRateForm.reset();
        $("#freight-rate-grid-datatable").DataTable().clear().destroy();
    }

    getFreightRateGridDetails() {
        var input = new FreightRateInputModel();

        var selectedFreightRateRuleTypes = this.viewFreightRateForm.get('FreightRateRuleTypes').value as DropdownItem[];
        var selectedTableTypes = this.viewFreightRateForm.get('TableTypes').value as DropdownItem[];
        var selectedCustomers = this.viewFreightRateForm.get('Customers').value as DropdownItem[];
        var selectedCarriers = this.viewFreightRateForm.get('Carriers').value as DropdownItem[];
        var selectedSourceRegions = this.viewFreightRateForm.get('SourceRegions').value as DropdownItem[];
        var selectedTerminalAndBulkPlants = this.viewFreightRateForm.get('TerminalsAndBulkPlants').value as DropdownItemExt[];

        input.StartDate = this.viewFreightRateForm.controls.fromDate.value;
        input.EndDate = this.viewFreightRateForm.controls.toDate.value;
        input.IsArchived = this.viewFreightRateForm.controls.isArchived.value;

        if (selectedFreightRateRuleTypes != null && selectedFreightRateRuleTypes != undefined && selectedFreightRateRuleTypes.length > 0) {
            var freightRateRuleTypeIds = selectedFreightRateRuleTypes.map(s => s.Id);
            input.FreightRateRuleTypeIds = freightRateRuleTypeIds.join(',');
        }

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


        this.IsLoading = true;
        this.cdr.detectChanges();
        this.freightRateRulesService.getFreightRateGridDetails(input).subscribe(data => {
            this.IsLoading = false
            if(data && data.length>0){
                this.FreightRateList = data as FreightRateGridModel[];
            }

            this.dtTrigger.next();

        });
    }

    private datatableInventoryRerender(): void {
        if (this.datatableElement.dtInstance) {
            this.datatableElement.dtInstance.then((dtInstance: DataTables.Api) => {
                dtInstance.destroy();
            });
        }
    }
    setfromDate($event) {
        this.viewFreightRateForm.controls.fromDate.setValue($event);
    }

    settoDate($event) {
        this.viewFreightRateForm.controls.toDate.setValue($event);
    }

    initializeFreightRateDatatableGrid() {
        let exportColumns = { columns: ':visible' };
        this.dtOptions = {
            dom: '<"html5buttons"B>lTfgitp',
            buttons: [
                { extend: 'colvis' },
                { extend: 'copy', exportOptions: exportColumns },
                { extend: 'csv', title: 'Freight Rate', exportOptions: exportColumns },
                { extend: 'pdf', title: 'Freight Rate', orientation: 'landscape', exportOptions: exportColumns },
                { extend: 'print', exportOptions: exportColumns }
            ],
            pagingType: 'first_last_numbers',
            pageLength: 10,
            lengthMenu: [[10, 25, 50, 100, -1], [10, 25, 50, 100, "All"]]
        };
       
    }
}


