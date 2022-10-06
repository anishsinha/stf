import { Component, OnInit, ViewChild, ElementRef, Input, AfterViewInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { DropdownItem } from 'src/app/statelist.service';
import { ProductMappingModel, ProductMappingGridModel } from '../models/ProductMappingModel';
import { Declarations } from 'src/app/declarations.module';
import { Subject, concat } from 'rxjs';
import { map } from 'rxjs/operators';
import { CarrierService } from 'src/app/carrier/service/carrier.service';
declare var currentCompanyId: any;

@Component({
    selector: 'app-product-mapping',
    templateUrl: './product-mapping.component.html',
    styleUrls: ['./product-mapping.component.css']
})
export class ProductMappingComponent implements OnInit, AfterViewInit {
    public IsLoading: boolean = false;
    public StateList: DropdownItem[] = [];
    public CityList: DropdownItem[] = [];
    public TerminalList: DropdownItem[] = [];
    public FuelTypeList: DropdownItem[] = [];
    public ProductMappingList: ProductMappingGridModel[] = [];
    public UpdateProductMappingList: ProductMappingGridModel[] = [];
    public ProductMapping: ProductMappingModel;
    public IsShowBulkUploadPopup: boolean = false;
    public ProductMappingForm: FormGroup;
    public ddlSettingsByCode = {};
    public ddlSettingsById = {};
    public ddlSettingsByIdSingleSelect = {};
    public CurrentCompanyId: any;
    public SelectedCountryId: number;
    public MaxFileUploadSize: number = 1048576; // 1MB
    dtOptions: any = {};
    dtTrigger: Subject<any> = new Subject();
    public popoverTitle: string = 'Are you sure?';
    public confirmButtonText: string = 'Yes';
    public cancelButtonText: string = 'No';
    public SelectedFiles: File[] = [];
    @Input() countryId: number;
    public IsValidForm: boolean = true;
    @ViewChild('MyProductId') MyProductId: ElementRef;
    @ViewChild('btnCloseBulkUploadPopup') CloseBulkUploadPopup: ElementRef;
    constructor(private fb: FormBuilder, private carrierService: CarrierService) {
        this.ProductMappingForm = this.fb.group({
            States: this.fb.control([]),
            Cities: this.fb.control([]),
            Terminals: this.fb.control([]),
            FuelTypes: this.fb.control([], [Validators.required]),
            MyProductId: this.fb.control(''),
            BackOfficeProductId: this.fb.control(''),
            DriverProductId: this.fb.control(''),
          //  TerminalItemCode: this.fb.control(''),
            CompanyId: this.fb.control(0),
        });
    }

    ngOnInit() {
        this.ddlSettingsById = {
            singleSelection: false,
            idField: 'Id',
            textField: 'Name',
            selectAllText: 'Select All',
            unSelectAllText: 'UnSelect All',
            itemsShowLimit: 5,
            allowSearchFilter: true
        };

        this.ddlSettingsByCode = {
            singleSelection: false,
            idField: 'Code',
            textField: 'Name',
            selectAllText: 'Select All',
            unSelectAllText: 'UnSelect All',
            itemsShowLimit: 5,
            allowSearchFilter: true
        };

        this.ddlSettingsByIdSingleSelect = {
            singleSelection: true,
            idField: 'Id',
            textField: 'Name',
            itemsShowLimit: 5,
            allowSearchFilter: true
        };

        this.initializeProductMappingDatatableGrid();
        this.CurrentCompanyId = Number(currentCompanyId);       
        this.getServingFuelTypesByCompany();
    }

    ngOnChanges() {
        this.clearForm();
        this.SelectedCountryId = Number(this.countryId);
        if (isNaN(this.SelectedCountryId) || this.SelectedCountryId == 0) {
            this.getDefaultServingCountry();
        }
        else {
            this.getStates();
            this.getProductMappingGridDetails();
        }
    }

    ngAfterViewInit() {
        this.getProductMappingGridDetails();
    }

    getProductMappingGridDetails() {
        this.IsLoading = true;
        var selectedStates = this.ProductMappingForm.get('States').value as DropdownItem[];
        var selectedCities = this.ProductMappingForm.get('Cities').value as DropdownItem[];
        var selectedTerminals = this.ProductMappingForm.get('Terminals').value as DropdownItem[];
        var selectedFuelTypes = this.ProductMappingForm.get('FuelTypes').value as DropdownItem[];

        var input = new ProductMappingModel();
        if (selectedStates != null && selectedStates != undefined && selectedStates.length > 0) {
            var stateIds = selectedStates.map(s => s.Id);
            input.StateIds = stateIds.join(',');
        }
        if (selectedCities != null && selectedCities != undefined && selectedCities.length > 0) {
            var cityIds = selectedCities.map(s => s.Name);
            input.CityIds = cityIds.join(',');
        }
        if (selectedTerminals != null && selectedTerminals != undefined && selectedTerminals.length > 0) {
            var terminalIds = selectedTerminals.map(s => s.Id);
            input.TerminalIds = terminalIds.join(',');
        }
        if (selectedFuelTypes != null && selectedFuelTypes != undefined && selectedFuelTypes.length > 0) {
            var fuelTypeIds = selectedFuelTypes.map(s => s.Id);
            input.FuelTypeIds = fuelTypeIds.join(',');
        }
        input.CompanyId = this.CurrentCompanyId;
        input.CountryId = this.SelectedCountryId;

        if (input.CompanyId != undefined && input.CompanyId > 0) {
            this.carrierService.getProductMappingGridDetails(input).subscribe(data => {               
                this.IsLoading = false;
                this.ProductMappingList = data as ProductMappingGridModel[];
                this.dtTrigger.next();
            });
        }
    }

    getDefaultServingCountry() {
        this.IsLoading = true;
        this.carrierService.getDefaultServingCountry(this.CurrentCompanyId).subscribe(data => {
            this.IsLoading = false;
            this.SelectedCountryId = Number(data.DefaultCountryId);
            this.getStates();
            this.getProductMappingGridDetails();
        });
    }

    getStates() {
        this.IsLoading = true;
        if (this.SelectedCountryId != undefined && this.SelectedCountryId > 0) {
            this.carrierService.getStates(this.SelectedCountryId).subscribe(data => {
                this.IsLoading = false;
                this.StateList = data as DropdownItem[];
            });
        }
    }

    getCities(stateIds: number[]) {
        this.IsLoading = true;
        this.carrierService.getCities(stateIds).subscribe(data => {
            this.IsLoading = false;
            this.CityList = data as DropdownItem[];
        });
    }

    getTerminals() {
        var selectedStates = this.ProductMappingForm.get('States').value as DropdownItem[];
        var selectedCities = this.ProductMappingForm.get('Cities').value as DropdownItem[];

        if (selectedStates.length == 0) {
            this.ProductMappingForm.get('States').patchValue([]);
            this.TerminalList = [];
            return;
        }

        this.IsLoading = true;
        var input = new ProductMappingModel();
        if (selectedStates != null && selectedStates != undefined && selectedStates.length > 0) {
            var stateIds = selectedStates.map(s => s.Id);
            input.StateIds = stateIds.join(',');
        }
        if (selectedCities != null && selectedCities != undefined && selectedCities.length > 0) {
            var cityIds = selectedCities.map(s => s.Name);
            input.CityIds = cityIds.join(',');
        }
        input.CompanyId = this.CurrentCompanyId;
        this.carrierService.getTerminals(input).subscribe(data => {
            this.IsLoading = false;
            this.TerminalList = data as DropdownItem[];
        });
    }

    getServingFuelTypesByCompany() {
        this.IsLoading = true;
        this.carrierService.getServingFuelTypesByCompany(this.CurrentCompanyId).subscribe(data => {
            this.IsLoading = false;
            this.FuelTypeList = data as DropdownItem[];
        });
    }

    onSubmit() {
        var myProductId = this.ProductMappingForm.get('MyProductId').value;
        var backOfficeProductId = this.ProductMappingForm.get('BackOfficeProductId').value;
        var driverProductId = this.ProductMappingForm.get('DriverProductId').value;
      //  var terminalItemCode = this.ProductMappingForm.get('TerminalItemCode').value;
        if ((myProductId == undefined || myProductId == null || myProductId == '') &&
            (backOfficeProductId == undefined || backOfficeProductId == null || backOfficeProductId == '') &&
            (driverProductId == undefined || driverProductId == null || driverProductId == ''))
            /*&& (terminalItemCode == undefined || terminalItemCode == null || terminalItemCode ==''))*/ {
            Declarations.msgerror('Please provide My Product ID or Back Office Product ID or Driver Product ID', undefined, undefined);
            return;
        }

        this.ProductMappingForm.get("CompanyId").patchValue(this.CurrentCompanyId);
        this.IsValidForm = true;
        if (!this.ProductMappingForm.valid) {
            this.IsValidForm = false;
        }
        else {
            this.submitForm();
        }
    }

    submitForm() {
        this.IsLoading = true;
        this.carrierService.saveProductMapping(this.ProductMappingForm.value).subscribe(data => {
            if (data.StatusCode == 0) {  
                this.IsLoading = false;
                this.clearForm();    
                this.MyProductId.nativeElement.click();
                this.getProductMappingGridDetails();
                Declarations.msgsuccess(data.StatusMessage, undefined, undefined);
            }
            else if (data.StatusCode == 2) {
                Declarations.msgwarning(data.StatusMessage, undefined, undefined);
            }
            else {
                Declarations.msgerror(data == null ? 'Failed' : data.StatusMessage, undefined, undefined);
            }
        });
    }

    updateProductNames(mappingId: number) {
        var rowToUpdate = this.UpdateProductMappingList.filter(map => map.Id === mappingId);
        if (rowToUpdate.length == 0) {
            Declarations.msgerror('No updated records found', undefined, undefined);
            return;
        }

        this.IsLoading = true;
        this.carrierService.updateProductNames(rowToUpdate).subscribe(data => {
            this.IsLoading = false;
            if (data.StatusCode == 0) {
                this.UpdateProductMappingList = [];
                this.filterGrid();
                Declarations.msgsuccess(data.StatusMessage, undefined, undefined);
            }
            else if (data.StatusCode == 2) {
                Declarations.msgwarning(data.StatusMessage, undefined, undefined);
            }
            else {
                Declarations.msgerror(data == null ? 'Failed' : data.StatusMessage, undefined, undefined);
            }
        });
    }

    cancelUpdateProductNames() {
        //this.filterGrid();
    }

    editProductNames(mapping: ProductMappingGridModel, key: string, $event: any) {
        var nameToUpdate = $event.target.innerText;
        var existingId = '';
        switch (key) {
            case 'MyProductId':
                existingId = mapping.MyProductId;
                break;
            case 'DriverProductId':
                existingId = mapping.DriverProductId;
                break;
            case 'BackOfficeProductId':
                existingId = mapping.BackOfficeProductId;
                break;
            //case 'TerminalItemCode':
            //    existingId = mapping.TerminalItemCode;
            default:
                break;
        }

        if (nameToUpdate == undefined || nameToUpdate == null || nameToUpdate.trim() == '' || nameToUpdate == '--')
            nameToUpdate = '';

        if (existingId == undefined || existingId == null || existingId.trim() == '' || existingId == '--')
            existingId = '';

        if (nameToUpdate == existingId)
            return;

        mapping[key] = nameToUpdate.trim();
        var obj = this.UpdateProductMappingList.filter(map => map.Id === mapping.Id);

        if (this.UpdateProductMappingList.length == 0) {
            this.UpdateProductMappingList.push(mapping);
        }
        else if (obj.length == 1) {
            obj[key] = nameToUpdate.trim();
        }
        else {
            this.UpdateProductMappingList.push(mapping);
        }
    }

    deleteMapping(mappingId: number, companyId: number) {
        if (mappingId == undefined || mappingId <= 0)
            return;
        var model = new ProductMappingGridModel();
        model.Id = mappingId;
        model.CompanyId = companyId;
        this.IsLoading = true;
        this.carrierService.postDeleteProductMapping(model).subscribe(data => {
            this.IsLoading = false;
            if (data.StatusCode == 0) {
                this.clearForm();
                this.getProductMappingGridDetails();
                Declarations.msgsuccess(data.StatusMessage, undefined, undefined);
            }
            else if (data.StatusCode == 2) {
                Declarations.msgwarning(data.StatusMessage, undefined, undefined);
            }
            else {
                Declarations.msgerror(data == null ? 'Failed' : data.StatusMessage, undefined, undefined);
            }
        });
    }

    filterGrid() {
        $("#product-mapping-grid-datatable").DataTable().clear().destroy();
        this.getProductMappingGridDetails();
    }

    onStateSelect(state: any) {
        this.getCitiesByStateId();
        this.getTerminals();
    }

    onStateDeSelect(state: any) {
        this.ProductMappingForm.get('Cities').patchValue([]);
        this.ProductMappingForm.get('Terminals').patchValue([]);
        this.getCitiesByStateId();
        this.getTerminals();
    }

    onStateSelectAll(states: any) {
        this.ProductMappingForm.get('States').patchValue(states);
        this.getCitiesByStateId();
        this.getTerminals();
    }

    onStateDeSelectAll() {
        this.ProductMappingForm.get('Cities').patchValue([]);
        this.ProductMappingForm.get('Terminals').patchValue([]);
        this.CityList = [];
        this.TerminalList = [];
    }

    getCitiesByStateId() {
        var selectedStates = this.ProductMappingForm.get('States').value as DropdownItem[];
        if (selectedStates != null && selectedStates != undefined && selectedStates.length > 0) {
            var stateIds = selectedStates.map(m => m.Id);
            this.getCities(stateIds);
        }
        else {
            this.CityList = [];
        }
    }

    clearForm() {
        this.ProductMappingForm.reset();
        this.ProductMappingForm.get('States').setValue([]);
        this.ProductMappingForm.get('Cities').setValue([]);
        this.ProductMappingForm.get('Terminals').setValue([]);
        this.ProductMappingForm.get('FuelTypes').setValue([]);
        this.CityList = [];
        this.TerminalList = [];
        $("#product-mapping-grid-datatable").DataTable().clear().destroy();
    }

    clearFilter() {
        this.clearForm();
        this.getProductMappingGridDetails();
    }

    onCitySelect(city: any) {
        this.getTerminals();
    }

    onCityDeSelect(city: any) {
        this.ProductMappingForm.get('Terminals').patchValue([]);
        this.getTerminals();
    }

    onCitySelectAll(cities: any) {
        this.ProductMappingForm.get('Cities').patchValue(cities);
        this.getTerminals();
    }

    onCityDeSelectAll() {
        this.ProductMappingForm.get('Terminals').patchValue([]);
        this.ProductMappingForm.get('Cities').patchValue([]);
        this.getTerminals();
        //this.CityList = [];
    }

    selectFiles(files: File[]) {
        if (files != null && files != undefined && files.length > 0)
            this.SelectedFiles = files;
    }

    uploadProductMappingFile() {
        var files = this.SelectedFiles;
        if (files.length === 0)
            return;
       
        const formData = new FormData();
        for (var file of files) {
            if (!this.isValidFile(file)) {
                return;
            }
            formData.append(file.name, file);
        }
        
        this.IsLoading = true;
        this.carrierService.postBulkUploadTemplate(formData).subscribe(data => {
            this.IsLoading = false;
            if (data.StatusCode == 0) {
                this.CloseBulkUploadPopup.nativeElement.click();
                Declarations.msgsuccess(data.StatusMessage, undefined, undefined);
                this.SelectedFiles = [];
                //$("#product-mapping-grid-datatable").DataTable().clear().destroy();
            }
            else if (data.StatusCode == 2) {
                Declarations.msgwarning(data.StatusMessage, undefined, undefined);
            }
            else {
                Declarations.msgerror(data == null ? 'Failed' : data.StatusMessage, undefined, undefined);
            }
        });
    }

    isValidFile(file: File) {
        var isValid = true;

        var extension = this.getExtension(file.name);
        if (extension == undefined || extension == null || extension == '' || extension.toLowerCase() != 'csv') {
            Declarations.msgerror('Invalid file, only csv files are allowed', undefined, undefined);
            isValid = false;
            return isValid;
        }

        if (file.size > this.MaxFileUploadSize) {
            Declarations.msgerror('Invalid file size, file size should not be greater than 1 MB', undefined, undefined);
            isValid = false;
            return isValid;
        }

        return isValid;
    }

    downloadCsvTemplate() {
        this.IsLoading = true;
        var timestamp = new Date().getTime();
        this.carrierService.downloadProductMappingTemplate(timestamp).subscribe(blob => {
            const a = document.createElement('a')
            const objectUrl = URL.createObjectURL(blob)
            a.href = objectUrl
            a.download = 'ProductMapping_Template.csv';
            a.click();
            URL.revokeObjectURL(objectUrl);

            this.IsLoading = false;            
        });
    }

    getExtension(fileName) {
        // extract file name from full path ...
        var basename = fileName.split(/[\\/]/).pop();

        // (supports `\\` and `/` separators)
        var pos = basename.lastIndexOf(".");       // get last position of `.`

        if (basename === "" || pos < 1)            // if file name is empty or ...
            return "";                             //  `.` not found (-1) or comes first (0)

        return basename.slice(pos + 1);            // extract extension ignoring `.`
    }

    showBulkUploadPopup() {
        this.IsShowBulkUploadPopup = true;
    }

    closePopup() {
        this.IsShowBulkUploadPopup = false;
    }

    initializeProductMappingDatatableGrid() {
        let exportColumns = { columns: [0, 1, 2, 3, 4, 5] };
        this.dtOptions = {
            dom: '<"html5buttons"B>lTfgitp',
            buttons: [
                { extend: 'colvis' },
                { extend: 'copy', exportOptions: exportColumns },
                { extend: 'csv', title: 'Product Mapping', exportOptions: exportColumns },
                { extend: 'pdf', title: 'Product Mapping', orientation: 'landscape', exportOptions: exportColumns },
                { extend: 'print', exportOptions: exportColumns }
            ],
            pagingType: 'first_last_numbers',
            pageLength: 10,
            lengthMenu: [[10, 25, 50, 100, -1], [10, 25, 50, 100, "All"]]
            //,
            //rowCallback: (row: Node, data: any[] | Object, index: number) => {
            //    const self = this;
            //    // Unbind first in order to avoid any duplicate handler
            //    // (see https://github.com/l-lin/angular-datatables/issues/87)
            //    $('td', row).unbind('click');
            //    $('td', row).bind('click', () => {
            //        self.someClickHandler(data, event);
            //    });
            //    return row;
            //}
        };
    }

    //someClickHandler(info: any, $event: any): void {
    //    console.log($event);
    //    console.log($event.target.textContent);
    //}
}
