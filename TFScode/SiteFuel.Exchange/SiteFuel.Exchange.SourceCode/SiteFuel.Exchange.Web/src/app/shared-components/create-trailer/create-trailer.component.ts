import { Component, OnInit, Output, EventEmitter } from '@angular/core';
import { FormGroup, FormBuilder, Validators, FormArray } from '@angular/forms';
import { Declarations } from '../../declarations.module';
import { DropdownItem } from '../../statelist.service';
import { Compartment, TruckDetailsModel } from 'src/app/carrier/model';
import { CarrierService } from 'src/app/carrier/service/carrier.service';
import { LicenceRequirement, TrailerType, TruckStatus } from 'src/app/app.enum';
import { RegExConstants } from 'src/app/app.constants';
declare function closeSlidePanel(): any;
declare var defaultUoM: any;

@Component({
    selector: 'app-create-trailer',
    templateUrl: './create-trailer.component.html',
    styleUrls: ['./create-trailer.component.css']
})


export class CreateTrailerComponent implements OnInit {
    public MinDate: Date = new Date();
    public MaxDate: Date = new Date();
    public IsLoading: boolean;
    public IsCompartments: boolean = false;
    public StatusEnum: typeof TruckStatus = TruckStatus;
    public TrailerTypeStatusEnum: typeof TrailerType = TrailerType;
    public LicenceRequirementEnum: typeof LicenceRequirement = LicenceRequirement;
    public Statuses = [];
    public TrailerTypes = [];
    public FuelTypes = [];
    public LicenceRequirements = [];
    public FreightForm: FormGroup;
    public DdlSettings = {};
    public DefaultUoM;
    @Output() onSubmitGroupData: EventEmitter<any> = new EventEmitter<any>();

    constructor(private fb: FormBuilder, private carrierService: CarrierService) {
        this.FreightForm = this.fb.group({
            Id: this.fb.control(''),
            Name: this.fb.control(''),
            Owner: this.fb.control(''),
            LicencePlate: this.fb.control('', [Validators.required]),
            TruckId: this.fb.control('', [Validators.required]),
            FuelCapacity: this.fb.control('', [Validators.pattern(RegExConstants.DecimalNumber)]),
            ContractNumber: this.fb.control(''),
            Status: this.fb.control(TruckStatus.Active),
            TrailerType: this.fb.control(TrailerType.Lead),
            LicenceRequirement: this.fb.control(null, Validators.required),
            Compartments: this.fb.array([]),
            ExpirationDate: this.fb.control('', [Validators.required]),
            IsPump: this.fb.control("1"),
            IsFilldCompatible: this.fb.control(false),
            SmartDeviceId: this.fb.control('', [Validators.required]),
            OptimizedCapacity: this.fb.control('')
        });
    }

    ngOnInit() {
        this.DdlSettings = {
            singleSelection: false,
            idField: 'Id',
            textField: 'Name',
            enableCheckAll: false,
            selectAllText: 'Select All',
            unSelectAllText: 'UnSelect All',
            itemsShowLimit: 1,
            allowSearchFilter: true
        };
        //if (typeof defaultUoM !== 'undefined' && defaultUoM) {
        //    this.DefaultUoM = (defaultUoM == '' || defaultUoM == undefined) ? null : defaultUoM;
        //}
        this.IsLoading = false;
        this.Statuses = (Object.keys(this.StatusEnum).filter(k => typeof this.StatusEnum[k] === "number") as string[]).map(x => {
            return {
                Id: this.StatusEnum[x], Name: x == "InActive" ? "In-Active" : x == "UnderMaintenance" ? "Under-Maintenance" : x, Code: ""
            } as DropdownItem
        });
        this.TrailerTypes = (Object.keys(this.TrailerTypeStatusEnum).filter(k => typeof this.TrailerTypeStatusEnum[k] === "number") as string[]).map(x => { return { Id: this.TrailerTypeStatusEnum[x], Name: x, Code: "" } as DropdownItem });
        this.LicenceRequirements = (Object.keys(this.LicenceRequirementEnum).filter(k => typeof this.LicenceRequirementEnum[k] === "number") as string[]).map(x => { return { Id: this.LicenceRequirementEnum[x], Name: x == "Class1" ? "Class 1" : x == "Class3" ? "Class 3" : x, Code: "" } as DropdownItem });
        this.MaxDate.setFullYear(this.MinDate.getFullYear() + 30);
        this.IsCompartments = false;
        this.getDefaultUOM();
        this.getFuelType();
        
    }

    get Compartments(): FormArray {
        return this.FreightForm.get("Compartments") as FormArray
    }

