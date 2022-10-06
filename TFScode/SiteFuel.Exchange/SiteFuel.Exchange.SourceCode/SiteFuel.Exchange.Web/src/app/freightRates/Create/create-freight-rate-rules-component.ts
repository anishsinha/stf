import { Component, OnInit, Input, ViewChild, ChangeDetectorRef } from '@angular/core';
import { DataTableDirective } from 'angular-datatables';
import { FreightRateRulesService } from '../Services/freight-rate-rules.service'
import { FreightRateViewModel, FreightRateFuelGroupViewModel, FreightRateRouteTableViewModel, FreightRateRangeTableViewModel, FreightRatePtoPTable } from '../Models/createFreightRateRules'
import { Subject, forkJoin, merge } from 'rxjs';
import { map, mergeMap } from 'rxjs/operators';
import { FormBuilder, FormGroup, Validators, FormControl, FormArray, AbstractControl, ValidatorFn, ValidationErrors } from '@angular/forms';
import { IDropdownSettings } from 'ng-multiselect-dropdown';
import { DropdownItem, DropdownItemExt } from 'src/app/statelist.service';
import { Declarations } from 'src/app/declarations.module';
import * as moment from 'moment';
import { HttpClient } from '@angular/common/http';
import { FreightComponent } from '../../shared-components/Freight/freight.component';
import { BOOL_TYPE } from '@angular/compiler/src/output/output_ast';
import { FreightRateRuleType, FreightTableStatus, SourceRegionInputModel, TableType } from 'src/app/app.enum';


@Component({
    selector: 'app-create-freight-rate-rules',
    templateUrl: './create-freight-rate-rules-component.html',
    styleUrls: ['./create-freight-rate-rules-component.css']
})
export class CreateFreightRateRules implements OnInit {
    @ViewChild(FreightComponent) public freightComponent: FreightComponent;
    @ViewChild(DataTableDirective) datatableElement: DataTableDirective;
    public IsEditable: boolean = true;
    public rForm: FormGroup;
    public fgTemp: FormGroup;
    public fgTempG: FormGroup;
    public rcFormRange: FormGroup;
    public dtOptions: any = {};
    public DtTrigger: Subject<any> = new Subject();
    public IsLoading: boolean = false;
    public IsGeneratedSurchargeTable = false;
    public SingleSelectSettingsById = {};
    public MultiSelectSettingsById: IDropdownSettings;
    public ServiceResponse: any;
    public viewType = 1;
    public RuleId?: number;
    public RuleMode: string="CREATE";
    public freightRateRuleType: number = FreightRateRuleType.Range;
    public FrModel: FreightRateViewModel;
    public decimalSupportedRegx = /^(?:(?:0|[1-9][0-9]*)(?:\.[0-9]*)?|\.[0-9]+)$/;
    public intGreaterThanZeroRegx = /^[1-9][0-9]*$/;

    public SelectedFuelGroups: DropdownItem[]=[]; // for mixed fuel group.
   
    public disableInputControls: boolean = false;

    constructor(
        private freightRateRulesService: FreightRateRulesService,
        private http: HttpClient, 
        private cdr: ChangeDetectorRef,
        private _fb: FormBuilder) { }

    ngOnInit() {
        let exportInvitedColumns = { columns: ':visible' };
        this.dtOptions = {
            dom: '<"html5buttons"B>lTfgitp',
            aaSorting: [],
            orderable: false,
            buttons: [
                { extend: 'colvis' },
                { extend: 'copy', exportOptions: exportInvitedColumns },
                { extend: 'csv', title: 'Freight Rate P2P', exportOptions: exportInvitedColumns },
                { extend: 'pdf', title: 'Freight Rate P2P', orientation: 'landscape', exportOptions: exportInvitedColumns },
                { extend: 'print', exportOptions: exportInvitedColumns }
            ],
            pagingType: 'first_last_numbers',
            pageLength: 10,
            lengthMenu: [[10, 25, 50, 100, -1], [10, 25, 50, 100, "All"]]
        };

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

        this.freightRateRulesService.onSelectedFreightRateRuleId.subscribe(s => {
            if (s) {
                let stringify = JSON.parse(s);
                this.RuleId = stringify.RuleId;
                this.RuleMode = stringify.Mode;
                this.freightRateRuleType = stringify.FreightRateRuleType;
            }
        })

        // with order page integration
        let id = localStorage.getItem("FreightRateRuleId");
        if (id && +id > 0) {
            this.RuleId = Number(id);
            this.RuleMode = "VIEW";
            localStorage.removeItem("FreightRateRuleId");
        }

        this.createForm(this.freightRateRuleType);       
    }

    ngAfterViewInit() {        
        if (this.freightRateRuleType == FreightRateRuleType.P2P) {
            this.DtTrigger.next();
        }
        if (this.RuleId == null || this.RuleId == undefined) return;

        this.freightComponent.isLoadingSubject.next(true);
        this.freightComponent.IsLoaded = false;
        this.IsLoading = true;
        this.cdr.detectChanges();
        this.http.get(this.freightRateRulesService.getFreightRateDetailUrl + this.RuleId).pipe(
            map(fr => {
                const frModel = fr as FreightRateViewModel;
                return frModel;
            }),
            mergeMap(frModel => {                
                this.FrModel = frModel;
                let companyIds: number[] = [];
                if (this.FrModel.CustomerIds.length > 0) {
                    this.FrModel.CustomerIds.forEach(s => companyIds.push(s));
                }
                if (this.FrModel.CarrierIds.length > 0) {
                    this.FrModel.CarrierIds.forEach(s => companyIds.push(s));
                }
                var getSupplierCustomersUrl = "/FuelSurcharge/GetSupplierCustomers";
                var getTableTypesUrl = "/FuelSurcharge/GetTableTypes";
                var getCarriersUrl = '/Region/GetCarriers';
                var getFuelGroupsUrl = "/FuelGroup/GetFuelGroups?fuelGroupType=" + this.FrModel.FuelGroupType + "&companyIds=" + companyIds.join(',')
                var getSourceRegionsUrl = "/FuelSurcharge/GetSourceRegionsAsync";
                var getTerminalsAndBulkPlantsUrl = "/FuelSurcharge/GetTerminalsAndBulkPlantsAsync?regionIds=";
                var getCustomerJobsUrl = "/FreightRate/GetCustomerJobs?customerId=" + this.FrModel.CustomerIds.join(',');

                var sourceRegionInput = new SourceRegionInputModel();
                sourceRegionInput.TableType = this.FrModel.TableType.toString();
                sourceRegionInput.CustomerId = this.FrModel.CustomerIds;
                sourceRegionInput.CarrierId = this.FrModel.CarrierIds;
                
                const TerminalAndBulkPlans = this.http.get(getTerminalsAndBulkPlantsUrl + frModel.SourceRegionIds.join(','))
                const Customers = this.http.get(getSupplierCustomersUrl);
                const Tabletypes = this.http.get(getTableTypesUrl);
                const Carriers = this.http.get(getCarriersUrl);
                const FuelGroups = this.http.get(getFuelGroupsUrl);
                const SourceRegion = this.http.post(getSourceRegionsUrl, sourceRegionInput);
                const Locations = this.http.get(getCustomerJobsUrl);

                let requiredCalls = [Tabletypes, Customers, Carriers, FuelGroups, SourceRegion, TerminalAndBulkPlans];

                if (this.FrModel.FreightRateRuleType == FreightRateRuleType.P2P) {
                    requiredCalls = [Tabletypes, Customers, Carriers, FuelGroups, SourceRegion, TerminalAndBulkPlans, Locations];
                }                
                return forkJoin(requiredCalls);
            })).subscribe(result => {
                this.IsLoading = false;
                this.freightComponent.TableTypeList = result[0] as DropdownItem[];
                if (this.FrModel.CustomerIds != null && this.FrModel.CustomerIds.length > 0)
                    this.freightComponent.CustomerList = result[1] as DropdownItem[];

                if (this.FrModel.JobIds != null && this.FrModel.JobIds.length > 0)
                    this.freightComponent.LocationList = result[6] as DropdownItem[];

                if (this.FrModel.CarrierIds != null && this.FrModel.CarrierIds.length > 0)
                    this.freightComponent.CarrierList = result[2] as DropdownItem[];

                this.freightComponent.FuelGroupsList = result[3] as DropdownItem[];
                this.freightComponent.SourceRegionList = result[4] as DropdownItem[];

                if (this.FrModel.TerminalsAndBulkPlants != null && this.FrModel.TerminalsAndBulkPlants.length > 0)
                    this.freightComponent.TerminalsAndBulkPlantList = result[5] as DropdownItemExt[];

                
                this.Edit(this.FrModel);

            });
    }

