import { ChangeDetectorRef, Component, OnInit, ViewChild } from '@angular/core';
import { Subject } from 'rxjs';
import { FormGroup, FormControl, Validators, FormBuilder } from '@angular/forms';
import { DropdownItem, DropdownItemExt } from 'src/app/statelist.service';
import { IDropdownSettings } from 'ng-multiselect-dropdown';
import { MyLocalStorage } from 'src/app/my.localstorage';
import { RegionService } from 'src/app/company-addresses/region/service/region.service';
import { AccessorialFeesService } from '../service/accessorialfees.service';
import { Declarations } from 'src/app/declarations.module';
import { FuelSurchargeService } from 'src/app/fuelsurcharge/services/fuelsurcharge.service';
import { ViewAccessorialFeeModel, AccessorialFeeInputModel, AccessorialFeeGridModel } from '../model/accessorial-fees';
import { ViewFeesDetailsComponent } from './view-fees-details/view-fees-details.component';
import { SourceRegionInputModel } from 'src/app/app.enum';
import { DataTableDirective } from 'angular-datatables';

@Component({
    selector: 'app-view-accessorial-fees',
    templateUrl: './view-accessorial-fees.component.html',
    styleUrls: ['./view-accessorial-fees.component.css']
})
export class ViewAccessorialFeesComponent implements OnInit {

    public IsLoading: boolean = false;
    public viewAccessorialFeeForm: FormGroup;
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
    public Afmodel: ViewAccessorialFeeModel;
    public AccessorialFeeList: AccessorialFeeGridModel[] = [];
    public AccessorialFeeInput: AccessorialFeeInputModel;
    @ViewChild(DataTableDirective) datatableElement: DataTableDirective;
    constructor(private fb: FormBuilder, private regionService: RegionService, private fuelsurchargeService: FuelSurchargeService,
        private accessorialFeeService: AccessorialFeesService,private cdr : ChangeDetectorRef) {
    }

