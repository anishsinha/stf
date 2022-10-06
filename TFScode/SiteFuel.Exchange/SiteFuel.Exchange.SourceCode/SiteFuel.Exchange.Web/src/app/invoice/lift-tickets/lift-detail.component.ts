import { Component, OnInit, Output, EventEmitter, Input, SimpleChanges } from '@angular/core';
import { DropdownItem, StateDropdownExtendedItem, StatelistService } from 'src/app/statelist.service';
import { FormGroup, FormBuilder, FormArray, Validators, FormControl, AbstractControl } from '@angular/forms';
import { DropDetailModel, AddressModel } from '../models/DropDetail';
import { ActivatedRoute } from '@angular/router';
import { AddressService } from 'src/app/address.service';
import { Declarations } from 'src/app/declarations.module';
import { InvoiceService } from '../../invoice/services/invoice.service';
import { UoM } from 'src/app/app.enum';
import { convertTo24Hour, getSeconds, RegExConstants } from 'src/app/app.constants';
import { ValidationService } from 'src/app/services/validation.service';
import { DataService } from '../../services/data.service';

@Component({
    selector: 'app-lift-detail',
    templateUrl: './lift-detail.component.html',
    styleUrls: ['./lift-detail.component.css']
})
export class LiftDetailComponent implements OnInit {

    public _opened: boolean = false;
    public _animate: boolean = true;
    public _loading: boolean = false;
    public _loadingAddress: boolean = false;
    public _positionNum: number = 1;
    public _POSITIONS: Array<string> = ['left', 'right', 'top', 'bottom'];
    public _searchKeyword = 'name';

    public StateList: StateDropdownExtendedItem[] = [];
    public CountryList: DropdownItem[];
    public CountryGroupList: DropdownItem[];
    public BulkplantList: DropdownItem[];
    public BulkPlants: DropdownItem[];

    public TicketDetailForm: FormGroup;
    public Drops: DropDetailModel[];
    public EditIndex: number = -1;
    public OrderId: number;

    public UoM: string;

    public hasDuplicateLiftTicket: boolean;
    public liftticketnumberslist = [];
    public curLiftTicketNumber: number;
    @Input() public IsFrieghtPricingMethodAuto: boolean;
   
    @Input() public IsBadgeMandatory: boolean;
    public isBadgeMandatory: boolean = false;

    @Output() onTicketDetailAdded: EventEmitter<any> = new EventEmitter<any>();
    @Output() onTicketDetailUpdated: EventEmitter<any> = new EventEmitter<any>();
   
    constructor(private fb: FormBuilder, private route: ActivatedRoute, private stateService: StatelistService, private dataService: DataService,
        private addresService: AddressService, private invoiceService: InvoiceService,private validationService: ValidationService) {
        this.StateList = [];
        this.CountryList = [];
        this.CountryGroupList = [];
        this.BulkPlants = [];
        this.BulkplantList = [];

        this.OrderId = parseInt(this.route.snapshot.queryParamMap.get('orderId'), 10);
        this.TicketDetailForm = this.buildForm();
        if (this.TicketDetailForm) {
            if (this.isBadgeMandatory) {
                this.TicketDetailForm.controls['BadgeNumber'].setValidators([Validators.required]);
            } else {
                this.TicketDetailForm.controls['BadgeNumber'].clearValidators();
            }
        }
        this.stateService.getStates().subscribe(data => {
            this.StateList = data
        });

        this.stateService.getCountries().subscribe(data => {
            this.CountryList = data;
        });

        this.stateService.getCountryGroup(4).subscribe(data => {
            this.CountryGroupList = data;
        });     
    }

    ngOnInit() {

    }
    ngOnChanges(change: SimpleChanges) {
        if (change.IsBadgeMandatory && change.IsBadgeMandatory.currentValue != null) {
            this.isBadgeMandatory = change.IsBadgeMandatory.currentValue;
            if (this.TicketDetailForm) {
                if (this.isBadgeMandatory) {
                    this.TicketDetailForm.controls['BadgeNumber'].setValidators([Validators.required]);
                } else {
                    this.TicketDetailForm.controls['BadgeNumber'].clearValidators();
                }
            }
        }
        this.showBulkPlantOnLift();
    }
    initDrops(_drops: DropDetailModel[]) {
       // this.setUoM(_drops);
        if (_drops != null && _drops != undefined) {
            this.Drops = _drops;
            this.setProductsToFormArray();
        }
    }

