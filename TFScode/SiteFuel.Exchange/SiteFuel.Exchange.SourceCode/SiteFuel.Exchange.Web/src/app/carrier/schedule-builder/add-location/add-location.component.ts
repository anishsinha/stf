import { Component, Input, OnInit, Output, EventEmitter, ViewEncapsulation, ViewChild, OnDestroy } from '@angular/core';
import { AbstractControl, FormArray, FormBuilder, FormGroup, ValidatorFn, Validators } from '@angular/forms';
import { AddLocationService } from './add-location.service';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { UtilService } from 'src/app/services/util.service';
import { Geocode, MapConstants, StateModel } from './add-location.model';
import { DropdownItem, StatelistService } from 'src/app/statelist.service';
import { AddressService } from 'src/app/address.service';
import { Declarations } from 'src/app/declarations.module';
import { AddressModel } from 'src/app/invoice/models/DropDetail';
import * as moment from 'moment';
import { ConfirmationDialogService } from 'src/app/shared-components/confirmation-dialog/confirmation-dialog.service';
import { DataService } from 'src/app/services/data.service';
import { Subscription } from 'rxjs';
import { PricingSectionComponent } from '../../../shared-components/pricing-section/pricing-section.component';
import { Country, TierPricingType } from 'src/app/app.enum';
import { DropDown, RegExConstants } from 'src/app/app.constants';


@Component({
    selector: 'app-add-location',
    templateUrl: './add-location.component.html',
    styleUrls: ['./add-location.component.scss'],
    encapsulation: ViewEncapsulation.None
})
export class AddLocationComponent implements OnInit, OnDestroy {

    @Input() RegionId: FormGroup;
    @Input() MapsApiKey: string;
    @Output() OnTheFlyLocationCreate: EventEmitter<any> = new EventEmitter<any>();
    @ViewChild(PricingSectionComponent) private pricingModuleComponent: PricingSectionComponent;
    locationForm: FormGroup;
    PickupForm: FormGroup;
    formSubmited = false;

    _isLoadingProducts = false;
    _opened: boolean = false;
    _loading: boolean = false;
    _initiated: boolean = false;

    mapConstants: MapConstants = new MapConstants();
    preferencesSetting: any = null;
    isCompanyNameExist: boolean = false;
    isJobNameExist: boolean = false;
    isPhoneNumberValid: boolean = null;

    allJobList: Array<DropDown> = [];
    currucyList = [];
    CountryList: DropdownItem[] = [];
    CountryGroupList: DropdownItem[] = [];
    statesList: StateModel[] = [];
    filteredStatesList: StateModel[] = [];
    FuelProductsList = [];
    MarineProductsList = [];
    FeeTypesList = [];
    defaultCompanyCountryId = 0;
    CompanyContactPersonsList = [];

    CompanyContactPersonsDetails: any;
    AllTPOCompaniesList: Array<DropDown> = [];
    //pickup for dr
    BulkPlants: DropdownItem[] = [];
    BulkplantList: DropdownItem[] = [];
    isReadOnly: boolean = false;
    UoM: number = 0;
    Terminals = [];
    _minimumConst = 0.00001;
    CountryBasedZipcodeLabel: string = "Zip";
    _loadingAddress: boolean = false;
    IsTBDRequest: boolean = false;
    FuelTypeId: number = null;
    ProductTypeName: string = "";
    productDetails: any;
    tbdDrProductId: number;
    tbdDrProductTypeId: number;
    public CompDdlSetting = {};

    constructor(
        private fb: FormBuilder,
        private addLocationService: AddLocationService,
        private utilService: UtilService,
        private modalService: NgbModal,
        private stateService: StatelistService,
        private addresService: AddressService,
        private confirmationDialogService: ConfirmationDialogService,
        private dataService: DataService,) { }

    get f() { return this.locationForm['controls']; }

    tbdAddLocationSubscription: Subscription = new Subscription();

    ngOnInit() {
        this.subscribeTbdAddLocation();
        this.CompDdlSetting = {
            singleSelection: true,
            closeDropDownOnSelection: true,
            idField: 'id',
            textField: 'text',
            enableCheckAll: false,
            itemsShowLimit: 1,
            allowSearchFilter: true
        };
    }

    ngOnDestroy() { this.tbdAddLocationSubscription ? this.tbdAddLocationSubscription.unsubscribe() : null; }

    onCompSelect(company: any, isSelected: boolean) {

        if(isSelected){
          this.f.CustomerDetails.get('CompanyId').setValue(company.id);
          this.companyChanged(company.id);
        }
        else{
          this.CompanyContactPersonsList = [];
          this.allJobList = [];
        }
    
        this.f.CustomerDetails.get('UserId').setValue(null);
        this.f.CustomerDetails.get('PhoneNumber').setValue(null);
        this.f.CustomerDetails.get('Email').setValue(null);
        // this.f.AddressDetails.get('JobId').setValue(null);
        // this.f.AddressDetails.get('JobName').setValue(null);
      }

    openPanel(TBD_DR?: any) {

        if (!this.RegionId) {
            Declarations.msgerror("Region not selected.", null, null);
            return;
        }

        this._initiated ? null : this._initiated = true
        this._opened = true;
        this.formSubmited = false;
        this.mapConstants = new MapConstants();

        this.IsTBDRequest = TBD_DR? true: false;
        this.initailizeLocationForm();
        this.f.IsTBDRequest.setValue(this.IsTBDRequest);

        if (TBD_DR) {
            let _this = this;
            if (TBD_DR.ProductTypeId != 10) {
                this.addLocationService.getTfxProduct(TBD_DR.FuelTypeId).subscribe(data => {
                    _this.productDetails = data;
                    if (data) {
                        if (_this.f.FuelDetails.get('FuelDisplayGroupId').value != data.DisplayGroupId) {
                            _this.deliveryTypeChanged(data.DisplayGroupId);
                        }
                        _this.f.FuelDetails.get('FuelDisplayGroupId').setValue(data.DisplayGroupId);
                        _this.f.FuelDetails.get('FuelTypeId').setValue(data.Id);
                    }
                });
            }
            else {
                this.f.FuelDetails.get('FuelTypeId').setValue(this.tbdDrProductId);
                this.f.FuelDetails.get('FuelDisplayGroupId').setValue(3);
            }
        }
        else {
            this.IsTBDRequest = false;
            this.FuelTypeId = null;
            this.ProductTypeName = null;
        }
        this.initPickupForm();
        this.getAllTPOCompanies();
        this.getCurrecyList();
        this.getFuelProducts();
        this.getCountries();
        this.getCountryGroup();
        this.getStatesOfAllCountries();
        this.getPreferencesSettings();
    }

