import { Component, OnInit, Inject, Output, EventEmitter } from '@angular/core';
import { FormGroup, FormBuilder, FormControl, Validators, AbstractControl } from '@angular/forms';
import { IDropdownSettings } from 'ng-multiselect-dropdown';
import { DropdownItem, DropdownItemExt } from 'src/app/statelist.service';
import { FuelSurchargeService } from 'src/app/fuelsurcharge/services/fuelsurcharge.service';
import { RegionService } from 'src/app/company-addresses/region/service/region.service';
import { HttpClient } from '@angular/common/http';
import { CreateAccessorialFeeModel } from '../model/accessorial-fees';
import * as moment from 'moment';
import { from, forkJoin, merge } from 'rxjs';
import { FeeModel } from 'src/app/invoice/models/DropDetail';
import { Declarations } from 'src/app/declarations.module';
import {AccessorialFeesService } from '../service/accessorialfees.service';
import { DOCUMENT } from '@angular/common';
import { map, mergeMap, pairwise, startWith } from 'rxjs/operators';
import { CarrierService } from 'src/app/carrier/service/carrier.service';
import { FreightTableStatus, SourceRegionInputModel, TableType } from 'src/app/app.enum';
import { DataService } from 'src/app/services/data.service';
declare var currentUserCompanyId: any;


@Component({
  selector: 'app-create-accessorial-fees',
  templateUrl: './create-accessorial-fees.component.html',
  styleUrls: ['./create-accessorial-fees.component.css']
})
export class CreateAccessorialFeesComponent implements OnInit {
    public rcForm: FormGroup;
    public minDate = new Date();
    public maxDate = new Date();
    public SingleSelectSettingsById = {};
    public MultiSelectSettingsById: IDropdownSettings;
    public MultiSelectSettingsByGroup = {};

    public IsLoading: boolean = false;
    public SelectedCountryId: number = -1;
    public CurrentCompanyId: number;
    public AccessorialFeeId?: number;
    public AccessorialFeeMode: string = "CREATE";
    public TableTypeList: DropdownItem[];
    public CustomerList: DropdownItem[];
    public CarrierList: DropdownItem[];
    public SourceRegionList: DropdownItem[];
    public TerminalsAndBulkPlantList: DropdownItemExt[] = [];

    public Fees: FeeModel[] = [];
    public IsCustomerSelected = false;
    public IsMasterSelected = false;
    public IsCarrierSelected = false;
    public IsSourceRegionSelected = false;
    
    public decimalSupportedRegx = /^[0-9]\d{0,9}(\.\d{0,5})?%?$/;
    public SelectedTerminalsAndBulkPlants: DropdownItemExt[] = [];
    public Afmodel: CreateAccessorialFeeModel;
    public disableInputControls: boolean = false;
    public ServiceResponse: any;
    public IsEditable: boolean = true;
    public IsLoaded: boolean = true;
    @Output() onPageSubmit = new EventEmitter<any>();

    constructor(private fb: FormBuilder, private fuelsurchargeService: FuelSurchargeService, private dataService: DataService,
        private regionService: RegionService, private carrierService: CarrierService, private accesorialFeeService: AccessorialFeesService, private http: HttpClient,
        @Inject(DOCUMENT) private _document: Document) { }