    getAddedProductDetails(drops: any[]) {
        if (drops != null || drops != undefined) {
			var dropdates = drops.map(item => item.DropDate);
			if (dropdates.length > 0 && dropdates.indexOf(null) == -1 && dropdates != undefined && dropdates.indexOf("") == -1 && dropdates.indexOf(undefined) == -1) {
                this.findMinDate(dropdates);
            }

        }
    }
    findMinDate(dropdates: any[]) {
        var dates = [];
        dropdates.forEach((dropdate) => {
            var date = new Date(dropdate);
            dates.push(date);
        });
        //var maximumDate = new Date(Math.max.apply(null, dates)); 
        var minimumDate = new Date(Math.min.apply(null, dates));
        var minDate = minimumDate.toDateString();
        var date = this.getFormattedDate(minDate);
        if (date != null || date != undefined) {
            this.TicketDetailForm.get('LiftDate').patchValue(date);
        }
    }
    getFormattedDate(date: any) {
        var dt = new Date(date);
        var year = dt.getFullYear();
        var month = (1 + dt.getMonth()).toString();
        month = month.length > 1 ? month : '0' + month;
        var day = dt.getDate().toString();
        day = day.length > 1 ? day : '0' + day;
        return month + '/' + day + '/' + year;
    }

    //setUoM(drops: DropDetailModel[]): void {
    //    //None =0;
    //    //Gallons = 1,
    //    //Litres = 2
    //    if (drops != null && drops != undefined) {
    //        var UoM = drops.map(item => parseInt(item.UoM));
    //        if (UoM[0] == 1) {
    //            this.UoM = "Gallons";
    //        }
    //        else if (UoM[0] == 2) {
    //            this.UoM = "Litres";
    //        }
    //        else if (UoM[0] == 3) {
    //            this.UoM = "Barrels";
    //        }
    //        else if (UoM[0] == 4) {
    //            this.UoM = "MT";
    //        }
    //    }
    //}

    showBulkPlantOnLift() {
        if (this.IsFrieghtPricingMethodAuto) {
            this.addresService.getBulkPlantsForAutoFreightMethod(this.OrderId, '').subscribe((data: DropdownItem[]) => {
                this.BulkPlants = data.slice();
                this.BulkplantList = data;
            });
        } else {
            this.addresService.getBulkPlants('').subscribe(data => {
                this.BulkPlants = data.slice();
                this.BulkplantList = data;
            });
        }
    }

    setProductsToFormArray(): void {
        var prodArray = this.TicketDetailForm.get('Products') as FormArray;
        if (prodArray != undefined && prodArray != null) {
            prodArray.clear();
            var currentObj = this;
            this.Drops.forEach(function (elem, idx) {
                prodArray.push(currentObj.buildProduct(elem));
            });
        }
    }
  