    initailizeLocationForm() {

        this.locationForm = this.fb.group({

            //Id: this.fb.control(null),
            SendInvitationLink: this.fb.control(false),
            RegionId: this.fb.control(this.RegionId),
            IsSupressOrderPricing: this.fb.control(true),
            IsMarineLocation: this.fb.control(false),
            PreferenceSettingId: this.fb.control(0),
            IsTBDRequest: this.fb.control(false),
            IsRegularBuyer: this.fb.control(true),
            CustomerDetails: this.fb.group({
                //Id: this.fb.control(null),
                UserId: this.fb.control(null),
                CompanyId: this.fb.control(null),
                IsNewUser: this.fb.control(true),
                IsNewCompany: this.fb.control(true),
                CompanyName: this.fb.control(null, Validators.required),
                Name: this.fb.control(null, Validators.required),
                PhoneNumber: this.fb.control(null, [Validators.required, Validators.pattern(RegExConstants.Phone)]),
                Email: this.fb.control(null, [Validators.required, Validators.pattern(RegExConstants.Email)]),
                IsInvitationEnabled: this.fb.control(false),
                IsNotifySchedules: this.fb.control(false),
            }),

            AddressDetails: this.fb.group({
                Id: this.fb.control(null),
                IsNewJob: this.fb.control(true),
                JobName: this.fb.control(null, Validators.required),
                DisplayJobID: this.fb.control(null),
                JobId: this.fb.control(null),
                Address: this.fb.control(null),
                City: this.fb.control(null),
                DistanceCovered: this.fb.control(''),
                StateId: this.fb.control(null, Validators.required),
                CountryId: this.fb.control(1, Validators.required),
                CountryGroupId: this.fb.control(null),
                CountyName: this.fb.control(null),
                CountryCode: this.fb.control(null),
                Country: this.fb.control({ Id: null, Name: 'Dummy', Code: 'Dummy', Currency: 1, UoM: 1 }),
                CountryGroup: this.fb.control({ Id: null, Name: 'Dummy', Code: 'Dummy', Currency: 1, UoM: 1 }),
                //Country: this.fb.group({ Id: this.fb.control(1, Validators.required), Name: this.fb.control('Dummy'), Code: this.fb.control('Dummy'), Currency: this.fb.control(1), UoM: this.fb.control(1) }),
                StateName: this.fb.control(null),
                State: this.fb.control({ Id: null, Name: 'Dummy', Code: 'Dummy' }),
                //State: this.fb.control({ Id: this.fb.control(null, Validators.required), Name: this.fb.control('Dummy'), Code: this.fb.control('Dummy') }),
                Currency: this.fb.control(1),
                ZipCode: this.fb.control(null),
                IsProFormaPoEnabled: this.fb.control(false),
                SignatureEnabled: this.fb.control(false),
                IsGeocodeUsed: this.fb.control(false),
                Latitude: this.fb.control(null, Validators.pattern('^[0-9.-]*$')),
                Longitude: this.fb.control(null, Validators.pattern('^[0-9.-]*$')),
                TimeZoneName: this.fb.control(null),
                LocationManagedType: this.fb.control(0),
                IsCompanyOwned: this.fb.control(false),
                MarineUoM: this.fb.control(0),
                IsMarineLocation: this.fb.control(false),
                InventoryDataCaptureType: this.fb.control(0),
                UOM: this.fb.control(1)
            }),

            FuelDetails: this.fb.group({
                Id: this.fb.control(null),
                FuelDisplayGroupId: this.fb.control(1),
                FuelTypeId: this.fb.control(null, Validators.required),
                NonStandardFuelName: this.fb.control(null),
                NonStandardFuelDescription: this.fb.control(null),
                FuelRequestFees: this.intilizeFuelRequestFees(),
                FuelQuantity: this.fb.group({
                    Quantity: this.fb.control(0),
                    QuantityTypeId: this.fb.control(3),
                    MinimumQuantity: this.fb.control(0),
                    MaximumQuantity: this.fb.control(0),
                    QuantityIndicatorTypes: this.fb.control(1),
                }, { validator: this.CompareNumbers }),
                FuelPricing: this.fb.group({
                    FuelPricingDetails: this.fb.group({
                        FreightOnBoardTypes: this.fb.control(0)
                    })
                })
            }),

            FuelDeliveryDetails: this.fb.group({
                Id: this.fb.control(null),
                DeliveryTypeId: this.fb.control(1),
                StartDate: this.fb.control(moment(new Date()).format('MM/DD/YYYY')),
                EndDate: this.fb.control(null),
                StartTime: this.fb.control('08:00'),
                EndTime: this.fb.control('17:00'),
                SingleDeliverySubTypes: this.fb.control(0),
            }),

            FuelPricingDetails: this.fb.group({
                Id: this.fb.control(null),
                PricingTypeId: this.fb.control(3),
                PricePerGallon: this.fb.control(null),
                Code: this.fb.control(null),
                CodeId: this.fb.control(null),
                CodeDescription: this.fb.control(null),
                TempPricingCodeDetails: this.fb.control(null),
                RackAvgTypeId: this.fb.control(1),
                RackPrice: this.fb.control(0),
                EnableCityRack: this.fb.control(null),
                TerminalName: this.fb.control(null),
                TerminalId: this.fb.control(null),
                SupplierCostMarkupTypeId: this.fb.control(1),
                SupplierCostMarkupValue: this.fb.control(0),
                SupplierCost: this.fb.control(null),
                CityGroupTerminalId: this.fb.control(null),
                FuelPricingDetails: this.fb.group({
                    PricingSourceId: this.fb.control(1),
                    PricingCode: this.fb.control({ Id: null, Code: null, Description: null })
                }),
                //TIER PRICING
                IsTierPricingRequired: this.fb.control(null),
                TierPricing: this.fb.group({
                    TierPricingType: this.fb.control(TierPricingType.VolumeBased),
                    IsResetCumulation: this.fb.control(null),
                    AboveQuantityPricing: this.fb.control(null),
                    Pricings: this.fb.array([]),
                    ResetCumulationSetting: this.fb.group({
                        CumulationType: this.fb.control(1),
                        Day: this.fb.control(null),
                        Date: this.fb.control(null),
                    }),
                    DisplayCumulationFrequency: this.fb.control(null)
                }),
            }),

            DeliveryRequest: this.fb.group({
                Priority: this.fb.control(1),
                ScheduleQuantityType: this.fb.control(1),
                RequiredQuantity: this.fb.control(null, this.IsTBDRequest ? [] : [Validators.required, Validators.min(this._minimumConst)]),
                DeliveryLevelPO: this.fb.control(''),
                BadgeNo1: this.fb.control(null),
                BadgeNo2: this.fb.control(null),
                BadgeNo3: this.fb.control(null),
                DispatcherNote: this.fb.control(null),
                PickupLocationType: this.fb.control(0),
                Terminal: this.utilService.getTerminalForm(null, false),
                BulkPlant: this.utilService.getBulkPlantForm(null, false),
            })
        });
        this.setAddressValidators(Country.USA);

    }

