import { Component, OnInit, Output, EventEmitter } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { DriverViewModel } from '../../models/DriverManagementModel';
import { DropdownItem, DropdownItemExt } from 'src/app/statelist.service';
import { DriverService } from '../../services/driver.service';
import { Declarations } from 'src/app/declarations.module';
import { LicenceRequirement, TrailerType } from 'src/app/app.enum';
declare function closeSlidePanel(): any;

@Component({
    selector: 'app-create-driver',
    templateUrl: './create-driver.component.html',
    styleUrls: ['./create-driver.component.css']
})
export class CreateDriverComponent implements OnInit {
    public DriverModel: DriverViewModel;
    public MinDate: Date = new Date();
    public MaxDate: Date = new Date();
    public DriverForm: FormGroup;
    public TrailerTypeEnum: typeof TrailerType = TrailerType;
    public TrailerTypeList: DropdownItem[] = [];
    public LicenceTypeEnum: typeof LicenceRequirement = LicenceRequirement;
    public LicenceTypes: DropdownItemExt[] = [];
    //public DriverStatusEnum: typeof Status = Status;
    //public Statuses: DropdownItem[] = [];
    public RegionList: DropdownItem[] = [];
    public ContactNumberValidationMessage: string = "Invalid Contact number";
    public IsOnboarded: boolean = false;
    public IsLoading: boolean = false;
    public trailerDdlSettings = {};
    public regionDdlSettings = {};
    @Output() onSaveDriverData: EventEmitter<any> = new EventEmitter<any>();

    constructor(private fb: FormBuilder, private driverService: DriverService) {
        this.DriverForm = this.fb.group({
            DriverId: this.fb.control(0),
            FirstName: this.fb.control('', [Validators.required]),
            LastName: this.fb.control('', [Validators.required]),
            CompanyName: this.fb.control(''),
            Email: this.fb.control('', [Validators.required, Validators.pattern("^[A-Za-z0-9._%+-]+@[A-Za-z0-9.-]+\.[A-Za-z]{2,4}$")]),
            LicenseNumber: this.fb.control('', [Validators.required]),
            ExpiryDate: this.fb.control('', [Validators.required]),
            LicenseTypeId: this.fb.control(null),
            SelectedLicenseTypes: this.fb.control(null, [Validators.required]),
            TrailerType: this.fb.control(null),
            SelectedTrailerTypes: this.fb.control(null),
            ContactNumber: this.fb.control(''),
            //DriverStatus: this.fb.control(Status.Active),
            InvitedBy: this.fb.control(''),
            UserId: this.fb.control(0),
            //IsActive: this.fb.control(true),
            Regions: this.fb.control(null),
            SelectedRegions: this.fb.control(null),
            IsFilldAuthorized: this.fb.control(false),
        });
    }

    ngOnInit() {
        this.trailerDdlSettings = {
            singleSelection: false,
            idField: 'Id',
            textField: 'Name',
            selectAllText: 'Select All',
            unSelectAllText: 'UnSelect All',
            itemsShowLimit: 5,
            allowSearchFilter: true
        };

        this.regionDdlSettings = {
            singleSelection: true,
            idField: 'Code',
            textField: 'Name',
            itemsShowLimit: 5,
            allowSearchFilter: true
        };

        // load regions
        this.getRegions();

        this.TrailerTypeList = (Object.keys(this.TrailerTypeEnum).filter(k => typeof this.TrailerTypeEnum[k] === "number") as string[]).map(x => { return { Id: this.TrailerTypeEnum[x], Name: x, Code: "" } as DropdownItem });
        this.LicenceTypes = (Object.keys(this.LicenceTypeEnum).filter(k => typeof this.LicenceTypeEnum[k] === "number") as string[]).map(x => { return { Id: x, Name: x == "Class1" ? "Class 1" : x == "Class3" ? "Class 3" : x, Code: "" } as DropdownItemExt });
        //this.Statuses = (Object.keys(this.DriverStatusEnum).filter(k => typeof this.DriverStatusEnum[k] === "number") as string[]).map(x => { return { Id: this.DriverStatusEnum[x], Name: x == "InActive" ? "In-Active" : x, Code: "" } as DropdownItem });
        this.MaxDate.setFullYear(this.MinDate.getFullYear() + 30);
    }