    toggleOpen(shouldOpen: boolean, event: any) {
        this._opened = shouldOpen;
        if (shouldOpen) {
            if (event != null) {
                let ticketFormData = this.getTicketFormData(event.ticketDetail);
                this.setProductsToFormArray();
                this.TicketDetailForm.patchValue(ticketFormData);
                this.EditIndex = event.index;
            }
        } else {
            this.TicketDetailForm.reset();
            (this.TicketDetailForm.get('Products') as FormArray).clear();
        }
    }
    getTicketFormData(ticketDetail: any): any {
        let _products = [];
        ticketDetail.Products.forEach(x => {
            _products.push({
                ProductId: x.ProductId,
                ProductName: x.ProductName,
                GrossQuantity: x.GrossQuantity,
                NetQuantity: x.NetQuantity,
                DeliveredQuantity: x.DeliveredQuantity,
                BulkPlantId: x.BulkPlantId,
                BulkPlantName: x.BulkPlantName,
                Address: x.Address
            });
        });
        let data = {
            LiftTicketNumber: ticketDetail.LiftTicketNumber,
            BadgeNumber: ticketDetail.BadgeNumber,
            LiftDate: ticketDetail.LiftDate,
            Products: _products,
            LiftStartTime: ticketDetail.LiftStartTime,
            LiftEndTime: ticketDetail.LiftEndTime,
            CommonBulkPlantId: ticketDetail.CommonBulkPlantId,
            CommonBulkPlantName: ticketDetail.CommonBulkPlantName,
            CommonAddress: ticketDetail.CommonAddress,
        };
        return data;
    }
    buildForm() {
        return this.fb.group({
            Id: this.fb.control(''),
            LiftTicketNumber: this.fb.control('', Validators.required),
            LiftDate: this.fb.control('', Validators.required),
            BadgeNumber: this.fb.control(''),
            LiftStartTime: this.fb.control('', Validators.required),
            LiftEndTime: this.fb.control('', Validators.required),
            Products: this.fb.array([]),
            CommonAddress: this.buildAddress(new AddressModel()),
            CommonBulkPlantName: this.fb.control(''),
            CommonBulkPlantId: this.fb.control(''),
        });
    }

    buildProduct(model: DropDetailModel): FormGroup {
        var formG =  this.fb.group({
            ProductId: this.fb.control(model.FuelTypeId),
            ProductName: this.fb.control(model.FuelTypeName),
            NetQuantity: this.fb.control('', [Validators.pattern(RegExConstants.Quantity)]),
            DeliveredQuantity: this.fb.control('', [Validators.pattern(RegExConstants.Quantity)]),
            GrossQuantity: this.fb.control('', [Validators.pattern(RegExConstants.Quantity)]),
            BulkPlantId: this.fb.control(''),
            BulkPlantName: this.fb.control(''),
            Address: this.buildAddress(new AddressModel()),
            Image: this.fb.control(''),
            QuantityIndicatorTypeId: this.fb.control(model.QuantityIndicatorTypeId),
            UOM: this.fb.control('')
        });
        this.setUoM(model.UoM, model.JobCountryId, formG);
        return formG;
    }

    buildAddress(model: AddressModel): FormGroup {
        return this.fb.group({
            Address: this.fb.control(model.Address, [Validators.required]),
            Latitude: this.fb.control(model.Latitude, [Validators.required]),
            Longitude: this.fb.control(model.Longitude, [Validators.required]),
            City: this.fb.control(model.City, [Validators.required]),
            CountyName: this.fb.control(model.CountyName, [Validators.required]),
            State: this.fb.group({
                Id: this.fb.control(model.State.Id, [Validators.required]),
                Code: this.fb.control(model.State.Code, [Validators.required])
            }),
            Country: this.fb.group({
                Id: this.fb.control(model.Country.Id, [Validators.required]),
                Code: this.fb.control(model.Country.Code, [Validators.required])
            }),
            CountryGroup: this.fb.group({
                Id: this.fb.control(model.Country.Id),
                Code: this.fb.control(model.Country.Code)
            }),
            ZipCode: this.fb.control(model.ZipCode, [Validators.required]),
        });
    }

    setUoM(uom: any, jobCountryId: any, productForm: FormGroup): void {
        if (uom == UoM.MetricTons || uom == UoM.Barrels) {
            if (jobCountryId == 1 || jobCountryId == 4) {
                productForm.get('UOM').setValue("Gallons");
            }
            else if (jobCountryId == 2) {
                productForm.get('UOM').setValue("Litres");
            }
        }
        else {
            if (uom == UoM.Gallons) {
                productForm.get('UOM').setValue("Gallons");
            }
            else if (uom == UoM.Litres) {
                productForm.get('UOM').setValue("Litres");
            }
        }
    }

    getAddressByZip(product: FormGroup, event: any): void {
        var zipCode = event.target.value;
        var _address = product.get('Address') as FormGroup;
        if (RegExConstants.UsaZip.test(zipCode) || RegExConstants.CanZip.test(zipCode)) {
            this._loadingAddress = true;
            this.addresService.getAddress(zipCode)
                .subscribe(data => {
                    this._loadingAddress = false;
                    if (data != null && data != undefined && data.StateCode != null) {
                        data.Address = _address.get('Address').value;
                        if (data.CountryCode != 'CAR') {
                            data.CountryCode == 'US' || data.CountryCode == 'USA' ? data.CountryCode = 'USA' : data.CountryCode = 'CAN';
                        }
                        var addressModel = this.addressMapper(data);
                        _address.patchValue(addressModel);
                    }
                });
        }
    }

