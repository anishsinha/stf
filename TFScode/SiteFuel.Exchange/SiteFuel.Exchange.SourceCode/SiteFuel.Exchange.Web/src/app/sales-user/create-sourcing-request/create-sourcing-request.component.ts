import { ChangeDetectorRef, Component, OnInit, ViewChild, ViewEncapsulation } from '@angular/core';
import { FormArray, FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { SalesUserService } from '../sales-user.service';
import { Declarations } from 'src/app/declarations.module';
import { ActivatedRoute, Params, Router } from '@angular/router';
import * as moment from 'moment';
import { ConfirmationDialogService } from '../../shared-components/confirmation-dialog/confirmation-dialog.service';
import { PricingSectionComponent } from 'src/app/shared-components/pricing-section/pricing-section.component';
import { ContactPersonModel, FeeModel, GeneralNote, Geocode, MapConstants } from '../sales-user.model';
import { SourcingRequestStatus, TierPricingType } from 'src/app/app.enum';
import { DropDown, RegExConstants } from 'src/app/app.constants';
declare var CurrentUserId: any;
declare var IsSalesUser: boolean;

@Component({
    selector: 'app-create-sourcing-request',
    templateUrl: './create-sourcing-request.component.html',
    styleUrls: ['./create-sourcing-request.component.scss'],
    encapsulation: ViewEncapsulation.None,
})
export class CreateSourcingRequestComponent implements OnInit {

    @ViewChild(PricingSectionComponent) private pricingModuleComponent: PricingSectionComponent;
    pageloader = false;
    keyword = 'Code';
    Approved_Terminals_keyword = "Name";
    public PrcingCodevalue = '';
    popoverTitle = 'Create PO';
    popoverMessage = 'Are you sure want to create PO?';
    cancelClicked = false;
    isPriceCodeLoading = false;
    public userId: number;
    sourcingRequestForm: FormGroup;
    public editSourcingId: number = 0;
    IsLoading = false;
    formSubmited = false;
    public companyExits = false;
    public isPersonNew = true;
    public TruckTypeLoadList = [];
    public FreightOnBoardTypesList = [];
    public allJobList: Array<DropDown> = [];
    public countryList = [];
    public currucyList = [];
    public UomList = [];
    public statesList = [];
    public filteredStatesList = [];
    public FuelProductsList = [];
    public FeeTypesList = [];
    public FeeSubTypesList = [];
    public FeeConstraintTypesList = [];
    public PaymentMethodsList = [];
    public RackAvgPricingTypesList = [];
    public CompanyContactPersonsList = [];
    public CitySourcingGroupTerminalPriceAvailableList = [];
    public SourcingCityGroupTerminalsList = [];
    public ClosedTerminalList = [];
    public OpisTerminalList = [];
    public CompanyContactPersonsDetails: any;
    public AllTPOCompaniesList: Array<DropDown> = [];
    public RequestStatus = SourcingRequestStatus;
    public GeneralNotesHistory: GeneralNote[] = [];
    public pricingCodes = [];
    public pricingfeedTypeId = 0;
    public pricingfuelClassTypeId = 0;
    public UserContext: any = {};
    public IsSuppressPricing = false;
    public isValidMobile: boolean = true;
    public billableList = [{ Id: '1', Name: 'Net' }, { Id: '2', Name: 'Gross' }];
    public MaxInputDate: Date = moment().add(1, 'year').toDate();
    //public isShowNote:boolean=false;
    @ViewChild('approveTerminalAuto') approveTerminalAuto;
    public LeadFees: FeeModel[];
    public displayCurrancy: string;
    public isSalesUserType: boolean;
    public countryGroupList = [];
    public DispatchRegionList = [];
    public companyPreferenceSetting = false;
    
    regexPhone: RegExp = /^\(?([0-9]{3})\)?[-. ]?([0-9]{3})[-. ]?([0-9]{4})$/;
    popoverLostTitle = 'Request Lost';
    popoverLostMessage = 'This request will be lost. Are you sure want to lose request?';

    // map settings
    mapConstants: MapConstants = new MapConstants();
    UoM: number = 0;
    public CompDdlSetting = {};

    constructor(private fb: FormBuilder,
        private salesUserService: SalesUserService,
        private route_active: ActivatedRoute,
        private router: Router,
        private changeDetectorRef: ChangeDetectorRef,
        private confirmationDialogService: ConfirmationDialogService) {

        this.getUserContext();
        this.initailizeSourcingReqForm();
    }

    ngAfterViewInit() {
        if (typeof CurrentUserId !== "undefined") {
            this.userId = CurrentUserId;
        }
    }

    ngOnInit() {
        this.pageloader = true
        this.getFreightOnBoardTypes();
        this.getTruckLoadType();
        this.getUoMList();
        this.getCurrecyList();
        this.getCountryList();
        // this.setCurrency(1);
        this.getPaymentMethods();
        //this.getFuelProducts();
        this.getStatesOfAllCountries();
        this.getRackAvgPricingTypes();
        this.getAllTPOCompanies();

        if (this.f.AddressDetails.get('CountryId').value == 2) { //canada
            this.mapConstants.CenterLat = 56.14;
            this.mapConstants.CenterLon = -106.34;
        }
        this.isSalesUserType = (typeof IsSalesUser !== undefined) && IsSalesUser;

        this.getcountryGroupList();
        this.GetDispatchRegions();

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

    onCompSelect(company: any, isSelected: boolean) {
        if (isSelected) {
            this.f.CustomerDetails.get('CompanyId').setValue(company.id);
            this.getSourcingCompanyContactPersons(company.id);
            this.getJobLists(company.id, this.f.TruckLoadType.value, this.f.FreightOnBoardType.value);
            this.isRegularBuyerUpdate(company.id, false);
        }
        else {
            this.CompanyContactPersonsList = [];
            this.f.AddressDetails['controls']['IsNewJob'].setValue(true);
            this.allJobList = [];
            this.setRegularBuyerValidation(true);
        }
        this.ClearAddress();
        this.f.CustomerDetails.get('UserId').setValue(null);
        this.f.CustomerDetails.get('PhoneNumber').setValue(null);
        this.f.CustomerDetails.get('Email').setValue(null);
        this.f.AddressDetails.get('JobId').setValue(null);
        this.f.AddressDetails.get('JobName').setValue(null);
        this.f.AddressDetails.get('TempJob').setValue(null);
    }

    onJobSelect(job: any, companyName: string, isSelected: boolean) {

        if (isSelected) {
            this.f.AddressDetails.get('JobId').setValue(job.id);
            this.f.AddressDetails.get('JobName').setValue(job.text);
            this.getJobDetails(job.id, companyName);
        }
        else {
            this.f.AddressDetails.get('JobId').setValue(null);
            this.f.AddressDetails.get('JobName').setValue(null);
            //this.sourcingRequestForm.get('AddressDetails').reset();
            // this.f.AddressDetails.get('DisplayJobID').setValue(null);
            // this.f.AddressDetails.get('Address').setValue(null);
            // this.f.AddressDetails.get('ZipCode').setValue(null);
            // this.f.AddressDetails.get('City').setValue(null);
            // this.f.AddressDetails.get('CountyName').setValue(null);
            // this.f.AddressDetails.get('StateId').setValue(null);
            // this.f.AddressDetails.get('Latitude').setValue(null);
            // this.f.AddressDetails.get('Longitude').setValue(null);
        }
    }

    companyExistanceChanged(isNew: boolean) {
        if (isNew) {
            this.f.CustomerDetails.get('CompanyName').setValue(null);
            this.f.CustomerDetails.get('UserId').setValue(null);
            this.f.CustomerDetails.get('PhoneNumber').setValue(null);
            this.f.CustomerDetails.get('Email').setValue(null);

            this.f.AddressDetails.get('JobId').setValue(null);
            this.f.AddressDetails.get('JobName').setValue(null);

            this.f.AddressDetails.get('IsNewJob').setValue(true);
            this.setRegularBuyerValidation(isNew);
            this.sourcingRequestForm.get('IsSupressOrderPricing').setValue(this.companyPreferenceSetting);
            this.f.CustomerDetails.get('TempCompany').setValue(null);
            this.onCompSelect(null, false);
        }
        else {
            this.ClearAddress();
        }
        this.clickNewPerson(true);
    }

    setRegularBuyerValidation(isNewCompany: boolean) {
        this.sourcingRequestForm.get('IsSupressOrderPricing').setValue(this.companyPreferenceSetting);
        this.pricingModuleComponent.toggleSuppressPricing(this.companyPreferenceSetting);
        this.f.IsRegularBuyer.setValue(!isNewCompany);
    }


    initailizeSourcingReqForm() {
        this.sourcingRequestForm = this.fb.group({
            Id: this.fb.control(null),
            TruckLoadType: this.fb.control(null),
            FreightOnBoardType: this.fb.control(null),
            AccountingCompanyId: this.fb.control(null),
            DisplayRequestId: this.fb.control(null),
            RequestName: this.fb.control(null),
            SalesUserId: this.fb.control(null),
            GeneralNote: this.fb.control(null),
            RequestStatus: this.fb.control(0),
            IsSupressOrderPricing: this.fb.control(false),
            IsRegularBuyer: this.fb.control(false),
            CustomerDetails: this.fb.group({
                Id: this.fb.control(null),
                UserId: this.fb.control(null),
                CompanyId: this.fb.control(null),
                IsNewCompany: this.fb.control(true),
                CompanyName: this.fb.control(null, Validators.required),
                Name: this.fb.control(null),
                PhoneNumber: this.fb.control(null),
                Email: this.fb.control(null, [Validators.required, Validators.email]),
                IsInvitationEnabled: this.fb.control(null),
                IsNotifyDeliveries: this.fb.control(null),
                IsNotifySchedules: this.fb.control(null),
                TempCompany: this.fb.control(null),
                ContactPersons: this.initializeContactPersons([]),
            }),
            AddressDetails: this.fb.group({
                Id: this.fb.control(null),
                JobName: this.fb.control(null, Validators.required),
                DisplayJobID: this.fb.control(null),
                JobId: this.fb.control(null),
                IsNewJob: this.fb.control(true),
                Address: this.fb.control(null),
                City: this.fb.control(null, Validators.required),
                StateId: this.fb.control(null, Validators.required),
                CountryId: this.fb.control(null, Validators.required),
                CountyName: this.fb.control(null),
                CountryName: this.fb.control(null),
                CountryCode: this.fb.control(null),
                StateName: this.fb.control(null),
                Currency: this.fb.control(null),
                ZipCode: this.fb.control(null),
                IsProFormaPoEnabled: this.fb.control(null),
                SignatureEnabled: this.fb.control(null),
                IsGeocodeUsed: this.fb.control(true),
                Latitude: this.fb.control(this.mapConstants.CenterLat, Validators.pattern('^[0-9.-]*$')),
                Longitude: this.fb.control(this.mapConstants.CenterLon, Validators.pattern('^[0-9.-]*$')),
                TimeZoneName: this.fb.control(null),
                LocationManagedType: this.fb.control(null),
                IsCompanyOwned: this.fb.control(null),
                MarineUoM: this.fb.control(null),
                IsMarineLocation: this.fb.control(null),
                InventoryDataCaptureType: this.fb.control(null),
                UOM: this.fb.control(1),
                DispatchRegionId: this.fb.control(null),
                TempJob: this.fb.control(null),
            }),
            FuelDetails: this.fb.group({
                Id: this.fb.control(null),
                FuelDisplayGroupId: this.fb.control(1),
                FuelTypeId: this.fb.control(null, Validators.required),
                QuantityTypeId: this.fb.control(3),
                Quantity: this.fb.control(0, [Validators.pattern(RegExConstants.DecimalNumber)]),
                MinimumQuantity: this.fb.control(0, [Validators.pattern(RegExConstants.DecimalNumber)]),
                MaximumQuantity: this.fb.control(0, [Validators.pattern(RegExConstants.DecimalNumber)]),
                QuantityIndicatorTypes: this.fb.control(1),
                NonStandardFuelName: this.fb.control(null),
                NonStandardFuelDescription: this.fb.control(null),
                IsTierPricing: this.fb.control(null),
                PricingTypeId: this.fb.control(2),
                PricePerGallon: this.fb.control(null),
                Fees: this.intilizeFuelRequestFees(),
            }),
            FuelDeliveryDetails: this.fb.group({
                Id: this.fb.control(null),
                DeliveryTypeId: this.fb.control(2),
                StartDate: this.fb.control(moment(new Date()).format('MM/DD/YYYY')),
                EndDate: this.fb.control(null),
                StartTime: this.fb.control('8:00 AM'),
                EndTime: this.fb.control('5:00 PM'),
                SingleDeliverySubTypes: this.fb.control(0),
                PaymentMethods: this.fb.control(null),
                PaymentTermId: this.fb.control(1),
                NetDays: this.fb.control(0),
                IsPrePostDipRequired: this.fb.control(null),
                OrderEnforcementId: this.fb.control(1),
            }),
            FuelPricingDetails: this.fb.group({
                Id: this.fb.control(null),
                LeadRequestId: this.fb.control(null),
                PricingTypeId: this.fb.control(2),
                PricePerGallon: this.fb.control(null, Validators.required),
                Code: this.fb.control(null),
                TempCode: this.fb.control(null),
                CodeId: this.fb.control(null),
                CodeDescription: this.fb.control(null),
                RackAvgTypeId: this.fb.control(1),
                RackPrice: this.fb.control(0),
                EnableCityRack: this.fb.control(null),
                TerminalName: this.fb.control(null),
                TempTerminalName: this.fb.control(null),
                TerminalId: this.fb.control(null),
                SupplierCostMarkupTypeId: this.fb.control(1),
                SupplierCostMarkupValue: this.fb.control(0),
                SupplierCost: this.fb.control(null),
                SupplierCostTypeId: this.fb.control(null),
                MarkertBasedPricingTypeId: this.fb.control(null),
                CityGroupTerminalId: this.fb.control(null),
                CityGroupTerminalName: this.fb.control(null),
                CityGroupTerminalStateId: this.fb.control(null),
                BrokerMarkUp: this.fb.control(null),
                Currency: this.fb.control(null),
                ExchangeRate: this.fb.control(null),
                IsTierPricingRequired: this.fb.control(null),
                DifferentFuelPrices: this.fb.control(null),
                FormattedPricing: this.fb.control(null),
                FuelTypeId: this.fb.control(null),
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
                PricingSourceId: this.fb.control(1),
                PricingNote: this.fb.control(null),
                TempPricingCodeDetails: this.fb.control(null),
                FuelPricingDetails: this.fb.group({
                    PricingSourceId: this.fb.control(1),
                    PricingCode: this.fb.control({ Id: null, Code: null, Description: null })
                }),
            }),
            AdditionalDetailsViewModel: this.fb.group({
                Id: this.fb.control(null),
                IsAssetTracked: this.fb.control(null),
                IsAssetDropStatusEnabled: this.fb.control(null)
            })
        });

    }
    get f() { return this.sourcingRequestForm.controls; }
    removeValidator() {
        this.f.CustomerDetails.get('CompanyName').setValidators([]);
        this.f.CustomerDetails.get('CompanyName').updateValueAndValidity();
    }
    addValidators() {
        let val = [Validators.required];
        this.f.CustomerDetails.get('CompanyName').setValidators(val);
        this.f.CustomerDetails.get('CompanyName').updateValueAndValidity();
    }

    initializeContactPersons(contactPersons: ContactPersonModel[]) {
        let contactPersonsForm = this.fb.array([]);
        for (var i = 0; i < contactPersons.length; i++) {
            var _contactPersonForm = this.fb.group({
                Name: this.fb.control(contactPersons[i].Name),
                PhoneNumber: this.fb.control(contactPersons[i].PhoneNumber, [Validators.required, Validators.pattern(this.regexPhone)]),
                Email: this.fb.control(contactPersons[i].Email, [Validators.required, Validators.email]),
                IsValidMobileNumber: this.fb.control(contactPersons[i].IsPhoneNumberConfirmed)
            });
            contactPersonsForm.push(_contactPersonForm);
        }
        return contactPersonsForm;
    }

    initializeContactPerson(contactPerson: ContactPersonModel) {
        return this.fb.group({
            Name: this.fb.control(contactPerson.Name),
            PhoneNumber: this.fb.control(contactPerson.PhoneNumber, [Validators.required, Validators.pattern(this.regexPhone)]),
            Email: this.fb.control(contactPerson.Email, [Validators.required, Validators.email]),
            IsPhoneNumberConfirmed: this.fb.control(contactPerson.IsPhoneNumberConfirmed)
        });
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

    public getDetailsPage(Id: any) {
        this.router.navigate(['SourcingRequest/Details/' + Id]);
    }


    clickNewPerson(isnew) {
        this.isPersonNew = isnew;
        if (isnew) {
            this.f.CustomerDetails.get('Name').setValidators([Validators.required]);
            this.f.CustomerDetails.get('UserId').setValidators([]);

        } else {
            this.f.CustomerDetails.get('UserId').setValidators([Validators.required]);
            this.f.CustomerDetails.get('Name').setValidators([]);
        }
        this.f.CustomerDetails.get('Name').updateValueAndValidity();
        this.f.CustomerDetails.get('UserId').updateValueAndValidity();

    }
    getPreferencesSettings() {
        this.IsLoading = true;
        let _this = this;
        this.salesUserService.GetPreferencesSettings().subscribe(data => {
            if (data) {
                this.companyPreferenceSetting = data.IsSupressOrderPricing;
                var FreightOnBoardType = this.FreightOnBoardTypesList.find(t => t.Id == data.FreightOnBoardType);
                var truckload = this.TruckTypeLoadList.find(t => t.Id == data.TruckLoadType);
                var truckLoadType;
                if (truckload) {
                    truckLoadType = truckload.Id;
                }
                if (FreightOnBoardType != null) {
                    this.sourcingRequestForm.get('FreightOnBoardType').patchValue(FreightOnBoardType.Id);
                }
                this.sourcingRequestForm.get('TruckLoadType').patchValue(truckLoadType);
                this.sourcingRequestForm.get('CustomerDetails').patchValue(data.CustomerDetails);
                this.sourcingRequestForm.get('AddressDetails').patchValue(data.AddressDetails);
                this.sourcingRequestForm.get('IsSupressOrderPricing').patchValue(data.IsSupressOrderPricing);
                this.setCurrency(data.AddressDetails.Currency);
                this.sourcingRequestForm.controls['FuelDetails']['controls']['FuelDisplayGroupId'].patchValue(data.FuelDetails.FuelDisplayGroupId);
                this.sourcingRequestForm.controls['FuelDetails']['controls']['QuantityTypeId'].patchValue(data.FuelDetails.QuantityTypeId);

                if (!_this.f.IsSupressOrderPricing.value) {
                    _this.f.FuelPricingDetails.get('PricePerGallon').setValidators([Validators.required]);
                }
                _this.f.FuelPricingDetails.get('TierPricing').get('ResetCumulationSetting').get('CumulationType').setValue(1);
                if (_this.pricingModuleComponent) {
                    _this.pricingModuleComponent.setPricingCode();
                }

                if (!this.editSourcingId) {
                    if (this.DispatchRegionList && this.DispatchRegionList.length == 1) {
                        this.sourcingRequestForm.get('AddressDetails').get('DispatchRegionId').setValue(this.DispatchRegionList[0].Id);
                    }
                }
                this.IsLoading = false;
                this.pageloader = false;
            }
        });
    }
    getAllTPOCompanies() {
        this.IsLoading = true;
        this.salesUserService.GetAllTPOCompanies().subscribe((data => {
            if (data) {
                var listdata = data.map(item => {
                    return {
                        id: item.Id,
                        text: item.Name
                    }
                });
                this.AllTPOCompaniesList = listdata;

                //this.IsLoading = false;
            }
            let esourcingId: number = this.route_active.snapshot.params.Id;
            if (esourcingId && esourcingId > 0) {
                this.editSourcingId = esourcingId;
                this.getSourcingDetails();
            } else {
                this.getFuelProducts();
                this.getPreferencesSettings();
            }
            this.IsLoading = false;
        }));
    }
    public getSourcingCompanyContactPersons(event: any) {
        var companyId = event;
        this.IsLoading = true;

        this.salesUserService.GetSourcingCompanyContactPersons(companyId).subscribe(data => {
            if (data) {
                this.CompanyContactPersonsList = data;
                let companyname = this.AllTPOCompaniesList.filter(x => x.id == companyId);
                this.sourcingRequestForm.get('CustomerDetails').get('CompanyName').setValue(companyname[0].text);
                this.IsLoading = false;
            } else {
                this.IsLoading = false;
            }
        });
    }
    public getSourcingContactPersonDetails(userId: number) {
        this.salesUserService.GetSourcingContactPersonDetails(userId).subscribe(data => {
            if (data) {
                this.CompanyContactPersonsDetails = data;
                // this.CompanyContactPersonsDetails['IsNewCompany']=false;
                this.sourcingRequestForm.get('CustomerDetails').get('PhoneNumber').setValue(this.CompanyContactPersonsDetails.PhoneNumber);
                this.sourcingRequestForm.get('CustomerDetails').get('Email').setValue(this.CompanyContactPersonsDetails.Email);
                // this.sourcingRequestForm.get('CustomerDetails').patchValue(this.CompanyContactPersonsDetails);
            }
        });
    }

    public getTruckLoadType() {
        this.TruckTypeLoadList = [];
        this.salesUserService.GetTruckLoadType().subscribe(data => {
            if (data && data.length > 0) {
                this.TruckTypeLoadList = data;
                this.sourcingRequestForm.get('TruckLoadType').setValue(data[0].Id);
            }
        });
    }
    public getFreightOnBoardTypes() {
        this.FreightOnBoardTypesList = [];
        this.salesUserService.GetFreightOnBoardTypes().subscribe(data => {
            if (data && data.length > 0) {
                this.FreightOnBoardTypesList = data;
                this.sourcingRequestForm.get('FreightOnBoardType').setValue(data[0].Id);
            }
        });
    }

    public getCountryList() {
        this.salesUserService.GetCountryList().subscribe(data => {
            if (data && data.length > 0) {
                this.countryList = data;
            }
        });
    }

    public getCurrecyList() {
        this.salesUserService.GetCurrenyList().subscribe(data => {
            if (data && data.length > 0) {
                this.currucyList = data;
            }
        });
    }
    public getUoMList() {
        this.salesUserService.GetUoMList().subscribe(data => {
            if (data && data.length > 0) {
                this.UomList = data;
            }
        });
    }

    public getStatesOfAllCountries(countryId?: number) {
        this.salesUserService.GetStatesOfAllCountries(countryId).subscribe(data => {
            if (data && data.length > 0) {
                this.statesList = data;
                this.filteredStatesList = this.statesList;
            }
        });
    }
    public getcountryGroupList() {
        this.salesUserService.GetCountryGroupList(4).subscribe(data => {
            if (data && data.length > 0) {
                this.countryGroupList = data;
            }
        });
    }

    public getJobLists(companyId, isFtl, foAsTerminal) {
        let companyName = this.AllTPOCompaniesList.find(t => t.id == companyId).text;
        let ftlvalue = isFtl == "FullTruckLoad" ? true : false;
        let tervalue = foAsTerminal == "Terminal" ? true : false;
        this.salesUserService.GetJobLists(companyName, ftlvalue, tervalue).subscribe(data => {
            if (data) {
                let joblistdata = data.map(item => {
                    return {
                        id: item.Id,
                        text: item.Name
                    }
                });
                this.allJobList = joblistdata;
            }
        });
    }
    public getJobDetails(jobId: string, companyName: string) {
        let job = this.allJobList.find(x => x.id == jobId);
        if (job != null) {
            this.salesUserService.GetJobDetails(job.text, companyName).subscribe(data => {
                if (data) {
                    this.sourcingRequestForm.get('AddressDetails').patchValue(data.AddressDetails);
                }
            });
        }


    }

    public getFuelProducts() {
        //this.f.FuelDetails.get('FuelTypeId').setValue(null);

        let companyId = this.sourcingRequestForm.controls['CustomerDetails']['controls']['CompanyId'].value || 0;
        let jobId = this.sourcingRequestForm.controls['AddressDetails']['controls']['JobId'].value || 0;

        var productDisplayGroupId = this.f.FuelDetails.get('FuelDisplayGroupId').value ? this.f.FuelDetails.get('FuelDisplayGroupId').value : 1;
        this.IsLoading = true;
        this.salesUserService.GetFuelProducts(productDisplayGroupId, companyId, jobId).subscribe(data => {
            if (data) {
                this.FuelProductsList = data;
                this.IsLoading = false;
            } else {
                this.IsLoading = false;
            }
        });
    }
    public getProductListByZip() {
        //this.f.FuelDetails.get('FuelTypeId').setValue(null);
        let zipCode = this.sourcingRequestForm.controls['AddressDetails']['controls']['ZipCode'].value;
        this.IsLoading = true;
        this.salesUserService.GetProductListByZip(zipCode).subscribe(data => {
            if (data) {
                this.FuelProductsList = data;
                this.IsLoading = false;
            } else {
                this.IsLoading = false;
            }
        });
    }

    public getAllFeeTypes(companyId: any, currency: any, truckLoadType: any) {
        this.salesUserService.GetAllFeeTypes(companyId, currency, truckLoadType).subscribe(data => {
            if (data) {
                this.FeeTypesList = data;
            }
        });
    }
    public getAllFeeSubTypes(feeTypeId: any, Currency: any) {
        this.salesUserService.GetAllFeeSubTypes(feeTypeId, Currency).subscribe(data => {
            if (data) {
                this.FeeSubTypesList = data;
            }
        });
    }
    public getAllFeeConstraintTypes() {
        this.salesUserService.GetAllFeeConstraintTypes().subscribe(data => {
            if (data) {
                this.FeeConstraintTypesList = data;
            }
        });
    }
    public getPaymentMethods() {
        this.salesUserService.PaymentMethods().subscribe(data => {
            if (data) {
                this.PaymentMethodsList = data;
            }
        });
    }
    public getRackAvgPricingTypes() {
        this.salesUserService.GetRackAvgPricingTypes().subscribe(data => {
            if (data) {
                this.RackAvgPricingTypesList = data;
            }
        });
    }

    addFees() {
        let fee = this.sourcingRequestForm.get('FuelDetails').get('FuelRequestFees') as FormArray;
        fee.push(
            this.fb.group({
                FeeTypeId: this.fb.control(null, Validators.required),
                FeeSubTypeId: this.fb.control(null, Validators.required),
                FeeSubTypeName: this.fb.control(null, Validators.required),
                Fee: this.fb.control(null, Validators.required),
                FeeDetails: this.fb.control(null, Validators.required),
                FeeConstraintTypeId: this.fb.control(null, Validators.required),
                IncludeInPPG: this.fb.control(null, Validators.required),
                OtherFeeTypeId: this.fb.control(null, Validators.required),
            })
        );
    }

    public isSourcingCompanyExist() {
        this.companyExits = false;
        if (this.f.CustomerDetails.get('CompanyName').value) {
            this.salesUserService.IsSourcingCompanyExist(this.f.CustomerDetails.get('IsNewCompany').value, this.f.CustomerDetails.get('CompanyName').value).subscribe(data => {
                if (data != null || data != undefined) {
                    this.companyExits = data;
                }
            });
        }

    }
    public GetDispatchRegions() {
        this.DispatchRegionList = [];
        this.salesUserService.GetDispatchRegions().subscribe(data => {
            if (data && data.length > 0) {
                this.DispatchRegionList = data;
                if (data.length == 1) {
                    this.sourcingRequestForm.get('AddressDetails').get('DispatchRegionId').setValue(data[0].Id);
                }
            }
        });
    }

    public getSourcingDetails() {
        this.pageloader = true;
        this.changeDetectorRef.detectChanges()
        let _this = this;
        // this.initailizeSourcingReqForm();
        this.salesUserService.GetSourcingDetailsById(this.editSourcingId).subscribe(data => {
            if (data) {
                // this.sourcingRequestForm.patchValue(data);
                this.sourcingRequestForm.get('Id').patchValue(data.Id);
                this.sourcingRequestForm.get('FreightOnBoardType').patchValue(data.FreightOnBoardType);
                this.sourcingRequestForm.get('TruckLoadType').patchValue(data.TruckLoadType);
                this.sourcingRequestForm.get('AccountingCompanyId').patchValue(data.AccountingCompanyId);
                this.sourcingRequestForm.get('DisplayRequestId').patchValue(data.DisplayRequestId);
                this.sourcingRequestForm.get('RequestName').patchValue(data.RequestName);
                this.sourcingRequestForm.get('RequestStatus').patchValue(data.RequestStatus);
                this.sourcingRequestForm.get('SalesUserId').patchValue(data.SalesUserId);
                this.sourcingRequestForm.get('IsSupressOrderPricing').patchValue(data.IsSupressOrderPricing);
                if (!data.CustomerDetails.IsNewCompany) {
                    this.getSourcingCompanyContactPersons(data.CustomerDetails.CompanyId);
                    this.getJobLists(data.CustomerDetails.CompanyId, this.f.TruckLoadType.value, this.f.FreightOnBoardType.value);
                }
                if (data.CustomerDetails.UserId) {
                    this.isPersonNew = false;
                }
                this.sourcingRequestForm.get('CustomerDetails').patchValue(data.CustomerDetails);
                let contactPersons = this.sourcingRequestForm.get('CustomerDetails').get('ContactPersons') as FormArray;
                if (data.CustomerDetails && data.CustomerDetails.ContactPersons && data.CustomerDetails.ContactPersons.length > 0) {
                    for (var i = 0; i < data.CustomerDetails.ContactPersons.length; i++) {
                        contactPersons.push(this.initializeContactPerson(data.CustomerDetails.ContactPersons[i]));
                    }
                }
                this.sourcingRequestForm.get('AddressDetails').patchValue(data.AddressDetails);
                this.sourcingRequestForm.get('FuelDetails').patchValue(data.FuelDetails);
                this.LeadFees = data.FuelDetails.Fees;
                this.sourcingRequestForm.get('FuelDeliveryDetails').patchValue(data.FuelDeliveryDetails);
                this.sourcingRequestForm.get('AdditionalDetailsViewModel').patchValue(data.AdditionalDetailsViewModel);
                //this.getSourcingCityGroupTerminals(data.AddressDetails.StateId,(data.FuelPricingDetails.Code=="AXIS") ? 1 : 2);
                this.PrcingCodevalue = data.FuelPricingDetails.Code;
                this.GeneralNotesHistory = data.GeneralNotesHistory;
                // this.sourcingRequestForm.get('FuelPricingDetails').patchValue(data.FuelPricingDetails);
                // this.sourcingRequestForm.get('FuelPricingDetails').get('TempCode').patchValue(data.FuelPricingDetails);
                // this.sourcingRequestForm.get('FuelPricingDetails').get('TempTerminalName').patchValue(data.FuelPricingDetails.TerminalName);

                if (_this.pricingModuleComponent) {
                    _this.pricingModuleComponent.patchExistingPricingDetails(data.FuelPricingDetails);
                }

                this.UpdateViewedStatus();
                this.setCurrency(data.AddressDetails.Currency);

                if (!data.CustomerDetails.IsNewCompany) {
                    this.f.CustomerDetails.get('TempCompany').setValue([{ id: data.CustomerDetails.CompanyId, text: data.CustomerDetails.CompanyName }]);
                }
                if (!data.AddressDetails.IsNewJob) {
                    this.f.AddressDetails.get('TempJob').setValue([{ id: data.AddressDetails.JobId, text: data.AddressDetails.JobName }]);
                }

                this.getFuelProducts();
                this.isRegularBuyerUpdate(data.CustomerDetails.CompanyId, true);

                this.pageloader = false;
                this.changeDetectorRef.detectChanges();
            }
        });
    }

    onSubmit() {
        this.formSubmited = true;
        this.resetPOValidation();
        this.setSaveValidations();
        if (this.sourcingRequestForm.invalid || this.companyExits) {
            return false;
        }

        if (this.f.FuelPricingDetails.get('IsTierPricingRequired').value) {
            let pricings = this.f.FuelPricingDetails.get('TierPricing').get('Pricings') as FormArray;
            pricings.controls.forEach((pricing: FormGroup) => {
                pricing.get('UoM').setValue(this.UoM);
                pricing.get('Currency').setValue(this.UoM);
            })
        }

        if (this.editSourcingId > 0) {
            this.pageloader = true;
            this.changeDetectorRef.detectChanges()
            this.salesUserService.SaveEditSourcingDetails(this.sourcingRequestForm.getRawValue()).subscribe(data => {
                this.pageloader = false;
                this.changeDetectorRef.detectChanges()
                if (data && data.StatusCode == 0) {
                    Declarations.msgsuccess("Request Updated successfully", undefined, undefined);
                    this.router.navigate(['SalesUser/SourcingRequest/Index']);
                } else {
                    Declarations.msgerror(data.StatusMessage, undefined, undefined);
                    return;
                }
            });
        } else {
            this.pageloader = true;
            this.changeDetectorRef.detectChanges()
            this.salesUserService.CreateSourcingRequest(this.sourcingRequestForm.getRawValue()).subscribe(data => {
                this.pageloader = false;
                this.changeDetectorRef.detectChanges()
                if (data && data.StatusCode == 0) {
                    Declarations.msgsuccess("Request created successfully.", undefined, undefined);
                    this.router.navigate(['SalesUser/SourcingRequest/Index']);
                } else {
                    Declarations.msgerror(data.StatusMessage, undefined, undefined);
                    return;
                }
            });
        }
    }

    setSaveValidations() {
        if (this.f.CustomerDetails.get('IsNewCompany').value) {
            this.f.CustomerDetails.get('Name').setValidators([Validators.required]);
            this.f.CustomerDetails.get('Name').updateValueAndValidity();
        } else {
            if (this.isPersonNew) {
                this.f.CustomerDetails.get('Name').setValidators([Validators.required]);
                this.f.CustomerDetails.get('Name').updateValueAndValidity();
            } else {
                this.f.CustomerDetails.get('UserId').setValidators([Validators.required]);
                this.f.CustomerDetails.get('UserId').updateValueAndValidity();
            }
        }

        if (this.f.FuelDetails.get('QuantityTypeId').value == 1) {
            this.f.FuelDetails.get('Quantity').setValidators([Validators.required, Validators.pattern('^[0-9.-]*$')]);
            this.f.FuelDetails.get('Quantity').updateValueAndValidity();
        } else if (this.f.FuelDetails.get('QuantityTypeId').value == 2) {
            this.f.FuelDetails.get('MinimumQuantity').setValidators([Validators.required, Validators.pattern('^[0-9.-]*$')]);
            this.f.FuelDetails.get('MinimumQuantity').updateValueAndValidity();

            this.f.FuelDetails.get('MaximumQuantity').setValidators([Validators.required, Validators.pattern('^[0-9.-]*$')]);
            this.f.FuelDetails.get('MaximumQuantity').updateValueAndValidity();
        }

        this.setNetDaysValidation(this.sourcingRequestForm.get('IsSupressOrderPricing').value);
    }
    createPO() {
        this.setPOValidation();
        this.formSubmited = true;
        this.pageloader = true;
        if (this.sourcingRequestForm.valid && !this.companyExits) {
            this.pageloader = true;
            this.salesUserService.CreateOrderFromSourcingRequest(this.sourcingRequestForm.value).subscribe(data => {
                if (data.StatusCode == 0) {
                    Declarations.msgsuccess("PO created successfully.", undefined, undefined);
                    this.router.navigate([]).then(result => { window.location.href = "/Supplier/Order/View"; });
                    this.pageloader = false;
                } else {
                    Declarations.msgerror(data.StatusMessage, undefined, undefined);
                    this.pageloader = false;
                    return;
                }
            });
        } else {
            this.pageloader = false;
        }
    }

    setPOValidation() {
        let required = [Validators.required];
        this.f.TruckLoadType.setValidators(required);
        this.f.TruckLoadType.updateValueAndValidity();

        this.f.FreightOnBoardType.setValidators(required);
        this.f.FreightOnBoardType.updateValueAndValidity();

        // this.f.AccountingCompanyId.setValidators(required);
        // this.f.AccountingCompanyId.updateValueAndValidity();

        // this.f.DisplayRequestId.setValidators(required);
        // this.f.DisplayRequestId.updateValueAndValidity();

        // this.f.RequestName.setValidators(required);
        // this.f.RequestName.updateValueAndValidity();

        if (this.f.FuelDeliveryDetails.get('DeliveryTypeId').value == 1) {
            this.f.FuelDeliveryDetails.get('SingleDeliverySubTypes').setValidators(required);
            this.f.FuelDeliveryDetails.get('SingleDeliverySubTypes').updateValueAndValidity();
        } else {
            this.f.FuelDeliveryDetails.get('SingleDeliverySubTypes').setValidators([]);
            this.f.FuelDeliveryDetails.get('SingleDeliverySubTypes').updateValueAndValidity();
        }

        this.f.FuelDeliveryDetails.get('StartDate').setValidators(required);
        this.f.FuelDeliveryDetails.get('StartDate').updateValueAndValidity();

        this.f.FuelDeliveryDetails.get('StartTime').setValidators(required);
        this.f.FuelDeliveryDetails.get('StartTime').updateValueAndValidity();

        this.f.FuelDeliveryDetails.get('EndTime').setValidators(required);
        this.f.FuelDeliveryDetails.get('EndTime').updateValueAndValidity();

        if (this.f.FuelDetails.get('QuantityTypeId').value == 1) {
            this.f.FuelDetails.get('Quantity').setValidators([Validators.required, Validators.pattern('^[0-9.-]*$')]);
            this.f.FuelDetails.get('Quantity').updateValueAndValidity();
        } else if (this.f.FuelDetails.get('QuantityTypeId').value == 2) {
            this.f.FuelDetails.get('MinimumQuantity').setValidators([Validators.required, Validators.pattern('^[0-9.-]*$')]);
            this.f.FuelDetails.get('MinimumQuantity').updateValueAndValidity();

            this.f.FuelDetails.get('MaximumQuantity').setValidators([Validators.required, Validators.pattern('^[0-9.-]*$')]);
            this.f.FuelDetails.get('MaximumQuantity').updateValueAndValidity();
        } else {
            this.f.FuelDetails.get('Quantity').setValidators([Validators.pattern('^[0-9.-]*$')]);
            this.f.FuelDetails.get('Quantity').updateValueAndValidity();
            this.f.FuelDetails.get('MinimumQuantity').setValidators([Validators.pattern('^[0-9.-]*$')]);
            this.f.FuelDetails.get('MinimumQuantity').updateValueAndValidity();
            this.f.FuelDetails.get('MaximumQuantity').setValidators([Validators.pattern('^[0-9.-]*$')]);
            this.f.FuelDetails.get('MaximumQuantity').updateValueAndValidity();
        }

        this.f.AddressDetails.get('CountyName').setValidators(required);
        this.f.AddressDetails.get('CountyName').updateValueAndValidity();
        this.f.AddressDetails.get('StateId').setValidators(required);
        this.f.AddressDetails.get('StateId').updateValueAndValidity();
        this.f.AddressDetails.get('ZipCode').setValidators(required);
        this.f.AddressDetails.get('ZipCode').updateValueAndValidity();
        this.f.AddressDetails.get('Address').setValidators(required);
        this.f.AddressDetails.get('Address').updateValueAndValidity();
        this.setNetDaysValidation(this.sourcingRequestForm.get('IsSupressOrderPricing').value);


        this.f.CustomerDetails.get('PhoneNumber').setValidators(required);
        this.f.CustomerDetails.get('PhoneNumber').updateValueAndValidity();
        this.f.CustomerDetails.get('Email').setValidators(required);
        this.f.CustomerDetails.get('Email').updateValueAndValidity();
    }

    setNetDaysValidation(isSuppressPricing: boolean) {
        if (this.f.FuelDeliveryDetails.get('PaymentTermId').value == 1 && !isSuppressPricing) {
            this.f.FuelDeliveryDetails.get('NetDays').setValidators([Validators.required, Validators.min(1)]);
            this.f.FuelDeliveryDetails.get('NetDays').updateValueAndValidity();
        } else {
            this.f.FuelDeliveryDetails.get('NetDays').setValidators([]);
            this.f.FuelDeliveryDetails.get('NetDays').updateValueAndValidity();
        }
    }
    resetPOValidation() {

        this.f.TruckLoadType.setValidators([]);
        this.f.TruckLoadType.updateValueAndValidity();

        this.f.FreightOnBoardType.setValidators([]);
        this.f.FreightOnBoardType.updateValueAndValidity();

        this.f.AccountingCompanyId.setValidators([]);
        this.f.AccountingCompanyId.updateValueAndValidity();

        this.f.DisplayRequestId.setValidators([]);
        this.f.DisplayRequestId.updateValueAndValidity();

        this.f.RequestName.setValidators([]);
        this.f.RequestName.updateValueAndValidity();

        this.f.FuelDeliveryDetails.get('SingleDeliverySubTypes').setValidators([]);
        this.f.FuelDeliveryDetails.get('SingleDeliverySubTypes').updateValueAndValidity();

        this.f.FuelDeliveryDetails.get('StartDate').setValidators([]);
        this.f.FuelDeliveryDetails.get('StartDate').updateValueAndValidity();

        this.f.FuelDeliveryDetails.get('StartTime').setValidators([]);
        this.f.FuelDeliveryDetails.get('StartTime').updateValueAndValidity();

        this.f.FuelDeliveryDetails.get('EndTime').setValidators([]);
        this.f.FuelDeliveryDetails.get('EndTime').updateValueAndValidity();

        this.f.FuelDeliveryDetails.get('NetDays').setValidators([]);
        this.f.FuelDeliveryDetails.get('NetDays').updateValueAndValidity();

        this.f.CustomerDetails.get('PhoneNumber').setValidators([]);
        this.f.CustomerDetails.get('PhoneNumber').updateValueAndValidity();
        this.f.CustomerDetails.get('Email').setValidators([]);
        this.f.CustomerDetails.get('Email').updateValueAndValidity();
        this.f.AddressDetails.get('ZipCode').setValidators([]);
        this.f.AddressDetails.get('ZipCode').updateValueAndValidity();
        this.f.AddressDetails.get('Address').setValidators([]);
        this.f.AddressDetails.get('Address').updateValueAndValidity();
        this.f.AddressDetails.get('CountyName').setValidators([]);
        this.f.AddressDetails.get('CountyName').updateValueAndValidity();
    }


    getAddress() {

        let address = this.f.AddressDetails.get('Address').value || '';
        let state = this.f.AddressDetails.get('StateName').value || '';
        let country = this.f.AddressDetails.get('CountryCode').value || ''
        let city = this.f.AddressDetails.get('City').value || '';
        let zipcode = this.f.AddressDetails.get('ZipCode').value || '';

        if (address == '' || state == '' || country == '' || zipcode == '')
            return;

        address = address + " " + city + " " + state + " " + country + " " + zipcode;

        this.salesUserService.GetAddress(address).subscribe((data) => {
            this.updateAddressData(data)
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

    public acceptRequest() {
        this.salesUserService.ChangesSourcingRequestStatus(this.RequestStatus.Accepted, this.editSourcingId).subscribe(data => {
            if (data.StatusCode == 0) {
                Declarations.msgsuccess("Request Accepted.", undefined, undefined);
                this.router.navigate(['SalesUser/SourcingRequest/Index']);
            } else {
                Declarations.msgerror(data.StatusMessage, undefined, undefined);
                return;
            }
        });
    }

    public loseRequest() {
        this.salesUserService.ChangesSourcingRequestStatus(this.RequestStatus.Lost, this.editSourcingId).subscribe(data => {
            if (data.StatusCode == 0) {
                Declarations.msginfo("Request Lost.", undefined, undefined);
                this.router.navigate(['SalesUser/SourcingRequest/Index']);
            } else {
                Declarations.msgerror(data.StatusMessage, undefined, undefined);
                return;
            }
        });
    }
    // openPriceCodeModal(pricingcodeModal) {
    //   this.getPricingCodes();
    //   this.modalService.open(pricingcodeModal, { windowClass: 'pricingcode-modal', size: 'lg', scrollable: true });
    // }

    public isCitySourcingGroupTerminalPriceAvailable(jobId: any, fueltypeId: any, selectedCityRackId: any, lattitude?: any, longitude?: any, countryCode?: string, sourceId?: any) {
        this.salesUserService.IsCitySourcingGroupTerminalPriceAvailable(jobId, fueltypeId, selectedCityRackId, lattitude, longitude, countryCode, sourceId).subscribe(data => {
            if (data) {
                this.CitySourcingGroupTerminalPriceAvailableList = data;
            }
        });
    }
    // async getSourcingCityGroupTerminals(stateId: any, sourceId: any) {
    //   this.salesUserService.GetSourcingCityGroupTerminals(stateId,sourceId).subscribe(data => {
    //       if (data) {
    //               this.SourcingCityGroupTerminalsList = data;
    //           }
    //       });
    //   }
    // public getClosedTerminal(event) {
    //  var fuelTypeId=this.f.FuelDetails.get('FuelTypeId').value;
    //  var latitude=this.f.AddressDetails.get('Latitude').value;
    //  var longitude=this.f.AddressDetails.get('Longitude').value;
    //  var countryId=this.f.AddressDetails.get('CountryId').value;
    //  var pricingCodeId=this.f.FuelPricingDetails.get('CodeId').value;
    //  var CityGroupTerminalId=this.f.FuelPricingDetails.get('CityGroupTerminalId').value;
    // //  var terminal= this.SourcingCityGroupTerminalsList.find(t => t.Id == CityGroupTerminalId).Name;
    // var terminal= "";
    // var pricingSourceId=this.f.FuelPricingDetails.get('PricingSourceId').value;

    //  if(pricingSourceId==2){
    //   //  this.getOpisTerminals(CityGroupTerminalId,latitude,longitude,countryId,terminal,pricingSourceId);

    //       this.salesUserService.GetOpisTerminals(CityGroupTerminalId, latitude, longitude, countryId, terminal, pricingSourceId).subscribe(data => {
    //           if (data) {
    //                   this.ClosedTerminalList = data;
    //                   event.stopPropagation();
    //                   this.approveTerminalAuto.open();
    //               }
    //           });

    //  }else{
    //   this.salesUserService.GetClosedTerminal(fuelTypeId, latitude, longitude, countryId, pricingCodeId, terminal, pricingSourceId).subscribe(data => {
    //     if (data) {
    //             this.ClosedTerminalList = data;
    //         }
    //     });
    //  }
    // }

    // getApprovedTerminal(event){
    //   var fuelTypeId=this.f.FuelDetails.get('FuelTypeId').value;
    //   var latitude=this.f.AddressDetails.get('Latitude').value;
    //   var longitude=this.f.AddressDetails.get('Longitude').value;
    //   var countryId=this.f.AddressDetails.get('CountryId').value;
    //   var pricingCodeId=this.f.FuelPricingDetails.get('CodeId').value;
    //   var CityGroupTerminalId=this.f.FuelPricingDetails.get('CityGroupTerminalId').value;
    //   var terminal=event;
    //   var pricingSourceId=this.f.FuelPricingDetails.get('PricingSourceId').value;
    //   if(pricingSourceId==2){
    //     this.getOpisTerminals(CityGroupTerminalId,latitude,longitude,countryId,terminal,pricingSourceId);
    //   }else{
    //    this.salesUserService.GetClosedTerminal(fuelTypeId, latitude, longitude, countryId, pricingCodeId, terminal, pricingSourceId).subscribe(data => {
    //      if (data) {
    //              this.ClosedTerminalList = data;
    //          }
    //      });
    //   }
    // }

    public getOpisTerminals(cityRackId: any, latitude: any, longitude: any, countryId: any, terminal: string, source: any) {
        this.salesUserService.GetOpisTerminals(cityRackId, latitude, longitude, countryId, terminal, source).subscribe(data => {
            if (data) {
                this.ClosedTerminalList = data;
                this.approveTerminalAuto.open();
            }
        });
    }
    // setTermialvalue(event){
    //   this.f.FuelPricingDetails.get('TerminalName').patchValue(event.Name);
    // }
    // getPricingCodes(isFromControl?: any, prefix?: any) {
    //   this.isPriceCodeLoading=true;
    //   var filterData = {};
    //   if (!prefix) {
    //     prefix = "";
    //   }
    //   if (isFromControl) {
    //     filterData = {
    //       "PricingSourceId": this.f.FuelPricingDetails.get('PricingSourceId').value,
    //       "PricingTypeId": this.f.FuelPricingDetails.get('PricingTypeId').value,
    //       "tfxProdId": this.f.FuelDetails.get("FuelTypeId").value,
    //       "feedTypeId": 0,
    //       "fuelClassTypeId": 0,
    //       "Prefix": prefix
    //     }
    //   } else {
    //     filterData = {
    //       "PricingSourceId": this.f.FuelPricingDetails.get('PricingSourceId').value,
    //       "PricingTypeId": this.f.FuelPricingDetails.get('PricingTypeId').value,
    //       "tfxProdId": this.f.FuelDetails.get("FuelTypeId").value,
    //       "feedTypeId": this.pricingfeedTypeId,
    //       "fuelClassTypeId": this.pricingfuelClassTypeId,
    //       "Prefix": ""
    //     }

    //   }

    //   this.salesUserService.GetPricingCodes(filterData).subscribe(data => {
    //     if (data) {
    //       this.pricingCodes = data.PricingCodes;
    //       this.isPriceCodeLoading=false;
    //     }
    //   });
    // }
    // getSelectedPricingCode(item: any) {
    //   this.modalService.dismissAll();
    //   var pricingCodeDisplayData = this.getPricingDisplayData(item);
    //   if (item) {
    //     this.f.FuelPricingDetails.get('Code').patchValue(item.Code);
    //     this.f.FuelPricingDetails.get('CodeId').patchValue(item.Id);
    //     this.f.FuelPricingDetails.get('PricingTypeId').patchValue(item.PricingTypeId);
    //     this.f.FuelPricingDetails.get('CodeDescription').patchValue(pricingCodeDisplayData);
    //     this.f.FuelPricingDetails.get('PricingSourceId').patchValue(item.PricingSourceId);
    //   }
    //   if (item.PricingSourceId == 1) {
    //     this.f.FuelPricingDetails.get('EnableCityRack').setValue(false);
    //   }
    //   else {
    //     this.f.FuelPricingDetails.get('EnableCityRack').setValue(true);
    //      this.getSourcingCityGroupTerminals(this.f.AddressDetails.get('StateId').value,this.f.FuelPricingDetails.get('PricingSourceId').value)
    //   }
    //   this.setRackTerminalValidation();
    // }

    setRackTerminalValidation() {
        let isChecked = this.f.FuelPricingDetails.get('EnableCityRack').value;
        if (isChecked) {
            this.f.FuelPricingDetails.get('CityGroupTerminalId').setValidators([Validators.required]);
            this.f.FuelPricingDetails.get('CityGroupTerminalId').updateValueAndValidity();
        }
        else {
            this.f.FuelPricingDetails.get('CityGroupTerminalId').setValidators([]);
            this.f.FuelPricingDetails.get('CityGroupTerminalId').updateValueAndValidity();
        }
    }
    getPricingDisplayData(item) {
        var displayData = '';
        if (item != undefined || item != null) {
            if (item.PricingTypeId == 2) {
                displayData += item.PricingSource + ', ' + "Fixed";
            }
            else if (item.PricingTypeId == 4) {
                displayData += item.PricingSource + ', ' + "Fuel Cost";
            }
            else if (item.PricingTypeId == 1) {
                displayData += item.PricingSource + ', ' + item.RackAvgPricingType;
                if (item.PricingSourceId == 2 || item.PricingSourceId == 3) {
                    displayData += ', ' + item.FeedType + ', ' + item.WeekendPricingDay;
                }
                if (item.PricingSourceId == 2) {
                    displayData += ', ' + item.FuelClassType + ', ' + item.QuantityIndicator;
                }
            }
        }
        return displayData;
    }

    public getAddressByZip() {
        var zipCode = this.f.AddressDetails.get('ZipCode').value;
        if (zipCode) {
            this.salesUserService.GetAddressByZip(zipCode).subscribe(data => {
                if (data) {
                    this.f.AddressDetails.get('CountryName').patchValue(data.CountryName);
                    var country = this.countryList.find(t => t.Code.includes(data.CountryCode));
                    if (country) {
                        var countryId = country.Id;
                        this.f.AddressDetails.get('CountryCode').patchValue(countryId);
                        if (countryId == 1) {
                            this.f.AddressDetails.get('CountryId').patchValue(countryId);
                            this.f.AddressDetails.get('Currency').patchValue(1);
                            this.f.AddressDetails.get('UOM').patchValue(1);
                        } else {
                            this.f.AddressDetails.get('CountryId').patchValue(countryId);
                            this.f.AddressDetails.get('Currency').patchValue(2);
                            this.f.AddressDetails.get('UOM').patchValue(2);
                        }

                        this.f.AddressDetails.get('Address').patchValue(data.Address);
                        this.f.AddressDetails.get('CountyName').patchValue(data.CountyName);
                        this.f.AddressDetails.get('City').patchValue(data.City);
                        var stateId = this.statesList.find(x => x.StateCode == data.StateCode).StateId;
                        this.f.AddressDetails.get('StateId').patchValue(stateId);
                        this.f.AddressDetails.get('StateName').patchValue(data.StateName);
                        this.f.AddressDetails.get('Latitude').patchValue(data.Latitude);
                        this.f.AddressDetails.get('Longitude').patchValue(data.Longitude);
                        this.mapConstants.CenterLat = data.Latitude;
                        this.mapConstants.CenterLon = data.Longitude;
                        this.filteredStatesList = this.statesList.filter(s => s.CountryId == countryId) || [];
                    }
                }
            });
        }

    }

    // addValidationPrice(){
    //   let required = [Validators.required];
    //   if (this.f.FuelPricingDetails.get('PricingTypeId').value == 1) {
    //     this.f.FuelPricingDetails.get('Code').setValidators(required);
    //     this.f.FuelPricingDetails.get('Code').updateValueAndValidity();
    //     this.f.FuelPricingDetails.get('RackPrice').setValidators(required);
    //     this.f.FuelPricingDetails.get('RackPrice').updateValueAndValidity();
    //   }if(this.f.FuelPricingDetails.get('PricingTypeId').value == 4){
    //     this.f.FuelPricingDetails.get('SupplierCostMarkupValue').setValidators(required);
    //     this.f.FuelPricingDetails.get('SupplierCostMarkupValue').updateValueAndValidity();
    //     this.f.FuelPricingDetails.get('PricePerGallon').setValidators([]);
    //     this.f.FuelPricingDetails.get('PricePerGallon').updateValueAndValidity();
    //     this.f.FuelPricingDetails.get('Code').setValidators([]);
    //     this.f.FuelPricingDetails.get('Code').updateValueAndValidity();
    //     this.f.FuelPricingDetails.get('RackPrice').setValidators([]);
    //     this.f.FuelPricingDetails.get('RackPrice').updateValueAndValidity();
    //   } else if(this.f.FuelPricingDetails.get('PricingTypeId').value == 2){
    //     this.f.FuelPricingDetails.get('PricePerGallon').setValidators(required);
    //     this.f.FuelPricingDetails.get('PricePerGallon').updateValueAndValidity();
    //     this.f.FuelPricingDetails.get('Code').setValidators([]);
    //     this.f.FuelPricingDetails.get('Code').updateValueAndValidity();
    //     this.f.FuelPricingDetails.get('RackPrice').setValidators([]);
    //     this.f.FuelPricingDetails.get('RackPrice').updateValueAndValidity();
    //     this.f.FuelPricingDetails.get('SupplierCostMarkupValue').setValidators([]);
    //     this.f.FuelPricingDetails.get('SupplierCostMarkupValue').updateValueAndValidity();


    //   }
    // }

    getUserContext() {
        this.salesUserService.GetUserContext().subscribe(data => {
            this.UserContext = data;
        })
    }
    public onChangeMobile(event: any) {
        if (this.f.CustomerDetails.get('PhoneNumber').value) {
            this.salesUserService.IsPhoneNumberValid(this.f.CustomerDetails.get('PhoneNumber').value).subscribe(data => {
                this.isValidMobile = data;
            })
        } else {
            this.isValidMobile = true;
        }

    }

    markerDragEnd(event) {
        this.confirmationDialogService.confirm('Map update', 'Geo Codes shifted to a new location!')
            .then((confirmed) => (confirmed == true) ? this.updateGeoCode(event.coords.lat, event.coords.lng) : this.previousLatLon())
            .catch(() => this.previousLatLon());
    }

    updateGeoCode(lat, lng) {
        this.salesUserService.GetAddressByLongLat(lat, lng).subscribe(data => {
            this.updateAddressData(data);
        })
    }
    public previousLatLon() {
        this.mapConstants.CenterLat = this.f.AddressDetails.get('Latitude').value;
        this.mapConstants.CenterLon = this.f.AddressDetails.get('Longitude').value;
    }

    public UpdateViewedStatus() {
        this.salesUserService.UpdateViewedStatus(true, this.editSourcingId).subscribe(data => {
        })
    }
    public onCancel() {
        this.router.navigate(['SalesUser/SourcingRequest/Index']);
    }

    countryChanged() {
        this.f.AddressDetails.get('StateId').setValue(null)
        this.filteredStatesList = this.statesList.filter(s => s.CountryId == this.f.AddressDetails.get('CountryId').value) || [];
        (this.f.AddressDetails.get('CountryId').value == 2) ? this.UoM = 2 : this.UoM = 1;
        this.f.AddressDetails.get('UOM').setValue(this.UoM);
    }
    countryGroupChanged(selectedCountryGroupId: any) {
        if (selectedCountryGroupId) {
            var countryGroup = selectedCountryGroupId.target.value;
            if (countryGroup === 'Select') {
                this.filteredStatesList = this.statesList.filter(s => s.CountryId == this.f.AddressDetails.get('CountryId').value) || [];
                this.f.AddressDetails.get('StateId').setValue(null)
            }
            else {
                this.filteredStatesList = this.statesList.filter(s => s.CountryGroupId == countryGroup) || [];
                this.f.AddressDetails.get('StateId').setValue(null)
            }
        }
    }
    updateAddressData(address: Geocode) {
        let countryId = (address.CountryCode == 'US') ? 1 : (address.CountryCode == 'CA' ? 2 : this.f.AddressDetails.get('CountryId').value);
        let state = this.statesList.find(st => st.StateName.toLowerCase() == address.StateName.toLowerCase());
        let stateId = (state && state.StateId) ? state.StateId : this.f.AddressDetails.get('StateId').value;

        this.f.AddressDetails.get('Address').patchValue(address.Address);
        this.f.AddressDetails.get('City').patchValue(address.City);
        this.f.AddressDetails.get('ZipCode').patchValue(address.ZipCode);
        this.f.AddressDetails.get('CountyName').patchValue(address.CountyName);
        this.f.AddressDetails.get('CountryCode').patchValue(address.CountryCode);
        this.f.AddressDetails.get('CountryId').patchValue(countryId);
        this.f.AddressDetails.get('StateName').patchValue(address.StateName);
        this.f.AddressDetails.get('StateId').patchValue(stateId);

        if (address.Latitude) {
            this.f.AddressDetails.get('Latitude').patchValue(address.Latitude);
            this.f.AddressDetails.get('Longitude').patchValue(address.Longitude);
        }

        this.filteredStatesList = this.statesList.filter(s => s.CountryId == countryId) || [];
        this.UoM = countryId == 2 ? 2 : 1;

    }

    // changePricingCode(){
    //   if(this.f.FuelPricingDetails.get('PricingTypeId').value == 2){
    //     this.f.FuelPricingDetails.get('Code').patchValue('A-120000');
    //     this.f.FuelPricingDetails.get('Code').patchValue('');
    //     this.f.FuelPricingDetails.get('RackAvgTypeId').patchValue('');
    //     this.f.FuelPricingDetails.get('RackPrice').patchValue('');
    //     this.f.FuelPricingDetails.get('CityGroupTerminalId').patchValue('');
    //     this.f.FuelPricingDetails.get('TerminalName').patchValue('');
    //   }else if(this.f.FuelPricingDetails.get('PricingTypeId').value == 4){
    //     this.f.FuelPricingDetails.get('Code').patchValue('A-140000');
    //     this.f.FuelPricingDetails.get('RackAvgTypeId').patchValue('');
    //     this.f.FuelPricingDetails.get('RackPrice').patchValue('');
    //     this.f.FuelPricingDetails.get('CityGroupTerminalId').patchValue('');
    //     this.f.FuelPricingDetails.get('TerminalName').patchValue('');
    //   }else if(this.f.FuelPricingDetails.get('PricingTypeId').value == 1){
    //     this.f.FuelPricingDetails.get('Code').patchValue('');
    //     // this.f.FuelPricingDetails.get('PricePerGallon').patchValue('');
    //     this.f.FuelPricingDetails.get('SupplierCostMarkupValue').patchValue('');
    //     this.f.FuelPricingDetails.get('SupplierCostMarkupTypeId').patchValue('');
    //   }
    // }

    setCurrency(Currency: any) {
        this.UoM = Currency;
        if (Currency == "1") {
            this.displayCurrancy = "USD";
        }
        else if (Currency == "2") {
            this.displayCurrancy = "CAD";
        }
    }
    //ToogleNotes(){
    //  this.isShowNote= !this.isShowNote;
    //}

    deliveryTypeIdChanged(deliveryTypeId: number) {
        if (deliveryTypeId == 1) {
            this.f.FuelPricingDetails.get('TierPricing').get('TierPricingType').setValue(2);
        }
    }
    updateFormControlValidators(control: any, validators: any[]) {
        control.setValidators(validators);
        control.updateValueAndValidity();
    }

    deliveryTypeChanged(type: number) {
        this.f.FuelDetails.get('FuelTypeId').setValue(null);
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
            //DONT UPDATE VALIDATION IF TIER PRICING ALREADY CHECKED
            if (!this.f.FuelPricingDetails.get('IsTierPricingRequired').value) {
                this.pricingModuleComponent.pricingTypeChanged(2)
            }
        }
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
    UpdateSuppressPricing(value: boolean) {
        this.sourcingRequestForm.get('IsSupressOrderPricing').setValue(value);
        this.setNetDaysValidation(value);
    }
    isRegularBuyerUpdate(companyId: number, isEditRequest: boolean) {

        if (companyId > 0) {
            this.salesUserService.GetValidTPOCompany(companyId).subscribe(data => {
                this.f.IsRegularBuyer.setValue(!data);
                if (!isEditRequest) {
                    if (data) {
                        this.sourcingRequestForm.get('IsSupressOrderPricing').setValue(this.companyPreferenceSetting);
                        this.pricingModuleComponent.toggleSuppressPricing(this.companyPreferenceSetting);
                    }
                    else {
                        this.sourcingRequestForm.get('IsSupressOrderPricing').setValue(false);
                        this.pricingModuleComponent.toggleSuppressPricing(false);
                        this.f.AddressDetails['controls']['IsNewJob'].setValue(false);
                        this.clickNewPerson(false);
                    }
                }
            });
        }
    }
    ClearAddress() {
        this.f.AddressDetails['controls']['JobId'].setValue(null);
        this.f.AddressDetails['controls']['JobName'].setValue(null);
        this.f.AddressDetails['controls']['DisplayJobID'].setValue(null);
        this.f.AddressDetails['controls']['Address'].setValue(null);
        this.f.AddressDetails['controls']['ZipCode'].setValue(null);
        this.f.AddressDetails['controls']['City'].setValue(null);
        this.f.AddressDetails['controls']['CountyName'].setValue(null);
        this.f.AddressDetails['controls']['StateId'].setValue(null);
        this.f.AddressDetails['controls']['Latitude'].setValue(null);
        this.f.AddressDetails['controls']['Longitude'].setValue(null);
        this.f.AddressDetails['controls']['TimeZoneName'].setValue(null);
    }
}
