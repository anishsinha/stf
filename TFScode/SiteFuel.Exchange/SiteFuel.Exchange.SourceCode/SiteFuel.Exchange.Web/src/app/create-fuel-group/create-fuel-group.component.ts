import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Component, OnInit, Output, EventEmitter, ViewEncapsulation } from '@angular/core';
import { AbstractControl, FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { IDropdownSettings } from 'ng-multiselect-dropdown';
import { DropdownCustomItem, DropdownItem } from 'src/app/statelist.service';
import { Declarations } from 'src/app/declarations.module';
import { FuelSurchargeService } from 'src/app/fuelsurcharge/services/fuelsurcharge.service'
import { RegionService } from 'src/app/company-addresses/region/service/region.service';
import { ExternalMappingsService } from '../self-service-alias/service/externalmappings.service';
import { BehaviorSubject, merge } from 'rxjs';
import { pairwise, startWith } from 'rxjs/operators';
import { FreightTableStatus, FuelGroupType, TableType } from 'src/app/app.enum';
import { FuelGroupViewModel } from '../self-service-alias/models/FuelGroupGridViewModel';


const httpOptions = {
    headers: new HttpHeaders({ 'Content-Type': 'application/json' })
};

@Component({
    selector: 'app-create-fuel-group',
    templateUrl: './create-fuel-group.component.html',
    styleUrls: ['./create-fuel-group.component.css'],
    encapsulation: ViewEncapsulation.None
})
export class CreateFuelGroupComponent implements OnInit {
    @Output() result = new EventEmitter();
    public isLoadingSubject: BehaviorSubject<any>;
    public nativeServiceCall: boolean = true;

    ProductTypeList: DropdownItem[] = [];
    FuelTypeList: DropdownCustomItem[] = [];

    TableTypeList: DropdownItem[] = [];
    CustomerList: DropdownItem[] = [];
    CarrierList: DropdownItem[] = [];

    public IsCustomSelected = false;
    public IsCustomerSelected = false;
    public IsCarrierSelected = false;
    public ViewOrEdit: any;

    public fuelGroupMapping: FuelGroupViewModel;
    public IsEditable: boolean = true;
    public IsProductTypeSelected: boolean = false;
    
    public rcForm: FormGroup;
   
   
    public createFuelGroupUrl = '/FuelGroup/CreateFuelGroup';
    public updateFuelGroupUrl = '/FuelGroup/UpdateFuelGroup';
    

    public SingleSelectSettingsById = {};
    public MultiSelectSettingsById: IDropdownSettings;

    constructor(private httpService: HttpClient, private _fb: FormBuilder,
        private fuelSurchargeService: FuelSurchargeService,
        private RegionService: RegionService,
        private externalMappingsService: ExternalMappingsService    ) { }
  
    ngOnInit() {
        this.isLoadingSubject = new BehaviorSubject(false);
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

        
        this.init();

        merge(
            this.rcForm.controls['GroupTypeCustom'].get('TableTypes').valueChanges
        )
            .pipe(startWith(null), pairwise())
            .subscribe(([prev, next]: [any, any]) => {
                if (JSON.stringify(prev) != JSON.stringify(next)) this.onTableTypeSelect(prev, next);
            })

        merge(
            this.rcForm.controls['GroupTypeStandard'].get('ProductTypes').valueChanges
        )
            .pipe(startWith(null), pairwise())
            .subscribe(([prev, next]: [any, any]) => {
                if (JSON.stringify(prev) != JSON.stringify(next)) this.onProductTypeChange(prev, next);
            });

        merge(
            this.rcForm.controls['GroupTypeCustom'].get('Customers').valueChanges
        )
            .pipe(startWith(null), pairwise())
            .subscribe(([prev, next]: [any, any]) => {
                if (JSON.stringify(prev) != JSON.stringify(next)) this.onCustomerSelect(prev, next);
            })


        merge(
            this.rcForm.controls['GroupTypeCustom'].get('Carriers').valueChanges
        )
            .pipe(startWith(null), pairwise())
            .subscribe(([prev, next]: [any, any]) => {
                if (JSON.stringify(prev) != JSON.stringify(next)) this.onCarrierSelect(prev, next);
            })




        this.getProductTypeList();
        this.getTableTypeList();
        this.getCustomerList();
        this.getCarrierList();
    }

    init() {
        this.rcForm = this._fb.group({
            Id: new FormControl(''),
            GroupName: new FormControl('', [Validators.required]),
            FuelGroupType: new FormControl(1),
            GroupTypeStandard: new FormGroup({
                ProductTypes: new FormControl(''), //select top 1 * from MstProductTypes --product type
                FuelTypes: new FormControl('')  //select top 1 * from MstTfxProducts --  fuel type
            }),
            GroupTypeCustom: new FormGroup({
                TableTypes: new FormControl(''), // customer or carrier selection controls
                Customers: new FormControl(''),
                Carriers: new FormControl('')
            }),
            FreightTableStatus: ['']
        });
    }

    public onCustomerSelect(prev: any, item: any) {
        
        this.isLoadingSubject.next(true);
        this.rcForm.controls['GroupTypeStandard'].get('ProductTypes').patchValue([]);
        this.rcForm.controls['GroupTypeStandard'].get('FuelTypes').patchValue([]);
        this.isLoadingSubject.next(false);
    }

    public onCarrierSelect(prev: any, item: any) {
       
        this.isLoadingSubject.next(true);
        this.rcForm.controls['GroupTypeStandard'].get('ProductTypes').patchValue([]);
        this.rcForm.controls['GroupTypeStandard'].get('FuelTypes').patchValue([]);
        this.isLoadingSubject.next(false);
    }

    private onTableTypeSelect(prev: any, item: any) {
       
        this.IsCarrierSelected = false;
        this.IsCustomerSelected = false;
        this.isLoadingSubject.next(true);
        this.rcForm.controls['GroupTypeCustom'].get('Customers').patchValue([]);
        this.CustomerList = [];
        this.rcForm.controls['GroupTypeCustom'].get('Carriers').patchValue([]);
        this.CarrierList = [];
        this.rcForm.controls['GroupTypeStandard'].get('ProductTypes').patchValue([]);
        this.rcForm.controls['GroupTypeStandard'].get('FuelTypes').patchValue([]);
        if (item != null && item != "" && item != undefined && item.length==1) {
            if (item[0].Id == TableType.Customer) {
                this.IsCustomerSelected = true;
                this.getCustomerList();
            }
            else if (item[0].Id == TableType.Carrier) {
                this.IsCarrierSelected = true;
                this.getCarrierList();
            }
        }
        this.isLoadingSubject.next(false);
    }

    public FuelGroupTypeChange(): void {
        this.isLoadingSubject.next(true);
        this.IsProductTypeSelected = false;
        this.rcForm.controls['GroupTypeCustom'].get('TableTypes').patchValue([]);
        this.rcForm.controls['GroupTypeCustom'].get('Customers').patchValue([]);
        this.CustomerList = [];
        this.rcForm.controls['GroupTypeCustom'].get('Carriers').patchValue([]);
        this.CarrierList = [];
        this.rcForm.controls['GroupTypeStandard'].get('ProductTypes').patchValue([]);
        //this.ProductTypeList = [];
        this.rcForm.controls['GroupTypeStandard'].get('FuelTypes').patchValue([]);
        this.FuelTypeList = [];
        this.rcForm.controls['GroupName'].patchValue(null);
        this.rcForm.controls['Id'].patchValue(null);
        this.ViewOrEdit = "Create";
        this.isLoadingSubject.next(false);
    }

    public onProductTypeChange(prev: any, next: any) {
        if (!this.nativeServiceCall) {
            this.IsProductTypeSelected = true;
            return;
        }
        this.isLoadingSubject.next(true);

        let selProductTypes = this.rcForm.controls['GroupTypeStandard'].get('ProductTypes').value as DropdownItem[];
        if (selProductTypes != null && selProductTypes != undefined && selProductTypes.length > 0) {
            this.IsProductTypeSelected = true;
            this.rcForm.controls['GroupTypeStandard'].get('FuelTypes').patchValue([]);
            let editingcompanyId = -1;
            let cus = this.rcForm.controls.GroupTypeCustom.get('Customers').value;
            let car = this.rcForm.controls.GroupTypeCustom.get('Carriers').value;
            if (cus != null && cus.length > 0) {
                editingcompanyId = cus[0].Id;
            } else if (car != null && car.length > 0) {
                editingcompanyId = car[0].Id;
            }

            this.externalMappingsService.getFuelTypeList(selProductTypes.map(s => s.Id).join(','), this.rcForm.controls.FuelGroupType.value, +this.rcForm.controls.Id.value, editingcompanyId).subscribe(async (data) => {
                this.FuelTypeList = await (data);
                this.rcForm.controls['GroupTypeStandard'].get('FuelTypes').setValue(this.FuelTypeList.filter(r => !r.isDisabled))
                this.isLoadingSubject.next(false);
            });
        } else {
            this.rcForm.controls['GroupTypeStandard'].get('FuelTypes').patchValue([]);
            this.FuelTypeList = [];
            this.IsProductTypeSelected = false;
        }
        this.isLoadingSubject.next(false);
    }

    getProductTypeList() {
        this.isLoadingSubject.next(true);
        this.externalMappingsService.getProductTypeList().subscribe(async (data) => {
        this.ProductTypeList = await (data);
            this.isLoadingSubject.next(false);
    });
    }

    getFuelTypeList(productTypeIds: string, fuelGroupType:string) {
        this.isLoadingSubject.next(true);
        this.rcForm.controls['GroupTypeStandard'].get('FuelTypes').patchValue([]);
        let editingcompanyId = -1;
        let cus = this.rcForm.controls.GroupTypeCustom.get('Customers').value;
        let car = this.rcForm.controls.GroupTypeCustom.get('Carriers').value;
        if (cus != null && cus.length > 0) {
            editingcompanyId = cus[0].Id;
        } else if (car != null && car.length > 0) {
            editingcompanyId = car[0].Id;
        }
        this.externalMappingsService.getFuelTypeList(productTypeIds, fuelGroupType, +this.rcForm.controls.Id.value, editingcompanyId).subscribe(async (data) => {
            this.FuelTypeList = await (data);
            this.rcForm.controls['GroupTypeStandard'].get('FuelTypes').setValue(this.FuelTypeList.filter(r => !r.isDisabled))
            this.isLoadingSubject.next(false);
        });
    }

   
    getTableTypeList() {
        if (!this.nativeServiceCall) {return;}
        this.isLoadingSubject.next(true);
        this.fuelSurchargeService.getTableTypes().subscribe(async (data) => {
            let tblTypeList: DropdownItem[];
            tblTypeList = await (data);
            this.TableTypeList = tblTypeList.filter(x => x.Id != 1); // no master included.
            this.TableTypeList.forEach(res => res.Name = res.Name.replace("Specific", ""));
            //this.rcForm.controls.GroupTypeCustom.get('TableTypes').setValue(this.TableTypeList.slice(0, 1));
            this.isLoadingSubject.next(false);
        });

    }

    getCustomerList() {
        if (!this.nativeServiceCall) { return; }
        this.isLoadingSubject.next(true);
        this.fuelSurchargeService.getSupplierCustomers().subscribe(async (data) => {
            this.CustomerList = await (data);
            this.isLoadingSubject.next(false);
        });
    }

    getCarrierList() {
        if (!this.nativeServiceCall) { return; }
        this.isLoadingSubject.next(true);
        this.RegionService.getCarriers().subscribe(async (data) => {
            this.CarrierList = await (data);
            this.isLoadingSubject.next(false);
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

    private modeChangeCustomOrStandardValidators(fuelGroupType: number) {

        this.AddRemoveValidations([this.rcForm.controls['GroupTypeStandard'].get('ProductTypes'),
        this.rcForm.controls['GroupTypeStandard'].get('FuelTypes')],null);
        if (fuelGroupType == FuelGroupType.Standard) {
            this.AddRemoveValidations(null,
                [this.rcForm.controls['GroupTypeCustom'].get('Customers'),
                this.rcForm.controls['GroupTypeCustom'].get('Carriers'),
                this.rcForm.controls['GroupTypeCustom'].get('TableTypes')]);
        }
        if (fuelGroupType == FuelGroupType.Custom) {
            this.AddRemoveValidations([this.rcForm.controls['GroupTypeCustom'].get('TableTypes')], null);
            if (this.rcForm.controls['GroupTypeCustom'].get('TableTypes').value == null || this.rcForm.controls['GroupTypeCustom'].get('TableTypes').value == undefined) return;

            if (this.rcForm.controls['GroupTypeCustom'].get('TableTypes').value[0].Id == TableType.Customer) {
                this.AddRemoveValidations([this.rcForm.controls['GroupTypeCustom'].get('Customers')], [this.rcForm.controls['GroupTypeCustom'].get('Carriers')]);
            }
            else {
                this.AddRemoveValidations([this.rcForm.controls['GroupTypeCustom'].get('Carriers')], [this.rcForm.controls['GroupTypeCustom'].get('Customers')]);
            }
        }

    }

   public onSubmit() {
       this.modeChangeCustomOrStandardValidators(this.rcForm.controls.FuelGroupType.value);
       this.markFormGroupTouched(this.rcForm);
        if (this.rcForm.valid)
        this.Save();
    }

   public onCancel() {
       this.result.emit(true);
    }

    private Save() {
       
        this.isLoadingSubject.next(true);
        this.fuelGroupMapping= new FuelGroupViewModel();
        this.fuelGroupMapping.Id = this.rcForm.controls.Id.value;
        this.fuelGroupMapping.GroupName = this.rcForm.controls.GroupName.value.trim();
        this.fuelGroupMapping.FuelGroupType = this.rcForm.controls.FuelGroupType.value;
        
        if (this.fuelGroupMapping.FuelGroupType == FuelGroupType.Custom) {
            this.fuelGroupMapping.TableType = this.rcForm.controls.GroupTypeCustom.get('TableTypes').value[0].Id;
            if (this.fuelGroupMapping.TableType==TableType.Customer) {
                this.fuelGroupMapping.AssignedCompanyId = this.rcForm.controls.GroupTypeCustom.get('Customers').value[0].Id;
            }else if (this.fuelGroupMapping.TableType == TableType.Carrier) {
                this.fuelGroupMapping.AssignedCompanyId = this.rcForm.controls.GroupTypeCustom.get('Carriers').value[0].Id;
            }
        }

        var pIds = [];
        this.rcForm.controls.GroupTypeStandard.get('ProductTypes').value.forEach(res => { pIds.push(res.Id) });
        this.fuelGroupMapping.ProductTypeIds = pIds;

        var fIds = [];
        this.rcForm.controls.GroupTypeStandard.get('FuelTypes').value.forEach(res => { fIds.push(res.Id) });
        this.fuelGroupMapping.FuelTypeIds = fIds;

        this.fuelGroupMapping.FreightTableStatus = FreightTableStatus.Published;

        this.httpService.post(this.rcForm.controls.Id.value != null ? this.updateFuelGroupUrl:this.createFuelGroupUrl, this.fuelGroupMapping, httpOptions).pipe().subscribe((res:any) => {
            if (res.StatusCode == 0) {
                this.result.emit(true);
                let message = "";
                if (this.fuelGroupMapping.Id != null) {
                    message = "updated";
                }
                else if (this.fuelGroupMapping.FreightTableStatus == FreightTableStatus.Published) {
                    message = "created"
                } 
                Declarations.msgsuccess(this.fuelGroupMapping.GroupName + " fuel group " + message + " successfully.", undefined, undefined);

            } else
                Declarations.msgerror(res == null || res.StatusMessage == null ? 'Failed' : res.StatusMessage, undefined, undefined);
            this.isLoadingSubject.next(false);
        })
    }

   
 

}