    getAddressesByZip( event: any): void {
        var zipCode = event.target.value;
        var _address = this.TicketDetailForm.get('CommonAddress') as FormGroup;
        if (RegExConstants.UsaZip.test(zipCode) || RegExConstants.CanZip.test(zipCode)) {
            this._loadingAddress = true;
            this.addresService.getAddress(zipCode)
                .subscribe(data => {
                    this._loadingAddress = false;
                    if (data != null && data != undefined && data.StateCode != null) {
                        data.Address = _address.get('Address').value;
                        if (data.CountryCode != 'CAR') {
                            data.CountryCode == 'US' || data.CountryCode == 'USA' ? data.CountryCode = 'USA' : data.CountryCode = 'CAN';
                        }
                        var addressModel = this.addressMapper(data);
                        _address.patchValue(addressModel);
                    }
                });
        }
    }

    addressMapper(data: any): AddressModel {
        var state = this.StateList.find(x => x.Code == data.StateCode);
        var country = this.CountryList.find(x => x.Code == data.CountryCode);
        var countryGroup = this.CountryGroupList.find(x => x.Id == data.Id) || new DropdownItem();
        var _address = new AddressModel();
        _address.Address = data.Address;
        _address.City = data.City;
        _address.CountyName = data.CountyName
        _address.Latitude = data.Latitude;
        _address.Longitude = data.Longitude;
        _address.ZipCode = data.ZipCode;
        _address.State = state;
        _address.Country = country;
        _address.CountryGroup = countryGroup;
        return _address;
    }

    cancelTicketDetail() {
        this.TicketDetailForm.reset();
        this.toggleOpen(false, null);
    }
    addTicketDetail() {
        this.TicketDetailForm.markAllAsTouched();
        let products = this.TicketDetailForm.get('Products').value as any[];
        let invalidQuantityCount = 0;

        let address = this.TicketDetailForm.get('CommonAddress').value;
        let producstArr = this.TicketDetailForm.get('Products') as FormArray;
       // var CommonAddress = this.TicketDetailForm.get('Address').value;
        var commonTermId = this.TicketDetailForm.get('CommonBulkPlantId').value;
        var commonTerminalName = this.TicketDetailForm.get('CommonBulkPlantName').value;
        if (address &&  producstArr) {
            producstArr.controls.forEach(function (control) {
                control.get('Address').patchValue(address);
                control.get('BulkPlantName').setValue(commonTerminalName);
                control.get('BulkPlantId').setValue(commonTermId);
            });
        }
        products.forEach(function (product) {
            if (!(product.NetQuantity > 0 && product.GrossQuantity > 0 && product.DeliveredQuantity > 0)) {
                invalidQuantityCount++;
            }
        });
        if (this.isBadgeMandatory && !this.TicketDetailForm.get('BadgeNumber').value.trim()) {
            Declarations.msgerror("Please enter badge number", undefined, undefined);
            return;
        }

        if (products.length == invalidQuantityCount) {
            Declarations.msgerror("Delivered, gross and net quantity is required for atleast one product", undefined, undefined);
            return;
        }

        if (products.some(p => p.DeliveredQuantity > Math.max(p.GrossQuantity, p.NetQuantity))) {
            Declarations.msgerror("Delivered quantity should not be greater than net/gross quantity.", undefined, undefined);
            return;
        }

        var isValid = this.validateTicketDetails(); // validates Products formarray 

        var isValidQuantities = true;
        var invalidQuantities = this.validateQuantities();
        if (invalidQuantities.length > 0) {
            invalidQuantities.forEach(function (isFormValidInfo) {
                if (isFormValidInfo.isFormValid == false) {
                    Declarations.msgerror("Gross/net/delivered quantity is missing for product " + isFormValidInfo.ProductName, undefined, undefined);
                    isValidQuantities = false;
                }
            });
        }
        if (!isValidQuantities) {
            return;
        }
        if (isValid) {
            var liftTicketNumber = this.TicketDetailForm.get('LiftTicketNumber').value.trim().toLowerCase();
            if (this.liftticketnumberslist.indexOf(liftTicketNumber) != -1) {
                Declarations.msgerror("You are trying to enter duplicate Lift Ticket Numbers", undefined, undefined);
            }
            else {
                this.onTicketDetailAdded.emit(this.TicketDetailForm.value);
                this.toggleOpen(false, null);
            }
        }
        else {
            Declarations.msgerror('Please provide required details', undefined, undefined);
        }       
    }

