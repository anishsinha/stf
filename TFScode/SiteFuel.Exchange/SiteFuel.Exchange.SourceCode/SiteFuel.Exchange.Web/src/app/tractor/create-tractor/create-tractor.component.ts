import { Component, OnInit, Output, EventEmitter } from '@angular/core';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { TractorDetailsModel } from '../model';
import { TractorService } from '../service/tractor.service';
import { DropdownItem } from 'src/app/carrier-companies/service/assigncarrier.service';
import { Declarations } from 'src/app/declarations.module';
import { TractorStatus, TrailerType } from 'src/app/app.enum';
declare function closeSlidePanel(): any;
declare var defaultUoM: any;

@Component({
    selector: 'app-create-tractor',
    templateUrl: './create-tractor.component.html',
    styleUrls: ['./create-tractor.component.css']
})


export class CreateTractorComponent implements OnInit {
    public MinDate: Date = new Date();
    public MaxDate: Date = new Date();
    public editedTrailerType: boolean = false;
    public editedDriver: boolean = false;
    public isdriverfound: boolean = false;
    trailerselectedItem = [];
    trailerselectedItemValue: DropdownItem[] = [];
    driverselectedItem: DropdownItem[] = [];
    public IsLoading: boolean;
    public IsCompartments: boolean = false;
    public StatusEnum: typeof TractorStatus = TractorStatus;
    public TrailerTypeStatusEnum: typeof TrailerType = TrailerType;
    public Statuses = [];
    public TrailerTypesData: DropdownItem[] = [];
    public DriverList: DropdownItem[] = [];
    public TractorForm: FormGroup;
    public DdlSettings = {};
    public DdlSettingsTrailerType = {};
    public DefaultUoM;
    @Output() onSubmitGroupData: EventEmitter<any> = new EventEmitter<any>();

    constructor(private fb: FormBuilder, private tractorService: TractorService) {
        this.TractorForm = this.fb.group({
            Id: this.fb.control(''),
            TractorId: this.fb.control('', [Validators.required]),
            VIN: this.fb.control(''),
            Plate: this.fb.control('', [Validators.required]),
            Owner: this.fb.control(''),
            Status: this.fb.control(TractorStatus.Active),
            TrailerType: this.fb.control([]),
            Drivers: this.fb.control([]),
            ExpirationDate: this.fb.control('', [Validators.required]),
        });
    }

    ngOnInit() {
        this.DdlSettings = {
            singleSelection: false,
            idField: 'Id',
            textField: 'Name',
            enableCheckAll: true,
            selectAllText: 'Select All',
            unSelectAllText: 'UnSelect All',
            itemsShowLimit: 1,
            allowSearchFilter: true
        };
        this.DdlSettingsTrailerType = {
            singleSelection: false,
            idField: 'Id',
            textField: 'Name',
            enableCheckAll: true,
            selectAllText: 'Select All',
            unSelectAllText: 'UnSelect All',
            itemsShowLimit: 1,
            allowSearchFilter: true
        };
        this.editedTrailerType = false;
        this.editedDriver = false;
        this.isdriverfound = false;
        //this.DefaultUoM = defaultUoM;
        this.IsLoading = false;
        this.Statuses = (Object.keys(this.StatusEnum).filter(k => typeof this.StatusEnum[k] === "number") as string[]).map(x => {
            return {
                Id: this.StatusEnum[x], Name: x == "InActive" ? "In-Active" : x == "UnderMaintenance" ? "Under-Maintenance" : x, Code: ""
            } as DropdownItem
        });
        this.TrailerTypesData = (Object.keys(this.TrailerTypeStatusEnum).filter(k => typeof this.TrailerTypeStatusEnum[k] === "number") as string[]).map(x => { return { Id: this.TrailerTypeStatusEnum[x], Name: x, Code: "" } as DropdownItem });
        this.MaxDate.setFullYear(this.MinDate.getFullYear() + 30);

    }