    rerender_destroy(): void {
        if ((this.datatableElement && this.datatableElement.dtInstance)) {
            this.datatableElement.dtInstance.then((dtInstance: DataTables.Api) => {
                dtInstance.destroy();
                //this.DtTrigger.next();
            });
        }
    }

    rerender_trigger(): void {
        if ((this.datatableElement && this.datatableElement.dtInstance)) {
            this.datatableElement.dtInstance.then((dtInstance: DataTables.Api) => {
                //dtInstance.destroy();
                this.DtTrigger.next();
            });
        }
    }   
    private Edit(_fr: FreightRateViewModel) {
        if (this.rForm) {
            if (this.RuleMode != "COPY") {
                this.rForm.get('RuleId').setValue(_fr.Id);
                this.freightComponent.rcForm.get('TableName').setValue(_fr.Name);
                this.IsEditable = false;
            } else {
                this.RuleId = null;
            }
            this.rForm.get('RuleType').setValue(_fr.FreightRateRuleType);
            
            this.freightComponent.rcForm.get('FuelGroupType').setValue(_fr.FuelGroupType);

            this.freightComponent.rcForm.get('TableTypes').setValue(this.freightComponent.TableTypeList.filter(x => x.Id == _fr.TableType));

            if (_fr.TableType != TableType.Master) this.freightComponent.IsMasterSelected = false;

            if (_fr.CustomerIds != null && _fr.CustomerIds != undefined && _fr.CustomerIds.length > 0) {
                this.freightComponent.IsCustomerSelected = true;
                this.freightComponent.IsMasterSelected = false;
                if (this.freightComponent.CustomerList.length > 0 && _fr.CustomerIds.length > 0)
                    this.freightComponent.rcForm.get('Customers').setValue(this.freightComponent.CustomerList.filter(this.IdInComparer(_fr.CustomerIds)));

            }


            if (_fr.JobIds != null && _fr.JobIds != undefined && _fr.JobIds.length > 0) {
               
                if (this.freightComponent.LocationList.length > 0 && _fr.JobIds.length > 0)
                    this.freightComponent.rcForm.get('Locations').setValue(this.freightComponent.LocationList.filter(this.IdInComparer(_fr.JobIds)));

            }

            if (_fr.CarrierIds != null && _fr.CarrierIds != undefined && _fr.CarrierIds.length > 0) {
                this.freightComponent.IsCarrierSelected = true;
                this.freightComponent.IsMasterSelected = false;
                if (this.freightComponent.CarrierList.length > 0)
                    this.freightComponent.rcForm.get('Carriers').setValue(this.freightComponent.CarrierList.filter(this.IdInComparer(_fr.CarrierIds)));

            }

            if (this.freightComponent.SourceRegionList != null && this.freightComponent.SourceRegionList != undefined && _fr.SourceRegionIds != null && _fr.SourceRegionIds != undefined && _fr.SourceRegionIds.length > 0) {
                if (this.freightComponent.SourceRegionList.length > 0 && _fr.SourceRegionIds.length > 0)
                    this.freightComponent.rcForm.get('SourceRegions').setValue(this.freightComponent.SourceRegionList.filter(this.IdInComparer(_fr.SourceRegionIds)));
            }

            if (this.freightComponent.TerminalsAndBulkPlantList != null && this.freightComponent.TerminalsAndBulkPlantList != undefined && _fr.TerminalsAndBulkPlants != null && _fr.TerminalsAndBulkPlants != undefined && _fr.TerminalsAndBulkPlants.length > 0) {
                if (this.freightComponent.TerminalsAndBulkPlantList.length > 0 && _fr.TerminalsAndBulkPlants.length > 0) {
                    this.freightComponent.rcForm.controls.TerminalsAndBulkPlants.setValue(this.freightComponent.TerminalsAndBulkPlantList.filter(this.IdInComparers(_fr.TerminalsAndBulkPlants)));
                    this.freightComponent.IsSourceRegionSelected = true;
                }
            }

            this.freightComponent.rcForm.get('StartDate').setValue(moment(_fr.StartDate).format('MM/DD/YYYY'));

            if (_fr.EndDate != null && _fr.EndDate != undefined) {
                this.freightComponent.rcForm.get('EndDate').setValue(moment(_fr.EndDate).format('MM/DD/YYYY'));
            } 

            if (_fr.FuelGroupIds != null) {

                if (this.freightComponent.FuelGroupsList.length > 0 && _fr.FuelGroupIds.length > 0) {
                    this.freightComponent.rcForm.get('FuelGroups').setValue(this.freightComponent.FuelGroupsList.filter(this.IdInComparer(_fr.FuelGroupIds)));
                    this.SelectedFuelGroups = this.freightComponent.rcForm.get('FuelGroups').value as DropdownItem[];
                }

            }

            this.rerender_destroy();

            this.freightComponent.rcForm.get('StatusId').setValue(_fr.Status);
            if (_fr.FreightRateFuelGroups != null && _fr.FreightRateFuelGroups.length > 0) {

                (<FormArray>this.rForm.get('FuelGroupTable')).clear();
                (<FormArray>this.rForm.get('FuelGroupTable')).push(this.LoadFuelGroupTable(_fr));

            }

            if (_fr.FreightRateRouteTables != null && _fr.FreightRateRouteTables.length > 0) {

                let rTable = (<FormArray>this.rForm.get('RangeTable'))
                rTable.clear();
                const ftable = _fr.FreightRateRouteTables;
                this.groupBy(ftable, ftable => ftable.StartQuantity).forEach(res => {
                    (<FormArray>this.rForm.get('RangeTable')).push(this.LoadRouteTable(false, res, _fr.Id))
                });
                rTable.at(rTable.length - 1).get('IsLast').patchValue(true);
                this.freightComponent.IsLoaded = true;
            }
            else if (_fr.FreightRateRangeTables != null && _fr.FreightRateRangeTables.length > 0) {
                this.generateRangeTable(_fr.FreightRateRangeTables);
            }
            else if (_fr.FreightRatePtoPTables != null && _fr.FreightRatePtoPTables.length > 0) {
                this.generateP2PTable(_fr.FreightRatePtoPTables);
            }

            this.rerender_trigger();

        }

        if (this.RuleMode == "VIEW") {
            this.disableInputControls = true;
            this.freightComponent.disableInputControls = true;
        }
        
    }

    private groupBy(list, keyGetter) {
        const map = new Map();
        list.forEach((item) => {
            const key = keyGetter(item);
            const collection = map.get(key);
            if (!collection) {
                map.set(key, [item]);
            } else {
                collection.push(item);
            }
        });
        return map;
    }