    minDate = new Date();
    maxDate = new Date();
    public popoverTitle: string = 'Archive Confirmation';
    public popoverMessage: string = 'Do you want to archive?';
    public cancelClicked: boolean = false;
    public confirmClicked: boolean = false;
    @ViewChild(ViewFeesDetailsComponent) accessorialFeeComponent: ViewFeesDetailsComponent;
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
        this.viewAccessorialFeeForm = this.createForm();
        this.initializeAccessorialFeeDatatableGrid();
       
    }
    ngOnDestroy() {
        this.rerender_destroy();
    }
    ngAfterViewInit() {
        this.getAccessorialFeeGridDetails();
    }
    private createForm() {
        if (this.Afmodel == undefined || this.Afmodel == null) {
            this.Afmodel = new ViewAccessorialFeeModel();
        }

        return this.fb.group({
            TableTypes: new FormControl(this.Afmodel.TableTypes, [Validators.required]),
            Customers: new FormControl(this.Afmodel.Customers),
            Carriers: new FormControl(this.Afmodel.Carriers),
            SourceRegions: new FormControl(this.Afmodel.SourceRegions),
            TerminalsAndBulkPlants: new FormControl(this.SelectedTerminalsAndBulkPlants),
            fromDate: [''],
            toDate: [''],
            isArchived: false
        });
    }

    archiveAccessorialFee(accessorialFeeId: number) {
        this.IsLoading = true;
        this.accessorialFeeService.archiveAccessorialFee(accessorialFeeId)
            .subscribe((response: any) => {
                this.IsLoading = false;
                //this.serviceResponse = response;
                if (response.StatusCode == 0) {
                    Declarations.msgsuccess('Selected accessorial fee archived successfully', undefined, undefined);
                    this.filterGrid();
                }
            });
    }
   
    public onTableTypeSelect(item: any): void {
        this.IsMasterSelected = false;
        this.IsCustomerSelected = false;
        this.IsCarrierSelected = false;
        this.viewAccessorialFeeForm.get('Carriers').patchValue([]);
        this.viewAccessorialFeeForm.get('Customers').patchValue([]);
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
        this.viewAccessorialFeeForm.get('SourceRegions').patchValue([]);
        this.getSourceRegions(item.Id);
    }

    public onCarriersSelect(item: any): void {
        this.onCarriersOrCustomersChange();
    }

    public onCarriersDeSelect(item: any): void {
        this.viewAccessorialFeeForm.get('SourceRegions').patchValue([]);
        this.onCarriersOrCustomersChange();
    }

    public onCustomersSelect(item: any): void {
        this.onCarriersOrCustomersChange();
    }

    public onCustomersDeSelect(item: any): void {
        this.viewAccessorialFeeForm.get('SourceRegions').patchValue([]);
        this.onCarriersOrCustomersChange();
    }

    public onCarriersOrCustomersChange() {
        var selectedTableType = this.viewAccessorialFeeForm.get('TableTypes').value as DropdownItem[];
        this.getSourceRegions(selectedTableType[0].Id.toString());
    }

    private getTableTypes(): void {
        this.IsLoading = true;
        this.fuelsurchargeService.getTableTypes().subscribe(async (data) => {
            this.TableTypeList = await data;
        });
    }

    private getCarriers(): void {
        this.IsLoading = true;
        this.regionService.getCarriers()
            .subscribe(async (carriers: DropdownItem[]) => {
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
        this.IsLoading = true;
        let customerIds: number[] = [];
        let carrierIds: number[] = [];

        let selectedCustomers = this.viewAccessorialFeeForm.get('Customers').value as DropdownItem[];
        if (selectedCustomers != null && selectedCustomers != undefined && selectedCustomers.length > 0) {
            customerIds = selectedCustomers.map(s => s.Id);
        }

        let selectedCarriers = this.viewAccessorialFeeForm.get('Carriers').value as DropdownItem[];
        if (selectedCarriers != null && selectedCarriers != undefined && selectedCarriers.length > 0) {
            carrierIds = selectedCarriers.map(s => s.Id);
        }

        var sourceRegionInput = new SourceRegionInputModel();
        sourceRegionInput.TableType = tableType;
        sourceRegionInput.CustomerId = customerIds;
        sourceRegionInput.CarrierId = carrierIds;
        this.fuelsurchargeService.getSourceRegions(sourceRegionInput).subscribe(data => {
            this.SourceRegionList = data;
            this.IsLoading = false;
        });
    }

    private getTerminalsBulkPlant(): void {
        this.IsLoading = true;
        var selectedSourceRegions = this.viewAccessorialFeeForm.get('SourceRegions').value as DropdownItem[];
        if (selectedSourceRegions != undefined && selectedSourceRegions != null) {
            this.fuelsurchargeService.getTerminalsAndBulkPlants(selectedSourceRegions.map(s => s.Id).join(',')).subscribe(async (data) => {
                this.TerminalsAndBulkPlantList = await (data);
                this.IsLoading = false;
            });
        }
    }

    public onSourceRegionsDeSelect(item: any): void {
        this.IsSourceRegionSelected = this.Afmodel.SourceRegions.length > 0;
    }

    public onSourceRegionsDeSelectAll(item: any): void {
        this.IsSourceRegionSelected = false;
    }

    public onSourceRegionsSelect(item: any): void {
        this.getTerminalsBulkPlant();
        this.IsSourceRegionSelected = this.Afmodel.SourceRegions.length > 0;
    }

    filterGrid() {

        $("#accessorial-fee-grid-datatable").DataTable().clear().destroy();
        this.getAccessorialFeeGridDetails();
    }

    clearFilter() {
        this.clearForm();
        this.getAccessorialFeeGridDetails();
    }
    
    clearForm() {
        this.viewAccessorialFeeForm.reset();
        $("#accessorial-fee-grid-datatable").DataTable().clear().destroy();
        this.CustomerList = [];
        this.CarrierList = [];
        this.SourceRegionList = [];
    }
    
    getAccessorialFeeGridDetails() {
        this.cdr.detectChanges()
        var input = new AccessorialFeeInputModel();

        var selectedTableTypes = this.viewAccessorialFeeForm.get('TableTypes').value as DropdownItem[];
        var selectedCustomers = this.viewAccessorialFeeForm.get('Customers').value as DropdownItem[];
        var selectedCarriers = this.viewAccessorialFeeForm.get('Carriers').value as DropdownItem[];
        var selectedSourceRegions = this.viewAccessorialFeeForm.get('SourceRegions').value as DropdownItem[];
        var selectedTerminalAndBulkPlants = this.viewAccessorialFeeForm.get('TerminalsAndBulkPlants').value as DropdownItemExt[];

        input.StartDate = this.viewAccessorialFeeForm.controls.fromDate.value;
        input.EndDate = this.viewAccessorialFeeForm.controls.toDate.value;
        input.IsArchived = this.viewAccessorialFeeForm.controls.isArchived.value;

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
        this.accessorialFeeService.getAccessorialFeeGridDetails(input).subscribe(data => {
            this.IsLoading = false;
            if(data && data.length>0){
                this.AccessorialFeeList = data as AccessorialFeeGridModel[];
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
    public viewAccessorialFee(accessorialFeeId: number, mode: string) {
        let operation = { AccessorialFeeId: accessorialFeeId, Mode: mode };
        this.accessorialFeeService.onSelectedAccessorialFeeId.next(JSON.stringify(operation));
        this.accessorialFeeService.onSelectedTabChanged.next(1);
    }

    setfromDate($event) {
        this.viewAccessorialFeeForm.controls.fromDate.setValue($event);
    }

    settoDate($event) {
        this.viewAccessorialFeeForm.controls.toDate.setValue($event);
    }

    initializeAccessorialFeeDatatableGrid() {
        let exportColumns = { columns: ':visible' };
        this.dtOptions = {
            dom: '<"html5buttons"B>lTfgitp',
            buttons: [
                { extend: 'colvis' },
                { extend: 'copy', exportOptions: exportColumns },
                { extend: 'csv', title: 'Accessorial Fee', exportOptions: exportColumns },
                { extend: 'pdf', title: 'Accessorial Fee', orientation: 'landscape', exportOptions: exportColumns },
                { extend: 'print', exportOptions: exportColumns }
            ],
            pagingType: 'first_last_numbers',
            pageLength: 10,
            lengthMenu: [[10, 25, 50, 100, -1], [10, 25, 50, 100, "All"]]
        };
       
    }

}