    loadTractorDetail(tractor: TractorDetailsModel) {
        this.clearTractorForm();

        if (tractor.TrailerType.length > 0) {
            this.editedTrailerType = false;
            tractor.TrailerType.forEach((myObject) => {
                let dropdowndata = new DropdownItem();
                dropdowndata.Id = parseInt(myObject);
                dropdowndata.Name = TrailerType[dropdowndata.Id];
                this.trailerselectedItem.push(myObject);
                this.trailerselectedItemValue.push(dropdowndata)
            });
        }

        this.TractorForm.patchValue(tractor);
        this.TractorForm.get('TrailerType').patchValue(this.trailerselectedItemValue);
        //load the drivers
        this.getDriverdetails(true, tractor);
    }

    onSubmit() {
        //edit validation
        if (this.TractorForm.get('Id').value != null && this.TractorForm.get('Id').value != "") {
            var items = this.TractorForm.get('TrailerType').value as DropdownItem[];
            this.trailerselectedItem = [];
            items.forEach((myObject) => {
                this.trailerselectedItem.push(myObject.Id);
            });
        }
        if (this.trailerselectedItem.length == 0) {
            this.editedTrailerType = true;
            this.TractorForm.markAllAsTouched();
        }
        if (this.TractorForm.valid && this.trailerselectedItem.length > 0) {
            this.IsLoading = true;
            this.TractorForm.get('TrailerType').setValue(this.trailerselectedItem);
            this.TractorForm.get('Drivers').setValue(this.driverselectedItem);
            this.tractorService.postCreateTractors(this.TractorForm.value).subscribe(data => {
                this.IsLoading = false;
                if (data.StatusCode == 0) {
                    Declarations.msgsuccess(data.StatusMessage, undefined, undefined);
                    closeSlidePanel();
                    this.clearTractorForm();
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
            this.TractorForm.markAllAsTouched();
        }
    }

    clearTractorForm() {
        this.TractorForm.reset();
        this.setDefaultValue();
    }

    setDefaultValue() {
        this.trailerselectedItem = [];
        this.driverselectedItem = [];
        this.trailerselectedItemValue = [];
        this.TractorForm.get('TrailerType').setValue(this.trailerselectedItem);
        this.TractorForm.get('Drivers').setValue(this.driverselectedItem);
        this.TractorForm.get("Status").setValue(TractorStatus.Active);
        this.editedTrailerType = false;
        this.editedDriver = false;
    }
    public onTrailerSelect(item: any): void {
        this.trailerselectedItem.push(item.Id);
        if (this.trailerselectedItem.length == 0) {
            this.editedTrailerType = true;
        }
        else {
            this.editedTrailerType = false;
        }
        this.getDriverdetails(false, null);
    }
    public OnTrailerSelectAll(trailer: DropdownItem[]) {
        trailer.forEach(item => {
            this.trailerselectedItem.push(item.Id);
        });
        if (this.trailerselectedItem.length == 0) {
            this.editedTrailerType = true;
        }
        else {
            this.editedTrailerType = false;
        }
        this.getDriverdetails(false, null);
    }
    public onTrailerDeSelect(item: any): void {
        if (this.trailerselectedItem != null && this.trailerselectedItem.length > 0 && item != null) {
            var index = this.trailerselectedItem.findIndex(indexd => indexd == item.Id);
            if (index !== -1) {
                this.trailerselectedItem.splice(index, 1);
            }
        }
        if (this.trailerselectedItem.length == 0) {
            this.editedTrailerType = true;
        }
        else {
            this.editedTrailerType = false;
        }
        this.getDriverdetailsTrailerDeSelect();
    }
    public OnTrailerDeSelectAll(trailer: DropdownItem[]) {
        this.trailerselectedItem = [];
        this.editedTrailerType = true;       
        this.getDriverdetailsTrailerDeSelect();
    }
    getDriverdetailsTrailerDeSelect() {
        if (this.trailerselectedItem.length > 0) {
            this.IsLoading = true;
            var trailerTypeId = this.trailerselectedItem.join(',');
            this.tractorService.getCompanyDrivers(trailerTypeId).subscribe(t => {
                this.IsLoading = false;
                this.DriverList = t as DropdownItem[];
                if (this.DriverList.length == 0) {
                    this.isdriverfound = true;
                }
                else {
                    this.isdriverfound = false;
                }
                this.driverselectedItem.forEach(xItem => {
                    var indexItem = this.DriverList.findIndex(top => top.Id === xItem.Id);
                    if (indexItem == -1) {
                        var indexSelectedItem = this.driverselectedItem.findIndex(top => top.Id === xItem.Id);
                        if (indexSelectedItem != -1) {
                            this.driverselectedItem.splice(indexSelectedItem, 1);
                            this.driverselectedItem.slice();//refresh data.
                            this.TractorForm.get('Drivers').patchValue(this.driverselectedItem);
                        }
                    }
                });
                
            });
        }
        else {
            this.DriverList = [];
            this.isdriverfound = true;
        }
    }
    getDriverdetails(editMode, tractorObj) {
        if (this.trailerselectedItem.length > 0) {
            this.IsLoading = true;
            var trailerTypeId = this.trailerselectedItem.join(',');
            this.tractorService.getCompanyDrivers(trailerTypeId).subscribe(t => {
                this.IsLoading = false;
                this.DriverList = t as DropdownItem[];
                if (this.DriverList.length == 0) {
                    this.isdriverfound = true;
                }
                else {
                    this.isdriverfound = false;
                }
                if (editMode) {
                    this.setDriverSelectedValues(tractorObj);
                }
                
            });
        }
        else {
            this.DriverList = [];
            this.isdriverfound = true;
            if (editMode) {
                this.setDriverSelectedValues(tractorObj);
            }
        }
    }
    setDriverSelectedValues(tractor) {
        //set the driver selected value.
        if (tractor.Drivers.length > 0) {
            this.editedDriver = false;
            tractor.Drivers.forEach((myObject) => {
                this.DriverList.forEach((drmyObject) => {
                    if (drmyObject.Id == myObject.Id) {
                        this.driverselectedItem.push(myObject);
                    }
                });

            });
            this.TractorForm.get('Drivers').patchValue(this.driverselectedItem);

        }
    }
    setSelectedDate(date: string) {
        var _date = this.TractorForm.get('ExpirationDate');
        if (_date.value != date) {
            _date.setValue(date);
        }
    }
    public onDriverSelect(item: any): void {
        this.driverselectedItem.push(item);
        if (this.driverselectedItem.length == 0) {
            this.editedDriver = true;
        }
        else {
            this.editedDriver = false;
        }
    }
    public OnDriverSelectAll(driver: DropdownItem[]) {
        driver.forEach(item => {
            this.driverselectedItem.push(item);
        });
        if (this.driverselectedItem.length == 0) {
            this.editedDriver = true;
        }
        else {
            this.editedDriver = false;
        }
    }
    public onDriverDeSelect(item: any): void {
        if (this.driverselectedItem != null && this.driverselectedItem.length > 0 && item != null) {
            var index = this.driverselectedItem.findIndex(indexd => indexd.Id == item.Id);
            if (index !== -1) {
                this.driverselectedItem.splice(index, 1);
            }
        }
        if (this.driverselectedItem.length == 0) {
            this.editedDriver = true;
        }
        else {
            this.editedDriver = false;
        }
    }
    public OnDriverDeSelectAll(trailer: DropdownItem[]) {
        this.driverselectedItem = [];
        this.editedDriver = true;
    }

    getDefaultUOM() {
        this.IsLoading = true;
        this.tractorService.getDefaultUOM().subscribe(response => {
            this.IsLoading = false;
            this.DefaultUoM = response;
        });
    }
}