    buildCompartment(model: Compartment): FormGroup {
        return this.fb.group({
            CompartmentId: this.fb.control(model.CompartmentId, [Validators.required]),
            Capacity: this.fb.control(model.Capacity, [Validators.pattern(/^[0-9]\d*(\.\d+)?$/)]),
            FuelType: this.fb.control(model.FuelType != null ? model.FuelType : this.FuelTypes[0].Id),
            PumpId: this.fb.control(model.PumpId, [Validators.required]),
        });
    }
    getFuelType() {
        this.IsLoading = true;
        this.carrierService.getFuelTypes().subscribe(t => {
            this.IsLoading = false;
            this.FuelTypes = t as DropdownItem[];
        });
    }
    getDefaultUOM() {
        this.IsLoading = true;
        this.carrierService.getDefaultUOM().subscribe(response => {
            this.IsLoading = false;
            this.DefaultUoM = response;
        });
    }
    AddCompartment() {
        this.Compartments.push(this.buildCompartment(new Compartment()));
    }

    RemoveCompartment(i: number) {
        this.Compartments.removeAt(i);
    }

    toggleCompartment(event: any): void {
        if (event.isTrusted) {
            this.IsCompartments = !this.IsCompartments;
            if (!this.IsCompartments) {
                this.Compartments.clear();
            } else if (this.Compartments.length == 0) {
                this.AddCompartment();
            }
        }
    }

    setValidationForIsValidFilldCompatible(isFilldCompatible: boolean) {
        let val = isFilldCompatible ? [Validators.required] : [];
        this.FreightForm.controls['SmartDeviceId'].setValidators(val);
        this.FreightForm.controls['SmartDeviceId'].updateValueAndValidity();

        let groupItems: any = (this.FreightForm.get("Compartments") as FormArray).controls;

        for (let item of groupItems) {
            item.controls["PumpId"].setValidators(val);
            item.controls["PumpId"].updateValueAndValidity();
        }
        if (isFilldCompatible) {
            this.FreightForm.get("IsPump").setValue("1");
        }
    }
    loadTruckDetail(truck: TruckDetailsModel) {
        this.clearTrailerForm();
        let zero = 0;
        if (truck.LicenceRequirement == zero) {
            truck.LicenceRequirement = null;
        }
        this.FreightForm.patchValue(truck);
        this.IsCompartments = false;
        if (truck.Compartments != null && truck.Compartments.length > 0) {
            this.IsCompartments = true;
            truck.Compartments.forEach(x => this.Compartments.push(this.buildCompartment(x)));
        }
    }

    onSubmit() {
        this.setValidationForIsValidFilldCompatible(this.FreightForm.controls['IsFilldCompatible'].value);

        var totalCapacity = this.FreightForm.get('FuelCapacity').value as number;
        if (this.FreightForm.valid && this.Compartments.length > 0 && totalCapacity > 0) {
            var sumCompartmentCapacity: number = 0;
            this.Compartments.controls.forEach(t => sumCompartmentCapacity += parseFloat(t.get('Capacity').value));
            if (!(totalCapacity == sumCompartmentCapacity)) {
                Declarations.msgerror('Total compartment capacity should match Trailer capacity.', undefined, undefined);
                this.FreightForm.setErrors({ 'invalid': true });
                return false;
            }
        }
        if (this.FreightForm.valid) {
            this.IsLoading = true;
            this.carrierService.postCreateTruck(this.FreightForm.value).subscribe(data => {
                this.IsLoading = false;
                if (data.StatusCode == 0) {
                    Declarations.msgsuccess(data.StatusMessage, undefined, undefined);
                    closeSlidePanel();
                    this.clearTrailerForm();
                    this.onSubmitGroupData.emit();
                }
                else if (data.StatusCode == 2) {
                    Declarations.msgwarning(data.StatusMessage, undefined, undefined);
                }
                else {
                    Declarations.msgerror(data == null ? 'Failed' : data.StatusMessage, undefined, undefined);
                }
            })
        }
        else {
            this.FreightForm.markAllAsTouched();
        }
    }
    setSelectedDate(date: string) {
        var _date = this.FreightForm.get('ExpirationDate');
        if (_date.value != date) {
            _date.setValue(date);
        }
    }
    clearTrailerForm() {
        this.FreightForm.reset();
        this.Compartments.clear();
        this.IsCompartments = false;
        this.setDefaultValue();
    }

    setDefaultValue() {
        this.FreightForm.get("Status").setValue(TruckStatus.Active);
        this.FreightForm.get("TrailerType").setValue(TrailerType.Lead);
        this.FreightForm.get("LicenceRequirement").setValue(LicenceRequirement.Class1);
        this.FreightForm.get("IsFilldCompatible").setValue(false);
    }
}