    ngOnInit() {
        this.maxDate.setFullYear(this.maxDate.getFullYear() + 20);

        this.CurrentCompanyId = Number(currentUserCompanyId);
        this.getDefaultServingCountry();

        this.SingleSelectSettingsById = {
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
        this.MultiSelectSettingsById = {
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
            text: "Select Terminals or Bulk Plants",
            selectAllText: 'Select All',
            unSelectAllText: 'UnSelect All',
            searchPlaceholderText: 'Search',
            primaryKey: "Id",
            labelKey: "Name",
            enableSearchFilter: true,
            badgeShowLimit: 5,
            groupBy: "Code"
        };

        this.rcForm = this.createForm();
        this.getTableTypes();
        this.rcForm.controls['TableTypes'].patchValue([{ Id: TableType.Master, Name: "Master" }]);// default will master
        this.IsMasterSelected = true;
        
        this.getSourceRegions(TableType.Master.toString());

        this.accesorialFeeService.onSelectedAccessorialFeeId.subscribe(s => {
            if (s) {
                let stringify = JSON.parse(s);
                this.AccessorialFeeId = stringify.AccessorialFeeId;
                this.AccessorialFeeMode = stringify.Mode;
            }
        })

        let id = localStorage.getItem("AccessorialFeeId");
        if (id && +id > 0) {
            this.AccessorialFeeId = Number(id);
            this.AccessorialFeeMode = "VIEW";
            localStorage.removeItem("AccessorialFeeId");
        }
        merge(
            this.rcForm.get('SourceRegions').valueChanges
        )
            .pipe(startWith(null), pairwise())
            .subscribe(([prev, next]: [any, any]) => {
                if (this.IsLoaded && JSON.stringify(prev) != JSON.stringify(next)) this.SourceRegionChange(prev, next);
            });
    }


    getDefaultServingCountry() {
        this.carrierService.getDefaultServingCountry(this.CurrentCompanyId).subscribe(data => {
            this.SelectedCountryId = Number(data.DefaultCountryId);
        });
    }

    private getTableTypes(): void {
        this.fuelsurchargeService.getTableTypes().subscribe(async (data) => {
            this.TableTypeList = await (data);
            this.rcForm.controls['TableTypes'].setValue(this.TableTypeList.filter(x => x.Id == TableType.Master));// default will master
            this.rcForm.controls['TableTypeId'].setValue(TableType.Master);
            this.IsMasterSelected = true;
        });
    }

    private getSourceRegions(tableType: string): void {
        let customerIds: number[] = [];
        let carrierIds: number[] = [];

        let selectedCustomers = this.rcForm.get('Customers').value as DropdownItem[];
        if (selectedCustomers != null && selectedCustomers != undefined && selectedCustomers.length > 0) {
            customerIds = selectedCustomers.map(s => s.Id);
        }

        let selectedCarriers = this.rcForm.get('Carriers').value as DropdownItem[];
        if (selectedCarriers != null && selectedCarriers != undefined && selectedCarriers.length > 0) {
            carrierIds = selectedCarriers.map(s => s.Id);
        }

        var sourceRegionInput = new SourceRegionInputModel();
        sourceRegionInput.TableType = tableType;
        sourceRegionInput.CustomerId = customerIds;
        sourceRegionInput.CarrierId = carrierIds;
        this.fuelsurchargeService.getSourceRegions(sourceRegionInput).subscribe(async (data) => {
            this.SourceRegionList = await (data);
        });
    }

    private createForm() {
        return this.fb.group({
            AccessorialFeeId: new FormControl(''),
            TableTypeId: new FormControl(),
            TableName: new FormControl('', [Validators.required]),
            TableTypes: new FormControl('', [Validators.required]),
            Customers: new FormControl(''),
            Carriers: new FormControl(''),
            SourceRegions: new FormControl('', [Validators.required]),
            TerminalsAndBulkPlants: new FormControl(this.SelectedTerminalsAndBulkPlants),
            StartDate: new FormControl("", [Validators.required]),
            EndDate: new FormControl(""),
            StatusId: new FormControl(''),
        })
    }

    private AddRemoveValidations(requiredControls: AbstractControl[], notRequiredControls: AbstractControl[]) {
        if (requiredControls != null && requiredControls != undefined && requiredControls.length > 0) {

            requiredControls.forEach(ctrl => {
                ctrl.setValidators([Validators.required]);
                ctrl.updateValueAndValidity();
            });
        }
        if (notRequiredControls != null && notRequiredControls != undefined && notRequiredControls.length > 0) {

            notRequiredControls.forEach(ctrl => {
                ctrl.clearValidators();
                ctrl.updateValueAndValidity();
            });
        }

    }
    public onTableTypeSelect(item: any): void {
        this.IsMasterSelected = false;
        this.IsCustomerSelected = false;
        this.IsCarrierSelected = false;
        this.rcForm.get('Carriers').patchValue([]);
        this.rcForm.get('Customers').patchValue([]);
        this.rcForm.controls['TableTypeId'].setValue(item.Id);
        switch (item.Id) {
            case 1: //master
                this.IsMasterSelected = true;
                this.AddRemoveValidations([this.rcForm.get('TableTypes')], [this.rcForm.get('Customers'), this.rcForm.get('Carriers')]); //"Carriers,Customers"
                break;
            case 2: // customer
                this.getSupplierCustomers();
                this.getCarriers();
                this.IsCustomerSelected = true;
                this.AddRemoveValidations([this.rcForm.get('Customers')], [this.rcForm.get('Carriers')]);
                break;
            case 3: //carrier
                this.getSupplierCustomers();
                this.getCarriers();
                this.IsCarrierSelected = true;
                this.AddRemoveValidations([this.rcForm.get('Carriers')], [this.rcForm.get('Customers')]);
                break;
        }
        this.rcForm.get('SourceRegions').patchValue([]);
        this.getSourceRegions(item.Id);
    }
    
    public onCarriersSelect(item: any): void {
        this.onCarriersOrCustomersChange();
    }

    public onCustomersSelect(item: any): void {
        this.onCarriersOrCustomersChange();
    }

    public onCustomersDeSelect(item: any): void {
        this.rcForm.get('SourceRegions').patchValue([]);
        this.onCarriersOrCustomersChange();
    }

    public onCarriersDeSelect(item: any): void {
        this.rcForm.get('SourceRegions').patchValue([]);
        this.onCarriersOrCustomersChange();
    }

    public onCarriersOrCustomersChange() {
        var selectedTableType = this.rcForm.get('TableTypes').value as DropdownItem[];
        this.getSourceRegions(selectedTableType[0].Id.toString());
    }
    public onSourceRegionsDeSelect(item: any): void {
        var sr = this.rcForm.get('SourceRegions').value as DropdownItem[];
        this.IsSourceRegionSelected = sr.length > 0;
    }

    public onSourceRegionsDeSelectAll(item: any): void {
        this.IsSourceRegionSelected = false;
    }

    private getSupplierCustomers(): void {
        this.fuelsurchargeService.getSupplierCustomers().subscribe(async (data) => {
            this.CustomerList = await (data);
        });
    }

    public SourceRegionChange(prev: any, next: any) {
        if (prev == null && next.length == 0) return;
        this.rcForm.controls.TerminalsAndBulkPlants.patchValue([]);
      
        this.IsSourceRegionSelected = false
        var ids = [];
        
        let selectedSourceRegions = this.rcForm.get('SourceRegions').value as DropdownItem[];
        if (selectedSourceRegions.length > 0) {
            selectedSourceRegions.forEach(s => ids.push(s.Id));
            this.fuelsurchargeService.getTerminalsAndBulkPlants(ids.join(',')).subscribe(async (data) => {
                this.TerminalsAndBulkPlantList = await (data);
                this.rcForm.controls.TerminalsAndBulkPlants.setValue(this.TerminalsAndBulkPlantList);
                this.IsSourceRegionSelected = true;
            });
        }
    }

    private getCarriers(): void {
        this.regionService.getCarriers()
            .subscribe(async (carriers: DropdownItem[]) => {
                this.CarrierList = await carriers;
            });
    }

    private getTerminalsBulkPlant(): void {
        var selectedSourceRegions = this.rcForm.get('SourceRegions').value as DropdownItem[];
        if (selectedSourceRegions != undefined && selectedSourceRegions != null) {
            this.IsSourceRegionSelected = true;
            this.fuelsurchargeService.getTerminalsAndBulkPlants(selectedSourceRegions.map(s => s.Id).join(',')).subscribe(async (data) => {             
                this.TerminalsAndBulkPlantList = await (data);
            });
        }
    }

    public onSubmit(status: any): void {       
        let accessorialFeeName = this.rcForm.get('TableName').value;
        if (accessorialFeeName == null || accessorialFeeName == undefined || accessorialFeeName == "")
        {
            Declarations.msgerror(" Table Name is required", undefined, undefined);
            return;
        }

        let AccessorialDate = this.rcForm.get('StartDate').value;
        if (AccessorialDate == null || AccessorialDate == undefined || AccessorialDate == "")
        {
            Declarations.msgerror(" Date is required", undefined, undefined);
            return;

        }

        let feeModel = this.createPostObject(status);
        if (feeModel.Status == FreightTableStatus.Draft && +this.rcForm.controls['StatusId'].value == FreightTableStatus.Published) {
            if (this.rcForm.get('AccessorialFeeId').value != "") {
                Declarations.msgerror("Not allowed. " + this.rcForm.get('TableName').value + " is in published mode.", undefined, undefined);
                this.IsLoading = false;
                return;
            }
        }
        else if (feeModel.Status == FreightTableStatus.Published) {
            this.rcForm.markAllAsTouched();
            if (this.rcForm.valid) {
                let fees = this.rcForm.get('Fees').value as DropdownItem[];
                if (fees == null || fees == undefined || fees.length == 0) {
                    Declarations.msgerror("Please add Fee(s)", undefined, undefined);
                    return;
                }
            }
        }
        this.IsLoading = true;
        if (this.rcForm.get('AccessorialFeeId').value != "") {
            this.accesorialFeeService.updateAccessorialFee(feeModel)
                .subscribe((response: any) => {
                    this.ServiceResponse = response;
                    if (response != null && response.StatusCode == 0) {
                        let message = " edited";
                        if (feeModel.Status == FreightTableStatus.Draft) {
                            message = " saved draft"
                        }
                        Declarations.msgsuccess(feeModel.Name + message + " successfully.", undefined, undefined);
                        this.IsLoading = false;
                        this.changeViewType(2);
                    }
                    else {
                        this.IsLoading = false;
                        Declarations.msgerror(response == null || response.StatusMessage == null ? 'Failed' : response.StatusMessage, undefined, undefined);
                    }
                });
        } else {
            this.accesorialFeeService.createAccessorialFee(feeModel)
                .subscribe((response: any) => {
                    this.ServiceResponse = response;
                    if (response != null && response.StatusCode == 0) {
                        let message = "";
                        if (feeModel.Status == FreightTableStatus.Published) {
                            message = " created"
                        }
                        else if (feeModel.Status == FreightTableStatus.Draft) {
                            message = " saved draft"
                        }
                        Declarations.msgsuccess(feeModel.Name + message + " successfully.", undefined, undefined);
                        this.IsLoading = false;
                        this.changeViewType(2);
                    }
                    else {
                        this.IsLoading = false;
                        Declarations.msgerror(response == null || response.StatusMessage == null ? 'Failed' : response.StatusMessage, undefined, undefined);
                    }
                });
        }
    }
    public clearForm() {
        this.rcForm.get('TableName').patchValue([]);
        this.rcForm.get('TableTypes').patchValue([]);
        this.rcForm.get('SourceRegions').patchValue([]);
        this.rcForm.get('TerminalsAndBulkPlants').patchValue([]);
        this.rcForm.get('StartDate').patchValue([]);
        this.rcForm.get('EndDate').patchValue([]);
        this.rcForm.controls['Fees'].reset();

        this.disableInputControls = false;
        this.dataService.removeFeesOnCreateNewSubject();
    }

    public onCancel(): void {
        if (this.AccessorialFeeMode == "VIEW") {
            this.disableInputControls = false;
            this.AccessorialFeeId = null;
        }
       
        if (this.AccessorialFeeMode == "EDIT") {
            this.AccessorialFeeId = null;
        }

        if (this.AccessorialFeeId != null) {
            this.changeToViewTab();
        }
        else {
            this._document.defaultView.location.reload();
        } 
    }

    public changeToViewTab() {
        this.accesorialFeeService.onSelectedTabChanged.next(1);

    }

    public removeValidators(form: FormGroup) {
        for (const key in form.controls) {
            if (key == 'TableName') {
                continue;
            }
            else {
                form.get(key).clearValidators();
                form.get(key).updateValueAndValidity();
            }            
        }
    }

    public changeViewType(viewType: any): void {
        this.onPageSubmit.emit(viewType);
    }


    public createPostObject(status: any): CreateAccessorialFeeModel {
        let feeModel = new CreateAccessorialFeeModel();
        feeModel.Id = this.rcForm.get('AccessorialFeeId').value;
        feeModel.Name = this.rcForm.get('TableName').value;
        feeModel.Status = status;
        let selectedTerminalBulkplant = this.rcForm.get('TerminalsAndBulkPlants').value as DropdownItemExt[];
        if (selectedTerminalBulkplant != null && selectedTerminalBulkplant != undefined && selectedTerminalBulkplant.length > 0)
        {
            feeModel.TerminalsAndBulkPlants = this.rcForm.get('TerminalsAndBulkPlants').value;
        }
        let selectedCustomers = this.rcForm.get('Customers').value as DropdownItem[];
        if (selectedCustomers != null && selectedCustomers != undefined && selectedCustomers.length > 0)
        {
            selectedCustomers.forEach(t => feeModel.CustomerIds.push(t.Id));
        }
        let selectedCarriers = this.rcForm.get('Carriers').value as DropdownItem[];
        if (selectedCarriers != null && selectedCarriers != undefined && selectedCarriers.length > 0)
        {
            selectedCarriers.forEach(t => feeModel.CarrierIds.push(t.Id));
        }
        let endDate = this.rcForm.get('EndDate').value;
        let startDate = this.rcForm.get('StartDate').value;
        if (endDate == "" || endDate == undefined || endDate == null)
        {
            endDate = null;
        }
        if (startDate == "" || startDate == undefined || startDate == null)
        {
            startDate = null;
        }
        feeModel.StartDate = startDate;
        feeModel.EndDate = endDate;        
        feeModel.Fees = this.rcForm.get('Fees').value;
        let sourceRegions = this.rcForm.get('SourceRegions').value as DropdownItem[];
        if (sourceRegions != null && sourceRegions != undefined && sourceRegions.length > 0) {
            sourceRegions.forEach(t => feeModel.SourceRegionIds.push(t.Id));
        }
        let tableType = this.rcForm.get('TableTypes').value as DropdownItem[];
        if (tableType != null && tableType != undefined && tableType.length > 0)
        {
            feeModel.TableType = tableType[0].Id;
        }
        feeModel.CountryId = this.SelectedCountryId;
        
        return feeModel;
    }

    public getBulkPlantTerminalIds(type: string): number[] {
        let Ids = [];
        if (type === 'Terminals') {
            let selectedTerminalBulkplant = this.rcForm.get('TerminalsAndBulkPlants').value as DropdownItemExt[];
            let selectedTerminals = selectedTerminalBulkplant.filter(t => t.Code === 'Terminals');
            if (selectedTerminals != null && selectedTerminals != undefined && selectedTerminals.length > 0) {
                selectedTerminals.forEach(function (terminal) {
                    let terminalId = parseInt(terminal.Id.replace("Terminals_", ""));
                    if (!isNaN(terminalId)) {
                        Ids.push(terminalId);
                    }
                });
            }

        }
        else if (type === 'BulkPlants') {
            let selectedTerminalBulkplant = this.rcForm.get('TerminalsAndBulkPlants').value as DropdownItemExt[];
            let selectedBulkPlants = selectedTerminalBulkplant.filter(t => t.Code === 'BulkPlants');
            if (selectedBulkPlants != null && selectedBulkPlants != undefined && selectedBulkPlants.length > 0) {
                selectedBulkPlants.forEach(function (bulkplant) {
                    let bulkplantId = parseInt(bulkplant.Id.replace("BulkPlants_", ""));
                    if (!isNaN(bulkplantId)) {
                        Ids.push(bulkplantId);
                    }
                });
            }
        }
        return Ids;
    }

    ngAfterViewInit() {
        if (this.AccessorialFeeId != null && this.AccessorialFeeId != undefined) {
            this.getAccessorialFee(this.AccessorialFeeId); //existing Accessorial Fee.
        }
    }

    //GET
    private getAccessorialFee(accessorialFeeId: number): void {
        this.IsLoading = true;
        this.IsLoaded = false;
        let sorceRegionIds: string = "";
        this.http.get(this.accesorialFeeService.getAccessorialFeeUrl + accessorialFeeId).pipe(
            map(af => {
                const afModel = af as CreateAccessorialFeeModel;
                return afModel;
            }),
            mergeMap(afModel => {
                this.Afmodel = afModel;
                let companyIds: number[] = [];
                if (this.AccessorialFeeId != null && this.AccessorialFeeMode.toUpperCase() == "COPY") { // on copy 
                    this.Afmodel.Id = null;
                    this.Afmodel.Name = "";
                }
                const customers = this.http.get(this.fuelsurchargeService.getSupplierCustomersUrl);
                const carriers = this.http.get(this.regionService.getCarriersUrl);

                if (this.Afmodel.TableType == TableType.Customer && this.Afmodel.CustomerIds.length > 0) {
                    this.Afmodel.CustomerIds.forEach(s => companyIds.push(s));
                }
                if (this.Afmodel.TableType == TableType.Carrier && this.Afmodel.CarrierIds.length > 0) {
                    this.Afmodel.CarrierIds.forEach(s => companyIds.push(s));
                }

                var sourceRegionInput = new SourceRegionInputModel();
                sourceRegionInput.TableType = this.Afmodel.TableType.toString();
                sourceRegionInput.CustomerId = this.Afmodel.CustomerIds;
                sourceRegionInput.CarrierId = this.Afmodel.CarrierIds;

                const sourceRegions = this.http.post(this.fuelsurchargeService.getSourceRegionsUrl, sourceRegionInput);
                const tableTypes = this.http.get(this.fuelsurchargeService.getTableTypesUrl);

                if (this.Afmodel.SourceRegionIds != null && this.Afmodel.SourceRegionIds != undefined && this.Afmodel.SourceRegionIds.length > 0) {
                    sorceRegionIds = this.Afmodel.SourceRegionIds.map(s => s).join(',');
                    this.IsSourceRegionSelected = true;
                }
                const terminalAndBulkPlans = this.http.get(this.fuelsurchargeService.getTerminalsAndBulkPlantsUrl + sorceRegionIds)
                return forkJoin([customers, carriers, sourceRegions, terminalAndBulkPlans, tableTypes]);
            })).subscribe(result => {
                this.IsLoading = false;
                this.IsMasterSelected = false;
                this.IsCustomerSelected = false;
                this.IsCarrierSelected = false;

                if (this.Afmodel.TableType == TableType.Master) {
                    this.IsMasterSelected = true;
                }
                else if (this.Afmodel.TableType == TableType.Customer) {
                    this.IsCustomerSelected = true;
                }
                else if (this.Afmodel.TableType == TableType.Carrier) {
                    this.IsCarrierSelected = true;
                }

                if (this.Afmodel.TableType != TableType.Master) {
                    this.CustomerList = result[0] as DropdownItem[];
                    this.CarrierList = result[1] as DropdownItem[];
                }

                this.SourceRegionList = result[2] as DropdownItem[];
                if (this.Afmodel.TerminalsAndBulkPlants != null && this.Afmodel.TerminalsAndBulkPlants != undefined && this.Afmodel.TerminalsAndBulkPlants.length > 0) {
                    this.TerminalsAndBulkPlantList = result[3] as DropdownItemExt[];
                }
                this.TableTypeList = result[4] as DropdownItem[];
                this.Edit(this.Afmodel);

            });
    }

    //Edit
    private Edit(_af: CreateAccessorialFeeModel) {
      if (this.rcForm) {

            if (this.AccessorialFeeMode != "COPY") {
                this.rcForm.controls['AccessorialFeeId'].setValue(_af.Id);
                this.rcForm.controls['TableTypes'].setValue(_af.TableType);
                this.rcForm.controls['TableName'].setValue(_af.Name);
                this.IsEditable = false;
            } else {
                this.AccessorialFeeId = null;
            }
           
          this.rcForm.controls['TableTypes'].setValue(this.TableTypeList.filter(x => x.Id == _af.TableType));

            if (_af.TableType != TableType.Master) this.IsMasterSelected = false;

            if (_af.CustomerIds != null && _af.CustomerIds != undefined && _af.CustomerIds.length > 0) {
                if (this.CustomerList.length > 0 && _af.CustomerIds.length > 0) this.rcForm.controls['Customers'].setValue(this.CustomerList.filter(this.IdInComparer(_af.CustomerIds)));
            }

            if (_af.CarrierIds != null && _af.CarrierIds != undefined && _af.CarrierIds.length > 0) {
                if (this.CarrierList.length > 0 && _af.CarrierIds.length > 0) this.rcForm.controls['Carriers'].setValue(this.CarrierList.filter(this.IdInComparer(_af.CarrierIds)));
            }

            if (this.SourceRegionList != null && this.SourceRegionList != undefined && _af.SourceRegionIds != null && _af.SourceRegionIds != undefined && _af.SourceRegionIds.length > 0) {
                if (this.SourceRegionList.length > 0 && _af.SourceRegionIds.length > 0)
                    this.rcForm.controls['SourceRegions'].setValue(this.SourceRegionList.filter(this.IdInComparer(_af.SourceRegionIds)));
            }
            if (this.TerminalsAndBulkPlantList != null && this.TerminalsAndBulkPlantList != undefined && _af.TerminalsAndBulkPlants != null && _af.TerminalsAndBulkPlants != undefined && _af.TerminalsAndBulkPlants.length > 0) {
                if (this.TerminalsAndBulkPlantList.length > 0 && _af.TerminalsAndBulkPlants.length > 0) {
                    this.rcForm.controls['TerminalsAndBulkPlants'].setValue(this.TerminalsAndBulkPlantList.filter(this.ComparerWithId(_af.TerminalsAndBulkPlants)));
                }
            }

            this.rcForm.get('StartDate').setValue(moment(_af.StartDate).format('MM/DD/YYYY'));

            if (_af.EndDate != null && _af.EndDate != undefined) {
                this.rcForm.get('EndDate').setValue(moment(_af.EndDate).format('MM/DD/YYYY'));
            }

            this.Fees = _af.Fees;
            
            this.rcForm.controls['StatusId'].setValue(_af.Status);
            this.IsLoading = false;
            this.IsLoaded = true;
        }

        if (this.AccessorialFeeMode == "VIEW") {
            this.disableInputControls = true;
        }
    }

    private IdInComparer(otherArray) {
        return function (current) {
            return otherArray.filter(function (other) {
              
                return other == current.Id
            }).length == 1;
        }
    }

    private ComparerWithId(otherArray) {
        return function (current) {
            return otherArray.filter(function (other) {
                return other.Id == current.Id
            }).length == 1;
        }
    }
}