    intilizeFuelRequestFees() {
        let _FRFArray = this.fb.array([]);
        _FRFArray.push(this.fb.group({
            FeeTypeId: this.fb.control(null),
            FeeSubTypeId: this.fb.control(null),
            FeeSubTypeName: this.fb.control(null),
            Fee: this.fb.control(null),
            FeeDetails: this.fb.control(null),
            FeeConstraintTypeId: this.fb.control(null),
            IncludeInPPG: this.fb.control(null),
            OtherFeeTypeId: this.fb.control(null),
        }));
        return _FRFArray;
    }

    updateFormControlValidators(control: any, validators: any[]) {
        control.setValidators(validators);
        control.updateValueAndValidity();
    }

    CompareNumbers(control: AbstractControl) {
        if (control.get('MinimumQuantity').value > control.get('MaximumQuantity').value) {
            control.get('MaximumQuantity').setErrors({ compareNumber: true })
        }
        else {
            return null
        }
    }

    getPreferencesSettings() {
        let _this = this;
        if (this.preferencesSetting) {
            //this.setTreferenceSetting()
            return;
        }
        this.preferencesSetting = null;
        this.addLocationService.GetPreferencesSettings().subscribe(data => {
            if (data) {
                _this.locationForm.patchValue(data);
                _this.defaultCompanyCountryId = data.AddressDetails.Country.Id;
                _this.mapConstants = new MapConstants(_this.defaultCompanyCountryId);
                _this.locationForm.get('AddressDetails').get('CountryId').setValue(_this.defaultCompanyCountryId);
                _this.UoM = _this.defaultCompanyCountryId == 1 ? 1 : 2;
                _this.locationForm.get('FuelDeliveryDetails.StartDate').patchValue(moment(new Date()).format('MM/DD/YYYY'));
                _this.locationForm.get('RegionId').patchValue(_this.RegionId);
                if (!_this.f.IsSupressOrderPricing.value) {
                    _this.f.FuelPricingDetails.get('PricePerGallon').setValidators([Validators.required]);
                }
                if (this.IsTBDRequest) {
                    _this.f.IsTBDRequest.setValue(this.IsTBDRequest);
                    if (_this.tbdDrProductTypeId != 10) {
                        if (_this.productDetails) {
                            if (_this.f.FuelDetails.get('FuelDisplayGroupId').value != _this.productDetails.DisplayGroupId) {
                                _this.deliveryTypeChanged(_this.productDetails.DisplayGroupId);
                            }
                            _this.f.FuelDetails.get('FuelDisplayGroupId').setValue(_this.productDetails.DisplayGroupId);
                            _this.f.FuelDetails.get('FuelTypeId').setValue(_this.productDetails.Id);
                        }
                    }
                    else {
                        _this.f.FuelDetails.get('FuelTypeId').setValue(_this.tbdDrProductId);
                        _this.f.FuelDetails.get('FuelDisplayGroupId').setValue(3);
                    }
                }
                _this.f.FuelPricingDetails.get('TierPricing').get('ResetCumulationSetting').get('CumulationType').setValue(1);
                if (_this.pricingModuleComponent) {
                    _this.pricingModuleComponent.setPricingCode();
                }
            }
        });
    }


    quantityTypeChanged(type: number) {

        this.updateFormControlValidators(this.f.FuelDetails.get('FuelQuantity').get('Quantity'), []);
        this.updateFormControlValidators(this.f.FuelDetails.get('FuelQuantity').get('MinimumQuantity'), []);
        //this.updateFormControlValidators(this.f.FuelDetails.get('FuelQuantity').get('MaximumQuantity'), []);

        this.f.FuelDetails.get('FuelQuantity').get('Quantity').setValue(0);
        this.f.FuelDetails.get('FuelQuantity').get('MinimumQuantity').setValue(0);
        this.f.FuelDetails.get('FuelQuantity').get('MaximumQuantity').setValue(0);

        //Fixed
        if (type == 1) {
            //add validation for Quantity
            this.updateFormControlValidators(this.f.FuelDetails.get('FuelQuantity').get('Quantity'), [Validators.required, Validators.min(this._minimumConst)]);
        }
        //Range
        else if (type == 2) {
            this.updateFormControlValidators(this.f.FuelDetails.get('FuelQuantity').get('MinimumQuantity'), [Validators.required, Validators.min(this._minimumConst)]);
            //this.updateFormControlValidators(this.f.FuelDetails.get('FuelQuantity').get('MaximumQuantity'), [Validators.required]);
        }
        //Not Specified//No validation required
    }

    setFuelTypevalidation(type: number) {
        //other
        if (type == 3) {
            this.updateFormControlValidators(this.f.FuelDetails.get('FuelTypeId'), []);
            this.updateFormControlValidators(this.f.FuelDetails.get('NonStandardFuelName'), [Validators.required]);
        }
        //In Location Area//Common//Less common
        else {
            this.updateFormControlValidators(this.f.FuelDetails.get('FuelTypeId'), [Validators.required]);
            this.updateFormControlValidators(this.f.FuelDetails.get('NonStandardFuelName'), []);
        }
    }

