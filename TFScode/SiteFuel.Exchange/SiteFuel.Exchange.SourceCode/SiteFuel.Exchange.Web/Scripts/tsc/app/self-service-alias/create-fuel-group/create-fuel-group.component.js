import { __awaiter, __decorate } from "tslib";
import { HttpHeaders } from '@angular/common/http';
import { Component, Output, EventEmitter, ViewEncapsulation } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { FuelGroupType, FuelGroupViewModel } from '../models/FuelGroupGridViewModel';
import { Declarations } from 'src/app/declarations.module';
import { BehaviorSubject, merge } from 'rxjs';
import { pairwise, startWith } from 'rxjs/operators';
import { FreightTableStatus, TableType } from 'src/app/app.enum';
const httpOptions = {
    headers: new HttpHeaders({ 'Content-Type': 'application/json' })
};
let CreateFuelGroupComponent = class CreateFuelGroupComponent {
    constructor(httpService, _fb, fuelSurchargeService, RegionService, externalMappingsService) {
        this.httpService = httpService;
        this._fb = _fb;
        this.fuelSurchargeService = fuelSurchargeService;
        this.RegionService = RegionService;
        this.externalMappingsService = externalMappingsService;
        this.result = new EventEmitter();
        this.nativeServiceCall = true;
        this.ProductTypeList = [];
        this.FuelTypeList = [];
        this.TableTypeList = [];
        this.CustomerList = [];
        this.CarrierList = [];
        this.IsCustomSelected = false;
        this.IsCustomerSelected = false;
        this.IsCarrierSelected = false;
        this.IsEditable = true;
        this.IsProductTypeSelected = false;
        this.createFuelGroupUrl = '/FuelGroup/CreateFuelGroup';
        this.updateFuelGroupUrl = '/FuelGroup/UpdateFuelGroup';
        this.SingleSelectSettingsById = {};
    }
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
        merge(this.rcForm.controls['GroupTypeCustom'].get('TableTypes').valueChanges)
            .pipe(startWith(null), pairwise())
            .subscribe(([prev, next]) => {
            if (JSON.stringify(prev) != JSON.stringify(next))
                this.onTableTypeSelect(prev, next);
        });
        merge(this.rcForm.controls['GroupTypeStandard'].get('ProductTypes').valueChanges)
            .pipe(startWith(null), pairwise())
            .subscribe(([prev, next]) => {
            if (JSON.stringify(prev) != JSON.stringify(next))
                this.onProductTypeChange(prev, next);
        });
        merge(this.rcForm.controls['GroupTypeCustom'].get('Customers').valueChanges)
            .pipe(startWith(null), pairwise())
            .subscribe(([prev, next]) => {
            if (JSON.stringify(prev) != JSON.stringify(next))
                this.onCustomerSelect(prev, next);
        });
        merge(this.rcForm.controls['GroupTypeCustom'].get('Carriers').valueChanges)
            .pipe(startWith(null), pairwise())
            .subscribe(([prev, next]) => {
            if (JSON.stringify(prev) != JSON.stringify(next))
                this.onCarrierSelect(prev, next);
        });
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
                ProductTypes: new FormControl(''),
                FuelTypes: new FormControl('') //select top 1 * from MstTfxProducts --  fuel type
            }),
            GroupTypeCustom: new FormGroup({
                TableTypes: new FormControl(''),
                Customers: new FormControl(''),
                Carriers: new FormControl('')
            }),
            FreightTableStatus: ['']
        });
    }
    onCustomerSelect(prev, item) {
        this.isLoadingSubject.next(true);
        this.rcForm.controls['GroupTypeStandard'].get('ProductTypes').patchValue([]);
        this.rcForm.controls['GroupTypeStandard'].get('FuelTypes').patchValue([]);
        this.isLoadingSubject.next(false);
    }
    onCarrierSelect(prev, item) {
        this.isLoadingSubject.next(true);
        this.rcForm.controls['GroupTypeStandard'].get('ProductTypes').patchValue([]);
        this.rcForm.controls['GroupTypeStandard'].get('FuelTypes').patchValue([]);
        this.isLoadingSubject.next(false);
    }
    onTableTypeSelect(prev, item) {
        this.IsCarrierSelected = false;
        this.IsCustomerSelected = false;
        this.isLoadingSubject.next(true);
        this.rcForm.controls['GroupTypeCustom'].get('Customers').patchValue([]);
        this.CustomerList = [];
        this.rcForm.controls['GroupTypeCustom'].get('Carriers').patchValue([]);
        this.CarrierList = [];
        this.rcForm.controls['GroupTypeStandard'].get('ProductTypes').patchValue([]);
        this.rcForm.controls['GroupTypeStandard'].get('FuelTypes').patchValue([]);
        if (item != null && item != "" && item != undefined && item.length == 1) {
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
    FuelGroupTypeChange() {
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
    onProductTypeChange(prev, next) {
        if (!this.nativeServiceCall) {
            this.IsProductTypeSelected = true;
            return;
        }
        this.isLoadingSubject.next(true);
        let selProductTypes = this.rcForm.controls['GroupTypeStandard'].get('ProductTypes').value;
        if (selProductTypes != null && selProductTypes != undefined && selProductTypes.length > 0) {
            this.IsProductTypeSelected = true;
            this.rcForm.controls['GroupTypeStandard'].get('FuelTypes').patchValue([]);
            let editingcompanyId = -1;
            let cus = this.rcForm.controls.GroupTypeCustom.get('Customers').value;
            let car = this.rcForm.controls.GroupTypeCustom.get('Carriers').value;
            if (cus != null && cus.length > 0) {
                editingcompanyId = cus[0].Id;
            }
            else if (car != null && car.length > 0) {
                editingcompanyId = car[0].Id;
            }
            this.externalMappingsService.getFuelTypeList(selProductTypes.map(s => s.Id).join(','), this.rcForm.controls.FuelGroupType.value, +this.rcForm.controls.Id.value, editingcompanyId).subscribe((data) => __awaiter(this, void 0, void 0, function* () {
                this.FuelTypeList = yield (data);
                this.rcForm.controls['GroupTypeStandard'].get('FuelTypes').setValue(this.FuelTypeList.filter(r => !r.isDisabled));
                this.isLoadingSubject.next(false);
            }));
        }
        else {
            this.rcForm.controls['GroupTypeStandard'].get('FuelTypes').patchValue([]);
            this.FuelTypeList = [];
            this.IsProductTypeSelected = false;
        }
        this.isLoadingSubject.next(false);
    }
    getProductTypeList() {
        this.isLoadingSubject.next(true);
        this.externalMappingsService.getProductTypeList().subscribe((data) => __awaiter(this, void 0, void 0, function* () {
            this.ProductTypeList = yield (data);
            this.isLoadingSubject.next(false);
        }));
    }
    getFuelTypeList(productTypeIds, fuelGroupType) {
        this.isLoadingSubject.next(true);
        this.rcForm.controls['GroupTypeStandard'].get('FuelTypes').patchValue([]);
        let editingcompanyId = -1;
        let cus = this.rcForm.controls.GroupTypeCustom.get('Customers').value;
        let car = this.rcForm.controls.GroupTypeCustom.get('Carriers').value;
        if (cus != null && cus.length > 0) {
            editingcompanyId = cus[0].Id;
        }
        else if (car != null && car.length > 0) {
            editingcompanyId = car[0].Id;
        }
        this.externalMappingsService.getFuelTypeList(productTypeIds, fuelGroupType, +this.rcForm.controls.Id.value, editingcompanyId).subscribe((data) => __awaiter(this, void 0, void 0, function* () {
            this.FuelTypeList = yield (data);
            this.rcForm.controls['GroupTypeStandard'].get('FuelTypes').setValue(this.FuelTypeList.filter(r => !r.isDisabled));
            this.isLoadingSubject.next(false);
        }));
    }
    getTableTypeList() {
        if (!this.nativeServiceCall) {
            return;
        }
        this.isLoadingSubject.next(true);
        this.fuelSurchargeService.getTableTypes().subscribe((data) => __awaiter(this, void 0, void 0, function* () {
            let tblTypeList;
            tblTypeList = yield (data);
            this.TableTypeList = tblTypeList.filter(x => x.Id != 1); // no master included.
            this.TableTypeList.forEach(res => res.Name = res.Name.replace("Specific", ""));
            //this.rcForm.controls.GroupTypeCustom.get('TableTypes').setValue(this.TableTypeList.slice(0, 1));
            this.isLoadingSubject.next(false);
        }));
    }
    getCustomerList() {
        if (!this.nativeServiceCall) {
            return;
        }
        this.isLoadingSubject.next(true);
        this.fuelSurchargeService.getSupplierCustomers().subscribe((data) => __awaiter(this, void 0, void 0, function* () {
            this.CustomerList = yield (data);
            this.isLoadingSubject.next(false);
        }));
    }
    getCarrierList() {
        if (!this.nativeServiceCall) {
            return;
        }
        this.isLoadingSubject.next(true);
        this.RegionService.getCarriers().subscribe((data) => __awaiter(this, void 0, void 0, function* () {
            this.CarrierList = yield (data);
            this.isLoadingSubject.next(false);
        }));
    }
    markFormGroupTouched(formGroup) {
        Object.values(formGroup.controls).forEach(control => {
            control.markAsTouched();
            if (control.controls) {
                this.markFormGroupTouched(control);
            }
        });
    }
    AddRemoveValidations(requiredControls, notRequiredControls) {
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
    modeChangeCustomOrStandardValidators(fuelGroupType) {
        this.AddRemoveValidations([this.rcForm.controls['GroupTypeStandard'].get('ProductTypes'),
            this.rcForm.controls['GroupTypeStandard'].get('FuelTypes')], null);
        if (fuelGroupType == FuelGroupType.Standard) {
            this.AddRemoveValidations(null, [this.rcForm.controls['GroupTypeCustom'].get('Customers'),
                this.rcForm.controls['GroupTypeCustom'].get('Carriers'),
                this.rcForm.controls['GroupTypeCustom'].get('TableTypes')]);
        }
        if (fuelGroupType == FuelGroupType.Custom) {
            this.AddRemoveValidations([this.rcForm.controls['GroupTypeCustom'].get('TableTypes')], null);
            if (this.rcForm.controls['GroupTypeCustom'].get('TableTypes').value == null || this.rcForm.controls['GroupTypeCustom'].get('TableTypes').value == undefined)
                return;
            if (this.rcForm.controls['GroupTypeCustom'].get('TableTypes').value[0].Id == TableType.Customer) {
                this.AddRemoveValidations([this.rcForm.controls['GroupTypeCustom'].get('Customers')], [this.rcForm.controls['GroupTypeCustom'].get('Carriers')]);
            }
            else {
                this.AddRemoveValidations([this.rcForm.controls['GroupTypeCustom'].get('Carriers')], [this.rcForm.controls['GroupTypeCustom'].get('Customers')]);
            }
        }
    }
    onSubmit() {
        this.modeChangeCustomOrStandardValidators(this.rcForm.controls.FuelGroupType.value);
        this.markFormGroupTouched(this.rcForm);
        if (this.rcForm.valid)
            this.Save();
    }
    onCancel() {
        this.result.emit(true);
    }
    Save() {
        this.isLoadingSubject.next(true);
        this.fuelGroupMapping = new FuelGroupViewModel();
        this.fuelGroupMapping.Id = this.rcForm.controls.Id.value;
        this.fuelGroupMapping.GroupName = this.rcForm.controls.GroupName.value.trim();
        this.fuelGroupMapping.FuelGroupType = this.rcForm.controls.FuelGroupType.value;
        if (this.fuelGroupMapping.FuelGroupType == FuelGroupType.Custom) {
            this.fuelGroupMapping.TableType = this.rcForm.controls.GroupTypeCustom.get('TableTypes').value[0].Id;
            if (this.fuelGroupMapping.TableType == TableType.Customer) {
                this.fuelGroupMapping.AssignedCompanyId = this.rcForm.controls.GroupTypeCustom.get('Customers').value[0].Id;
            }
            else if (this.fuelGroupMapping.TableType == TableType.Carrier) {
                this.fuelGroupMapping.AssignedCompanyId = this.rcForm.controls.GroupTypeCustom.get('Carriers').value[0].Id;
            }
        }
        var pIds = [];
        this.rcForm.controls.GroupTypeStandard.get('ProductTypes').value.forEach(res => { pIds.push(res.Id); });
        this.fuelGroupMapping.ProductTypeIds = pIds;
        var fIds = [];
        this.rcForm.controls.GroupTypeStandard.get('FuelTypes').value.forEach(res => { fIds.push(res.Id); });
        this.fuelGroupMapping.FuelTypeIds = fIds;
        this.fuelGroupMapping.FreightTableStatus = FreightTableStatus.Published;
        this.httpService.post(this.rcForm.controls.Id.value != null ? this.updateFuelGroupUrl : this.createFuelGroupUrl, this.fuelGroupMapping, httpOptions).pipe().subscribe((res) => {
            if (res.StatusCode == 0) {
                this.result.emit(true);
                let message = "";
                if (this.fuelGroupMapping.Id != null) {
                    message = "updated";
                }
                else if (this.fuelGroupMapping.FreightTableStatus == FreightTableStatus.Published) {
                    message = "created";
                }
                Declarations.msgsuccess(this.fuelGroupMapping.GroupName + " fuel group " + message + " successfully.", undefined, undefined);
            }
            else
                Declarations.msgerror(res == null || res.StatusMessage == null ? 'Failed' : res.StatusMessage, undefined, undefined);
            this.isLoadingSubject.next(false);
        });
    }
};
__decorate([
    Output()
], CreateFuelGroupComponent.prototype, "result", void 0);
CreateFuelGroupComponent = __decorate([
    Component({
        selector: 'app-create-fuel-group',
        templateUrl: './create-fuel-group.component.html',
        styleUrls: ['./create-fuel-group.component.css'],
        encapsulation: ViewEncapsulation.None
    })
], CreateFuelGroupComponent);
export { CreateFuelGroupComponent };
//# sourceMappingURL=create-fuel-group.component.js.map