    validateQuantities(): any[] {
        var isFormValid = true;
        var invalidFormInfo = [];
        var products = this.TicketDetailForm.get('Products').value;
        products.forEach(function (product) {

            if ((product.NetQuantity > 0 || product.GrossQuantity > 0 || product.DeliveredQuantity > 0) && !(product.NetQuantity > 0 && product.GrossQuantity > 0 && product.DeliveredQuantity > 0)) {
                isFormValid = false;
                invalidFormInfo.push({
                    isFormValid: isFormValid,
                    ProductName: product.ProductName
                });
            }
        });
        return invalidFormInfo;
    }
    updatedTicketDetail() {
        this.TicketDetailForm.markAllAsTouched();
        let products = this.TicketDetailForm.get('Products').value as any[];
        let invalidQuantityCount = 0;

        products.forEach(function (product) {
            if (!(product.NetQuantity > 0 && product.GrossQuantity > 0 && product.DeliveredQuantity > 0)) {
                invalidQuantityCount++;
            }
        });

        let address = this.TicketDetailForm.get('CommonAddress').value;
        let producstArr = this.TicketDetailForm.get('Products') as FormArray;
        // var CommonAddress = this.TicketDetailForm.get('Address').value;
        var commonTermId = this.TicketDetailForm.get('CommonBulkPlantId').value;
        var commonTerminalName = this.TicketDetailForm.get('CommonBulkPlantName').value;
        if (address && producstArr) {
            producstArr.controls.forEach(function (control) {
                control.get('Address').patchValue(address);
                control.get('BulkPlantName').setValue(commonTerminalName);
                control.get('BulkPlantId').setValue(commonTermId);
            });
        }

        if (products.length == invalidQuantityCount) {
            Declarations.msgerror("Delivered, gross and net quantity is required for atleast one product", undefined, undefined);
            return;
        }

        if (products.some(p => p.DeliveredQuantity > Math.max(p.GrossQuantity, p.NetQuantity))) {
            Declarations.msgerror("Delivered quantity should not be greater than net/gross quantity.", undefined, undefined);
            return;
        }

        var isValidQuantities = true;
        var invalidQuantities = this.validateQuantities();
        if (invalidQuantities.length > 0) {
            invalidQuantities.forEach(function (isFormValidInfo) {
                if (isFormValidInfo.isFormValid == false) {
                    Declarations.msgerror("Delivered/gross/net quantity is missing for product " + isFormValidInfo.ProductName, undefined, undefined);
                    isValidQuantities = false;
                }
            });
        }
        if (!isValidQuantities) {
            return;
        }
        if (this.isBadgeMandatory && !this.TicketDetailForm.get('BadgeNumber').value.trim()) {
            Declarations.msgerror("Please enter badge number", undefined, undefined);
            return;
        }

        var isValid = this.validateTicketDetails();
        if (isValid) {
            var updatedLiftnumber = this.TicketDetailForm.get('LiftTicketNumber').value.trim().toLowerCase();
            if ((this.curLiftTicketNumber != updatedLiftnumber) && (this.liftticketnumberslist.indexOf(updatedLiftnumber) != -1)) {
                Declarations.msgerror("You are trying to add duplicate Lift Ticket Numbers", undefined, undefined);
            }
            else {
                var eventData = {
                    ticketDetail: this.TicketDetailForm.value,
                    index: this.EditIndex
                };
                this.onTicketDetailUpdated.emit(eventData);
                this.toggleOpen(false, null);
                this.EditIndex = -1;
                this.liftticketnumberslist.splice(this.liftticketnumberslist.indexOf(this.curLiftTicketNumber), 1);
                this.liftticketnumberslist.push(updatedLiftnumber);
            }
        }
        else {
            Declarations.msgerror('Please provide required details ', undefined, undefined);
        }
    }