    deliveryTypeChanged(type: number) {

        //SET VALIDATION
        this.setFuelTypevalidation(type);

        //GET PRODUCTS
        if (type == 1 || type == 2) {
            this.getFuelProducts();
        }
        else if (type == 4) {
            this.getProductListByZip();
        }
        if (type == 3 && this.pricingModuleComponent) {
            this.f.FuelPricingDetails.get('PricingTypeId').setValue(2);
            //DONT UPDATE VALIDATION IF TIER PRICING ALREADU CHECKED
            if(!this.f.FuelPricingDetails.get('IsTierPricingRequired').value){
                this.pricingModuleComponent.pricingTypeChanged(2)
            }
        }
    }

    getStatesOfAllCountries() {

        if (this.statesList.length > 0) {
            this.filteredStatesList = this.statesList.filter(s => s.CountryId == this.f.AddressDetails.get('CountryId').value) || [];
            return;
        }

        this.addLocationService.getStatesOfAllCountries().subscribe(data => {
            if (data && data.length > 0) {
                this.statesList = data;
                this.filteredStatesList = this.statesList.filter(s => s.CountryId == this.f.AddressDetails.get('CountryId').value) || [];
            }
        });
    }

    get StatesListByCountry(): any[] {
        let countryCode = this.PickupForm.controls['BulkPlant']['controls'].Country.get("Code").value;
        if (countryCode && this.CountryList && this.CountryList.length > 0) {
            
            countryCode = countryCode == "US" ? "USA" : countryCode;
            let countryId = 0;
            let county = this.CountryList.find(c => c.Code == countryCode);
            if (county && county.Id)
                countryId = county.Id;
                
            if (countryId == 4) {
                let countryGroupCode = this.PickupForm.controls['BulkPlant']['controls'].CountryGroup.get("Id").value;
                return this.statesList.filter(t => t.CountryId == 4 && t.CountryGroupId == countryGroupCode);
            }
            else {
                return this.statesList.filter(t => t.CountryId == countryId);
            }
        } 
    }
    countryChanged() {
        this.f.AddressDetails.get('StateId').setValue(null)
        let _countryId = this.f.AddressDetails.get('CountryId').value;
        _countryId == 2 ? this.UoM = 2 : this.UoM = 1

        if (_countryId != 4) {
            this.filteredStatesList = this.statesList.filter(s => s.CountryId == this.f.AddressDetails.get('CountryId').value) || [];
        } else {
            this.filteredStatesList = this.statesList.filter(s => s.CountryGroupId == this.f.AddressDetails.get('CountryGroupId').value) || [];
        }        

        //UPDATE MAP POINTER
        // this.mapConstants = new MapConstants(_countryId);
        // this.f.AddressDetails.get('Latitude').setValue(this.mapConstants.CenterLat);
        // this.f.AddressDetails.get('Longitude').setValue(this.mapConstants.CenterLon);

        this.getAddress();
        this.setAddressValidators(_countryId);

    }

    getTPOContactPersonDetails(userId: number) {

        this.addLocationService.getTPOContactPersonDetails(userId).subscribe(data => {
            if (data) {
                this.CompanyContactPersonsDetails = data;
                this.locationForm.get('CustomerDetails').get('PhoneNumber').setValue(this.CompanyContactPersonsDetails.PhoneNumber);
                this.locationForm.get('CustomerDetails').get('Email').setValue(this.CompanyContactPersonsDetails.Email);
            }
        });
    }

    getFuelProducts() {

        if(this.f.AddressDetails.get('IsMarineLocation').value){
            return;
        }
        this.f.FuelDetails.get('FuelTypeId').setValue(null);
        this.FuelProductsList = [];

        let companyId = this.f.CustomerDetails['controls']['CompanyId'].value || 0;
        let jobId = this.f.AddressDetails['controls']['JobId'].value || 0;
        let productDisplayGroupId = this.f.FuelDetails.get('FuelDisplayGroupId').value ? this.f.FuelDetails.get('FuelDisplayGroupId').value : 1;

        this._isLoadingProducts = true;
        this.addLocationService.getFuelProducts(productDisplayGroupId, companyId, jobId).subscribe(data => {
            if (data && data.length > 0) {
                this.FuelProductsList = data;
                //this.f.FuelDetails.get('FuelTypeId').setValue(data[0].Id);
            }
            this._isLoadingProducts = false;
        });
    }

    currentRadious: number = 100;
    getProductListByZip(radious?: number) {

        this.f.FuelDetails.get('FuelTypeId').setValue(null);
        //this.FuelProductsList = [];

        if(!this.AddressRawData){
            return;
        }

        let address = this.f.AddressDetails.get('Address').value;
        let state = this.AddressRawData.StateName;
        let city = this.AddressRawData.City;
        let country = (this.AddressRawData.CountryCode == 'US') ? 'USA' : (address.CountryCode == 'CA' ? 'CAN' : 'IND');
        let zipcode = this.AddressRawData.ZipCode;
        let input = address + ' ' + city + ' ' + state + ' ' + country + ' ' + zipcode;
        !radious ? this.currentRadious = 100 : null;

        if (zipcode) {
            this._isLoadingProducts = true;
            this.addLocationService.getProductListByZip(input, this.currentRadious).subscribe(data => {
                this._isLoadingProducts = false;
                if (data && data.length > 0) {
                    if (this.currentRadious == 100) {
                        this.FuelProductsList = data;
                    }
                    else {
                        this.FuelProductsList = this.FuelProductsList.concat(data);
                    }
                    this.f.FuelDetails.get('FuelTypeId').setValue(data[0].Id);
                }
                this.currentRadious += 100;
                this._isLoadingProducts = false;
            });
        }
    }

    fuelTypeIdChanged(value: string) {
        if (value == 'seeMore') {
            this.f.FuelDetails.get('FuelTypeId').setValue(null);
            this.getProductListByZip(this.currentRadious);
        }
    }