    private groupByWithMultipleProperty(array, f) {
        let groups = {};
        array.forEach(function (o) {
            var group = JSON.stringify(f(o));
            groups[group] = groups[group] || [];
            groups[group].push(o);
        });
        return Object.keys(groups).map(function (group) {
            return groups[group];
        })
    }

    private generateP2PTable(fRange: FreightRatePtoPTable[], Id?: number) {
        this.freightComponent.isLoadingSubject.next(true);

        if (fRange != null && fRange.length > 0) {

            let rTable = (<FormArray>this.rForm.get('RangeTable'))
            rTable.clear();
            let ftable = this.groupByWithMultipleProperty(fRange, function (item) {
                return [item.TerminalAndBulkPlantName, item.LocationName, item.LocationAddress, item.LaneID, item.AssumedMiles];
            });
            let sameName = "";
            ftable.forEach(res => {                
                (<FormArray>this.rForm.get('RangeTable')).push(this.LoadP2PTable(sameName == "" || sameName != res[0].TerminalAndBulkPlantName?true:false, res, Id));
                sameName = res[0].TerminalAndBulkPlantName;
            });
            
        }
        this.freightComponent.ShowMessage = false;
        this.freightComponent.isLoadingSubject.next(false);
        
        
        this.freightComponent.IsLoaded = true;
    }


    private generateRangeTable(fRange: FreightRateRangeTableViewModel[],Id?:number) {
        this.freightComponent.isLoadingSubject.next(true);
        if (fRange != null && fRange.length > 0) {

            let rTable = (<FormArray>this.rForm.get('RangeTable'))
            rTable.clear();
            const ftable = fRange;
            this.groupBy(ftable, ftable => ftable.UptoQuantity).forEach(res => {
                (<FormArray>this.rForm.get('RangeTable')).push(this.LoadRangeTable(false, res, Id))
            });
            rTable.at(rTable.length - 1).get('IsLast').patchValue(true);
        }

        this.freightComponent.ShowMessage = false;
        this.freightComponent.isLoadingSubject.next(false);
        this.freightComponent.IsLoaded = true;
    }

    private LoadP2PTable(IsLast: boolean, rt: FreightRatePtoPTable[], Id: number): FormGroup {
        return this._fb.group({
            Id: this._fb.control(Id),
            TerminalAndBulkPlantName: this._fb.control(rt[0].TerminalAndBulkPlantName),
            TerminalId: this._fb.control(rt[0].TerminalId),
            BulkPlantId: this._fb.control(rt[0].BulkPlantId),
            LocationName: this._fb.control(rt[0].LocationName),
            LocationAddress: this._fb.control(rt[0].LocationAddress),
            JobId: this._fb.control(rt[0].JobId),
            LaneID: this._fb.control(rt[0].LaneID),
            AssumedMiles: this._fb.control(rt[0].AssumedMiles, [Validators.required, Validators.pattern(this.decimalSupportedRegx)]),
            IsLaneRequired: this._fb.control(rt[0].IsLaneRequired),
            IsLast: this._fb.control(IsLast),
            group: this.LoadGroupInP2PTable(rt)
        });
    }
    private LoadRangeTable(IsLast: boolean, rt: FreightRateRangeTableViewModel[], Id: number): FormGroup {
        return this._fb.group({
            Id: this._fb.control(Id),
            UptoQuantity: this._fb.control(rt[0].UptoQuantity, [Validators.required, Validators.pattern(this.intGreaterThanZeroRegx)]),
            IsLast: this._fb.control(IsLast),
            group: this.LoadGroupInRangeTable(rt)
        });
    }

    private LoadGroupInP2PTable(rt: FreightRatePtoPTable[]): FormArray {
        let fg = new FormArray([]);
        rt.forEach(x => {
            fg.push(this._fb.group({
                FuelGroupId: new FormControl(x.FuelGroupId),
                FuelGroupName: new FormControl(x.FuelGroupName),
                RateValue: new FormControl(x.RateValue, [Validators.required, Validators.pattern(this.decimalSupportedRegx)])
            }));
        });
        return fg;
    }

    private LoadGroupInRangeTable(rt: FreightRateRangeTableViewModel[]): FormArray {
        let fg = new FormArray([]);
        rt.forEach(x => {
            fg.push(this._fb.group({
                FuelGroupId: new FormControl(x.FuelGroupId),
                RateValue: new FormControl(x.RateValue, [Validators.required, Validators.pattern(this.decimalSupportedRegx)])
            }));
        });
        return fg;
    }

    private LoadRouteTable(IsLast: boolean, rt: FreightRateRouteTableViewModel[], Id: number): FormGroup {
        return this._fb.group({
            Id: this._fb.control(Id),
            StartQuantity: this._fb.control(rt[0].StartQuantity, [Validators.required, Validators.pattern(this.intGreaterThanZeroRegx)]),
            EndQuantity: this._fb.control(rt[0].EndQuantity, [Validators.required, Validators.pattern(this.decimalSupportedRegx)]),
            IsLast: this._fb.control(IsLast),
            group: this.LoadGroupInRouteTable(rt)
        });
    }

    private LoadGroupInRouteTable(rt: FreightRateRouteTableViewModel[]): FormArray {
        let fg = new FormArray([]);
        rt.forEach(x => {
            fg.push(this._fb.group({
                FuelGroupId: new FormControl(x.FuelGroupId),
                RateValue: new FormControl(x.RateValue, [Validators.required, Validators.pattern(this.decimalSupportedRegx)])
            }));
        });
        return fg;
    }


    private LoadFuelGroupTable(_fr: FreightRateViewModel): FormGroup {
        if (_fr.FreightRateRuleType == FreightRateRuleType.Route) {
            return this._fb.group({
                group: this.LoadGroupInFuelGroupTable(_fr)
            });
        }
        else if (_fr.FreightRateRuleType == FreightRateRuleType.Range || _fr.FreightRateRuleType == FreightRateRuleType.P2P) {
            let selectedFuelGroup: DropdownItem[] = [];
            if (_fr.FreightRateCalcPrefFuelGroupId != null) {                
                selectedFuelGroup=this.freightComponent.FuelGroupsList.filter(res => res.Id == _fr.FreightRateCalcPrefFuelGroupId);
            }
            return this._fb.group({
                group: this.LoadGroupInFuelGroupTable(_fr),
                FreightRateCalcPreferenceType: new FormControl(_fr.FreightRateCalcPreferenceType),
                FuelGroups: new FormControl(selectedFuelGroup, [Validators.required]),
                MixLoadMinValue: new FormControl(_fr.MixLoadMinValue, [Validators.required, Validators.pattern(this.decimalSupportedRegx)])
            });
        }
    }

    private LoadGroupInFuelGroupTable(_fr: FreightRateViewModel): FormArray {
        let fg = new FormArray([]);
        _fr.FreightRateFuelGroups.forEach(x => {
            fg.push(this._fb.group({
                FuelGroupId: new FormControl(x.FuelGroupId),
                MinQuantity: new FormControl(x.MinQuantity, [Validators.required, Validators.pattern(this.decimalSupportedRegx)])
            }));
        });
        return fg;
    }




    private createForm(ruleType: number) {
        if (ruleType == FreightRateRuleType.Route) {
            this.rForm = this._fb.group({
                RuleId: new FormControl(''),
                RuleType: new FormControl(ruleType),
                FuelGroupTable: this._fb.array([
                ]),
                RangeTable: this._fb.array([
                ])

            });
        }
        else if (ruleType == FreightRateRuleType.Range) {

            this.rForm = this._fb.group({
                RuleId: new FormControl(''),
                RuleType: new FormControl(ruleType),
                FuelGroupTable: this._fb.array([
                ]),
                RangeTable: this._fb.array([
                ])
            });
        }

        else if (ruleType == FreightRateRuleType.P2P) {

            this.rForm = this._fb.group({
                RuleId: new FormControl(''),
                RuleType: new FormControl(ruleType),
                FuelGroupTable: this._fb.array([
                ]),
                RangeTable: this._fb.array([
                ])
            });           
        }

    }