    onSubmit() {
        // validate contact number     
        var contactNumber = this.DriverForm.get('ContactNumber').value;
        var licenseType = this.DriverForm.get('SelectedLicenseTypes').value;

        this.formatContactNumber(contactNumber);
        this.onLicenseTypeChange(licenseType);

        if (this.DriverForm.valid) {
            var driverForm = this.DriverForm.value;

            var driverId = (driverForm.DriverId == "" || driverForm.DriverId == null) ? 0 : driverForm.DriverId;
            this.DriverForm.get("DriverId").patchValue(driverId);

            var userId = (driverForm.UserId == "" || driverForm.UserId == null) ? 0 : driverForm.UserId;
            this.DriverForm.get("UserId").patchValue(userId);


            if (driverForm.SelectedTrailerTypes != null && driverForm.SelectedTrailerTypes.length > 0) {
                var trailerTypeIds = [];
                driverForm.SelectedTrailerTypes.forEach(t => { trailerTypeIds.push(this.TrailerTypeEnum[t.Name]); });
                this.DriverForm.get("TrailerType").patchValue(trailerTypeIds);
            }

            if (driverForm.SelectedRegions != null && driverForm.SelectedRegions.length > 0) {
                var regionIds = [];
                driverForm.SelectedRegions.forEach(t => { regionIds.push(t.Code) });
                this.DriverForm.get("Regions").patchValue(regionIds);
            }

            if (driverForm.SelectedLicenseTypes != null) {
                this.DriverForm.get("LicenseTypeId").patchValue(driverForm.SelectedLicenseTypes);
            }

            //if (driverForm.IsActive == undefined || driverForm.IsActive == null) {
            //    var isActive = driverForm.DriverStatus == 1 ? true : false;
            //    this.DriverForm.get("IsActive").patchValue(isActive);
            //}
           if(!driverForm.IsFilldAuthorized)
           { 
            this.DriverForm.get("IsFilldAuthorized").patchValue(false);
           }
            this.submitForm();
        }
        else {
            this.DriverForm.markAllAsTouched();
        }
    }

    loadDriverDetail(driver: DriverViewModel) {
        this.clearForm();
        if (driver.ContactNumber == 'NA') { driver.ContactNumber = null; }
        this.DriverForm.patchValue(driver);

        if (driver.UserId && driver.UserId > 0) {
            this.DriverForm.get("FirstName").disable();
            this.DriverForm.get("LastName").disable();
            this.DriverForm.get("Email").disable();
        }
        if (driver.TrailerType != null) {
            var trailerTypesToBind = this.TrailerTypeList.filter(t => driver.TrailerType.indexOf(t.Id) != -1);
            this.DriverForm.controls.SelectedTrailerTypes.setValue(trailerTypesToBind);
        }

        if (driver.Regions != null) {
            var regionsToBind = this.RegionList.filter(t => driver.Regions.indexOf(t.Code) != -1);
            this.DriverForm.controls.SelectedRegions.setValue(regionsToBind);
        }

        if (driver.LicenseTypeId != null)
            this.DriverForm.controls.SelectedLicenseTypes.setValue(driver.LicenseTypeId);
    }

    submitForm() {
        this.IsLoading = true;
        this.driverService.postAddDriver(this.DriverForm.getRawValue()).subscribe(data => {
            this.IsLoading = false;
            if (data.StatusCode == 0) {
                Declarations.msgsuccess(data.StatusMessage, undefined, undefined);
                closeSlidePanel();
                this.clearForm();
                this.onSaveDriverData.emit();
            }
            else if (data.StatusCode == 2) {
                Declarations.msgwarning(data.StatusMessage, undefined, undefined);
            }
            else {
                Declarations.msgerror(data == null ? 'Failed' : data.StatusMessage, undefined, undefined);
            }
        });
    }

    getRegions() {
        this.IsLoading = true;
        this.driverService.getRegions().subscribe(data => {
            this.IsLoading = false;
            this.RegionList = data as DropdownItem[];
        });
    }

    clearForm() {
        this.DriverForm.reset();
        this.setDefaultValue();
        this.DriverForm.get("FirstName").enable();
        this.DriverForm.get("LastName").enable();
        this.DriverForm.get("Email").enable();
    }

    setDefaultValue() {
        // this.DriverForm.get("DriverStatus").patchValue(Status.Active);
        this.DriverForm.get("Regions").patchValue(null);
        this.DriverForm.get("TrailerType").patchValue(null);

        this.IsOnboarded = false;
    }

    setSelectedDate(date: string) {
        var _date = this.DriverForm.get('ExpiryDate');
        if (_date.value != date) {
            _date.setValue(date);
        }
    }

    onLicenseTypeChange(licenseType: any) {
        if (licenseType == null || licenseType == "null")
            this.DriverForm.controls['SelectedLicenseTypes'].setErrors({ 'required': true });
        else
            this.DriverForm.controls['SelectedLicenseTypes'].setErrors(null);
    }

    formatContactNumber(contactNumber: string) {
        if (contactNumber != null && contactNumber != '') {
            contactNumber = contactNumber.split('-').join("");
            if (contactNumber.length == 10) {
                contactNumber = contactNumber.replace(/(\d{3})(\d{3})(\d{4})/, "$1-$2-$3")

                this.DriverForm.controls['ContactNumber'].setErrors(null);
                this.DriverForm.get("ContactNumber").patchValue(contactNumber);
            }
            else {
                this.DriverForm.controls['ContactNumber'].setErrors({ 'incorrect': true });
            }
        }
        else {
            this.DriverForm.controls['ContactNumber'].setErrors(null);
        }
    }
}