    markerDragEnd(event: any) {
        let state = this.f.AddressDetails.get('StateName').value || '';
        let country = this.f.AddressDetails.get('CountryCode').value || ''
        let city = this.f.AddressDetails.get('City').value || '';
        let zipcode = this.f.AddressDetails.get('ZipCode').value || '';
        if (city == "" || state == "" || country == "" || zipcode == "") {
            this.updateGeoCode(event.coords.lat, event.coords.lng);
        }
        else {
            this.confirmationDialogService.confirm('Map update', 'Geo Codes shifted to a new location!')
                .then((confirmed) => (confirmed == true) ? this.updateGeoCode(event.coords.lat, event.coords.lng) : this.previousLatLon())
                .catch(() => this.previousLatLon());
        }
    }

    updateGeoCode(lat: any, lng: any) {
        this.addLocationService.GetAddressByLongLat(lat, lng).subscribe(response => {
            if (response) {
                this.updateAddress(response, true);
                this.f.AddressDetails.get('Latitude').patchValue(lat.toFixed(8));
                this.f.AddressDetails.get('Longitude').patchValue(lng.toFixed(8));
                this.getTimeZoneUsingLatLng(lat, lng);
            }
        });
    }

    previousLatLon() {
        if (!this.f.AddressDetails.get('Latitude').value || !this.f.AddressDetails.get('Longitude').value) {
            // let _mapConstants = new MapConstants(this.f.AddressDetails.get('CountryId').value)
            // this.f.AddressDetails.get('Latitude').setValue(_mapConstants.CenterLat);
            // this.f.AddressDetails.get('Longitude').setValue(_mapConstants.CenterLon);
        }
        else {
            this.mapConstants.CenterLat = this.f.AddressDetails.get('Latitude').value;
            this.mapConstants.CenterLon = this.f.AddressDetails.get('Longitude').value;
        }
    }

    getAddressByZip() {
        let zipCode = this.f.AddressDetails.get('ZipCode').value;
        let address = this.f.AddressDetails.get('Address').value || "";
        if (zipCode) {
            this.addLocationService.getAddressByZip(zipCode, address).subscribe((response) => {
                if (response) {
                    this.updateAddress(response)
                }
            });
        }
    }

    getAddress() {

        let address = this.f.AddressDetails.get('Address').value || '';
        let state = this.f.AddressDetails.get('StateName').value || '';
        let country = this.f.AddressDetails.get('CountryCode').value || ''
        let city = this.f.AddressDetails.get('City').value || '';
        let zipcode = this.f.AddressDetails.get('ZipCode').value || '';
        let DistanceCovered = this.f.AddressDetails.get('DistanceCovered').value || '';

        if (address == '' || state == '' || country == '' || zipcode == '')
            return;

        address = address + " " + city + " " + state + " " + country + " " + zipcode;

        this.addLocationService.GetAddress(address).subscribe((data) => {
            this.updateAddress(data)
        })
    }

    stateChanged() {
        if (this.pricingModuleComponent) {
            this.pricingModuleComponent.getCityGroupTerminals();
        }
        this.getAddress();
        this.setBillableQuantity();
    }

    setBillableQuantity() {
        let state = this.statesList.find(st => st.StateId == this.f.AddressDetails.get('StateId').value);
        if (state && state.QuantityIndicatorId) {
            this.f.FuelDetails.get('FuelQuantity').get('QuantityIndicatorTypes').setValue(state.QuantityIndicatorId);
        }
    }

    getTimeZoneUsingLatLng(lat, lng) {
        var times_Stamp = (Math.round((new Date().getTime()) / 1000)).toString();
        let _this = this;
        this.addLocationService.getTimeZoneName(lat, lng, times_Stamp, this.MapsApiKey).subscribe((data: any) => {
            if (data.timeZoneId != null) {
                var timeZoneName = data.timeZoneName;
                timeZoneName = _this.ParseTimeZone(timeZoneName);
                this.f.AddressDetails.get('TimeZoneName').patchValue(timeZoneName);
            }
        });
    }

    ParseTimeZone(timeZoneName) {
        if (timeZoneName != undefined && timeZoneName != null) {
            timeZoneName = timeZoneName.replace("Daylight", "Standard");
            timeZoneName = timeZoneName.replace("Alaska ", "Alaskan ");
            timeZoneName = timeZoneName.replace("Hawaii-Aleutian", "Hawaiian");
        }
        return timeZoneName
    }

    AddressRawData: Geocode = null;
    updateAddress(address: Geocode, isDragged?: boolean) {
        if (address.CountryCode && address.StateName) {
            this.AddressRawData = address;
            let countryId = (address.CountryCode == 'USA') ? 1 : (address.CountryCode == 'CAN' ? 2 : this.f.AddressDetails.get('CountryId').value);
            let state = this.statesList.find(st => st.StateName.toLowerCase() == address.StateName.toLowerCase());
            let stateId = (state && state.StateId) ? state.StateId : this.f.AddressDetails.get('StateId').value;
            this.UoM = countryId == 1 ? 1 : 2;
            this.f.AddressDetails.get('City').patchValue(address.City);
            this.f.AddressDetails.get('ZipCode').patchValue(address.ZipCode);
            this.f.AddressDetails.get('CountyName').patchValue(address.CountyName);
            this.f.AddressDetails.get('CountryCode').patchValue(address.CountryCode);
            this.f.AddressDetails.get('CountryId').patchValue(countryId);
            this.f.AddressDetails.get('StateName').patchValue(address.StateName);
            this.f.AddressDetails.get('StateId').patchValue(stateId);
            this.f.AddressDetails.get('State').patchValue({ Id: stateId, Code: address.StateName, Name: address.StateName });
            if (countryId == Country.USA) {
                this.f.AddressDetails.get('MarineUoM').patchValue(1);
                this.f.AddressDetails.get('Country').patchValue({ Id: countryId, Code: address.CountryCode, Name: address.CountryCode, Currency: 1, UoM: 1 });
            }
            else if (countryId == Country.CAN) {
                this.f.AddressDetails.get('MarineUoM').patchValue(2);
                this.f.AddressDetails.get('Country').patchValue({ Id: countryId, Code: address.CountryCode, Name: address.CountryCode, Currency: 2, UoM: 2 });
            }
            else {
                this.f.AddressDetails.get('MarineUoM').patchValue(1);
                this.f.AddressDetails.get('Country').patchValue({ Id: countryId, Code: address.CountryCode, Name: address.CountryCode, Currency: 2, UoM: 2 });
            }
            if (isDragged) {
                this.f.AddressDetails.get('Address').patchValue(address.Address);
            }
            if (address.Latitude) {
                this.f.AddressDetails.get('Latitude').patchValue(address.Latitude.toFixed(8));
                this.f.AddressDetails.get('Longitude').patchValue(address.Longitude.toFixed(8));
                this.mapConstants.CenterLat = address.Latitude;
                this.mapConstants.CenterLon = address.Longitude;
                this.getTimeZoneUsingLatLng(address.Latitude, address.Longitude);
            }

            this.filteredStatesList = this.statesList.filter(s => s.CountryId == countryId) || [];
        }
    }