    public changeViewType(value) {
        this.viewType = value;
    }
    public RuleTypeTypeChange(ruleType: number): void {
        this.freightRateRuleType = ruleType;
        this.refreshUI(ruleType);
    }
    private refreshUI(ruleType: number) {
        this.freightComponent.IsLoaded = false;
        this.freightComponent.isLoadingSubject.next(true);
        this.RuleId = null;
        this.IsEditable = true;
        this.createForm(ruleType);
        this.freightComponent.rcForm.get('TableTypes').setValue(this.freightComponent.TableTypeList.filter(x => x.Id == TableType.Master));

        this.freightComponent.SourceRegionList = [];
        this.freightComponent.TerminalsAndBulkPlantList = [];
        
        this.freightComponent.rcForm.get('TableName').patchValue('');
        this.freightComponent.rcForm.get('StartDate').setValue(moment(moment(new Date()).toDate()).format('MM/DD/YYYY'));
        this.freightComponent.FuelGroupsList = [];
        this.freightComponent.rcForm.get('FuelGroups').patchValue([]);

        if (ruleType != FreightRateRuleType.Range) {
            this.AddRemoveValidations(null, [this.freightComponent.rcForm.controls.RangeStartValue]);
            this.AddRemoveValidations(null, [this.freightComponent.rcForm.controls.RangeEndValue]);
            this.AddRemoveValidations(null, [this.freightComponent.rcForm.controls.RangeInterval]);
        } else {
            this.AddRemoveValidations([this.freightComponent.rcForm.controls.RangeStartValue],null );
            this.AddRemoveValidations([this.freightComponent.rcForm.controls.RangeEndValue],null);
            this.AddRemoveValidations([this.freightComponent.rcForm.controls.RangeInterval],null);
        }

        if (ruleType != FreightRateRuleType.P2P) {

            this.freightComponent.rcForm.get('TableTypes').setValue(this.freightComponent.TableTypeList.filter(x => x.Id == TableType.Master));
            this.freightComponent.onTableTypeSelect({ Id: TableType.Master });           
            this.DtTrigger.next();
           
        } else {
            this.freightComponent.rcForm.get('TableTypes').setValue(this.freightComponent.TableTypeList.filter(x => x.Id == TableType.Customer));
            this.freightComponent.onTableTypeSelect({ Id: TableType.Customer });
        }
        this.freightComponent.IsLoaded = true;
        this.freightComponent.isLoadingSubject.next(false);
        //this.rerender();
    }

    private disableAllControl(formGroup: FormGroup) {
        (<any>Object).values(formGroup.controls).forEach(control => {

            control.disable();
            if (control.controls) {
                this.disableAllControl(control);
            }
        });
    }