    validateTicketDetails(): boolean {
        var isValid = false;
            var _products = this.TicketDetailForm.get('Products') as FormArray;
            var _liftTicketNumber = this.TicketDetailForm.get('LiftTicketNumber').value;
            var _liftDate = this.TicketDetailForm.get('LiftDate').value;
            _products.controls.forEach(function (elem) {
                if (elem.get('BulkPlantName').value && elem.get('BulkPlantName').value.trim() != '' && _liftTicketNumber != "" &&
                    (_liftDate != "" && _liftDate != null && _liftDate != undefined) 
                    && (elem.get('Address') as FormGroup).valid)
                    isValid = true;
            });
        return isValid;
    }
    
    isInvalid(name: string): boolean {
        let result = this.TicketDetailForm.get(name).invalid &&
            (this.TicketDetailForm.get(name).dirty||this.TicketDetailForm.get(name).touched)
        return result;
    }

    isRequired(name: string): boolean {
        return this.TicketDetailForm.get(name).errors.required;
    }

    isInvalidPg(name: string, product: FormGroup): boolean {
        var result = product.get(name).invalid
            &&
            (
                product.get(name).dirty
                ||
                product.get(name).touched
            )
        return result;
    }

    isRequiredPg(name: string, product: FormGroup): boolean {
        return product.get(name).errors.required;
    }

    onBulkPlantSelected(product: FormGroup, event: any): void {
        this.dataService.setDropTerminalSelectedSubject(this.OrderId);
        product.controls['BulkPlantName'].setValue(event.Name);
        product.controls['BulkPlantId'].setValue(event.Id);
        var _address = product.get('Address') as FormGroup;
        this.BulkPlants = this.BulkplantList.slice();
        this.getBulKPlantAddress(event.Name, _address);
    }

    onBulkPlantsSelected(event: any): void {
        this.dataService.setDropTerminalSelectedSubject(this.OrderId);
        this.TicketDetailForm.controls['CommonBulkPlantName'].setValue(event.Name);
        this.TicketDetailForm.controls['CommonBulkPlantId'].setValue(event.Id);
        var _address = this.TicketDetailForm.get('CommonAddress') as FormGroup;
        this.BulkPlants = this.BulkplantList.slice();
        this.getBulKPlantAddress(event.Name, _address);
    }

    onBulkPlantSearched(event: any): void {
        let keyword = (event.target && event.target.value) ? event.target.value.toLowerCase() : '';
        this.BulkPlants = this.BulkplantList.slice().filter(function (elem) {
            return elem.Name.toLowerCase().indexOf(keyword) >= 0;
        });
    }

    ValidateBolTime(LiftstartTime: AbstractControl, LiftEndTime: AbstractControl) {

        this.validationService.removeError(LiftstartTime, 'invalidTime');
        this.validationService.removeError(LiftEndTime, 'invalidTime');

        let stringLiftStartTime = LiftstartTime.value;
        let stringLiftEndTime = LiftEndTime.value;

        if (stringLiftStartTime && stringLiftEndTime) {
            
            let StartTimeLift = parseInt(getSeconds(convertTo24Hour(stringLiftStartTime)));
            let EndTimeLift = parseInt(getSeconds(convertTo24Hour(stringLiftEndTime)));

            if ((StartTimeLift > EndTimeLift)) {
                this.validationService.addError(LiftstartTime, 'invalidTime');
            }
        };
    }

    setStateName(product: FormGroup, event: any) {
        product.get('Address.State.Code').setValue(event.target.selectedOptions[0].text);
    }

    setStatesName( event: any) {
        this.TicketDetailForm.get('CommonAddress.State.Code').setValue(event.target.selectedOptions[0].text);
    }

    setCountryName(product: FormGroup, event: any) {
        product.get('Address.Country.Code').setValue(event.target.selectedOptions[0].text);
    }