    isCompanyExits() {
        this.isCompanyNameExist = false;
        if (this.f.CustomerDetails.get('CompanyName').value) {
            this.addLocationService.isTpoCompanyExist(this.f.CustomerDetails.get('IsNewCompany').value, this.f.CustomerDetails.get('CompanyName').value).subscribe(data => {
                if (data != null || data != undefined) {
                    this.isCompanyNameExist = data;
                }
            });
        }
    }

    isJobExist(companyId?: string) {
        this.isJobNameExist = false;
        if (!this.f.CustomerDetails.get('IsNewCompany').value && this.f.AddressDetails.get('JobName').value && (companyId || this.f.CustomerDetails.get('CompanyId').value)) {
            this.addLocationService.isJobNameExist(this.f.AddressDetails.get('JobName').value, companyId || this.f.CustomerDetails.get('CompanyId').value).subscribe(response => {
                if (response != null && response != undefined) {
                    this.isJobNameExist = response;
                }
            });
        }
    }

    IsPhoneNumberValid() {

        this.isPhoneNumberValid = true;
        let phone = this.f.CustomerDetails.get('PhoneNumber').value;

        if (phone && this.f.CustomerDetails.get('PhoneNumber').valid) {
            this.addLocationService.IsPhoneNumberValid(phone).subscribe(data => {
                if (data != null || data != undefined) {
                    this.isPhoneNumberValid = data;
                }
            });
        }
    }

    newCompanyChecked(isNewCompany: boolean) {

        this.f.CustomerDetails.get('CompanyId').setValue(null);
        this.f.CustomerDetails.get('UserId').setValue(null);
        this.f.CustomerDetails.get('IsNewUser').setValue(true);

        this.updateFormControlValidators(this.f.CustomerDetails.get('Name'), [Validators.required]);
        this.updateFormControlValidators(this.f.CustomerDetails.get('UserId'), []);

        if (isNewCompany) {
            this.f.CustomerDetails.get('Email').setValue(null);
            this.f.CustomerDetails.get('PhoneNumber').setValue(null);
            this.f.CustomerDetails.get('CompanyName').setValue(null);

            this.updateFormControlValidators(this.f.CustomerDetails.get('CompanyName'), [Validators.required]);
            this.updateFormControlValidators(this.f.CustomerDetails.get('CompanyId'), []);
        }
        else {
            this.updateFormControlValidators(this.f.CustomerDetails.get('CompanyName'), []);
            this.updateFormControlValidators(this.f.CustomerDetails.get('CompanyId'), [Validators.required]);

            this.isJobExist();
        }

    }

    companyChanged(companyId: string) {
        this.getTPOCompanyContactPersons(companyId);
        this.isJobExist(companyId)
    }

    _isLoadingPerson: boolean = false;
    getTPOCompanyContactPersons(companyId: string) {
        if (companyId) {

            this.f.CustomerDetails.get('IsNewUser').setValue(false);
            this.updateFormControlValidators(this.f.CustomerDetails.get('Name'), []);
            this.updateFormControlValidators(this.f.CustomerDetails.get('UserId'), [Validators.required]);

            this.CompanyContactPersonsList = [];
            this._isLoadingPerson = true;
            this.addLocationService.getTPOCompanyContactPersons(companyId).subscribe(data => {
                if (data && data.length > 0) {
                    this.CompanyContactPersonsList = data;
                    let companyname = this.AllTPOCompaniesList.filter(x => x.id == companyId);
                    this.locationForm.get('CustomerDetails').get('CompanyName').setValue(companyname[0].text);
                }
                this._isLoadingPerson = false;
            });
        }
    }

    getCurrecyList() {

        if (this.currucyList.length > 0) {
            return;
        }

        this.addLocationService.getCurrenyList().subscribe(data => {
            if (data && data.length > 0) {
                this.currucyList = data;
            }
        });
    }

    getAllTPOCompanies() {

        if (this.AllTPOCompaniesList.length > 0) {
            return;
        }

        this._isLoadingProducts = true;
        this.addLocationService.getAllTPOCompanies().subscribe(data => {
            if (data && data.length > 0) {
                this.AllTPOCompaniesList = data.map((item: any) => {
                    return { id: item.Id, text: item.Name }
                });
            }
            this._isLoadingProducts = false;
        });
    }

    clickNewPerson(isnew: boolean) {
        if (isnew) {
            this.updateFormControlValidators(this.f.CustomerDetails.get('Name'), [Validators.required]);
            this.updateFormControlValidators(this.f.CustomerDetails.get('UserId'), []);
        }
        else {
            this.updateFormControlValidators(this.f.CustomerDetails.get('Name'), []);
            this.updateFormControlValidators(this.f.CustomerDetails.get('UserId'), [Validators.required]);
        }
        this.f.CustomerDetails.get('IsNewUser').setValue(isnew);
    }

    getCountries() {
        if (this.CountryList.length == 0) {
            this.stateService.getCountries().subscribe(x => this.CountryList = x);
        }

    }

    getCountryGroup() {
        if (this.CountryGroupList.length == 0) {
            this.stateService.getCountryGroup(4).subscribe(x => this.CountryGroupList = x);
        }

    }