    private markFormGroupTouched(formGroup: FormGroup) {
        (<any>Object).values(formGroup.controls).forEach(control => {

            control.markAsTouched();
            if (control.controls) {
                this.markFormGroupTouched(control);
            }
        });
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

    private clearAllValidations(formGroup: FormGroup) {
        (<any>Object).values(formGroup.controls).forEach(control => {
            control.clearValidators();            
            control.updateValueAndValidity();
            control.markAsTouched();
            if (control.controls) {
                this.clearAllValidations(control);
            }
        });
    }
    public onSubmit(freightTableStatus: number): void {
        this.IsLoading = true;
        if (freightTableStatus == FreightTableStatus.Draft) {
            if (this.rForm.get('RuleId').value != "") {
                Declarations.msgerror("Not allowed. " + this.freightComponent.rcForm.get('TableName').value + " is in edit mode.", undefined, undefined);
                this.IsLoading = false;
                return;
            }
            this.clearAllValidations(this.rForm); // clear all validation
            this.AddRemoveValidations([this.freightComponent.rcForm.controls.TableName], null); // minimum validation for draft
            if (this.freightComponent.rcForm.valid) this.Save(freightTableStatus);
            this.IsLoading = false;
            return;
           }
        this.freightComponent.ValidateOnSubmit(freightTableStatus);

        if (this.RuleMode == "COPY" && !this.freightComponent.rcForm.get('TableName').valid) {
            this.freightComponent.ShowMessage = false;
        }
        // clear validation of last row and second column of RangeTable
        let rTable = (<FormArray>this.rForm.get('RangeTable'));
        if (rTable.length > 0 && (+this.rForm.get('RuleType').value) == FreightRateRuleType.Route) {
            rTable.at(rTable.length - 1).get('EndQuantity').clearValidators();
            rTable.at(rTable.length - 1).get('EndQuantity').updateValueAndValidity();
            rTable.at(rTable.length - 1).get('EndQuantity').markAsTouched();
        }
        else if (rTable.length > 0 && (+this.rForm.get('RuleType').value) == FreightRateRuleType.Range) {
            this.AddRemoveValidations(null,
                [this.freightComponent.rcForm.get('RangeStartValue'), this.freightComponent.rcForm.get('RangeEndValue'), this.freightComponent.rcForm.get('RangeInterval')]);

            let selectedFuelGroups = this.freightComponent.rcForm.get('FuelGroups').value as DropdownItem[];
            if (selectedFuelGroups.length == 1) {
                this.AddRemoveValidations(null,
                    [(<FormArray>this.rForm.get('FuelGroupTable')).at(0).get('MixLoadMinValue'), (<FormArray>this.rForm.get('FuelGroupTable')).at(0).get('FuelGroups')]);
            }
            this.ClearDuplicateUpToQuantity();
        }
        else if (rTable.length > 0 && (+this.rForm.get('RuleType').value) == FreightRateRuleType.P2P) {
            let selectedFuelGroups = this.freightComponent.rcForm.get('FuelGroups').value as DropdownItem[];
            if (selectedFuelGroups.length == 1) {
                this.AddRemoveValidations(null,
                    [(<FormArray>this.rForm.get('FuelGroupTable')).at(0).get('MixLoadMinValue'), (<FormArray>this.rForm.get('FuelGroupTable')).at(0).get('FuelGroups')]);
            }
        }

        if (rTable.length > 0 &&
            (+this.rForm.get('RuleType').value == FreightRateRuleType.P2P || +this.rForm.get('RuleType').value == FreightRateRuleType.Range) &&
            (((<FormArray>this.rForm.get('FuelGroupTable')).at(0).get('FreightRateCalcPreferenceType').value != 1) ||
            (<FormArray>this.rForm.get('FuelGroupTable')).at(0).get('MixLoadMinValue').value==0)) {

            this.AddRemoveValidations(null,
                [(<FormArray>this.rForm.get('FuelGroupTable')).at(0).get('FuelGroups')]);
        }
        
        this.markFormGroupTouched(this.rForm)

        this.IsLoading = false;
        if (this.rForm.valid && this.freightComponent.rcForm.valid && !this.freightComponent.ShowMessage) {
            this.Save(freightTableStatus);
        }


    }
    private IsGroupExist(table: string): number {
        let fgTable = (<FormArray>this.rForm.get(table));
        let fgtableArray = fgTable.value as any[];
        return fgtableArray.findIndex((s) => s['group']);
    }

    private IsDuplicateUpToQuantity():boolean {
        let UptoQuantity = [];
        let rangeTable = (<FormArray>this.rForm.get('RangeTable'));
        let isExist = this.IsGroupExist("RangeTable");
        let flag = false;
        if (isExist != -1) {
            var rT = (<FormArray>this.rForm.get('RangeTable')).length - 1;
            for (let i = 0; i <= rT; i++) {

                if (!UptoQuantity.includes(+(rangeTable.at(i).get('UptoQuantity').value))) {
                    UptoQuantity.push(+(rangeTable.at(i).get('UptoQuantity').value));

                } else {
                    rangeTable.at(i).get('UptoQuantity').setErrors({ DuplicateEntry: true });
                    rangeTable.at(i).get('UptoQuantity').markAsTouched();
                    flag = true;
                }
            }
        }
        return flag;
    }
    private ClearDuplicateUpToQuantity() {
        let rangeTable = (<FormArray>this.rForm.get('RangeTable'));
        let isExist = this.IsGroupExist("RangeTable");
        if (isExist != -1) {
            var rT = (<FormArray>this.rForm.get('RangeTable')).length - 1;
            for (let i = 0; i <= rT; i++) {
                if (rangeTable.at(i).get('UptoQuantity')?.errors?.DuplicateEntry) {
                    rangeTable.at(i).get('UptoQuantity').markAsTouched();
                }
            }
        }
    }

    private Save(freightTableStatus: FreightTableStatus): void {
        this.IsLoading = true;
        
        var saveModel = new FreightRateViewModel();
        saveModel.Id = this.rForm.get('RuleId').value;
        saveModel.Name = this.freightComponent.rcForm.get('TableName').value;
        saveModel.Status = freightTableStatus;
        saveModel.FuelGroupType = this.freightComponent.rcForm.get('FuelGroupType').value;
        saveModel.FreightRateRuleType = this.rForm.get('RuleType').value;

        saveModel.StartDate = this.freightComponent.rcForm.get('StartDate').value;

        if (this.freightComponent.rcForm.get('EndDate').value != null && this.freightComponent.rcForm.get('EndDate').value != undefined) {
            saveModel.EndDate = this.freightComponent.rcForm.get('EndDate').value;
        }

        saveModel.TableType = this.freightComponent.rcForm.get('TableTypes').value[0].Id;

        
        this.freightComponent.rcForm.get('Customers').value.forEach(res => saveModel.CustomerIds.push(res.Id));        
        this.freightComponent.rcForm.get('Carriers').value.forEach(res => saveModel.CarrierIds.push(res.Id));

        this.freightComponent.rcForm.get('SourceRegions').value.forEach(res => saveModel.SourceRegionIds.push(res.Id));

        saveModel.TerminalsAndBulkPlants = this.freightComponent.rcForm.get('TerminalsAndBulkPlants').value;
        let isExist = 0;
        isExist = this.IsGroupExist("FuelGroupTable");
        if (isExist != -1) {
            var gList = (<FormArray>((<FormArray>this.rForm.get('FuelGroupTable')).at(0).get('group'))).length - 1;
            var fgT = (<FormArray>this.rForm.get('FuelGroupTable')).length - 1
            for (let i = 0; i <= fgT; i++) {
                for (let j = 0; j <= gList; j++) {
                    let group = new FreightRateFuelGroupViewModel();
                    group.FuelGroupId = (<FormArray>((<FormArray>this.rForm.get('FuelGroupTable')).at(i).get('group'))).at(j).get('FuelGroupId').value;
                    group.MinQuantity = (<FormArray>((<FormArray>this.rForm.get('FuelGroupTable')).at(i).get('group'))).at(j).get('MinQuantity').value;
                    if (+this.rForm.get('RuleType').value == FreightRateRuleType.Range || +this.rForm.get('RuleType').value == FreightRateRuleType.P2P) {
                        saveModel.MixLoadMinValue = (<FormArray>this.rForm.get('FuelGroupTable')).at(i).get('MixLoadMinValue').value;

                        saveModel.FreightRateCalcPreferenceType = (<FormArray>this.rForm.get('FuelGroupTable')).at(i).get('FreightRateCalcPreferenceType').value;
                        let selectedFuelGroup: DropdownItem[] = (<FormArray>this.rForm.get('FuelGroupTable')).at(i).get('FuelGroups').value;
                        saveModel.FreightRateCalcPrefFuelGroupId = selectedFuelGroup.length == 1 ? selectedFuelGroup[0].Id : null;


                    }
                    saveModel.FreightRateFuelGroups.push(group);
                }
            }
        }
        isExist = this.IsGroupExist("RangeTable");
        if (isExist!=-1) {
            gList = (<FormArray>((<FormArray>this.rForm.get('RangeTable')).at(0).get('group'))).length - 1;
            var rT = (<FormArray>this.rForm.get('RangeTable')).length - 1;
            let rangeTable = (<FormArray>this.rForm.get('RangeTable'));
            if (+this.rForm.get('RuleType').value == FreightRateRuleType.Route) {
                for (let i = 0; i <= rT; i++) {
                    for (let j = 0; j <= gList; j++) {

                        let group = new FreightRateRouteTableViewModel();
                        group.FuelGroupId = (<FormArray>(rangeTable.at(i).get('group'))).at(j).get('FuelGroupId').value;
                        group.StartQuantity = rangeTable.at(i).get('StartQuantity').value;
                        if (rangeTable.at(i).get('IsLast').value != true) {
                            group.EndQuantity = rangeTable.at(i).get('EndQuantity').value;
                        } else {
                            group.EndQuantity = null;
                        }
                        group.RateValue = (<FormArray>(rangeTable.at(i).get('group'))).at(j).get('RateValue').value;
                        saveModel.FreightRateRouteTables.push(group);
                    }
                }
            }
            else if (+this.rForm.get('RuleType').value == FreightRateRuleType.Range) {

                if (this.IsDuplicateUpToQuantity()) {
                    this.IsLoading = false;
                    return;
                }
                for (var i = 0; i <= rT; i++) {
                    for (var j = 0; j <= gList; j++) {
                        let group = new FreightRateRangeTableViewModel();
                        group.FuelGroupId = (<FormArray>(rangeTable.at(i).get('group'))).at(j).get('FuelGroupId').value;
                        group.UptoQuantity = rangeTable.at(i).get('UptoQuantity').value;                        
                        group.RateValue = (<FormArray>(rangeTable.at(i).get('group'))).at(j).get('RateValue').value;
                        saveModel.FreightRateRangeTables.push(group);
                    }
                }
            }
            else if (+this.rForm.get('RuleType').value == FreightRateRuleType.P2P) {

                for (let i = 0; i <= rT; i++) {
                    for (let j = 0; j <= gList; j++) {
                        let group = new FreightRatePtoPTable();
                        group.FuelGroupId = (<FormArray>(rangeTable.at(i).get('group'))).at(j).get('FuelGroupId').value;
                        group.FuelGroupName = (<FormArray>(rangeTable.at(i).get('group'))).at(j).get('FuelGroupName').value;
                        group.RateValue = (<FormArray>(rangeTable.at(i).get('group'))).at(j).get('RateValue').value;

                        group.AssumedMiles = rangeTable.at(i).get('AssumedMiles').value;
                        group.IsLaneRequired = rangeTable.at(i).get('IsLaneRequired').value;
                        group.LaneID = rangeTable.at(i).get('LaneID').value;
                        group.LocationAddress = rangeTable.at(i).get('LocationAddress').value;
                        group.LocationName = rangeTable.at(i).get('LocationName').value;
                        group.JobId = rangeTable.at(i).get('JobId').value;
                        group.TerminalAndBulkPlantName = rangeTable.at(i).get('TerminalAndBulkPlantName').value;
                        group.BulkPlantId = rangeTable.at(i).get('BulkPlantId').value;
                        group.TerminalId = rangeTable.at(i).get('TerminalId').value;
                        saveModel.FreightRatePtoPTables.push(group);
                    }
                }

            }
        }
        if (this.rForm.get('RuleId').value != "") {
            this.freightRateRulesService.updateFreightRate(saveModel)
                .subscribe((response: any) => {
                    this.ServiceResponse = response;
                    if (response != null && response.StatusCode == 0) {
                        let message = " edited";
                        if (saveModel.Status == FreightTableStatus.Draft) {
                            message = " saved draft"
                        }
                        Declarations.msgsuccess(saveModel.Name + message + " successfully.", undefined, undefined);
                        this.IsLoading = false;
                        this.changeToViewTab();
                    }
                    else {
                        this.IsLoading = false;
                        Declarations.msgerror(response == null || response.StatusMessage == null ? 'Failed' : response.StatusMessage, undefined, undefined);
                    }
                });
        } else {
            this.freightRateRulesService.createFreightRate(saveModel)
                .subscribe((response: any) => {
                    this.ServiceResponse = response;
                    if (response != null && response.StatusCode == 0) {
                        let message = "";
                        if (saveModel.Status == FreightTableStatus.Published) {
                            message = " created"
                        }
                        else if (saveModel.Status == FreightTableStatus.Draft) {
                            message = " saved draft"
                        }
                        Declarations.msgsuccess(saveModel.Name + message + " successfully." , undefined, undefined);
                        this.IsLoading = false;
                        this.changeToViewTab();
                    }
                    else {
                        this.IsLoading = false;
                        Declarations.msgerror(response == null || response.StatusMessage == null ? 'Failed' : response.StatusMessage, undefined, undefined);
                    }
                });
        }
    }

    public changeToViewTab() {
        this.freightRateRulesService.onSelectedTabChanged.next(2);
    }

    public clearForm() {
        this.disableInputControls = false;
        this.freightComponent.disableInputControls = false;
        this.refreshUI(FreightRateRuleType.Range);
    }

    public onCancel(): void {
        if (this.RuleMode == "VIEW") {
            this.disableInputControls = false;
            this.freightComponent.disableInputControls = false;
            this.RuleId = null;
        }
        if (this.RuleId != null) {
            this.changeToViewTab();
        }
        else {
            this.refreshUI(FreightRateRuleType.Range);
        }
        
    }

    public ngOnDestroy(): void {
        this.DtTrigger.unsubscribe();
    }

    private IdInComparer(otherArray) {
        return function (current) {
            return otherArray.filter(function (other) {
                return other == current.Id
            }).length == 1;
        }
    }
    IdInComparers(otherArray) {
        return function (current) {
            return otherArray.filter(function (other) {
                return other.Id == current.Id
            }).length == 1;
        }
    }


    public GetControlNames(formGroup: FormGroup): string[] {
        var ctrlName: string[] = [];
        (<any>Object).keys(formGroup.controls).forEach((key: string) => {
            const abstractControl = formGroup.get(key);
            ctrlName.push(key);
            if (abstractControl instanceof FormGroup) {
                this.GetControlNames(abstractControl);
            }
        });
        return ctrlName;
    }
   
    public onStartQuantityLostFocus(index: number): void {

        let rTable = (<FormArray>this.rForm.get('RangeTable'));
        let StartQuantity = rTable.at(index).get('StartQuantity').value;
        let UpperRowEndQuantity;
        if (index > 0) {           
            UpperRowEndQuantity = rTable.at(index - 1).get('EndQuantity').value;
            let SameRowEndQuantity = rTable.at(index).get('EndQuantity').value;
            if (+StartQuantity <= +UpperRowEndQuantity) {
                rTable.at(index).get('StartQuantity').setErrors({ InvalidRange: true });
                rTable.at(index).get('StartQuantity').markAsTouched();
            }
            else if (+StartQuantity >= SameRowEndQuantity)
            {
                rTable.at(index).get('StartQuantity').setErrors({ InvalidRange: true });
                rTable.at(index).get('StartQuantity').markAsTouched();
            }
            else {
                rTable.at(index).get('StartQuantity').setErrors(null);
                rTable.at(index).get('StartQuantity').markAsTouched();
            }
        } else {
            UpperRowEndQuantity = rTable.at(index).get('EndQuantity').value;
            if (UpperRowEndQuantity) {
                if (+StartQuantity >= +UpperRowEndQuantity) {
                    rTable.at(index).get('StartQuantity').setErrors({ InvalidRange: true });
                    rTable.at(index).get('StartQuantity').markAsTouched();
                }
                else {
                    rTable.at(index).get('StartQuantity').setErrors(null);
                    rTable.at(index).get('StartQuantity').markAsTouched();
                }
            }
        }
        
    }
        

    public onEndQuantityLostFocus(index: number): void {
        let rTable = (<FormArray>this.rForm.get('RangeTable'));
        let StartQuantity = rTable.at(index).get('StartQuantity').value;
        let EndQuantity = rTable.at(index).get('EndQuantity').value;
        if (!isNaN(+EndQuantity) && !isNaN(+StartQuantity)) {
            if (+StartQuantity >= +EndQuantity) {
                rTable.at(index).get('EndQuantity').setErrors({ InvalidRange: true });
                rTable.at(index).get('EndQuantity').markAsTouched();
            } else {
                rTable.at(index).get('EndQuantity').setErrors(null);
                rTable.at(index).get('EndQuantity').markAsTouched();
            }
        }
        this.onStartQuantityLostFocus(index);
    }
    public onStartQuantityKeyPress(event:any,index: number): void {
        if (index == 0 && !isNaN(event.data)) {
            var gList = (<FormArray>((<FormArray>this.rForm.get('FuelGroupTable')).at(0).get('group'))).length - 1;
            var fgT = (<FormArray>this.rForm.get('FuelGroupTable')).length - 1;
            let StartQuantity = (<FormArray>this.rForm.get('RangeTable')).at(index).get('StartQuantity').value;
            
            for (let i = 0; i <= fgT; i++) {
                for (let j = 0; j <= gList; j++) {
                    (<FormArray>((<FormArray>this.rForm.get('FuelGroupTable')).at(i).get('group'))).at(j).get('MinQuantity').patchValue(+StartQuantity);
                }
            }
        }

    }
   
    public onEndQuantityKeyPress(index:number): void {
        let rTable = (<FormArray>this.rForm.get('RangeTable'));
        let EndQuantity = rTable.at(index).get('EndQuantity').value;        
        if (!isNaN(+EndQuantity)) {            
            if (+EndQuantity) {
                rTable.at(index + 1).get('StartQuantity').patchValue((+EndQuantity + 1));
            }
        }
    }

    public AddRange(srNmber: number) {
        this.rerender_destroy();
        if ((<FormArray>this.rForm.get('FuelGroupTable')).length == 0)
        {
            return;
        };
        let rTable = (<FormArray>this.rForm.get('RangeTable'));
        let EndQuantity = "";
        if (rTable.length > 0 && +this.rForm.get('RuleType').value == FreightRateRuleType.Route) {
            rTable.at(rTable.length - 1).get('IsLast').patchValue(false);
            EndQuantity = rTable.at(rTable.length - 2).get('EndQuantity').value;
            if (EndQuantity != "") rTable.at(rTable.length - 1).get('StartQuantity').patchValue((+EndQuantity + 1));
            rTable.push(this.CreateRangeTable(true, +this.rForm.get('RuleType').value));
        }
        else if (rTable.length > 0 && +this.rForm.get('RuleType').value == FreightRateRuleType.Range) {
            let UptoQuantity = rTable.at(srNmber).get('UptoQuantity').value;
            rTable.insert(srNmber + 1, this.CreateRangeTable(false, +this.rForm.get('RuleType').value));
            /*if (UptoQuantity != "") rTable.at(srNmber + 1).get('UptoQuantity').patchValue(0);*/
            rTable.at(rTable.length - 1).get('IsLast').patchValue(true);
        }
        // no manaul row add for P2P
        else {
           
            if (+this.rForm.get('RuleType').value == FreightRateRuleType.Route) {
                rTable.push(this.CreateRangeTable(false, + this.rForm.get('RuleType').value));
                rTable.push(this.CreateRangeTable(true, +this.rForm.get('RuleType').value));
            }
            else if (+ this.rForm.get('RuleType').value == FreightRateRuleType.Range) {
                this.markFormGroupTouched(this.rForm)
                if (!this.freightComponent.rcForm.get('RangeStartValue').valid
                    || !this.freightComponent.rcForm.get('RangeStartValue').valid
                    || !this.freightComponent.rcForm.get('RangeInterval').valid) { return }

                let start = this.freightComponent.rcForm.get('RangeStartValue').value;
                let stop = this.freightComponent.rcForm.get('RangeEndValue').value;
                let step = this.freightComponent.rcForm.get('RangeInterval').value;
                this.GenerateRange(+start, +stop, +step).forEach(res => {
                    rTable.push(this.CreateRangeTable(false, + this.rForm.get('RuleType').value, Math.round(res)));
                });
            }
            else if (+ this.rForm.get('RuleType').value == FreightRateRuleType.P2P) {

                if (!this.freightComponent.rcForm.get('TerminalsAndBulkPlants').valid
                    || !this.freightComponent.rcForm.get('Locations').valid) { return }
                
                let trbls = this.freightComponent.rcForm.get('TerminalsAndBulkPlants').value as DropdownItemExt[];
                function sortForTerminalsAndBulkPlants(a, b) {
                    if (a.Name < b.Name) {
                        return -1;
                    }
                    if (a.Name > b.Name) {
                        return 1;
                    }
                    return 0;
                }
                trbls.sort(sortForTerminalsAndBulkPlants);
                let sLocations = this.freightComponent.rcForm.get('Locations').value as DropdownItem[];
                let JobIds: number[] = [];
                sLocations.forEach(s => JobIds.push(s.Id));
                let locs = this.freightComponent.LocationList.filter(this.IdInComparer(JobIds));
                for (var i = 0; i < trbls.length; i++) {

                    for (var j = 0; j < locs.length; j++) {
                        rTable.push(this.CreateRangeTable(false, + this.rForm.get('RuleType').value));                        
                        rTable.at(rTable.length - 1).get("TerminalAndBulkPlantName").patchValue(trbls[i].Name);
                        if (trbls[i].Id.startsWith('Terminals_')) {
                            rTable.at(rTable.length - 1).get("TerminalId").patchValue(trbls[i].Id.split("Terminals_")[1]);
                            rTable.at(rTable.length - 1).get("LaneID").patchValue(trbls[i].Id.split("Terminals_")[1].concat(" " + locs[j].Id.toString()));
                        }
                        else if (trbls[i].Id.startsWith('BulkPlants_')) {                            
                            rTable.at(rTable.length - 1).get("BulkPlantId").patchValue(trbls[i].Id.split("BulkPlants_")[1]);
                            rTable.at(rTable.length - 1).get("LaneID").patchValue(trbls[i].Id.split("BulkPlants_")[1].concat(" " + locs[j].Id.toString()));
                        }                       

                        rTable.at(rTable.length - 1).get("LocationName").patchValue(locs[j].Name);
                        rTable.at(rTable.length - 1).get("LocationAddress").patchValue(locs[j].Code);
                        rTable.at(rTable.length - 1).get("JobId").patchValue(locs[j].Id);
                        
                        if (j == 0) {
                            rTable.at(rTable.length - 1).get("IsLast").patchValue(true);
                        }
                    }
                    
                    
                }
                //this.DtTrigger.next();
                this.rerender_trigger();
                
            }
        }

    }

    private GenerateRange(start:number, end:number, step:number = 0) {
        let output = [];
        if (typeof end === 'undefined') {
            end = start;
            start = 0;
        }
        for (let i = start; i <= end; i += step) {
            output.push(i);
        }
        return output;
    }
    public onEndQuantityChange(textInput : any, index : number) {        
        if (this.intGreaterThanZeroRegx.test(textInput)) {
           
            (<FormArray>this.rForm.get('RangeTable')).at(index + 1).get('StartQuantity').patchValue((+textInput + 1));
        }
        else {
            (<FormArray>this.rForm.get('RangeTable')).at(index + 1).get('StartQuantity').patchValue('');
        }
        
    }
    private CreateRangeTable(IsLast: boolean, ruleType: number, upTo? : number): FormGroup {
        if (ruleType ==FreightRateRuleType.Route) {
            return this._fb.group({
                Id: this._fb.control(''),
                StartQuantity: this._fb.control(IsLast ? '' : 1, [Validators.required, Validators.pattern(this.intGreaterThanZeroRegx)]),
                EndQuantity: this._fb.control('', [Validators.required, Validators.pattern(this.decimalSupportedRegx)]),
                IsLast: this._fb.control(IsLast),
                group: this.addGroupInRangeTable()
            });
        }
        else if (ruleType == FreightRateRuleType.Range) {
            return this._fb.group({
                Id: this._fb.control(''),
                UptoQuantity: this._fb.control(upTo, [Validators.required, Validators.pattern(this.intGreaterThanZeroRegx)]),
                IsLast: this._fb.control(IsLast),
                group: this.addGroupInRangeTable()
            });
        }

        else if (ruleType == FreightRateRuleType.P2P) {
            return this._fb.group({
                Id: this._fb.control(''),
                TerminalAndBulkPlantName:this._fb.control(''),
                TerminalId:this._fb.control(''),
                BulkPlantId:this._fb.control(''),
                LocationName:this._fb.control(''),
                LocationAddress:this._fb.control(''),
                JobId:this._fb.control(''),
                LaneID:this._fb.control(''),
                AssumedMiles: this._fb.control('', [Validators.required, Validators.pattern(this.decimalSupportedRegx)]),
                IsLaneRequired:this._fb.control(true),
                IsLast: this._fb.control(IsLast),
                group: this.addGroupInRangeTable()
            });
        }
    }
   
    public addGroupInRangeTable(): FormArray {
        let fg = new FormArray([]);
        this.freightComponent.rcForm.get('FuelGroups').value.forEach(x => {
            fg.push(this._fb.group({
                FuelGroupId: new FormControl(x.Id),
                FuelGroupName: new FormControl(x.Name),
                RateValue: new FormControl('', [Validators.required, Validators.pattern(this.decimalSupportedRegx)])
            }));
        });
        return fg;
    }

    public generateOutput() {
        this.freightComponent.generateTable();        
    }

    public getOutput($event) { //event which is coming from freight component
        (<FormArray>this.rForm.get('RangeTable')).clear();
        this.AddRange(0);
    }
    public onFuelGroupChange($event) { //event which is coming from freight component
        (<FormArray>this.rForm.get('FuelGroupTable')).clear();

        if ((+this.rForm.get('RuleType').value) == FreightRateRuleType.Range) {
            this.freightComponent.rcForm.get('RangeStartValue').patchValue('');
            this.freightComponent.rcForm.get('RangeEndValue').patchValue('');
            this.freightComponent.rcForm.get('RangeInterval').patchValue('');
        }

        this.AddFuelGroup(+this.rForm.get('RuleType').value);
      
    }

    public AddFuelGroup(ruleType:number) {
        (<FormArray>this.rForm.get('FuelGroupTable')).push(this.CreateFuelGroupTable(ruleType));
    }
    
    private CreateFuelGroupTable(ruleType: number): FormGroup {
        this.SelectedFuelGroups = this.freightComponent.rcForm.get('FuelGroups').value as DropdownItem[];
        if (this.SelectedFuelGroups.length == 0) {
            return this._fb.group({});
        }
        else if (ruleType == FreightRateRuleType.Route) {
            return this._fb.group({
                group: this.addFuelGroupTable()
            });
        }
        else if (ruleType == FreightRateRuleType.Range || ruleType == FreightRateRuleType.P2P) {
                return this._fb.group({
                    group: this.addFuelGroupTable(),
                    FreightRateCalcPreferenceType: new FormControl(1),
                    FuelGroups: new FormControl('', [Validators.required]),
                    MixLoadMinValue: new FormControl(0, [Validators.required, Validators.pattern(this.decimalSupportedRegx)])
                });
        }

        
    }

    public addFuelGroupTable(): FormArray {
        let fg = new FormArray([]);
        this.freightComponent.rcForm.get('FuelGroups').value.forEach(x => {
            fg.push(this._fb.group({
                FuelGroupId: new FormControl(x.Id),
                FuelGroupName: new FormControl(x.Name),
                MinQuantity: new FormControl(1, [Validators.required, Validators.pattern(this.decimalSupportedRegx)])
            }));
        });
        return fg;
    }

    

    public AddRanges(srNmber: number) {
        this.AddRange(srNmber);
    }

    public RemoveRanges(srNmber: number) {
        let rTable = (<FormArray>this.rForm.get('RangeTable'));
        if (+this.rForm.get('RuleType').value == FreightRateRuleType.Route) {
            if (rTable.length > 2) {
                rTable.removeAt(rTable.length - 1);
                // rTable.removeAt(rTable.length - 1);
                if (rTable.length > 0) rTable.at(rTable.length - 1).get('IsLast').patchValue(true);
            }
        }
        else if (+this.rForm.get('RuleType').value == FreightRateRuleType.Range) {

            if (rTable.length > 1) {
                rTable.removeAt(srNmber);
                if (rTable.length > 0) rTable.at(rTable.length - 1).get('IsLast').patchValue(true);
            }
        }
    }

    public isNumber(n) {
        return /^-?[\d.]+(?:e-?\d+)?$/.test(n);
    }


    public onImportClick($event) {
        
        
        let rows = [];
        let columns = [];
        let firstRow = [];
        rows = $event.split(/\r?\n/);
        //sainity check 1
        columns = rows[0].split(","); //first row
        firstRow = rows[0].split(",");  //first row

        if (this.freightRateRuleType == FreightRateRuleType.Range) {
            let frs: FreightRateRangeTableViewModel[] = [];
            if (columns.length - 1 != this.SelectedFuelGroups.length || columns[0] != "UptoQuantity") {
                Declarations.msgerror("Template mistmach with selected fuel group name.", undefined, undefined);
                return;
            }

            let i = 0;
            for (i = 0; i < this.SelectedFuelGroups.length; i++) {
                if (this.SelectedFuelGroups[i].Name != columns[i + 1]) {
                    Declarations.msgerror("Template mistmach with selected fuel group name order.", undefined, undefined);
                    return;
                }
            }


            for (i = 1; i < rows.length; i++) {
                columns = rows[i].split(",");
                if (columns.length == 1) break;
                for (let j = 0; j < columns.length; j++) {
                    let str = columns[j].replace(/\s/g, "");
                    if (!this.isNumber(str)) {
                        Declarations.msgerror("Invalid number " + str + ".", undefined, undefined);
                        return;
                    }
                }
            }

            rows = $event.split(/\r?\n/);
            for (i = 1; i < rows.length; i++) {
                columns = rows[i].split(",");
                if (columns.length == 1) break;
                //let fr = new FreightRateRangeTableViewModel();
                let frtVM = { UptoQuantity: 0, FuelGroupId: 0, RateValue: 0 };
                for (let j = 0; j < columns.length; j++) {
                    let str = columns[j].replace(/\s/g, "");
                    if (j == 0) {
                        frtVM.UptoQuantity = +str;
                    } else {
                        frtVM.FuelGroupId = this.SelectedFuelGroups.filter(r => r.Name == firstRow[j])[0].Id;
                        frtVM.RateValue = +str;
                        frs.push({ UptoQuantity: frtVM.UptoQuantity, FuelGroupId: frtVM.FuelGroupId, RateValue: frtVM.RateValue });
                    }
                }
            }

            this.generateRangeTable(frs);
        }
        else if (this.freightRateRuleType == FreightRateRuleType.P2P) {

            //TODO : validation is not yet converd.
            let frP2P: FreightRatePtoPTable[] = [];
            rows = $event.split(/\r?\n/);
            let i = 0;
            for (i = 1; i < rows.length; i++) {
                columns = rows[i].split(",");
                if (columns.length == 1) break;
                
                //Terminal/Bulk Plants,Location Name,Location Address,LaneId,Assumed Miles,FuelGroupStd1,FuelGroupStd2,IsLaneRequired
                //Terminals_811|Buckeye - Roseton,14038|loc 1 sep,57 wall street,,,,,
                let row = columns[0].split("|");
                let terminalInfo = { TerminalId: "", TerminalAndBulkPlantName:"" };
                let bulkPlantInfo = { BulkPlantId: "", TerminalAndBulkPlantName: ""};
                if (columns[0].startsWith('Terminals_')) {
                    terminalInfo = { TerminalId: row[0].trim(), TerminalAndBulkPlantName: row[1].trim() };
                }
                else if (columns[0].startsWith('BulkPlants_')) {
                    bulkPlantInfo = { BulkPlantId: row[0].trim(), TerminalAndBulkPlantName: row[1].trim() };
                }             
                 row = columns[1].split("|");
                let locationInfo = {
                    JobId: row[0].trim(), LocationName: row[1], LocationAddress: columns[2].trim()
                };
                //let lastPipline = row[2].split(","); //loc 1 sep,57 wall street,laneId,assumedMile,fg1,fg2,islaneRequired
                let LaneId = columns[3].trim();
                let AssumedMiles = columns[4].trim();
                let IsLaneRequired = columns[columns.length-1].trim();
                
                for (let j = 0; j < this.SelectedFuelGroups.length; j++) {
                    let fp2p = new FreightRatePtoPTable();
                    fp2p.TerminalId = terminalInfo.TerminalId == "" ? null : +terminalInfo.TerminalId.split('_')[1];
                    fp2p.BulkPlantId = bulkPlantInfo.BulkPlantId == "" ? null : +bulkPlantInfo.BulkPlantId.split('_')[1];
                    fp2p.TerminalAndBulkPlantName = terminalInfo.TerminalId == "" ? bulkPlantInfo.TerminalAndBulkPlantName: terminalInfo.TerminalAndBulkPlantName;
                    fp2p.LocationName = locationInfo.LocationName;
                    fp2p.LocationAddress = locationInfo.LocationAddress;
                    fp2p.JobId = +locationInfo.JobId;
                    fp2p.LaneID =LaneId;
                    fp2p.AssumedMiles = AssumedMiles;
                    fp2p.IsLaneRequired = (IsLaneRequired=="0")?false:true;
                    fp2p.FuelGroupId = this.SelectedFuelGroups[0].Id;
                    fp2p.RateValue = +columns[4+j].trim();
                    frP2P.push(fp2p);
                }
            }

            //this.freightComponent.closePopup();

            this.generateP2PTable(frP2P);
        }


        Declarations.msgsuccess("Import successful.", undefined, undefined);

    }

}