    setCountrysName(event: any) {
        this.TicketDetailForm.get('CommonAddress.Country.Code').setValue(event.target.selectedOptions[0].text);
    }

    //StatesListByCountrys() {
    //    this.filterStates(this.TicketDetailForm.get('CommonAddress.Country.Id').value, this.TicketDetailForm.get('Address.CountryGroup.Id').value);
    //}
    
    get StatesListByCountry(): any {
        return (product: FormGroup) => this.filterStates(product.get('Address.Country.Id').value, product.get('Address.CountryGroup.Id').value);
    }

    get StatesListsByCountry(): any {
        return ()=> this.filterStates(this.TicketDetailForm.get('CommonAddress.Country.Id').value, this.TicketDetailForm.get('CommonAddress.CountryGroup.Id').value);
    }

    private filterStates(countryId: number, countryGroupId: number): any[] {
        if (countryGroupId) {
            return this.StateList.filter(t => t.CountryId == countryId && t.CountryGroupId == countryGroupId);
        } else {
            return this.StateList.filter(t => t.CountryId == countryId);
        }
    }

    getBulKPlantAddress(bulkPlantName: string, address: FormGroup) {
        this.addresService.GetBulkPlantDetails(bulkPlantName).subscribe((response: AddressModel) => {
            
            if(response.CountryGroup.Id == 0)
                response.CountryGroup.Id = null;
            
            address.patchValue(response);
        });
    }

    //getAddedBolDetails(boldetails: any) {
    //    console.log("lift details:getAddedBolDetails");
    //    console.log(boldetails);
    //    var newProds = [];
    //    boldetails.map(item => item.Products.map(prod => newProds.push(prod)));       
    //    console.log(newProds);
    //    this.clearValidators(newProds);
    //}

    //clearValidators(products: any[]) {
    //    var _lift = this.TicketDetailForm.get('Products') as FormArray;
    //    products.forEach(function (x) {
    //        var _group = _lift.controls.find(function (y) {
    //            return y.get('ProductId').value == x.ProductId;
    //        });
    //        console.log("liftDetails: groups=>");
    //        console.log(_group.value);
    //        if (_group != undefined && _group != null) {
    //            _group.get('LiftQuantity').clearValidators();
    //            _group.get('LiftQuantity').updateValueAndValidity();
    //            _group.get('BulkPlantName').clearValidators();
    //            _group.get('BulkPlantName').updateValueAndValidity();
    //        }
    //    });
    //}




    // Duplicate lift ticket number validation methods 
    getAddedLiftTickets(items: any) {
        var tktnumbers = items.map(item => item.LiftTicketNumber);
        if (tktnumbers.length > 0 && tktnumbers[0] != null) {
            this.liftticketnumberslist = items.map(item => item.LiftTicketNumber.trim().toLowerCase());
        }
    }
    getDeletedLift(items: any) {
        this.liftticketnumberslist.length = 0;
        this.liftticketnumberslist = items.map(item => item.LiftTicketNumber.trim().toLowerCase());
    }
    getEditedLiftTickets(items: any) {
        //this.curLiftTicketNumber = items.LiftTicketNumber.trim().toLowerCase();
        var tktnumber = items.LiftTicketNumber;
        if (tktnumber != null) {
            this.curLiftTicketNumber = tktnumber.trim().toLowerCase();
        }
    }

    setDeliveredQuantValidation(arr: number[], control: FormControl) {
        let max = Math.max(...arr);
        if (control.value > max)
            this.validationService.addError(control, 'maxQuantity');
        else
            this.validationService.removeError(control, 'maxQuantity');
    }

    //prepopulate the bilable qty set at order level in lift delivered qty text box.
    setLiftDeliveredQuantity(product: FormControl) {
        let quantityIndicatorTypeId = product.get('QuantityIndicatorTypeId').value;
        if (quantityIndicatorTypeId == 1) {
            product.get('DeliveredQuantity').setValue(product.get('NetQuantity').value);
        }
        else if (quantityIndicatorTypeId == 2) {
            product.get('DeliveredQuantity').setValue(product.get('GrossQuantity').value);
        }
    }
}