    //DR - PICKUP LOCATION BULK PLANT
    editPickupLocation(): void {

        this.initPickupForm();
        let pricingCode = this.f.FuelPricingDetails.get('FuelPricingDetails').get('PricingCode').value;
        let jobCountryId = this.f.AddressDetails.get('CountryId').value,
            pricingCodeId = pricingCode ? pricingCode.Id : 0,
            fuelType = this.f.FuelDetails.get('FuelTypeId').value,
            jobLatitude = this.f.AddressDetails.get('Latitude').value,
            jobLongitude = this.f.AddressDetails.get('Longitude').value,
            isSupressOrderPricing = this.locationForm.get('IsSupressOrderPricing').value;
        if (jobCountryId > 0 && pricingCodeId > 0 && fuelType > 0 && jobLatitude != 0 && jobLongitude != 0) {
            this.addLocationService.getFuelTerminals(jobCountryId, pricingCodeId, fuelType, this.defaultCompanyCountryId, isSupressOrderPricing, jobLatitude, jobLongitude, '').subscribe((data: DropdownItem[]) => {
                this.Terminals = data;
            });
        }
        if (this.BulkPlants.length == 0) {
            this.addresService.getBulkPlants('').subscribe(data => {
                this.BulkPlants = data.slice();
                this.BulkplantList = data;
            });
        }

        this.isReadOnly = false;
    }

    initPickupForm() {
        this.PickupForm = this.fb.group({
            PickupLocationType: this.fb.control(1),
            Terminal: this.utilService.getTerminalForm(null, true),
            BulkPlant: this.utilService.getBulkPlantForm(null, true)
        });
    }

    onTerminalSelected(event: any): void {
        this.PickupForm.get('Terminal').patchValue({ Id: event.Id, Name: event.Name });
    }

    removePickupLocation() {
        this.locationForm.controls.DeliveryRequest.get('PickupLocationType').setValue(0);
        this.locationForm.controls.DeliveryRequest.get('Terminal').patchValue(this.utilService.getTerminalForm(null, false).value);
        this.locationForm.controls.DeliveryRequest.get('BulkPlant').patchValue(this.utilService.getBulkPlantForm(null, false).value);
    }

    setStateCode(event: any) {
        this.PickupForm.get('BulkPlant.State.Code').setValue(event.target.selectedOptions[0].text);
    }

    onBulkPlantSearched(event: any): void {
        let keyword = event.target.value.toLowerCase();
        this.BulkPlants = this.BulkplantList.slice().filter(function (elem) {
            return elem.Name && elem.Name.toLowerCase().indexOf(keyword) >= 0;
        });
        let bulkPlantName = this.PickupForm.get('BulkPlant.SiteName').value;
        this.isReadOnly = this.BulkPlants.filter(t => t.Name == bulkPlantName).length > 0;
    }

    onBulkPlantSelected(event: any): void {

        this.PickupForm.get('BulkPlant.SiteName').setValue(event.Name);
        this.PickupForm.get('BulkPlant.SiteId').setValue(event.Id);
        this.BulkPlants = this.BulkplantList.slice();
        this.getBulkPlantAddress(event.Name);

        if (this.PickupForm.get('BulkPlant.SiteName').valid) {
            this.isReadOnly = true;
        }
    }

    getBulkPlantAddress(bulkPlantName: string) {
        this.addresService.GetBulkPlantDetails(bulkPlantName).subscribe(response => {
            this.PickupForm.controls['BulkPlant'].patchValue({
                Address: response.Address,
                City: response.City,
                State: { Id: response.State.Id, Code: response.State.Code, Name: response.State.Code },
                Country: { Id: response.Country.Id, Code: response.Country.Code, Name: response.Country.Code },
                ZipCode: response.ZipCode,
                CountyName: response.CountyName,
                Latitude: response.Latitude,
                Longitude: response.Longitude
            });
        });
        this.PickupForm.markAllAsTouched();
        this.PickupForm.markAsDirty();
    }

    updatePickupLocation() {

        let pickupDetails = this.PickupForm.getRawValue();
        this.locationForm.controls.DeliveryRequest.get('PickupLocationType').setValue(pickupDetails.PickupLocationType);

        if ((this.PickupForm.get('PickupLocationType').value == 1 && this.PickupForm.get('Terminal').valid)) {
            this.locationForm.controls.DeliveryRequest.get('Terminal').patchValue(pickupDetails.Terminal);
        }
        else if ((this.PickupForm.get('PickupLocationType').value == 2 && this.PickupForm.get('BulkPlant').valid)) {
            this.locationForm.controls.DeliveryRequest.get('BulkPlant').patchValue(pickupDetails.BulkPlant);
        }
        else {
            Declarations.msgerror("Invalid pickup details.", null, null);
        }

        let dismiss = document.getElementById('btnDrPickupClose2') as HTMLElement;
        dismiss.click();

    }

    getAddressByZipForPickup(event: any): void {
        let zipCode = event.target.value;
        let regexUsa = new RegExp(RegExConstants.UsaZip);
        let regexCan = new RegExp(RegExConstants.CanZip);

        if (regexUsa.test(zipCode) || regexCan.test(zipCode)) {
            this._loadingAddress = true;
            this.addresService.getAddress(zipCode)
                .subscribe(data => {
                    this._loadingAddress = false;
                    if (data != null && data != undefined && data.StateCode != null) {
                        data.CountryCode == 'US' || data.CountryCode == 'USA' ? data.CountryCode = 'USA' : data.CountryCode = 'CAN';
                        let addressModel = this.addressMapper(data);
                        this.PickupForm.get('BulkPlant').patchValue({
                            City: addressModel.City,
                            State: addressModel.State,
                            Country: addressModel.Country,
                            ZipCode: addressModel.ZipCode,
                            CountyName: addressModel.CountyName,
                            Latitude: addressModel.Latitude,
                            Longitude: addressModel.Longitude
                        });
                        this.PickupForm.markAllAsTouched();
                        this.PickupForm.markAsDirty();
                        if (addressModel.Country.Code != "USA" && addressModel.Country.Code != "US") {
                            this.CountryBasedZipcodeLabel = "Postal Code";
                        }
                        else {
                            this.CountryBasedZipcodeLabel = "Zip";
                        }

                        this.mapConstants.CenterLat = data.Latitude;
                        this.mapConstants.CenterLon = data.Longitude;
                    }
                });
        }
    }

    addressMapper(data: any): AddressModel {

        let state = this.statesList.find(x => x.StateCode == data.StateCode);
        let country = this.CountryList.find(x => x.Code == data.CountryCode);

        let _address = new AddressModel();
        _address.Address = data.Address;
        _address.City = data.City;
        _address.CountyName = data.CountyName
        _address.Latitude = data.Latitude;
        _address.Longitude = data.Longitude;
        _address.ZipCode = data.ZipCode;
        _address.State = { Id: state.StateId, Code: state.StateCode, Name: state.StateName };
        _address.Country = country;
        return _address;
    }

