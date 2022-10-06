import { __awaiter, __decorate } from "tslib";
import { Component, ViewChild } from '@angular/core';
import { Validators } from '@angular/forms';
import { Declarations } from 'src/app/declarations.module';
import * as moment from 'moment';
let CreateNominationComponent = class CreateNominationComponent {
    constructor(fb, salesUserService, route_active, router, modalService, changeDetectorRef, confirmationDialogService) {
        this.fb = fb;
        this.salesUserService = salesUserService;
        this.route_active = route_active;
        this.router = router;
        this.modalService = modalService;
        this.changeDetectorRef = changeDetectorRef;
        this.confirmationDialogService = confirmationDialogService;
        this.pageloader = false;
        this.keyword = 'Code';
        this.Approved_Terminals_keyword = "Name";
        this.PrcingCodevalue = '';
        this.popoverTitle = 'Create PO';
        this.popoverMessage = 'Are you sure want to create PO?';
        this.cancelClicked = false;
        this.isPriceCodeLoading = false;
        this.editSourcingId = 0;
        this.IsLoading = false;
        this.formSubmited = false;
        this.companyExits = false;
        this.isPersonNew = true;
        this.TruckTypeLoadList = [];
        this.FreightOnBoardTypesList = [];
        this.countryList = [];
        this.currucyList = [];
        this.UomList = [];
        this.statesList = [];
        this.filteredStatesList = [];
        this.FuelProductsList = [];
        this.FeeTypesList = [];
        this.FeeSubTypesList = [];
        this.FeeConstraintTypesList = [];
        this.PaymentMethodsList = [];
        this.RackAvgPricingTypesList = [];
        this.CompanyContactPersonsList = [];
        this.CitySourcingGroupTerminalPriceAvailableList = [];
        this.SourcingCityGroupTerminalsList = [];
        this.ClosedTerminalList = [];
        this.OpisTerminalList = [];
        this.RequestStatus = SourcingRequestStatus;
        this.GeneralNotesHistory = [];
        this.pricingCodes = [];
        this.pricingfeedTypeId = 0;
        this.pricingfuelClassTypeId = 0;
        this.UserContext = {};
        this.isValidMobile = true;
        this.billableList = [{ Id: '1', Name: 'Net' }, { Id: '2', Name: 'Gross' }];
        this.MaxInputDate = moment().add(1, 'year').toDate();
        this.countryGroupList = [];
        // map settings
        this.mapConstants = new MapConstants();
        this.getUserContext();
        this.getAllTPOCompanies();
        this.initailizeSourcingReqForm();
    }
    ngAfterViewInit() {
        if (typeof CurrentUserId !== "undefined") {
            this.userId = CurrentUserId;
        }
        this.route_active.params.subscribe((params) => {
            this.editSourcingId = params['Id'] > 0 ? params['Id'] : null;
            if (this.editSourcingId > 0) {
                // this.pageloader= true;
                this.getSourcingDetails();
            }
            else {
                this.getPreferencesSettings();
            }
        });
    }
    ngOnInit() {
        this.getFreightOnBoardTypes();
        this.getTruckLoadType();
        this.getUoMList();
        this.getCurrecyList();
        this.getCountryList();
        // this.setCurrency(1);
        this.getPaymentMethods();
        this.getFuelProducts(0, 0);
        this.getStatesOfAllCountries();
        this.getRackAvgPricingTypes();
        if (this.f.AddressDetails.get('CountryId').value == 2) { //canada
            this.mapConstants.CenterLat = 56.14;
            this.mapConstants.CenterLon = -106.34;
        }
        this.isSalesUserType = (typeof IsSalesUser !== undefined) && IsSalesUser;
        this.getcountryGroupList();
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
                UOM: this.fb.control(1)
            }),
            FuelDetails: this.fb.group({
                Id: this.fb.control(null),
                FuelDisplayGroupId: this.fb.control(1),
                FuelTypeId: this.fb.control(null, Validators.required),
                QuantityTypeId: this.fb.control(3),
                Quantity: this.fb.control(0, [Validators.pattern(/^([0-9]\d*(\.\d+)?)$/)]),
                MinimumQuantity: this.fb.control(0, [Validators.pattern(/^([0-9]\d*(\.\d+)?)$/)]),
                MaximumQuantity: this.fb.control(0, [Validators.pattern(/^([0-9]\d*(\.\d+)?)$/)]),
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
                DeliveryTypeId: this.fb.control(1),
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
                PricePerGallon: this.fb.control(0, Validators.required),
                Code: this.fb.control(null),
                TempCode: this.fb.control(null),
                CodeId: this.fb.control(1),
                CodeDescription: this.fb.control(null),
                RackAvgTypeId: this.fb.control(1),
                RackPrice: this.fb.control(null),
                EnableCityRack: this.fb.control(null),
                TerminalName: this.fb.control(null),
                TempTerminalName: this.fb.control(null),
                TerminalId: this.fb.control(null),
                SupplierCostMarkupTypeId: this.fb.control(1),
                SupplierCostMarkupValue: this.fb.control(null),
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
                TierPricing: this.fb.control(null),
                PricingSourceId: this.fb.control(1),
                PricingNote: this.fb.control(null)
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
    getDetailsPage(Id) {
        this.router.navigate(['SourcingRequest/Details/' + Id]);
    }
    clickNewPerson(isnew) {
        this.isPersonNew = isnew;
        if (isnew) {
            this.f.CustomerDetails.get('Name').setValidators([Validators.required]);
            this.f.CustomerDetails.get('UserId').setValidators([]);
        }
        else {
            this.f.CustomerDetails.get('UserId').setValidators([Validators.required]);
            this.f.CustomerDetails.get('Name').setValidators([]);
        }
        this.f.CustomerDetails.get('Name').updateValueAndValidity();
        this.f.CustomerDetails.get('UserId').updateValueAndValidity();
    }
    getPreferencesSettings() {
        this.IsLoading = true;
        this.salesUserService.GetPreferencesSettings().subscribe(data => {
            if (data) {
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
                this.setCurrency(data.AddressDetails.Currency);
                this.sourcingRequestForm.controls['FuelDetails']['controls']['FuelDisplayGroupId'].patchValue(data.FuelDetails.FuelDisplayGroupId);
                this.sourcingRequestForm.controls['FuelDetails']['controls']['QuantityTypeId'].patchValue(data.FuelDetails.QuantityTypeId);
                this.IsLoading = false;
            }
            else {
                this.IsLoading = false;
            }
        });
    }
    getAllTPOCompanies() {
        return __awaiter(this, void 0, void 0, function* () {
            this.IsLoading = true;
            this.salesUserService.GetAllTPOCompanies().subscribe(yield (data => {
                if (data) {
                    var listdata = data.map(item => {
                        return {
                            id: item.Id,
                            text: item.Name
                        };
                    });
                    this.AllTPOCompaniesList = listdata;
                    this.IsLoading = false;
                }
                else {
                    this.IsLoading = false;
                }
            }));
        });
    }
    getSourcingCompanyContactPersons(event) {
        console.log(event);
        var companyId = event;
        this.IsLoading = true;
        this.salesUserService.GetSourcingCompanyContactPersons(companyId).subscribe(data => {
            if (data) {
                this.CompanyContactPersonsList = data;
                let companyname = this.AllTPOCompaniesList.filter(x => x.id == companyId);
                this.sourcingRequestForm.get('CustomerDetails').get('CompanyName').setValue(companyname[0].text);
                this.IsLoading = false;
            }
            else {
                this.IsLoading = false;
            }
        });
    }
    getSourcingContactPersonDetails(userId) {
        console.log(userId);
        this.salesUserService.GetSourcingContactPersonDetails(userId).subscribe(data => {
            if (data) {
                this.CompanyContactPersonsDetails = data;
                // this.CompanyContactPersonsDetails['IsNewCompany']=false;
                this.sourcingRequestForm.get('CustomerDetails').get('PhoneNumber').setValue(this.CompanyContactPersonsDetails.PhoneNumber);
                this.sourcingRequestForm.get('CustomerDetails').get('Email').setValue(this.CompanyContactPersonsDetails.Email);
                console.log(this.CompanyContactPersonsDetails);
                // this.sourcingRequestForm.get('CustomerDetails').patchValue(this.CompanyContactPersonsDetails);
            }
        });
    }
    getTruckLoadType() {
        this.TruckTypeLoadList = [];
        this.salesUserService.GetTruckLoadType().subscribe(data => {
            if (data && data.length > 0) {
                this.TruckTypeLoadList = data;
                this.sourcingRequestForm.get('TruckLoadType').setValue(data[0].Id);
            }
        });
    }
    getFreightOnBoardTypes() {
        this.FreightOnBoardTypesList = [];
        this.salesUserService.GetFreightOnBoardTypes().subscribe(data => {
            if (data && data.length > 0) {
                this.FreightOnBoardTypesList = data;
                this.sourcingRequestForm.get('FreightOnBoardType').setValue(data[0].Id);
            }
        });
    }
    getCountryList() {
        this.salesUserService.GetCountryList().subscribe(data => {
            if (data && data.length > 0) {
                this.countryList = data;
            }
        });
    }
    getCurrecyList() {
        this.salesUserService.GetCurrenyList().subscribe(data => {
            if (data && data.length > 0) {
                this.currucyList = data;
            }
        });
    }
    getUoMList() {
        this.salesUserService.GetUoMList().subscribe(data => {
            if (data && data.length > 0) {
                this.UomList = data;
            }
        });
    }
    getStatesOfAllCountries(countryId) {
        this.salesUserService.GetStatesOfAllCountries(countryId).subscribe(data => {
            if (data && data.length > 0) {
                this.statesList = data;
                this.filteredStatesList = this.statesList;
            }
        });
    }
    getcountryGroupList() {
        this.salesUserService.GetCountryGroupList(4).subscribe(data => {
            if (data && data.length > 0) {
                this.countryGroupList = data;
            }
        });
    }
    getJobLists(companyId, isFtl, foAsTerminal) {
        let companyName = this.AllTPOCompaniesList.find(t => t.id == companyId).text;
        let ftlvalue = isFtl == "FullTruckLoad" ? true : false;
        let tervalue = foAsTerminal == "Terminal" ? true : false;
        this.salesUserService.GetJobLists(companyName, ftlvalue, tervalue).subscribe(data => {
            if (data) {
                let joblistdata = data.map(item => {
                    return {
                        id: item.Id,
                        text: item.Name
                    };
                });
                this.allJobList = joblistdata;
            }
        });
    }
    getJobDetails(jobId, companyName) {
        let job = this.allJobList.find(x => x.id == jobId);
        if (job != null) {
            this.salesUserService.GetJobDetails(job.text, companyName).subscribe(data => {
                if (data) {
                    this.sourcingRequestForm.get('AddressDetails').patchValue(data.AddressDetails);
                }
            });
        }
    }
    getFuelProducts(companyId, jobId) {
        var productDisplayGroupId = this.f.FuelDetails.get('FuelDisplayGroupId').value ? this.f.FuelDetails.get('FuelDisplayGroupId').value : 1;
        this.IsLoading = true;
        this.salesUserService.GetFuelProducts(productDisplayGroupId, companyId, jobId).subscribe(data => {
            if (data) {
                this.FuelProductsList = data;
                this.IsLoading = false;
            }
            else {
                this.IsLoading = false;
            }
        });
    }
    getProductListByZip(zipCode) {
        this.IsLoading = true;
        this.salesUserService.GetProductListByZip(zipCode).subscribe(data => {
            if (data) {
                this.FuelProductsList = data;
                this.IsLoading = false;
            }
            else {
                this.IsLoading = false;
            }
        });
    }
    getAllFeeTypes(companyId, currency, truckLoadType) {
        this.salesUserService.GetAllFeeTypes(companyId, currency, truckLoadType).subscribe(data => {
            if (data) {
                this.FeeTypesList = data;
            }
        });
    }
    getAllFeeSubTypes(feeTypeId, Currency) {
        this.salesUserService.GetAllFeeSubTypes(feeTypeId, Currency).subscribe(data => {
            if (data) {
                this.FeeSubTypesList = data;
            }
        });
    }
    getAllFeeConstraintTypes() {
        this.salesUserService.GetAllFeeConstraintTypes().subscribe(data => {
            if (data) {
                this.FeeConstraintTypesList = data;
            }
        });
    }
    getPaymentMethods() {
        this.salesUserService.PaymentMethods().subscribe(data => {
            if (data) {
                this.PaymentMethodsList = data;
            }
        });
    }
    getRackAvgPricingTypes() {
        this.salesUserService.GetRackAvgPricingTypes().subscribe(data => {
            if (data) {
                this.RackAvgPricingTypesList = data;
            }
        });
    }
    addFees() {
        let fee = this.sourcingRequestForm.get('FuelDetails').get('FuelRequestFees');
        fee.push(this.fb.group({
            FeeTypeId: this.fb.control(null, Validators.required),
            FeeSubTypeId: this.fb.control(null, Validators.required),
            FeeSubTypeName: this.fb.control(null, Validators.required),
            Fee: this.fb.control(null, Validators.required),
            FeeDetails: this.fb.control(null, Validators.required),
            FeeConstraintTypeId: this.fb.control(null, Validators.required),
            IncludeInPPG: this.fb.control(null, Validators.required),
            OtherFeeTypeId: this.fb.control(null, Validators.required),
        }));
    }
    isSourcingCompanyExist() {
        this.companyExits = false;
        if (this.f.CustomerDetails.get('CompanyName').value) {
            this.salesUserService.IsSourcingCompanyExist(this.f.CustomerDetails.get('IsNewCompany').value, this.f.CustomerDetails.get('CompanyName').value).subscribe(data => {
                if (data != null || data != undefined) {
                    this.companyExits = data;
                }
            });
        }
    }
    getSourcingDetails() {
        this.pageloader = true;
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
                if (!data.CustomerDetails.IsNewCompany) {
                    this.getSourcingCompanyContactPersons(data.CustomerDetails.CompanyId);
                    this.getJobLists(data.CustomerDetails.CompanyId, this.f.TruckLoadType.value, this.f.FreightOnBoardType.value);
                }
                if (data.CustomerDetails.UserId) {
                    this.isPersonNew = false;
                }
                this.sourcingRequestForm.get('CustomerDetails').patchValue(data.CustomerDetails);
                this.sourcingRequestForm.get('AddressDetails').patchValue(data.AddressDetails);
                this.sourcingRequestForm.get('FuelDetails').patchValue(data.FuelDetails);
                this.LeadFees = data.FuelDetails.Fees;
                this.sourcingRequestForm.get('FuelDeliveryDetails').patchValue(data.FuelDeliveryDetails);
                this.sourcingRequestForm.get('AdditionalDetailsViewModel').patchValue(data.AdditionalDetailsViewModel);
                this.getSourcingCityGroupTerminals(data.AddressDetails.StateId, (data.FuelPricingDetails.Code == "AXIS") ? 1 : 2);
                this.PrcingCodevalue = data.FuelPricingDetails.Code;
                this.GeneralNotesHistory = data.GeneralNotesHistory;
                this.sourcingRequestForm.get('FuelPricingDetails').patchValue(data.FuelPricingDetails);
                this.sourcingRequestForm.get('FuelPricingDetails').get('TempCode').patchValue(data.FuelPricingDetails);
                this.sourcingRequestForm.get('FuelPricingDetails').get('TempTerminalName').patchValue(data.FuelPricingDetails.TerminalName);
                this.changeDetectorRef.detectChanges();
                this.UpdateViewedStatus();
                this.pageloader = false;
            }
        });
    }
    onSubmit() {
        this.formSubmited = true;
        this.resetPOValidation();
        this.setSaveValidations();
        if (this.editSourcingId > 0) {
            this.saveEditDetails();
        }
        else {
            if (this.sourcingRequestForm.valid && !this.companyExits) {
                this.pageloader = true;
                this.salesUserService.CreateSourcingRequest(this.sourcingRequestForm.value).subscribe(data => {
                    if (data && data.StatusCode == 0) {
                        Declarations.msgsuccess("Request created successfully.", undefined, undefined);
                        this.router.navigate(['SalesUser/SourcingRequest/Index']);
                        this.pageloader = false;
                    }
                    else {
                        Declarations.msgerror("Failed", undefined, undefined);
                        this.pageloader = false;
                        return;
                    }
                });
            }
            else {
                this.pageloader = false;
                return;
            }
        }
    }
    setSaveValidations() {
        if (this.f.CustomerDetails.get('IsNewCompany').value == true) {
            this.f.CustomerDetails.get('Name').setValidators([Validators.required]);
            this.f.CustomerDetails.get('Name').updateValueAndValidity();
        }
        else {
            if (this.isPersonNew) {
                this.f.CustomerDetails.get('Name').setValidators([Validators.required]);
                this.f.CustomerDetails.get('Name').updateValueAndValidity();
            }
            else {
                this.f.CustomerDetails.get('UserId').setValidators([Validators.required]);
                this.f.CustomerDetails.get('UserId').updateValueAndValidity();
            }
        }
        if (this.f.FuelDetails.get('QuantityTypeId').value == 1) {
            this.f.FuelDetails.get('Quantity').setValidators([Validators.required, Validators.pattern('^[0-9.-]*$')]);
            this.f.FuelDetails.get('Quantity').updateValueAndValidity();
        }
        else if (this.f.FuelDetails.get('QuantityTypeId').value == 2) {
            this.f.FuelDetails.get('MinimumQuantity').setValidators([Validators.required, Validators.pattern('^[0-9.-]*$')]);
            this.f.FuelDetails.get('MinimumQuantity').updateValueAndValidity();
            this.f.FuelDetails.get('MaximumQuantity').setValidators([Validators.required, Validators.pattern('^[0-9.-]*$')]);
            this.f.FuelDetails.get('MaximumQuantity').updateValueAndValidity();
        }
        if (this.f.FuelDeliveryDetails.get('PaymentTermId').value == 1) {
            this.f.FuelDeliveryDetails.get('NetDays').setValidators([Validators.required, Validators.min(1)]);
            this.f.FuelDeliveryDetails.get('NetDays').updateValueAndValidity();
        }
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
                }
                else {
                    Declarations.msgerror(data.StatusMessage, undefined, undefined);
                    this.pageloader = false;
                    return;
                }
            });
        }
        else {
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
        }
        else {
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
        }
        else if (this.f.FuelDetails.get('QuantityTypeId').value == 2) {
            this.f.FuelDetails.get('MinimumQuantity').setValidators([Validators.required, Validators.pattern('^[0-9.-]*$')]);
            this.f.FuelDetails.get('MinimumQuantity').updateValueAndValidity();
            this.f.FuelDetails.get('MaximumQuantity').setValidators([Validators.required, Validators.pattern('^[0-9.-]*$')]);
            this.f.FuelDetails.get('MaximumQuantity').updateValueAndValidity();
        }
        else {
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
        if (this.f.FuelDeliveryDetails.get('PaymentTermId').value == 1) {
            this.f.FuelDeliveryDetails.get('NetDays').setValidators([Validators.required, Validators.min(1)]);
            this.f.FuelDeliveryDetails.get('NetDays').updateValueAndValidity();
        }
        else {
            this.f.FuelDeliveryDetails.get('NetDays').setValidators([]);
            this.f.FuelDeliveryDetails.get('NetDays').updateValueAndValidity();
        }
        this.f.CustomerDetails.get('PhoneNumber').setValidators(required);
        this.f.CustomerDetails.get('PhoneNumber').updateValueAndValidity();
        this.f.CustomerDetails.get('Email').setValidators(required);
        this.f.CustomerDetails.get('Email').updateValueAndValidity();
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
    // validateMinMaxValue(control:FormControl){
    //   // if(this.f.FuelDetails.get('MaximumQuantity').value > this.f.FuelDetails.get('MinimumQuantity').value){
    //   // }else{
    //   //   control.setErrors({'RangeError':true})
    //   //   control.updateValueAndValidity();
    //   // }
    //   // control.setErrors({'RangeError':true})
    //   // control.updateValueAndValidity();
    // }
    saveEditDetails() {
        this.pageloader = true;
        if (this.sourcingRequestForm.valid && !this.companyExits) {
            this.salesUserService.SaveEditSourcingDetails(this.sourcingRequestForm.value).subscribe(data => {
                if (data && data.StatusCode == 0) {
                    Declarations.msgsuccess("Request Updated successfully", undefined, undefined);
                    this.router.navigate(['SalesUser/SourcingRequest/Index']);
                    this.pageloader = false;
                }
                else {
                    Declarations.msgerror("Failed", undefined, undefined);
                    this.pageloader = false;
                    return;
                }
            });
        }
    }
    acceptRequest() {
        this.salesUserService.ChangesSourcingRequestStatus(this.RequestStatus.Accepted, this.editSourcingId).subscribe(data => {
            if (data.StatusCode == 0) {
                Declarations.msgsuccess("Request Accepted.", undefined, undefined);
                this.router.navigate(['SalesUser/SourcingRequest/Index']);
            }
            else {
                Declarations.msgerror(data.StatusMessage, undefined, undefined);
                return;
            }
        });
    }
    openPriceCodeModal(pricingcodeModal) {
        this.getPricingCodes();
        this.modalService.open(pricingcodeModal, { windowClass: 'pricingcode-modal', size: 'lg', scrollable: true });
    }
    isCitySourcingGroupTerminalPriceAvailable(jobId, fueltypeId, selectedCityRackId, lattitude, longitude, countryCode, sourceId) {
        this.salesUserService.IsCitySourcingGroupTerminalPriceAvailable(jobId, fueltypeId, selectedCityRackId, lattitude, longitude, countryCode, sourceId).subscribe(data => {
            if (data) {
                this.CitySourcingGroupTerminalPriceAvailableList = data;
            }
        });
    }
    getSourcingCityGroupTerminals(stateId, sourceId) {
        return __awaiter(this, void 0, void 0, function* () {
            this.salesUserService.GetSourcingCityGroupTerminals(stateId, sourceId).subscribe(data => {
                if (data) {
                    this.SourcingCityGroupTerminalsList = data;
                }
            });
        });
    }
    getClosedTerminal(event) {
        var fuelTypeId = this.f.FuelDetails.get('FuelTypeId').value;
        var latitude = this.f.AddressDetails.get('Latitude').value;
        var longitude = this.f.AddressDetails.get('Longitude').value;
        var countryId = this.f.AddressDetails.get('CountryId').value;
        var pricingCodeId = this.f.FuelPricingDetails.get('CodeId').value;
        var CityGroupTerminalId = this.f.FuelPricingDetails.get('CityGroupTerminalId').value;
        //  var terminal= this.SourcingCityGroupTerminalsList.find(t => t.Id == CityGroupTerminalId).Name;
        var terminal = "";
        var pricingSourceId = this.f.FuelPricingDetails.get('PricingSourceId').value;
        if (pricingSourceId == 2) {
            //  this.getOpisTerminals(CityGroupTerminalId,latitude,longitude,countryId,terminal,pricingSourceId);
            this.salesUserService.GetOpisTerminals(CityGroupTerminalId, latitude, longitude, countryId, terminal, pricingSourceId).subscribe(data => {
                if (data) {
                    this.ClosedTerminalList = data;
                    event.stopPropagation();
                    this.approveTerminalAuto.open();
                }
            });
        }
        else {
            this.salesUserService.GetClosedTerminal(fuelTypeId, latitude, longitude, countryId, pricingCodeId, terminal, pricingSourceId).subscribe(data => {
                if (data) {
                    this.ClosedTerminalList = data;
                }
            });
        }
    }
    getApprovedTerminal(event) {
        var fuelTypeId = this.f.FuelDetails.get('FuelTypeId').value;
        var latitude = this.f.AddressDetails.get('Latitude').value;
        var longitude = this.f.AddressDetails.get('Longitude').value;
        var countryId = this.f.AddressDetails.get('CountryId').value;
        var pricingCodeId = this.f.FuelPricingDetails.get('CodeId').value;
        var CityGroupTerminalId = this.f.FuelPricingDetails.get('CityGroupTerminalId').value;
        var terminal = event;
        var pricingSourceId = this.f.FuelPricingDetails.get('PricingSourceId').value;
        if (pricingSourceId == 2) {
            this.getOpisTerminals(CityGroupTerminalId, latitude, longitude, countryId, terminal, pricingSourceId);
        }
        else {
            this.salesUserService.GetClosedTerminal(fuelTypeId, latitude, longitude, countryId, pricingCodeId, terminal, pricingSourceId).subscribe(data => {
                if (data) {
                    this.ClosedTerminalList = data;
                }
            });
        }
    }
    getOpisTerminals(cityRackId, latitude, longitude, countryId, terminal, source) {
        this.salesUserService.GetOpisTerminals(cityRackId, latitude, longitude, countryId, terminal, source).subscribe(data => {
            if (data) {
                this.ClosedTerminalList = data;
                this.approveTerminalAuto.open();
            }
        });
    }
    setTermialvalue(event) {
        this.f.FuelPricingDetails.get('TerminalName').patchValue(event.Name);
    }
    getPricingCodes(isFromControl, prefix) {
        this.isPriceCodeLoading = true;
        var filterData = {};
        if (!prefix) {
            prefix = "";
        }
        if (isFromControl) {
            filterData = {
                "PricingSourceId": this.f.FuelPricingDetails.get('PricingSourceId').value,
                "PricingTypeId": this.f.FuelPricingDetails.get('PricingTypeId').value,
                "tfxProdId": this.f.FuelDetails.get("FuelTypeId").value,
                "feedTypeId": 0,
                "fuelClassTypeId": 0,
                "Prefix": prefix
            };
        }
        else {
            filterData = {
                "PricingSourceId": this.f.FuelPricingDetails.get('PricingSourceId').value,
                "PricingTypeId": this.f.FuelPricingDetails.get('PricingTypeId').value,
                "tfxProdId": this.f.FuelDetails.get("FuelTypeId").value,
                "feedTypeId": this.pricingfeedTypeId,
                "fuelClassTypeId": this.pricingfuelClassTypeId,
                "Prefix": ""
            };
        }
        this.salesUserService.GetPricingCodes(filterData).subscribe(data => {
            if (data) {
                this.pricingCodes = data.PricingCodes;
                this.isPriceCodeLoading = false;
            }
        });
    }
    getSelectedPricingCode(item) {
        this.modalService.dismissAll();
        var pricingCodeDisplayData = this.getPricingDisplayData(item);
        if (item) {
            this.f.FuelPricingDetails.get('Code').patchValue(item.Code);
            this.f.FuelPricingDetails.get('CodeId').patchValue(item.Id);
            this.f.FuelPricingDetails.get('PricingTypeId').patchValue(item.PricingTypeId);
            this.f.FuelPricingDetails.get('CodeDescription').patchValue(pricingCodeDisplayData);
            this.f.FuelPricingDetails.get('PricingSourceId').patchValue(item.PricingSourceId);
        }
        if (item.PricingSourceId == 1) {
            this.f.FuelPricingDetails.get('EnableCityRack').setValue(false);
        }
        else {
            this.f.FuelPricingDetails.get('EnableCityRack').setValue(true);
            this.getSourcingCityGroupTerminals(this.f.AddressDetails.get('StateId').value, this.f.FuelPricingDetails.get('PricingSourceId').value);
        }
        this.setRackTerminalValidation();
    }
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
    getAddressByZip() {
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
                        }
                        else {
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
    addValidationPrice() {
        let required = [Validators.required];
        if (this.f.FuelPricingDetails.get('PricingTypeId').value == 1) {
            this.f.FuelPricingDetails.get('Code').setValidators(required);
            this.f.FuelPricingDetails.get('Code').updateValueAndValidity();
            this.f.FuelPricingDetails.get('RackPrice').setValidators(required);
            this.f.FuelPricingDetails.get('RackPrice').updateValueAndValidity();
        }
        if (this.f.FuelPricingDetails.get('PricingTypeId').value == 4) {
            this.f.FuelPricingDetails.get('SupplierCostMarkupValue').setValidators(required);
            this.f.FuelPricingDetails.get('SupplierCostMarkupValue').updateValueAndValidity();
            this.f.FuelPricingDetails.get('PricePerGallon').setValidators([]);
            this.f.FuelPricingDetails.get('PricePerGallon').updateValueAndValidity();
            this.f.FuelPricingDetails.get('Code').setValidators([]);
            this.f.FuelPricingDetails.get('Code').updateValueAndValidity();
            this.f.FuelPricingDetails.get('RackPrice').setValidators([]);
            this.f.FuelPricingDetails.get('RackPrice').updateValueAndValidity();
        }
        else if (this.f.FuelPricingDetails.get('PricingTypeId').value == 2) {
            this.f.FuelPricingDetails.get('PricePerGallon').setValidators(required);
            this.f.FuelPricingDetails.get('PricePerGallon').updateValueAndValidity();
            this.f.FuelPricingDetails.get('Code').setValidators([]);
            this.f.FuelPricingDetails.get('Code').updateValueAndValidity();
            this.f.FuelPricingDetails.get('RackPrice').setValidators([]);
            this.f.FuelPricingDetails.get('RackPrice').updateValueAndValidity();
            this.f.FuelPricingDetails.get('SupplierCostMarkupValue').setValidators([]);
            this.f.FuelPricingDetails.get('SupplierCostMarkupValue').updateValueAndValidity();
        }
    }
    getUserContext() {
        this.salesUserService.GetUserContext().subscribe(data => {
            this.UserContext = data;
        });
    }
    onChangeMobile(event) {
        if (this.f.CustomerDetails.get('PhoneNumber').value) {
            this.salesUserService.IsPhoneNumberValid(this.f.CustomerDetails.get('PhoneNumber').value).subscribe(data => {
                this.isValidMobile = data;
            });
        }
        else {
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
        });
    }
    previousLatLon() {
        this.mapConstants.CenterLat = this.f.AddressDetails.get('Latitude').value;
        this.mapConstants.CenterLon = this.f.AddressDetails.get('Longitude').value;
    }
    UpdateViewedStatus() {
        this.salesUserService.UpdateViewedStatus(true, this.editSourcingId).subscribe(data => {
        });
    }
    onCancel() {
        this.router.navigate(['SalesUser/SourcingRequest/Index']);
    }
    countryChanged() {
        this.f.AddressDetails.get('StateId').setValue(null);
        this.filteredStatesList = this.statesList.filter(s => s.CountryId == this.f.AddressDetails.get('CountryId').value) || [];
    }
    countryGroupChanged(selectedCountryGroupId) {
        if (selectedCountryGroupId) {
            var countryGroup = selectedCountryGroupId.target.value;
            if (countryGroup === 'Select') {
                this.filteredStatesList = this.statesList.filter(s => s.CountryId == this.f.AddressDetails.get('CountryId').value) || [];
                this.f.AddressDetails.get('StateId').setValue(null);
            }
            else {
                this.filteredStatesList = this.statesList.filter(s => s.CountryGroupId == countryGroup) || [];
                this.f.AddressDetails.get('StateId').setValue(null);
            }
        }
    }
    updateAddressData(address) {
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
    }
    changePricingCode() {
        if (this.f.FuelPricingDetails.get('PricingTypeId').value == 2) {
            this.f.FuelPricingDetails.get('Code').patchValue('A-120000');
            this.f.FuelPricingDetails.get('Code').patchValue('');
            this.f.FuelPricingDetails.get('RackAvgTypeId').patchValue('');
            this.f.FuelPricingDetails.get('RackPrice').patchValue('');
            this.f.FuelPricingDetails.get('CityGroupTerminalId').patchValue('');
            this.f.FuelPricingDetails.get('TerminalName').patchValue('');
        }
        else if (this.f.FuelPricingDetails.get('PricingTypeId').value == 4) {
            this.f.FuelPricingDetails.get('Code').patchValue('A-140000');
            this.f.FuelPricingDetails.get('RackAvgTypeId').patchValue('');
            this.f.FuelPricingDetails.get('RackPrice').patchValue('');
            this.f.FuelPricingDetails.get('CityGroupTerminalId').patchValue('');
            this.f.FuelPricingDetails.get('TerminalName').patchValue('');
        }
        else if (this.f.FuelPricingDetails.get('PricingTypeId').value == 1) {
            this.f.FuelPricingDetails.get('Code').patchValue('');
            // this.f.FuelPricingDetails.get('PricePerGallon').patchValue('');
            this.f.FuelPricingDetails.get('SupplierCostMarkupValue').patchValue('');
            this.f.FuelPricingDetails.get('SupplierCostMarkupTypeId').patchValue('');
        }
    }
    setCurrency(Currency) {
        if (Currency == "1") {
            this.displayCurrancy = "USD";
        }
        else if (Currency == "2") {
            this.displayCurrancy = "CAD";
        }
    }
};
__decorate([
    ViewChild('approveTerminalAuto', { static: false })
], CreateNominationComponent.prototype, "approveTerminalAuto", void 0);
CreateNominationComponent = __decorate([
    Component({
        selector: 'app-create-nomination',
        templateUrl: './create-nomination.component.html',
        styleUrls: ['./create-nomination.component.scss']
    })
], CreateNominationComponent);
export { CreateNominationComponent };
export var SourcingRequestStatus;
(function (SourcingRequestStatus) {
    SourcingRequestStatus[SourcingRequestStatus["Submitted"] = 1] = "Submitted";
    SourcingRequestStatus[SourcingRequestStatus["Modified"] = 2] = "Modified";
    SourcingRequestStatus[SourcingRequestStatus["Accepted"] = 3] = "Accepted";
    SourcingRequestStatus[SourcingRequestStatus["AcceptedAndModified"] = 4] = "AcceptedAndModified";
    SourcingRequestStatus[SourcingRequestStatus["OrderCreated"] = 5] = "OrderCreated";
})(SourcingRequestStatus || (SourcingRequestStatus = {}));
export class Geocode {
}
export class MapIconUrl {
}
export class MapIconSize {
}
export class MapConstants {
    constructor() {
        this.CenterLat = 38;
        this.CenterLon = -98.35;
        this.ZoomArea = 15;
        this.IconUrl = { url: 'https://maps.google.com/mapfiles/ms/icons/blue-dot.png', scaledSize: { width: 40, height: 40 } };
    }
}
export class FeeModel {
}
export class ByQuantityModel {
    constructor() {
        this.MinQuantity = 0;
    }
}
export class GeneralNote {
}
//# sourceMappingURL=create-nomination.component.js.map