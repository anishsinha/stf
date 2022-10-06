import { AbstractControl, FormArray, FormBuilder, FormControl, FormGroup, ValidatorFn, Validators } from '@angular/forms';
import { ChangeDetectorRef, Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { AddLocationService } from '../../carrier/schedule-builder/add-location/add-location.service';
import { RackAvgTypes } from '../../carrier/schedule-builder/add-location/add-location.model';
import * as moment from 'moment';
import { DropdownItem } from 'src/app/statelist.service';
import { Declarations } from 'src/app/declarations.module';
import { PricingType } from 'src/app/app.enum';

@Component({
    selector: 'app-pricing-section',
    templateUrl: './pricing-section.component.html',
    styleUrls: ['./pricing-section.component.css']
})
export class PricingSectionComponent implements OnInit {

    @Input() public locationForm: FormGroup;
    @Input() public formSubmited: boolean = false;
    @Input() public UoM: number = 0;
    @Input() public IsTBD: boolean = false;
    @Input() public tbdDrProductTypeId?: number;
    @Input() public tbdDrProductId?: number;
    @Input() public productDetails?: any;
    @Output() DetectFormChange: EventEmitter<boolean> = new EventEmitter<boolean>();
    @Output() UpdateSuppressPricingChange = new EventEmitter<boolean>();


    public TierPricingTypeForm: FormGroup;

    _loadingPricingCodes: boolean = false;
    CityGroupTerminalsList: any[] = [];
    approvedTerminalList: any[] = [];
    _minimumConst = 0.00001;
    _zeroConst = 0;
    CountryBasedZipcodeLabel: string = "Zip";
    _loadingAddress: boolean = false;
    MaxInputDate: Date = moment().add(1, 'year').toDate();
    //pricing
    pricingCodes = [];
    pricingfeedTypeId = 0;
    pricingfuelClassTypeId = 0;
    pricingCodesArr = [{
        "PricingTypes": {
            "Fixed": {
                "Id": 1,
                "Code": "A-120000"
            },
            "FuelCost": {
                "Id": 4,
                "Code": "A-140000"
            }
        }
    }];
    RackAvgTypes: DropdownItem[] = RackAvgTypes;
    //TIER PRICING TYPE
    tierApprovedTerminalList: any[] = [];
    tierCityGroupTerminalsList: any[] = [];

    constructor(
        private fb: FormBuilder,
        private addLocationService: AddLocationService,
        private modalService: NgbModal,
        private changeDetectorRef: ChangeDetectorRef) { }

    get f() { return this.locationForm['controls']; }
    get fuelPricingForm() { return this.f.FuelPricingDetails['controls']; }
    get tierPricingForm() { return this.fuelPricingForm.TierPricing['controls']; }

    ngOnInit() {
        this.TierPricingTypeForm = this.initailizeTierPricingTypeForm();
    }
    pricingTypeChanged(type: number) {

        this.setPricingValidation(type);
        this.initilizeMarketBasedPrice();

        if (type == 1) {
            this.getPricingCodes();
        }
        else {
            this.setPricingCode();
        }
    }
    updateFormControlValidators(control: any, validators: any[]) {
        control.setValidators(validators);
        control.updateValueAndValidity();
    }
    openPriceCodeModal(pricingcodeModal) {
        this.getPricingCodes();
        this.modalService.open(pricingcodeModal, { windowClass: 'pricingcode-modal', size: 'lg', scrollable: true, backdrop: 'static', keyboard: false });
    }

    updateFuelType(pricingSourceId: number) {
        let fuelTypeId = this.tbdDrProductId;
        let fuelDetails = this.productDetails.find(t => t.PricingSourceId == pricingSourceId && t.TfxProductId == fuelTypeId);
        this.f.FuelDetails.get('FuelTypeId').setValue(fuelDetails.Id);
        this.f.FuelDetails.get('FuelDisplayGroupId').setValue(fuelDetails.DisplayGroupId);
    }

    getSelectedPricingCode(item: any) {
        document.getElementById('pricingcodeModal').click()
        if (!this.f.FuelPricingDetails.get('IsTierPricingRequired').value) {
            //this.modalService.dismissAll();
            let pricingCodeDisplayData = this.getPricingDisplayData(item);

            if (item) {
                this.f.FuelPricingDetails.get('TempPricingCodeDetails').patchValue(item);
                this.f.FuelPricingDetails.get('Code').patchValue(item.Code);
                this.f.FuelPricingDetails.get('CodeId').patchValue(item.Id);
                //this.f.FuelPricingDetails.get('PricingTypeId').patchValue(item.PricingTypeId);
                this.f.FuelPricingDetails.get('CodeDescription').patchValue(pricingCodeDisplayData);
                let existingPricingSource = this.f.FuelPricingDetails.get('FuelPricingDetails.PricingSourceId').value;
                this.f.FuelPricingDetails.get('FuelPricingDetails').get('PricingSourceId').patchValue(item.PricingSourceId);
                this.f.FuelPricingDetails.get('CityGroupTerminalId').setValue(null);
                if (this.IsTBD && this.tbdDrProductTypeId != 10) {
                    this.updateFuelType(item.PricingSourceId);
                }
                if (existingPricingSource != item.PricingSourceId) {
                    this.getCityGroupTerminals();
                }
                this.f.FuelPricingDetails.get('FuelPricingDetails').get('PricingCode').patchValue({ Id: item.Id, Code: item.Code, Description: pricingCodeDisplayData });
            }
            if (item.PricingSourceId == 1) {
                this.f.FuelPricingDetails.get('EnableCityRack').setValue(false);
            }
            else {
                this.f.FuelPricingDetails.get('EnableCityRack').setValue(true);
            }
            this.setRackTerminalValidation();
        }
        else {
            let pricingCodeDisplayData = this.getPricingDisplayData(item);

            if (item) {
                this.TierPricingTypeForm.get('TempPricingCodeDetails').patchValue(item);
                this.TierPricingTypeForm.get('Code').patchValue(item.Code);
                this.TierPricingTypeForm.get('CodeId').patchValue(item.Id);
                //this.TierPricingTypeForm.get('PricingTypeId').patchValue(item.PricingTypeId);
                this.TierPricingTypeForm.get('CodeDescription').patchValue(pricingCodeDisplayData);
                this.TierPricingTypeForm.get('FuelPricingDetails').get('PricingSourceId').patchValue(item.PricingSourceId);
                this.f.FuelPricingDetails.get('CityGroupTerminalId').setValue(null);

                this.getCityGroupTerminals_tpt();
                this.TierPricingTypeForm.get('FuelPricingDetails').get('PricingCode').patchValue({ Id: item.Id, Code: item.Code, Description: pricingCodeDisplayData });
            }
            if (item.PricingSourceId == 1) {
                this.TierPricingTypeForm.get('EnableCityRack').setValue(false);
            }
            else {
                this.TierPricingTypeForm.get('EnableCityRack').setValue(true);
            }
            this.setRackTerminalValidation_tpt();
        }
    }
    onPricingCodeSelected(item: any) {

        let pricingCodeDisplayData = this.getPricingDisplayData(item);

        this.f.FuelPricingDetails.get('Code').patchValue(item.Code);
        this.f.FuelPricingDetails.get('CodeId').patchValue(item.Id);
        this.f.FuelPricingDetails.get('PricingTypeId').patchValue(item.PricingTypeId);
        this.f.FuelPricingDetails.get('CodeDescription').patchValue(pricingCodeDisplayData);
        this.f.FuelPricingDetails.get('FuelPricingDetails').get('PricingSourceId').patchValue(item.PricingSourceId);
        this.f.FuelPricingDetails.get('FuelPricingDetails').get('PricingCode').patchValue({ Id: item.Id, Code: item.Code, Description: pricingCodeDisplayData });
        this.f.FuelPricingDetails.get('CityGroupTerminalId').setValue(null);

        if (item.PricingSourceId == 1) {
            this.f.FuelPricingDetails.get('EnableCityRack').setValue(false);
        }
        else {
            this.f.FuelPricingDetails.get('EnableCityRack').setValue(true);
        }
        this.setRackTerminalValidation();
        this.getCityGroupTerminals();
    }
    onApprovedTerminalSelected(event: any): void {
        this.f.FuelPricingDetails.get('TerminalName').patchValue(event.Name);
        this.f.FuelPricingDetails.get('TerminalId').patchValue(event.Id);
    }
    getCityGroupTerminals() {
        this.CityGroupTerminalsList = [];
        let selectedState = this.f.AddressDetails.get('StateId').value;
        if (selectedState > 0) {
            this.addLocationService.GetCityGroupTerminals(selectedState, false, this.f.FuelPricingDetails.get('FuelPricingDetails').get('PricingSourceId').value).subscribe(data => {
                if (data && data.length > 0) {
                    this.CityGroupTerminalsList = data;
                }
            });
        }
    }
    setPricingCode() {
        if (this.locationForm.get('FuelPricingDetails.PricingTypeId').value != 1) {
            let pricingCode = this.getPricingCode(this.locationForm.get('FuelPricingDetails.PricingTypeId').value, this.locationForm.get('FuelPricingDetails.FuelPricingDetails.PricingSourceId').value);
            if (pricingCode != null) {
                this.locationForm.get('FuelPricingDetails.FuelPricingDetails.PricingCode').patchValue(pricingCode);
            }
        }
    }
    getPricingCode(pricingTypeId, pricingSourceId) {
        var pricingType = pricingTypeId == 2 ? 'Fixed' : pricingTypeId == 4 ? 'FuelCost' : '';

        if (pricingType == '')
            return null;

        var pricing = this.pricingCodesArr.map(function (prc) {
            return prc.PricingTypes[pricingType];
        });

        if (pricing.length > 0)
            pricing = pricing[0];
        else
            pricing = null;

        return pricing;
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
    removePricingValidation() {
        this.updateFormControlValidators(this.f.FuelPricingDetails.get('PricePerGallon'), []);
        //this.updateFormControlValidators(this.f.FuelPricingDetails.get('SupplierCostMarkupTypeId'), []);
        this.updateFormControlValidators(this.f.FuelPricingDetails.get('SupplierCostMarkupValue'), []);
        this.updateFormControlValidators(this.f.FuelPricingDetails.get('RackPrice'), []);
        this.updateFormControlValidators(this.f.FuelPricingDetails.get('Code'), []);
    }
    setPricingValidation(type: number) {
        this.removePricingValidation();
        if (!this.f.IsSupressOrderPricing.value) {
            //Market Based
            if (type == 1) {
                this.updateFormControlValidators(this.f.FuelPricingDetails.get('RackPrice'), [Validators.required, Validators.min(this._zeroConst)]);
                this.updateFormControlValidators(this.f.FuelPricingDetails.get('Code'), [Validators.required]);
                this.f.FuelPricingDetails.get('RackAvgTypeId').setValue(1);
            }
            //Fuel cost
            else if (type == 4) {
                //this.updateFormControlValidators(this.f.FuelPricingDetails.get('SupplierCostMarkupTypeId'), [Validators.required]);
                this.updateFormControlValidators(this.f.FuelPricingDetails.get('SupplierCostMarkupValue'), [Validators.required, Validators.min(this._zeroConst)]);
                this.f.FuelPricingDetails.get('SupplierCostMarkupTypeId').setValue(1);
            }
            //Fixed
            else if (type == 2) {
                this.updateFormControlValidators(this.f.FuelPricingDetails.get('PricePerGallon'), [Validators.required, Validators.min(this._minimumConst)]);
            }
        }
    }
    initilizeMarketBasedPrice() {
        this.f.FuelPricingDetails.get('EnableCityRack').setValue(false);
        this.updateFormControlValidators(this.f.FuelPricingDetails.get('CityGroupTerminalId'), []);
        this.f.FuelPricingDetails.get('Code').patchValue(null);
        this.f.FuelPricingDetails.get('CodeId').patchValue(null);
        this.f.FuelPricingDetails.get('CodeDescription').patchValue(null);
        this.f.FuelPricingDetails.get('FuelPricingDetails').get('PricingCode').patchValue({ Id: null, Code: null, Description: null });
        this.f.FuelPricingDetails.get('TempPricingCodeDetails').patchValue(null);
    }
    setRackTerminalValidation() {
        let isChecked = this.f.FuelPricingDetails.get('EnableCityRack').value;
        if (isChecked) {
            this.updateFormControlValidators(this.f.FuelPricingDetails.get('CityGroupTerminalId'), [Validators.required]);
        }
        else {
            this.updateFormControlValidators(this.f.FuelPricingDetails.get('CityGroupTerminalId'), []);
        }
    }
    cityRackTerminalChanged() {
        let jobid = this.f.AddressDetails.get('JobId').value,
            fueltypeId = this.f.FuelDetails.get('FuelTypeId').value,
            selectedCityRackId = this.f.FuelPricingDetails.get('CityGroupTerminalId').value,
            lattitude = this.f.AddressDetails.get('Latitude').value,
            longitude = this.f.AddressDetails.get('Longitude').value,
            countryCode = this.f.AddressDetails.get('CountryCode').value,
            _countryCode = countryCode == "1" ? "USA" : "CAN",
            sourceId = this.f.FuelPricingDetails.get('FuelPricingDetails').get('PricingSourceId').value

        this.addLocationService.IsCityGroupTerminalPriceAvailable(jobid ? jobid : 0, fueltypeId, selectedCityRackId, lattitude, longitude, _countryCode, sourceId).subscribe(response => {
            if (response == false) {
                this.f.FuelPricingDetails.get('CityGroupTerminalId').setValue(null);
                Declarations.msgwarning("Pricing not available for this City Rack/Terminal. Try to assign another City Rack/Terminal.", null, null)
            }
        });
        if (sourceId != 1) {
            this.getOpisTerminals();
        }
    }
    getOpisTerminals() {
        let cityRackId = this.f.FuelPricingDetails.get('CityGroupTerminalId').value || 0,
            latitude = this.f.AddressDetails.get('Latitude').value,
            longitude = this.f.AddressDetails.get('Longitude').value,
            countryId = this.f.AddressDetails.get('CountryId').value,
            source = this.f.FuelPricingDetails.get('FuelPricingDetails').get('PricingSourceId').value
        this.addLocationService.getOpisTerminals(cityRackId, latitude, longitude, countryId, '', source).subscribe(response => {
            if (response) {
                this.approvedTerminalList = response;
            }
        });
    }
    getApprovedTerminal() {
        let pricingSourceId = this.f.FuelPricingDetails.get('FuelPricingDetails').get('PricingSourceId').value || 0;
        if (pricingSourceId == 1) {
            let pricingCodeId = this.f.FuelPricingDetails.get('CodeId').value || 0;
            let fuelTypeId = this.f.FuelDetails.get('FuelTypeId').value || 0;
            let latitude = this.f.AddressDetails['controls']['Latitude'].value || 0;
            let longitude = this.f.AddressDetails['controls']['Longitude'].value || 0;
            let countryId = this.f.AddressDetails.get('CountryId').value || 0;
            let terminal = this.f.FuelPricingDetails.get('TerminalId').value || '';
            let cityRackId = this.f.FuelPricingDetails.get('CityGroupTerminalId').value || '';

            this.addLocationService.getClosedTerminal(fuelTypeId, latitude, longitude, countryId, pricingCodeId, terminal, pricingSourceId, cityRackId).subscribe(data => {
                if (data) {
                    this.approvedTerminalList = data;
                }
            });
        }
    }
    addRemoveTierFee(isAdd: boolean) {
        let tierPricingArray = this.tierPricingForm.Pricings as FormArray;
        let arrayLength = tierPricingArray.controls.length;
        let lastIndex = arrayLength - 1;
        let lastRow = tierPricingArray.controls[lastIndex] as FormGroup;
        let secondLastRow = tierPricingArray.controls[lastIndex - 1] as FormGroup;
        let thirdLastRow = tierPricingArray.controls[lastIndex - 2] as FormGroup;

        if (isAdd) {
            let tierPricingArray = this.tierPricingForm.Pricings as FormArray;
            let newRow = this.getTierPricingForm(false);

            newRow.get('FromQuantity').setValue(secondLastRow.get('ToQuantity').value);
            lastRow.get('FromQuantity').setValue(0);
            lastRow.get('Quantity').setValue(0);
            tierPricingArray.insert((lastIndex), newRow);
        }
        else if (arrayLength > 2) {
            lastRow.get('FromQuantity').setValue(thirdLastRow.get('ToQuantity').value);
            lastRow.get('Quantity').setValue(thirdLastRow.get('ToQuantity').value);
            tierPricingArray.removeAt(lastIndex - 1);
        }
    }
    toQuantityChanged(value: number, index: number) {
        let nextPricingRow = this.tierPricingForm.Pricings.controls[index + 1] as FormGroup;
        if (nextPricingRow) {
            nextPricingRow.get('FromQuantity').setValue(value);
            nextPricingRow.get('Quantity').setValue(value);
        }
    }
    getTierPricingForm(isAboveQuantityRow: boolean): FormGroup {

        let validators: ValidatorFn[] = [];
        isAboveQuantityRow ? null : validators.push(Validators.required);

        return this.fb.group(
            {
                RequestPriceDetailId: this.fb.control(null),
                FromQuantity: this.fb.control(0, validators),
                ToQuantity: this.fb.control(0, validators),
                Quantity: this.fb.control(null),
                TerminalId: this.fb.control(null),
                TerminalName: this.fb.control(null),
                TempTerminalName: this.fb.control(null),
                CityGroupTerminalId: this.fb.control(null),
                CityGroupTerminalStateId: this.fb.control(null),
                DisplayPrice: this.fb.control(null, [Validators.required]),
                RackAvgTypeId: this.fb.control(null),
                PricePerGallon: this.fb.control(0),
                SupplierCostMarkupTypeId: this.fb.control(null),
                SupplierCostMarkupValue: this.fb.control(0),
                MarginTypeId: this.fb.control(null),
                Margin: this.fb.control(null),
                BasePrice: this.fb.control(null),
                RackPrice: this.fb.control(0),
                BaseSupplierCost: this.fb.control(null),
                Currency: this.fb.control(0),
                ExchangeRate: this.fb.control(null),
                UoM: this.fb.control(0),
                FuelTypeId: this.fb.control(null),
                JobId: this.fb.control(0),
                PricingSourceId: this.fb.control(1),
                PricingTypeId: this.fb.control(PricingType.PricePerGallon),
                //RackTypeId : this.fb.control(null),
                IncludeTaxes: this.fb.control(false),
                IsActive: this.fb.control(false),
                IsAboveQuantity: this.fb.control(isAboveQuantityRow),
                RowIndex: this.fb.control(1),
                TotalRows: this.fb.control(null),
                EstimatedPPG: this.fb.control(null),
                //PricingCode: this.fb.control({ Id: null, Code: null, Description: null }),
                PricingCode: this.fb.control({ Id: this.pricingCodesArr[0].PricingTypes.Fixed.Id, Code: this.pricingCodesArr[0].PricingTypes.Fixed.Code, Description: null }),
                TempPricingCodeDetails: this.fb.control(null),
                SupplierCost: this.fb.control(null),
                SupplierCostTypeId: this.fb.control(null),
                CreationTimeRackPPG: this.fb.control(null),
                MarkertBasedPricingTypeId: this.fb.control(null),
                IsEdit: this.fb.control(false),
                CityGroupTerminalName: this.fb.control(null),
                FuelDisplayGroupId: this.fb.control(0),
                IsTerminalPrice: this.fb.control(null),
                IsSupplierCostPrice: this.fb.control(null),
                IsFixedPrice: this.fb.control(null),
            }
        );
    }
    //TIER PRICING TYPE 
    initailizeTierPricingTypeForm(selectedRow?: any) {
        let _form = this.fb.group({
            Id: this.fb.control(null),
            PricingTypeId: this.fb.control(2, [Validators.required]),
            PricePerGallon: this.fb.control(null, [Validators.required, Validators.min(this._minimumConst)]),
            Code: this.fb.control(null),
            CodeId: this.fb.control(null),
            CodeDescription: this.fb.control(null),
            TempPricingCodeDetails: this.fb.control(null),
            RackAvgTypeId: this.fb.control(1),
            RackPrice: this.fb.control(0),
            EnableCityRack: this.fb.control(null),
            TerminalName: this.fb.control(null),
            TerminalId: this.fb.control(null),
            TempTerminalName: this.fb.control(null),
            SupplierCostMarkupTypeId: this.fb.control(1),
            SupplierCostMarkupValue: this.fb.control(0),
            SupplierCost: this.fb.control(null),
            CityGroupTerminalId: this.fb.control(null),
            FuelPricingDetails: this.fb.group({
                PricingSourceId: this.fb.control(1),
                PricingCode: this.fb.control({ Id: null, Code: null, Description: null })
            })
        });
        if (selectedRow) {
            _form.get('TerminalName').setValue(selectedRow.TerminalName);
            _form.get('TerminalId').setValue(selectedRow.TerminalId);
            _form.get('CityGroupTerminalId').setValue(selectedRow.CityGroupTerminalId);
            _form.get('RackAvgTypeId').setValue(selectedRow.RackAvgTypeId);
            _form.get('PricePerGallon').setValue(selectedRow.PricePerGallon);
            _form.get('SupplierCostMarkupTypeId').setValue(selectedRow.SupplierCostMarkupTypeId);
            _form.get('SupplierCostMarkupValue').setValue(selectedRow.SupplierCostMarkupValue);
            _form.get('SupplierCost').setValue(selectedRow.SupplierCost);
            _form.get('RackPrice').setValue(selectedRow.RackPrice);
            _form.get('PricingTypeId').setValue(selectedRow.PricingTypeId);
            _form.get('TempPricingCodeDetails').setValue(selectedRow.TempPricingCodeDetails);
            //_form.get('PricingSourceId').setValue(data.FuelPricingDetails.PricingSourceId);
            _form.get('FuelPricingDetails').get('PricingSourceId').setValue(selectedRow.PricingSourceId);
            //_form.get('PricingCode').setValue(data.FuelPricingDetails.PricingCode);
            _form.get('FuelPricingDetails').get('PricingCode').setValue(selectedRow.PricingCode);
        }
        return _form;
    }
    selectedPricingIndex: number = null;
    setPricingSourceClicked_tpt(index: number, SetTierPriceType: any) {
        this.selectedPricingIndex = index;
        //set form
        let selectedRow = this.tierPricingForm.Pricings.controls[this.selectedPricingIndex].getRawValue();
        //selectedRow.PricingSourceId = 1;
        this.TierPricingTypeForm = this.initailizeTierPricingTypeForm(selectedRow);

        let control = this.TierPricingTypeForm.get('PricingTypeId') as FormControl;
        this.setPricingValidation_tpt(this.TierPricingTypeForm.get('PricingTypeId').value);
        //CITY RACK FOR SOURCING REQ
        if (selectedRow.PricingCode && selectedRow.PricingCode.Code && selectedRow.PricingCode.Code.startsWith("O")) {
            this.getCityGroupTerminals_tpt();
            this.updateFormControlValidators(this.TierPricingTypeForm.get('CityGroupTerminalId'), [Validators.required]);
            this.TierPricingTypeForm.get('CityGroupTerminalId').setValue(null)
            this.TierPricingTypeForm.get('EnableCityRack').setValue(true);
            this.TierPricingTypeForm.get('FuelPricingDetails').get('PricingSourceId').setValue(2);
        }
        //this.TierPricingTypeForm.get('TempTerminalName').setValue(selectedRow.TempTerminalName || selectedRow.TempTerminalName)
        //open modal
        this.modalService.open(SetTierPriceType, { windowClass: 'pricingcode-modal', size: 'lg', scrollable: true });
        this.getPricingCodes();
    }
    pricingTypeChanged_tpt(type: number) {

        this.setPricingValidation_tpt(type);
        this.initilizeMarketBasedPrice_tpt();

        if (type == 1) {
            this.getPricingCodes();
        }
        else {
            this.setPricingCode_tpt();
        }
    }
    onPricingCodeSelected_tpt(item: any) {

        let pricingCodeDisplayData = this.getPricingDisplayData(item);

        this.TierPricingTypeForm.get('Code').patchValue(item.Code);
        this.TierPricingTypeForm.get('CodeId').patchValue(item.Id);
        this.TierPricingTypeForm.get('PricingTypeId').patchValue(item.PricingTypeId);
        this.TierPricingTypeForm.get('CodeDescription').patchValue(pricingCodeDisplayData);
        this.TierPricingTypeForm.get('FuelPricingDetails').get('PricingSourceId').patchValue(item.PricingSourceId);
        this.TierPricingTypeForm.get('FuelPricingDetails').get('PricingCode').patchValue({ Id: item.Id, Code: item.Code, Description: pricingCodeDisplayData });
        this.f.FuelPricingDetails.get('CityGroupTerminalId').setValue(null);
        
        if (item.PricingSourceId == 1) {
            this.TierPricingTypeForm.get('EnableCityRack').setValue(false);
        }
        else {
            this.TierPricingTypeForm.get('EnableCityRack').setValue(true);
        }
        this.setRackTerminalValidation_tpt();
        this.getCityGroupTerminals_tpt();
    }

    openPriceCodeModal_tpt(pricingcodeModal) {
        this.getPricingCodes();
        this.modalService.open(pricingcodeModal, { windowClass: 'pricingcode-modal', size: 'lg', scrollable: true, backdrop: 'static', keyboard: false });
    }
    setRackTerminalValidation_tpt() {
        let isChecked = this.TierPricingTypeForm.get('EnableCityRack').value;
        if (isChecked) {
            this.updateFormControlValidators(this.TierPricingTypeForm.get('CityGroupTerminalId'), [Validators.required]);
        }
        else {
            this.updateFormControlValidators(this.TierPricingTypeForm.get('CityGroupTerminalId'), []);
        }
    }
    _loaderTierPricingType: boolean = false;
    cityRackTerminalChanged_tpt() {
        let jobid = this.f.AddressDetails.get('JobId').value,
            fueltypeId = this.f.FuelDetails.get('FuelTypeId').value,
            selectedCityRackId = this.TierPricingTypeForm.get('CityGroupTerminalId').value,
            lattitude = this.f.AddressDetails.get('Latitude').value,
            longitude = this.f.AddressDetails.get('Longitude').value,
            countryCode = this.f.AddressDetails.get('CountryCode').value,
            _countryCode = countryCode == "1"? "USA" : "CAN",
            sourceId = this.TierPricingTypeForm.get('FuelPricingDetails').get('PricingSourceId').value

        this._loaderTierPricingType = true;
        this.addLocationService.IsCityGroupTerminalPriceAvailable(jobid ? jobid : 0, fueltypeId, selectedCityRackId, lattitude, longitude, _countryCode, sourceId).subscribe(response => {
            this._loaderTierPricingType = false;
            if (response == false) {
                this.TierPricingTypeForm.get('CityGroupTerminalId').setValue(null);
                Declarations.msgwarning("Pricing not available for this City Rack/Terminal. Try to assign another City Rack/Terminal.", null, null)
            }
        });
        if (sourceId != 1) {
            this.getOpisTerminals_tpt();
        }
    }
    getApprovedTerminal_tpt() {
        let pricingSourceId = this.TierPricingTypeForm.get('FuelPricingDetails').get('PricingSourceId').value || 0;
        if (pricingSourceId == 1) {
            let pricingCodeId = this.TierPricingTypeForm.get('CodeId').value || 0;
            let fuelTypeId = this.f.FuelDetails.get('FuelTypeId').value || 0;
            let latitude = this.f.AddressDetails['controls']['Latitude'].value || 0;
            let longitude = this.f.AddressDetails['controls']['Longitude'].value || 0;
            let countryId = this.f.AddressDetails.get('CountryId').value || 0;
            let terminal = this.TierPricingTypeForm.get('TerminalId').value || '';
            let cityRackId = this.TierPricingTypeForm.get('CityGroupTerminalId').value || '';

            this.addLocationService.getClosedTerminal(fuelTypeId, latitude, longitude, countryId, pricingCodeId, terminal, pricingSourceId, cityRackId).subscribe(data => {
                if (data) {
                    this.tierApprovedTerminalList = data;
                }
            });
        }
    }
    onApprovedTerminalSelected_tpt(event: any): void {
        this.TierPricingTypeForm.get('TerminalName').patchValue(event.Name);
        this.TierPricingTypeForm.get('TerminalId').patchValue(event.Id);
    }
    removePricingValidation_tpt() {
        this.updateFormControlValidators(this.TierPricingTypeForm.get('PricePerGallon'), []);
        this.updateFormControlValidators(this.TierPricingTypeForm.get('SupplierCostMarkupValue'), []);
        this.updateFormControlValidators(this.TierPricingTypeForm.get('RackPrice'), []);
        this.updateFormControlValidators(this.TierPricingTypeForm.get('Code'), []);
        this.updateFormControlValidators(this.TierPricingTypeForm.get('TempPricingCodeDetails'), []);
    }
    setPricingValidation_tpt(type: number) {
        this.removePricingValidation_tpt();
        if (!this.f.IsSupressOrderPricing.value) {
            //Market Based
            if (type == 1) {
                this.updateFormControlValidators(this.TierPricingTypeForm.get('RackPrice'), [Validators.required, Validators.min(this._zeroConst)]);
                //REMOVED FOR SOURCING
                //this.updateFormControlValidators(this.TierPricingTypeForm.get('Code'), [Validators.required]);
                this.updateFormControlValidators(this.TierPricingTypeForm.get('TempPricingCodeDetails'), [Validators.required]);

                this.TierPricingTypeForm.get('RackAvgTypeId').setValue(1);
            }
            //Fuel cost
            else if (type == 4) {
                this.updateFormControlValidators(this.TierPricingTypeForm.get('SupplierCostMarkupValue'), [Validators.required, Validators.min(this._zeroConst)]);
                this.TierPricingTypeForm.get('SupplierCostMarkupTypeId').setValue(1);
            }
            //Fixed
            else if (type == 2) {
                this.updateFormControlValidators(this.TierPricingTypeForm.get('PricePerGallon'), [Validators.required, Validators.min(this._minimumConst)]);
            }
        }
    }
    getOpisTerminals_tpt() {
        let cityRackId = this.TierPricingTypeForm.get('CityGroupTerminalId').value || 0,
            latitude = this.f.AddressDetails.get('Latitude').value,
            longitude = this.f.AddressDetails.get('Longitude').value,
            countryId = this.f.AddressDetails.get('CountryId').value,
            source = this.TierPricingTypeForm.get('FuelPricingDetails').get('PricingSourceId').value
        this.addLocationService.getOpisTerminals(cityRackId, latitude, longitude, countryId, '', source).subscribe(response => {
            if (response) {
                this.tierApprovedTerminalList = response;
            }
        });
    }
    getCityGroupTerminals_tpt() {
        this.tierCityGroupTerminalsList = [];
        let selectedState = this.f.AddressDetails.get('StateId').value;
        if (selectedState > 0) {
            this.addLocationService.GetCityGroupTerminals(selectedState, false, this.TierPricingTypeForm.get('FuelPricingDetails').get('PricingSourceId').value).subscribe(data => {
                if (data && data.length > 0) {
                    this.tierCityGroupTerminalsList = data;
                }
            });
        }
    }
    initilizeMarketBasedPrice_tpt() {
        this.TierPricingTypeForm.get('EnableCityRack').setValue(false);
        this.updateFormControlValidators(this.TierPricingTypeForm.get('CityGroupTerminalId'), []);
        this.TierPricingTypeForm.get('Code').patchValue(null);
        this.TierPricingTypeForm.get('CodeId').patchValue(null);
        this.TierPricingTypeForm.get('CodeDescription').patchValue(null);
        this.TierPricingTypeForm.get('FuelPricingDetails').get('PricingCode').patchValue({ Id: null, Code: null, Description: null });
        this.TierPricingTypeForm.get('TempPricingCodeDetails').patchValue(null);
    }
    setPricingCode_tpt() {
        if (this.TierPricingTypeForm.get('PricingTypeId').value != 1) {
            let pricingCode = this.getPricingCode(this.TierPricingTypeForm.get('PricingTypeId').value, this.TierPricingTypeForm.get('FuelPricingDetails.PricingSourceId').value);
            if (pricingCode != null) {
                this.TierPricingTypeForm.get('FuelPricingDetails.PricingCode').patchValue(pricingCode);
            }
        }
    }
    getRackAvgTypeNameById(id: number, price: string): string {
        let response = '';
        let rack = this.RackAvgTypes.find(r => r.Id == id)

        if (rack) {
            //let name = rack.Name;
            if (id == 1) {
                response = '+ $' + price;
            }
            else if (id == 2) {
                response = '- $' + price;
            }
            else if (id == 3) {
                response = '+ ' + price + '%';
            }
            else if (id == 4) {
                response = '- ' + price + '%';
            }
        }
        return response;
    }
    cumulationTypeChanged(checked: boolean, cumulationType: number) {

        this.updateFormControlValidators(this.tierPricingForm.ResetCumulationSetting.get('Day'), []);
        this.updateFormControlValidators(this.tierPricingForm.ResetCumulationSetting.get('Date'), []);

        if (checked) {
            //let cumulationType = this.tierPricingForm.ResetCumulationSetting.get('CumulationType').value
            if (cumulationType == 1 || cumulationType == 2) {
                this.updateFormControlValidators(this.tierPricingForm.ResetCumulationSetting.get('Day'), [Validators.required]);
            }
            else if (cumulationType == 3 || cumulationType == 4) {
                this.updateFormControlValidators(this.tierPricingForm.ResetCumulationSetting.get('Date'), [Validators.required]);
            }
        }
    }
    setPricing() {
        if (this.TierPricingTypeForm.invalid) {
            return;
        }
        this.modalService.dismissAll();
        let control: AbstractControl = this.tierPricingForm.Pricings.controls[this.selectedPricingIndex];
        let sourceObj = this.TierPricingTypeForm.getRawValue();

        control.get('TerminalName').setValue(sourceObj.TerminalName);
        control.get('TerminalId').setValue(sourceObj.TerminalId);
        control.get('CityGroupTerminalId').setValue(sourceObj.CityGroupTerminalId);
        control.get('RackAvgTypeId').setValue(sourceObj.RackAvgTypeId);
        control.get('PricePerGallon').setValue(sourceObj.PricePerGallon);
        control.get('SupplierCostMarkupTypeId').setValue(sourceObj.SupplierCostMarkupTypeId);
        control.get('SupplierCostMarkupValue').setValue(sourceObj.SupplierCostMarkupValue);
        control.get('SupplierCost').setValue(sourceObj.SupplierCost);
        control.get('RackPrice').setValue(sourceObj.RackPrice);
        control.get('PricingSourceId').setValue(sourceObj.FuelPricingDetails.PricingSourceId);
        control.get('PricingCode').setValue(sourceObj.FuelPricingDetails.PricingCode);
        control.get('PricingTypeId').setValue(sourceObj.PricingTypeId);
        control.get('TempPricingCodeDetails').setValue(sourceObj.TempPricingCodeDetails);
        // control.patchValue(this.TierPricingTypeForm.value);
        // control.get('TempPricingCodeDetails').setValue(sourceObj.TempPricingCodeDetails);
        // control.get('Code').setValue(sourceObj.Code);
        // control.get('CodeId').setValue(sourceObj.CodeId);
        // control.get('CodeDescription').setValue(sourceObj.CodeDescription);
        // control.get('EnableCityRack').setValue(sourceObj.EnableCityRack);
        // control.get('FuelPricingDetails').patchValue(sourceObj.FuelPricingDetails);

        if (sourceObj.PricingTypeId == 1) {
            control.get('DisplayPrice').setValue('Rack Avg ' + this.getRackAvgTypeNameById(sourceObj.RackAvgTypeId, sourceObj.RackPrice));
        }
        else if (sourceObj.PricingTypeId == 4) {
            control.get('DisplayPrice').setValue('Fuel Cost ' + this.getRackAvgTypeNameById(sourceObj.SupplierCostMarkupTypeId, sourceObj.SupplierCostMarkupValue));
        }
        else {
            control.get('DisplayPrice').setValue('$' + sourceObj.PricePerGallon);
        }
    }
    getPricingCodes() {
        let filterData = {}
        if (this.f.FuelPricingDetails.get('IsTierPricingRequired').value) {
            filterData = {
                "PricingSourceId": this.TierPricingTypeForm.get('FuelPricingDetails').get('PricingSourceId').value,//this.tierPricingForm.Pricings.controls[this.selectedPricingIndex].get('PricingSourceId').value,
                "PricingTypeId": this.TierPricingTypeForm.get('PricingTypeId').value,
                "tfxProdId": this.f.FuelDetails.get("FuelTypeId").value,
                "feedTypeId": this.pricingfeedTypeId,
                "fuelClassTypeId": this.pricingfuelClassTypeId,
                "Prefix": ""
            };
        }
        else {
            filterData = {
                "PricingSourceId": this.f.FuelPricingDetails.get('FuelPricingDetails').get('PricingSourceId').value,
                "PricingTypeId": this.f.FuelPricingDetails.get('PricingTypeId').value,
                "tfxProdId": this.f.FuelDetails.get("FuelTypeId").value,
                "feedTypeId": this.pricingfeedTypeId,
                "fuelClassTypeId": this.pricingfuelClassTypeId,
                "Prefix": ""
            }
        }

        this._loadingPricingCodes = true;
        this.addLocationService.getPricingCodes(filterData).subscribe(data => {
            this._loadingPricingCodes = false;
            if (data) {
                this.pricingCodes = data.PricingCodes;
            }
        });
    }
    toggleSuppressPricing(isChecked: boolean) {
        let isTierPriced = this.fuelPricingForm.IsTierPricingRequired.value;
        if (isChecked) {
            if (isTierPriced) {
                let pricing = this.tierPricingForm.Pricings as FormArray;
                pricing.clear();
                this.removePricingValidation_tpt();
            } else { 
                this.removePricingValidation();
            }
        } else {
            if (isTierPriced) {
                this.tierPricingEnabled(isTierPriced);
            } else {
                this.setPricingValidation(this.f.FuelPricingDetails.get('PricingTypeId').value);
            }
        }
        this.UpdateSuppressPricingChange.emit(isChecked);
    }
    tierPricingEnabled(isChecked: boolean) {

        let pricing = this.tierPricingForm.Pricings as FormArray;
        pricing.clear()
        if (isChecked) {
            pricing.push(this.getTierPricingForm(false));
            pricing.push(this.getTierPricingForm(true));
            this.removePricingValidation();
        } else {
            this.setPricingValidation(this.f.FuelPricingDetails.get('PricingTypeId').value);
        }
        this.changeDetectorRef.detectChanges();
    }
    patchExistingPricingDetails(fuelPricingDetails: any){

        fuelPricingDetails['TempPricingCodeDetails'] = fuelPricingDetails.Code;
        fuelPricingDetails['TempTerminalName'] = fuelPricingDetails.TerminalName;

        let code = { Code: fuelPricingDetails.Code, CodeDescription: fuelPricingDetails.CodeDescription, CodeId: fuelPricingDetails.CodeId };
        this.locationForm.get('FuelPricingDetails').patchValue(fuelPricingDetails);
        this.locationForm.get('FuelPricingDetails').get('FuelPricingDetails').get('PricingCode').patchValue(code);

        //CITY RACK
        if(fuelPricingDetails.EnableCityRack){
            this.updateFormControlValidators(this.f.FuelPricingDetails.get('CityGroupTerminalId'), [Validators.required]);
            this.getCityGroupTerminals();
        }
        if (fuelPricingDetails.Code && fuelPricingDetails.Code.startsWith("O")) {
            this.f.FuelPricingDetails.get('FuelPricingDetails').get('PricingSourceId').setValue(2)
        } else{
            this.f.FuelPricingDetails.get('FuelPricingDetails').get('PricingSourceId').setValue(1)
        }

        //TERMINAL DETAILS
        if(fuelPricingDetails.IsTierPricingRequired){

            let pricing = this.tierPricingForm.Pricings as FormArray;

            fuelPricingDetails.TierPricing?.Pricings && fuelPricingDetails.TierPricing.Pricings.forEach(row => {

                //SET QUANTITY
                if (row.IsAboveQuantity) {
                    row['Quantity'] = row['FromQuantity'];
                }
                //SET TEMP
                row['TempPricingCodeDetails'] = row['PricingCode'];
                row['TempTerminalName'] = row.TerminalName;

                let form = this.getTierPricingForm(row.IsAboveQuantity);
                form.patchValue(row);

                // //CITY RACK
                // if(row.EnableCityRack){
                //     this.updateFormControlValidators(form.get('CityGroupTerminalId'), [Validators.required]);
                //     form.get('CityGroupTerminalId').setValue(2)
                // }

                pricing.push(form);
            });
            
            this.removePricingValidation();
            this.changeDetectorRef.detectChanges();
        } else {
            this.setPricingValidation(this.f.FuelPricingDetails.get('PricingTypeId').value);
        }
    }
}