    onSubmit() {
        this.formSubmited = true;

        if (this.f.CustomerDetails.get('IsNewCompany').value) {
            //JOB VALIDATION NOT REQUIRED FOR NEW COMPANY
            this.isJobNameExist = false;
        }
        else {
            //COMPANY NAME VALIDATION NOT REQUIRED FOR EXISTING COMPANY
            this.isCompanyNameExist = false;
        }

        if (this.locationForm.valid && !this.isCompanyNameExist && !this.isJobNameExist)
        {
            //SET STATE COUNTRY DETAILS
            // let stateName = this.f.AddressDetails.get('StateName').value;
            // let stateId = this.f.AddressDetails.get('StateId').value;
            // let countryCode = this.f.AddressDetails.get('CountryCode').value;
            // let countryId = this.f.AddressDetails.get('CountryId').value;
            //let countryName = this.f.AddressDetails.get('CountryName').value;
            // this.f.AddressDetails.get('Country').patchValue({ Id: countryId, Code: countryCode, Name: countryCode, Currency: this.UoM, UoM: this.UoM });
            // this.f.AddressDetails.get('State').patchValue({ Id: stateId, Code: stateName, Name: stateName });
            
            if(this.f.FuelPricingDetails.get('IsTierPricingRequired').value){
                let pricings = this.f.FuelPricingDetails.get('TierPricing').get('Pricings') as FormArray;                
                pricings.controls.forEach((pricing: FormGroup)=>{
                    pricing.get('UoM').setValue(this.UoM);
                    pricing.get('Currency').setValue(this.UoM);
                })
            }
            
            var formValue = this.locationForm.getRawValue();
            if (formValue.DeliveryRequest.PickupLocationType != 2) {
                formValue.DeliveryRequest.BulkPlant = null;
            }
            else {
                let state = this.statesList.find(x => x.StateId == formValue.DeliveryRequest.BulkPlant.State.Id);
                formValue.DeliveryRequest.BulkPlant.State.Name = state.StateName;
                formValue.DeliveryRequest.BulkPlant.Country.Name = formValue.DeliveryRequest.BulkPlant.Country.Code;
            }
            this._loading = true;
            this.addLocationService.createOrder(formValue).subscribe((data: any) => {
                this._loading = false;
                if (data != null && parseInt(data.StatusCode) == 0) {
                    Declarations.msgsuccess(data.StatusMessage, undefined, undefined);
                    this._opened = false;
                    //closeSlidePanel();
                    //this.clearForm();   
                    if (this.IsTBDRequest) {
                        this.dataService.setGetOnTheFlyLocationDetailsSubject(data);
                    }
                    else {
                        this.OnTheFlyLocationCreate.emit();
                    }
                }
                else {
                    const statusMessage = data == null ? 'Failed' : data.StatusMessage;
                    Declarations.msgerror(statusMessage, undefined, undefined);
                }
            })
        }
    }
    marineCheckBoxChanged(value: boolean){
        this.f.FuelDetails.get('FuelTypeId').setValue(null);
        if(value){
            //
            this.f.FuelDetails.get('FuelDisplayGroupId').setValue(1);
            this.deliveryTypeChanged(1);
            //
            if(this.pricingModuleComponent){
                this.f.FuelPricingDetails.get('IsTierPricingRequired').setValue(false);
                this.pricingModuleComponent.tierPricingEnabled(false);
            }
            //
            this.getMarineProductList();
        }
        else {
            this.getFuelProducts();
        }
    }

    deliveryTypeIdChanged(deliveryTypeId: number) {
        if (deliveryTypeId == 1) {
            this.f.FuelPricingDetails.get('TierPricing').get('TierPricingType').setValue(2);
        }
    }
    getMarineProductList() {
        this.MarineProductsList = [];
        //let productDisplayGroupId = this.f.FuelDetails.get('FuelDisplayGroupId').value ? this.f.FuelDetails.get('FuelDisplayGroupId').value : 1;
        this._isLoadingProducts = true;
        this.addLocationService.getMarineProductList(6, 0,'', 1).subscribe(data => {
            if (data && data.length > 0) {
                this.MarineProductsList = data;
            }
            this._isLoadingProducts = false;
        });
    }
    subscribeTbdAddLocation() {
        this.tbdAddLocationSubscription = this.dataService.OpenOnTheFlyLocationFormSubject.subscribe((data: any) => {
            this.tbdDrProductId = data.FuelTypeId;
            this.tbdDrProductTypeId = data.ProductTypeId;
            this.openPanel(data);
        })
    }


    setAddressValidators(countryId: any) {
        if (countryId == Country.CAR) {
            this.f.AddressDetails.get('Address').clearValidators();
            this.f.AddressDetails.get('Address').updateValueAndValidity();
            this.f.AddressDetails.get('City').clearValidators();
            this.f.AddressDetails.get('City').updateValueAndValidity();
            this.f.AddressDetails.get('CountyName').clearValidators();
            this.f.AddressDetails.get('CountyName').updateValueAndValidity();
            this.f.AddressDetails.get('ZipCode').clearValidators();
            this.f.AddressDetails.get('ZipCode').updateValueAndValidity();
        }
        else {
            let validator = [Validators.required];
            this.f.AddressDetails.get('Address').setValidators(validator);
            this.f.AddressDetails.get('Address').updateValueAndValidity();
            this.f.AddressDetails.get('City').setValidators(validator);
            this.f.AddressDetails.get('City').updateValueAndValidity();
            this.f.AddressDetails.get('CountyName').setValidators(validator);
            this.f.AddressDetails.get('CountyName').updateValueAndValidity();
            this.f.AddressDetails.get('ZipCode').setValidators(validator);
            this.f.AddressDetails.get('ZipCode').updateValueAndValidity();
        }
    }

    isRequiredField(countryId: any) {        
        if (countryId == 1 || countryId == 2) {
            return true;
        }
        else if (countryId == 4) {
            return false;
        }
    }
    formatTime(timeInput) {
        Declarations.formatTime(timeInput);
    }
}
