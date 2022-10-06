import { ChangeDetectorRef, Component, ElementRef, OnInit, ViewChild } from '@angular/core';
import { FormArray, FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { InvitationService } from './invitation.service';
import { CarrierOnboardingBrandingModel, ServiceArea, StateListViewModel } from './third-party-invitation.model';
import { Declarations } from '../declarations.module';
import { RegExConstants } from '../app.constants';
import { Geocode } from '../carrier/schedule-builder/add-location/add-location.model';
import { debounceTime, distinctUntilChanged } from 'rxjs/operators';
import { ActivatedRoute, Router } from '@angular/router';
import { DropdownItem, StatelistService } from '../statelist.service';
import { ServiceOfferingType, WizardTabEnum } from 'src/app/app.enum';

@Component({
    selector: 'app-invitation',
    templateUrl: './invitation.component.html',
    styleUrls: ['./invitation.component.css']
})
export class InvitationComponent implements OnInit {

    public pageloader = false;
    public offeringloader = false;
    public _loadingAddress = false;
    public wizardForm: FormGroup;
    public emailExists = false;
    public isPhoneNumberValid = null;
    //public isSubmitted: boolean = false;

    public CountryList: DropdownItem[] = [];
    public statesList: StateListViewModel[] = [];
    public carrierOnboarding : CarrierOnboardingBrandingModel;
    public dataForEachServiceType: object = {};

    public filteredcityList = [];
    public invitedCompanyTypes = [];
    public AllTrailerAssetTypes: any = null;
    public PhoneTypes = [];
    public DdlSettings: any = {};
    public ZipDdlSettings: any = {};
    public ddlCitySettings: any = {};
    public formSubmited = false;
    public ServiceOfferingTypes: typeof ServiceOfferingType = ServiceOfferingType;
    public ServiceOfferingTypesDisplay: { [index: number]: string } = {};
    //fuel assets
    public fuelAssetForm: FormGroup;
    public _opened: boolean = false;
    public _initiated: boolean = false;
    //active wizard
    WizardTabEnum = WizardTabEnum;
    activeWizard = WizardTabEnum.ContactInfo;
    threshold: number = 1.0;
    @ViewChild('ContactInformation') ContactInformation: ElementRef;
    @ViewChild('CompanyInformation') CompanyInformation: ElementRef;
    @ViewChild('FleetInformation') FleetInformation: ElementRef;
    @ViewChild('ServiceOffering') ServiceOffering: ElementRef;
    //service offerings
    activeServiceOffering = ServiceOfferingType.FTL;

    //Branding
    logoImage = "../../../Content/images/truefill-logo.png";
    backgroundImage = "";

    //get accessors
    get f() { return this.wizardForm.controls; }
    get getFuelTrailerAssetTypeName(): any { return (parameter: number) => this.AllTrailerAssetTypes?.FuelTrailerAssetType.find((x: { Id: number; }) => x.Id == parameter).Name; }
    get getDefTrailerAssetTypeName(): any { return (parameter: number) => this.AllTrailerAssetTypes?.DefTrailerAssetType.find((x: { Id: number; }) => x.Id == parameter).Name; }
    get StatesListByCountry(): any[] { return this.statesList.filter(t => t.CountryId == this.f.CompanyInfo.get('CountryId').value) }
    get StatesListByCountryForService(): any { return (countryId: number) => this.statesList.filter(x => x.CountryId == countryId); }

    constructor(
        private route: ActivatedRoute,
        private router: Router,
        private fb: FormBuilder,
        private invitationService: InvitationService,
        private cdr: ChangeDetectorRef) {
        this.initailizeThirdPartyInviteForm();
        this.fuelAssetForm = this.getFuelAssetsFormGroup(true);
    }

    ngOnInit() {
        this.GetCarrierOnboardingForBranding();
        this.getCountryList();
        this.getStatesOfAllCountries();
        this.getThirdPartyCompanyTypes();
        this.getPhoneTypes();
        this.GetAllTrailerAssetTypes();
        this.GetCompanies();
        this.InitializeServiceDropdown();

        this.DdlSettings = {
            singleSelection: false,
            idField: 'Id',
            textField: 'Name',
            selectAllText: 'Select All',
            unSelectAllText: 'UnSelect All',
            itemsShowLimit: 2,
            allowSearchFilter: true,
            enableCheckAll: true
        }
        this.ZipDdlSettings = {
            singleSelection: false,
            idField: 'ZipCode',
            textField: 'ZipCode',
            selectAllText: 'Select All',
            unSelectAllText: 'UnSelect All',
            itemsShowLimit: 2,
            allowSearchFilter: true,
            enableCheckAll: true
        }
        this.ddlCitySettings = {
            singleSelection: false,
            idField: 'CityId',
            textField: 'CityName',
            selectAllText: 'Select All',
            unSelectAllText: 'UnSelect All',
            itemsShowLimit: 2,
            allowSearchFilter: true,
            enableCheckAll: true
        }
    }

    GetCarrierOnboardingForBranding() {
        let token = this.route.snapshot.queryParams.token;
        this.invitationService.GetCarrierOnboardingForBranding(token).subscribe(response => {
            if (response && response.IsBrandMyWebsite) {
                this.carrierOnboarding = response;
                this.logoImage = this.carrierOnboarding.ImageFilePath;
                this.backgroundImage = this.carrierOnboarding.CarrierOnboardingImageFilePath;
            }
        });
    }

    removeBtnPrimaryClass(template: any) {
        template.classList.remove('btn-primary');
    }
    getHeaderColor(): object {
        if (this.carrierOnboarding && this.carrierOnboarding.IsBrandMyWebsite && this.carrierOnboarding.HeaderColor)
            return { "background-color": this.carrierOnboarding.HeaderColor };
        else
            return {};
    }

    getButtonColor(): object {
        if (this.carrierOnboarding && this.carrierOnboarding.IsBrandMyWebsite && this.carrierOnboarding.ButtonColor)
            return { "background-color": this.carrierOnboarding.ButtonColor,"color":"white" ,"border":"none"};
        else
            return {};
    }

  

    InitializeServiceDropdown() {
        this.ServiceOfferingTypesDisplay[this.ServiceOfferingTypes[ServiceOfferingType.FTL]] = "FTL";
        this.ServiceOfferingTypesDisplay[this.ServiceOfferingTypes[ServiceOfferingType.LTL]] = "LTL";
        this.ServiceOfferingTypesDisplay[this.ServiceOfferingTypes[ServiceOfferingType.DEF]] = "DEF";
        this.ServiceOfferingTypesDisplay[this.ServiceOfferingTypes[ServiceOfferingType.LTLWetHosing]] = "LTL Wet Hosing";
        this.ServiceOfferingTypesDisplay[this.ServiceOfferingTypes[ServiceOfferingType.DEFWetHosing]] = "DEF Wet Hosing";
    }

    initailizeThirdPartyInviteForm() {
        this.wizardForm = this.fb.group({
            UserInfo: this.fb.group({
                Id: this.fb.control(null),
                Title: this.fb.control(null, [Validators.required]),
                FirstName: this.fb.control(null, [Validators.required]),
                LastName: this.fb.control(null, [Validators.required]),
                Email: this.fb.control(null, [Validators.required, Validators.pattern(RegExConstants.Email)]),
            }),
            CompanyInfo: this.fb.group({
                IsNewCompany: this.fb.control(true),
                CompanyName: this.fb.control(null, [Validators.required]),
                CompanyTypeId: this.fb.control(null, [Validators.required]),
                CompanyAddress: this.fb.control(null, [Validators.required]),
                CountryId: this.fb.control(1, [Validators.required]),
                StateId: this.fb.control(null, [Validators.required]),
                CountryName: this.fb.control(null),
                StateName: this.fb.control(null),
                City: this.fb.control(null, [Validators.required]),
                County: this.fb.control(null, [Validators.required]),
                Zip: this.fb.control(null, [Validators.required]),
                PhoneType: this.fb.control(null, [Validators.required]),
                PhoneNumber: this.fb.control(null, [Validators.required, Validators.pattern(RegExConstants.Phone)]),
            }),
            FleetInfo: this.fb.group({
                FuelAssets: this.fb.array([]),
                DefAssets: this.fb.array([])
            }),
            ServiceOffering: this.fb.array([]),
            Token: this.fb.control(this.route.snapshot.queryParams.token)
        });
        this.buildServiceOffering();
        this.bindLocalStorageData();
    }

    buildServiceOffering() {
        let serviceOffers = this.wizardForm.get('ServiceOffering') as FormArray;
        let serviceOfferings = Object.keys(ServiceOfferingType).filter(f => !isNaN(Number(f)));

        for (let index in serviceOfferings) {
            serviceOffers.push(this.fb.group({
                IsEnable: this.fb.control(null),
                AreaWide: this.fb.control(1),
                ServiceDeliveryType: [this.ServiceOfferingTypes[+index + 1]],
                ServiceAreas: this.fb.control(null),
                SelectedCountry: this.fb.control(null),
                SelectedStates: this.fb.control([]),
                SelectedCities: this.fb.control([]),
                SelectedZipCodes: this.fb.control([]),
            }));

            this.dataForEachServiceType[this.ServiceOfferingTypes[+index + 1] + '_CitiesByState'] = [] as ServiceArea[];
            this.dataForEachServiceType[this.ServiceOfferingTypes[+index + 1] + '_ZipCodesByCities'] = [] as ServiceArea[];
            this.dataForEachServiceType[this.ServiceOfferingTypes[+index + 1] + '_ApiData'] = [] as ServiceArea[];
        }
    }

    getFuelAssetsFormGroup(isFuelAssets: boolean) {
        return this.fb.group({
            FuelTrailerServiceTypeFTL: this.fb.control(null, isFuelAssets ? [Validators.required] : []),
            DEFTrailerServiceType: this.fb.control(null, !isFuelAssets ? [Validators.required] : []),
            Capacity: this.fb.control(null, [Validators.required, Validators.min(0.0001)]),
            TrailerHasPump: this.fb.control(false),
            IsTrailerMetered: this.fb.control(false),
            Count: this.fb.control(null, [Validators.required, Validators.min(1)]),
            PackagedGoods: this.fb.control(false),
            IsFuelAssets: this.fb.control(isFuelAssets),
        })
    }

    openFuelAssetForm(isFuelAssets: boolean) {
        this._opened = true;
        this.fuelAssetForm = this.getFuelAssetsFormGroup(isFuelAssets);
    }

    removeAsset(index: number, isFuelAssets: boolean) {
        let tempArray: FormArray;
        if (isFuelAssets) {
            tempArray = this.f.FleetInfo.get('FuelAssets') as FormArray;
        }
        else {
            tempArray = this.f.FleetInfo.get('DefAssets') as FormArray;
        }
        tempArray.removeAt(index);
    }

    getAddressByZip(zipCode: string) {
        if (zipCode) {
            this.invitationService.GetAddressByZip(zipCode).subscribe((response) => {
                if (response) {
                    this.updateAddress(response)
                }
            });
        }
    }

    updateAddress(address: Geocode) {
        if (address.CountryCode && address.StateName) {

            let countryId = (address.CountryCode == 'US') ? 1 : (address.CountryCode == 'CA' ? 2 : this.f.CompanyInfo.get('CountryId').value);
            let state = this.statesList.find(st => st.Name.toLowerCase() == address.StateName.toLowerCase());
            let stateId = (state && state.Id) ? state.Id : this.f.CompanyInfo.get('StateId').value;
            if (address.Address && address.Address != '' && this.f.CompanyInfo.get('CompanyAddress').value != '') {
                this.f.CompanyInfo.get('CompanyAddress').patchValue(address.Address);
            }
            this.f.CompanyInfo.get('City').patchValue(address.City);
            this.f.CompanyInfo.get('County').patchValue(address.CountyName);
            this.f.CompanyInfo.get('StateId').patchValue(stateId);
            this.f.CompanyInfo.get('CountryId').patchValue(countryId);
        }
    }

    onSubmitFleetInfo() {
        this._opened = false;
        let _fmArray: FormArray;
        if (this.fuelAssetForm.get('IsFuelAssets').value) {
            _fmArray = this.wizardForm.get('FleetInfo').get('FuelAssets') as FormArray;
        }
        else {
            _fmArray = this.wizardForm.get('FleetInfo').get('DefAssets') as FormArray;
        }
        _fmArray.push(this.fuelAssetForm)
    }

    public serviceCountryChanged(serviceOffering: FormGroup) {
        serviceOffering.get('SelectedStates').setValue([]);
        serviceOffering.get('SelectedCities').setValue([]);
        serviceOffering.get('SelectedZipCodes').setValue([]);
    }

    public GetCompanies() {
        this.pageloader = true;
        this.invitationService.GetCompanies().subscribe((data => {
            this.pageloader = false;
            if (data) {
                this.Companies = data;
            }
        }));
    }

    public companySeleted(data: any) {
        this.f.CompanyInfo.get('IsNewCompany').patchValue(false);
        this.disableCompanyControls(true);
        this.cdr.detectChanges();
    }

    Companies: any[] = [];
    companyLoader = false;
    public isCompanyNameExist(cName: string) {
        if (cName) {
            let _this = this;
            this.companyLoader = true;
            this.invitationService.IsCompanyNameExist(this.f.CompanyInfo.get('IsNewCompany').value, this.f.CompanyInfo.get('CompanyName').value).subscribe(data => {
                if(typeof _this.f.CompanyInfo.get('CompanyName').value !== 'object'){
                    _this.f.CompanyInfo.get('IsNewCompany').patchValue(!data);
                    //_this.cdr.detectChanges();
                }
                _this.disableCompanyControls(!_this.f.CompanyInfo.get('IsNewCompany').value);
                this.companyLoader = false;
            });
        }
    }

    disableCompanyControls(data: boolean){
        if (data) {
            this.f.CompanyInfo.get('CompanyTypeId').disable();
            this.f.CompanyInfo.get('CompanyAddress').disable();
            this.f.CompanyInfo.get('CountryId').disable();
            this.f.CompanyInfo.get('StateId').disable();
            this.f.CompanyInfo.get('CountryName').disable();
            this.f.CompanyInfo.get('StateName').disable();
            this.f.CompanyInfo.get('City').disable();
            this.f.CompanyInfo.get('County').disable();
            this.f.CompanyInfo.get('Zip').disable();
            this.f.CompanyInfo.get('PhoneType').disable();
            this.f.CompanyInfo.get('PhoneNumber').disable();
        }
        else {
            this.f.CompanyInfo.get('CompanyTypeId').enable();
            this.f.CompanyInfo.get('CompanyAddress').enable();
            this.f.CompanyInfo.get('CountryId').enable();
            this.f.CompanyInfo.get('StateId').enable();
            this.f.CompanyInfo.get('CountryName').enable();
            this.f.CompanyInfo.get('StateName').enable();
            this.f.CompanyInfo.get('City').enable();
            this.f.CompanyInfo.get('Zip').enable();
            this.f.CompanyInfo.get('PhoneType').enable();
            this.f.CompanyInfo.get('PhoneNumber').enable();
        }
    }
    public getCountryList() {
        this.invitationService.GetCountryList().subscribe(data => {
            if (data && data.length > 0) {
                this.CountryList = data;
            }
        });
    }

    public getStatesOfAllCountries() {
        this.invitationService.GetStatesOfAllCountries().subscribe(data => {
            if (data && data.length > 0) {
                this.statesList = data;
            }
        });
    }

    public getThirdPartyCompanyTypes() {
        this.invitationService.GetThirdPartyCompanyTypes().subscribe(data => {
            if (data && data.length > 0) {
                this.invitedCompanyTypes = data;
            }
        });
    }

    public GetAllTrailerAssetTypes() {
        this.invitationService.GetAllTrailerAssetTypes().subscribe(data => {
            if (data) {
                this.AllTrailerAssetTypes = data;
            }
        });
    }


    stateChanged(serviceOffering: FormGroup, index: number, newStateAdded: boolean, _selectedStates: any[]) {

        this.dataForEachServiceType[this.ServiceOfferingTypes[+index + 1] + '_CitiesByState'] = [] as ServiceArea[];
        this.dataForEachServiceType[this.ServiceOfferingTypes[+index + 1] + '_ApiData'] = [] as ServiceArea[];

        if (_selectedStates.length == 0) {
            serviceOffering.get('SelectedCities').patchValue([]);
            serviceOffering.get('SelectedZipCodes').patchValue([]);
            this.dataForEachServiceType[this.ServiceOfferingTypes[+index + 1] + '_ZipCodesByCities'] = [];
            return;
        }

        var stateslist = _selectedStates.map(t => t.Id).join(",");
        this.offeringloader = true;

        this.invitationService.GetCitiesFromStates(stateslist).pipe(debounceTime(1000)).subscribe(response => {

            if (response && response.length > 0) {
                this.dataForEachServiceType[this.ServiceOfferingTypes[+index + 1] + '_ApiData'] = response;
                this.dataForEachServiceType[this.ServiceOfferingTypes[+index + 1] + '_CitiesByState'] = response.filter((thing, i, arr) => {
                    return arr.indexOf(arr.find(t => t.CityId === thing.CityId)) === i;
                }) as ServiceArea[];
            }
            else if (!response) {
                Declarations.msgerror("Failed to load Cities.", "Failed", null)
            }
            if (!newStateAdded) {
                this.removeSelectedCitiesOfRemovedState(serviceOffering.get('SelectedCities') as FormControl, index);
            }

            this.offeringloader = false;;
        });
    }

    public stateChangedSingle(serviceOffering: FormGroup, index: number, newStateAdded: boolean) {
        let _areawide = serviceOffering.get('AreaWide').value as number;
        if (_areawide == 2) {
            let _selectedStates = serviceOffering.get('SelectedStates').value as any[];
            this.stateChanged(serviceOffering, index, newStateAdded, _selectedStates);
        }
    }

    public stateChangedAll(serviceOffering: FormGroup, index: number, newStateAdded: boolean, _selectedStates: any[]) {
        let _areawide = serviceOffering.get('AreaWide').value as number;
        if (_areawide == 2) {
            document.getElementById("stateDiv").click();
            this.stateChanged(serviceOffering, index, newStateAdded, _selectedStates);
        }
    }

    public removeSelectedCitiesOfRemovedState(selectedCitiesForm: FormControl, index: number) {
        let selectedCities = selectedCitiesForm.value as ServiceArea[];
        if (selectedCities.length > 0) {

            let availableCities = this.dataForEachServiceType[this.ServiceOfferingTypes[+index + 1] + '_CitiesByState'] as ServiceArea[];
            let finalCities = [];
            selectedCities.forEach(selectedCity => {
                if (availableCities.find(c => c.CityId == selectedCity.CityId)) {
                    finalCities.push(selectedCity)
                }
            });
            selectedCitiesForm.patchValue(finalCities);
        }
    }

    public cityChangedSingle(serviceOffering: FormGroup, index: number, newCityAdded: boolean) {
        let _selectedCities = serviceOffering.get('SelectedCities').value as any[];
        this.cityChanged(serviceOffering, index, newCityAdded, _selectedCities);
    }

    public cityChangedAll(serviceOffering: FormGroup, index: number, newCityAdded: boolean, _selectedCities: any[]) {
        this.cityChanged(serviceOffering, index, newCityAdded, _selectedCities);
    }

    public cityChanged(serviceOffering: FormGroup, index: number, newCityAdded: boolean, _selectedCities: any[]) {

        if (_selectedCities.length == 0) {
            this.dataForEachServiceType[this.ServiceOfferingTypes[+index + 1] + '_ZipCodesByCities'] = [] as ServiceArea[];
            serviceOffering.get('SelectedZipCodes').patchValue([]);
            return;
        }

        let _selectedCityIds: number[] = _selectedCities.map(c => { return c.CityId; })

        let allZipcodes = this.dataForEachServiceType[this.ServiceOfferingTypes[+index + 1] + '_ApiData'] as ServiceArea[];
        this.dataForEachServiceType[this.ServiceOfferingTypes[+index + 1] + '_ZipCodesByCities'] = allZipcodes.filter(c => _selectedCityIds.includes(c.CityId)) as ServiceArea[];

        if (!newCityAdded) {
            this.removeSelectedZipsOfRemovedCities(serviceOffering.get('SelectedZipCodes') as FormControl, index);
        }
    }

    public removeSelectedZipsOfRemovedCities(selectedZipsForm: FormControl, index: number) {
        let selectedZips = selectedZipsForm.value as ServiceArea[];
        if (selectedZips.length > 0) {

            let availableZips = this.dataForEachServiceType[this.ServiceOfferingTypes[+index + 1] + '_ZipCodesByCities'] as ServiceArea[];
            let finalZips = []
            selectedZips.forEach(zip => {
                if (availableZips.find(c => c.ZipCode == zip.ZipCode)) {
                    finalZips.push(zip)
                }
            });
            selectedZipsForm.patchValue(finalZips);
        }
    }

    public countryChanged() {
        this.f.CompanyInfo.get('CompanyAddress').setValue(null);
        this.f.CompanyInfo.get('Zip').setValue(null);
        this.f.CompanyInfo.get('City').setValue(null);
        this.f.CompanyInfo.get('County').setValue(null);
        this.f.CompanyInfo.get('StateId').setValue(null);
    }

    public isEmailExist() {
        this.emailExists = false;
        if (this.f.UserDetails.get('Email').value) {
            this.invitationService.IsEmailExist(this.f.UserInfo.get('Email').value).subscribe(data => {
                if (data != null || data != undefined) {
                    this.emailExists = data;
                }
            });
        }
    }

    public getPhoneTypes() {
        this.invitationService.GetPhoneTypes().subscribe(data => {
            if (data && data.length > 0) {
                this.PhoneTypes = data;
            }
        });
    }

    public IsPhoneNumberValid(phoneNumber: string) {
        this.isPhoneNumberValid = null;
        if (phoneNumber) {
            this.invitationService.IsPhoneNumberValid(phoneNumber).subscribe(data => {
                if (data != null || data != undefined) {
                    this.isPhoneNumberValid = data;
                }
            });
        }
    }

    public scrollToElemen(id: WizardTabEnum): void {
        this.activeWizard = id;
    }

    public setServiceQuestion(id: ServiceOfferingType) {
        this.activeServiceOffering = id;
    }

    public setLocalStorageData(): void {
        localStorage.setItem('wizardData', JSON.stringify(this.wizardForm.value));
    }

    public goToNextQuestion(){

        if(this.activeServiceOffering != this.ServiceOfferingTypes.DEFWetHosing){
            this.activeServiceOffering = +this.activeServiceOffering+1;
        }
    }

    public scrollToElement(id: WizardTabEnum) {
        this.activeWizard = id;
        if (id == WizardTabEnum.CompanyInfo) {
            this.CompanyInformation.nativeElement.scrollIntoView({ behavior: "smooth", block: "start", inline: "nearest" });
        } else if (id == WizardTabEnum.FleetInfo) {
            this.FleetInformation.nativeElement.scrollIntoView({ behavior: "smooth", block: "start", inline: "nearest" });
        } else if (id == WizardTabEnum.ServiceOfferings) {
            this.ServiceOffering.nativeElement.scrollIntoView({ behavior: "smooth", block: "start", inline: "nearest" });
        } else {
            this.activeWizard = WizardTabEnum.ContactInfo;
            this.ContactInformation.nativeElement.scrollIntoView({ behavior: "smooth", block: "start", inline: "nearest" });
        }
    }

    public onFinishAndSave() {
        this.setLocalStorageData();
    }

    updateServiceValidation(serviceOffering: FormGroup, serviceEnabled: boolean, areaWide: number){

        this.updateFormControlValidators(serviceOffering.get('SelectedCountry'), [])
        this.updateFormControlValidators(serviceOffering.get('SelectedStates'), [])
        this.updateFormControlValidators(serviceOffering.get('SelectedCities'), [])
        this.updateFormControlValidators(serviceOffering.get('SelectedZipCodes'), [])

        if (serviceEnabled) {

            this.updateFormControlValidators(serviceOffering.get('SelectedCountry'), [Validators.required])
            this.updateFormControlValidators(serviceOffering.get('SelectedStates'), [Validators.required])

            if (areaWide==2) {
                this.updateFormControlValidators(serviceOffering.get('SelectedCities'), [Validators.required])
                this.updateFormControlValidators(serviceOffering.get('SelectedZipCodes'), [Validators.required])
            }
        }
    }

    updateFormControlValidators(control: any, validators: any[]) {
        control.setValidators(validators);
        control.updateValueAndValidity();
     }

    public bindLocalStorageData(): void {
        let wizardFormData = localStorage.getItem('wizardData')
        if (wizardFormData) {
            let wizardFormDataJSON = JSON.parse(wizardFormData);
            this.f.UserInfo.patchValue(wizardFormDataJSON.UserInfo);
            this.f.CompanyInfo.patchValue(wizardFormDataJSON.CompanyInfo);
            this.f.ServiceOffering.patchValue(wizardFormDataJSON.ServiceOffering);

            // this.f.CompanyInfo.get('IsNewCompany').patchValue(!data);

            let FuelAssets = this.f.FleetInfo.get('FuelAssets') as FormArray;
            wizardFormDataJSON.FleetInfo.FuelAssets.forEach((fuelAsset: any) => {
                FuelAssets.push(this.fb.group({
                    FuelTrailerServiceTypeFTL: this.fb.control(fuelAsset.FuelTrailerServiceTypeFTL, [Validators.required]),
                    DEFTrailerServiceType: this.fb.control(fuelAsset.DEFTrailerServiceType, []),
                    Capacity: this.fb.control(fuelAsset.Capacity, [Validators.required, Validators.min(0.0001)]),
                    TrailerHasPump: this.fb.control(fuelAsset.TrailerHasPump),
                    IsTrailerMetered: this.fb.control(fuelAsset.IsTrailerMetered),
                    Count: this.fb.control(fuelAsset.Count, [Validators.required, Validators.min(1)]),
                    PackagedGoods: this.fb.control(fuelAsset.PackagedGoods),
                    IsFuelAssets: this.fb.control(true),
                }))
            });

            let DefAssets = this.f.FleetInfo.get('DefAssets') as FormArray;
            wizardFormDataJSON.FleetInfo.DefAssets.forEach((defAssets: any) => {
                DefAssets.push(this.fb.group({
                    FuelTrailerServiceTypeFTL: this.fb.control(defAssets.FuelTrailerServiceTypeFTL, []),
                    DEFTrailerServiceType: this.fb.control(defAssets.DEFTrailerServiceType, [Validators.required]),
                    Capacity: this.fb.control(defAssets.Capacity, [Validators.required, Validators.min(0.0001)]),
                    TrailerHasPump: this.fb.control(defAssets.TrailerHasPump),
                    IsTrailerMetered: this.fb.control(defAssets.IsTrailerMetered),
                    Count: this.fb.control(defAssets.Count, [Validators.required, Validators.min(1)]),
                    PackagedGoods: this.fb.control(defAssets.PackagedGoods),
                    IsFuelAssets: this.fb.control(false),
                }))
            });

            if(!this.f.CompanyInfo.get('IsNewCompany').value){
                this.f.CompanyInfo.get('IsNewCompany').markAllAsTouched();
                //this.isCompanyNameExist(this.f.CompanyInfo.get('CompanyName').value?.Name);
                this.disableCompanyControls(!this.f.CompanyInfo.get('IsNewCompany').value);
            }
        }
    }
    isSubmitted = false;
    public onSubmit() {
        
        this.formSubmited = true;

        if (!this.f.Token.value) {
            Declarations.msgwarning("Invalid invitation link.", undefined, undefined);
            return;
        }

        if (this.wizardForm.valid) {
            if (!this.f.CompanyInfo.get('IsNewCompany').value) {
                let input = this.wizardForm.value;
                input.CompanyInfo.IsNewCompany = false;

                this.pageloader = true;
                this.invitationService.SaveInvitedRequest(input).subscribe(response => {
                    this.pageloader = false;
                    localStorage.setItem('wizardData', '');
                    if (response && response.StatusCode == 0 && response.EntityId) {
                        this.isSubmitted = true;
                        //this.router.navigate(['/Invitation/Submit']); 
                        //this.router.navigateByUrl('/Submit');  // open welcome component
                        //Declarations.msgsuccess("Thank You for your information. email will be send to Company Admin to confirm account", undefined, undefined);
                        //Declarations.msgsuccess("Request created successfully.", undefined, undefined);
                        //this.router.navigate([window.location.href = "/Account/Register?supplierURL=&invitationId=" + response.EntityId]);
                    }
                    else {
                        Declarations.msgerror("Failed", undefined, undefined);
                    }
                });
                return;
            }
            else {
                //SET SERVICE OFFERINGS
                let serviceOffers = this.wizardForm.get('ServiceOffering') as FormArray;
                serviceOffers.controls.forEach((serviceOffer: FormControl, index: number) => {
                    let _serviceOffer = serviceOffer.value;
                    if (_serviceOffer.IsEnable && _serviceOffer.SelectedStates.length > 0) {
                        if (_serviceOffer.AreaWide == 2 && _serviceOffer.SelectedZipCodes.length > 0) {
                            let allZipCodes = this.dataForEachServiceType[this.ServiceOfferingTypes[+index + 1] + '_ZipCodesByCities'] as ServiceArea[];
                            let selectedZips = _serviceOffer.SelectedZipCodes.map((a: { ZipCode: string; }) => a.ZipCode) as string[];
                            serviceOffer.get('ServiceAreas').setValue(allZipCodes.filter(a => selectedZips.includes(a.ZipCode)));
                        }
                        else {
                            let allStates: ServiceArea[] = [];
                            _serviceOffer.SelectedStates.forEach((t) => { allStates.push({ StateId: t.Id } as ServiceArea); });
                            serviceOffer.get('ServiceAreas').setValue(allStates);
                        }
                    }
                });

                this.pageloader = true;
                this.invitationService.SaveInvitedRequest(this.wizardForm.value).subscribe(response => {
                    this.pageloader = false;
                    localStorage.setItem('wizardData', '');
                    if (response && response.StatusCode == 0 && response.EntityId) {
                        Declarations.msgsuccess("Request created successfully.", undefined, undefined);
                        if (response.EntityNumber == null || response.EntityNumber == "" || response.EntityNumber == undefined)
                            this.router.navigate([window.location.href = "/Account/Register?supplierURL=&invitationId=" + response.EntityId]);
                        else
                            this.router.navigate([window.location.href = "/Account/Register?supplierURL=" + response.EntityNumber +"&invitationId=" + response.EntityId]);
                    }
                    else {
                        Declarations.msgerror("Failed", undefined, undefined);
                    }
                });
            }


        }
    }
}