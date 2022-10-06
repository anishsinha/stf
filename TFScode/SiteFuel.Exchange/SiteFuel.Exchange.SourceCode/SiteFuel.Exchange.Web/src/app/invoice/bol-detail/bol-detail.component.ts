import { Component, OnInit, Output, EventEmitter, Input, SimpleChanges } from '@angular/core';
import { FormArray, FormBuilder, Validators, FormGroup, Form, AbstractControl, FormControl } from '@angular/forms';
import { map, tap, debounceTime, distinctUntilChanged, switchMap, catchError } from 'rxjs/operators';
import { Observable, of, from, iif } from 'rxjs';
import { DropdownItem } from 'src/app/statelist.service';
import { DropDetailModel} from '../models/DropDetail';
import { ActivatedRoute } from '@angular/router';
import { InvoiceService } from '../services/invoice.service';
import { Declarations } from 'src/app/declarations.module';
import { UoM } from 'src/app/app.enum';
import { convertTo24Hour, getSeconds, RegExConstants } from 'src/app/app.constants';
import { DataService } from 'src/app/services/data.service';
import { ValidationService } from 'src/app/services/validation.service';

@Component({
    selector: 'app-bol-detail',
    templateUrl: './bol-detail.component.html',
    styleUrls: ['./bol-detail.component.css']
})
export class BolDetailComponent implements OnInit {

    public _opened: boolean = false;
    public _animate: boolean = true;
    public _loading: boolean = false;
    public _loadingTerminals: boolean = false;
    public _positionNum: number = 1;
    public _POSITIONS: Array<string> = ['left', 'right', 'top', 'bottom'];

    public TerminalList: {};
    public Terminals = [];
    public minCharRequired = false;
    public searchError = false;
    public noTerminalFound = false;
    public _dropsForm: any[] = [];
    public BolDetailForm: FormGroup;
    public Drops: DropDetailModel[];
    public EditIndex: number = -1;
    public OrderId: number;

    public hasDuplicateBol: boolean;
    public bolnumberslist = [];
    public curBolNumber: number;
    public UoM: String;
    public Currency: string;
    @Input() public IsBadgeMandatory: boolean;
    public isBadgeMandatory: boolean = false;

    @Output() onBolDetailAdded: EventEmitter<any> = new EventEmitter<any>();
    @Output() onBolDetailUpdated: EventEmitter<any> = new EventEmitter<any>();

    constructor(
        private fb: FormBuilder, 
        private route: ActivatedRoute, 
        private invoiceService: InvoiceService, 
        private dataService: DataService,
        private validationService: ValidationService) {
        this.TerminalList = {};
    }

    ngOnInit() {
         
        this.OrderId = parseInt(this.route.snapshot.queryParamMap.get('orderId'), 10);
        this.BolDetailForm = this.buildForm();
        if (this.BolDetailForm) {
            if (this.isBadgeMandatory) {
                this.BolDetailForm.controls['BadgeNumber'].setValidators([Validators.required]);
            } else {
                this.BolDetailForm.controls['BadgeNumber'].clearValidators();
            }
        } 
    }
    ngOnChanges(change: SimpleChanges) {
        if (change.IsBadgeMandatory && change.IsBadgeMandatory.currentValue != null) {
            this.isBadgeMandatory = change.IsBadgeMandatory.currentValue;
            if (this.BolDetailForm) {
                if (this.isBadgeMandatory) {
                    this.BolDetailForm.controls['BadgeNumber'].setValidators([Validators.required]);
                } else{
                    this.BolDetailForm.controls['BadgeNumber'].clearValidators();
                }
            }          
        }
    }
    initDrops(_dropsForm: any[], _drops: DropDetailModel[]) {
        this._dropsForm = _dropsForm;
        //this.setUoM(_drops);
        this.setCurrency(_drops);
        // this.setProductValidation(_drops);
        if (_drops != null && _drops != undefined) {
            _drops.forEach(x => {
                if (this.TerminalList[x.FuelTypeId] == undefined || this.TerminalList[x.FuelTypeId] == null) {
                    this.TerminalList[x.FuelTypeId] = [];
                }
                this.loadTerminals(x.OrderId, x.FuelTypeId);
            });
            this.Drops = _drops;
            this.setProductsToFormArray();
        }
    }
    setUoM(uom: any, jobCountryId: any, productForm: FormGroup): void{
        if (uom == UoM.MetricTons || uom == UoM.Barrels) {
            if (jobCountryId == 1 || jobCountryId == 4 ) {
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
    //setUoM(drops: DropDetailModel[]): void {
    //    debugger;
    //    //None =0;
    //    //Gallons = 1,
    //    //Litres = 2
    //    if (drops != null && drops != undefined) {
    //        var uom = drops.map(item => parseInt(item.UoM));
    //        if (uom[0] == UoM.Barrels || uom[0] == UoM.MetricTons) {
    //            if (drops[0].JobCountryId == 1) {
    //                this.UoM = "Gallons";
    //            }
    //            else if (drops[0].JobCountryId == 2) {
    //                this.UoM = "Litres";
    //            }

    //        }
    //        else {
    //            if (uom[0] == 1) {
    //                this.UoM = "Gallons";
    //            }
    //            else if (uom[0] == 2) {
    //                this.UoM = "Litres";
    //            }
    //        }
                
    //        //else if (UoM[0] == 3) {
    //        //    this.UoM = "Barrels";
    //        //}
    //        //else if (UoM[0] == 4) {
    //        //    this.UoM = "MT";
    //        //}
    //    }
    //}
    setCurrency(drops: DropDetailModel[]) {
        if (drops != null && drops != undefined) {
            var currency = drops.map(item => parseInt(item.Currency));
        }
        if (currency[0] == 1) {
            this.Currency = "USD"
        }
        else if (currency[0] == 2) {
            this.Currency ="CAD"
        }
    }

    setProductsToFormArray(): void {
        var prodArray = this.BolDetailForm.get('Products') as FormArray;
        if (prodArray != undefined && prodArray != null) {
            prodArray.clear();
            this.BolDetailForm.get('CommonTerminalId').setValue(this.Drops[0].TerminalId);
            this.BolDetailForm.get('CommonTerminalName').setValue(this.Drops[0].TerminalName);          
            var currentObj = this;
            this.Drops.forEach(function (elem, idx) {
                prodArray.push(currentObj.buildProduct(elem));
                if (idx == currentObj.Drops.length - 1) {
                    currentObj.setPrice();
                }
            });
        }
    }

    setPrice() {
        var prodArray = this.BolDetailForm.get('Products') as FormArray;
        var currentObj = this;
        var date = new Date();
        prodArray.controls.forEach(function (_control: FormGroup, index) {
            if (_control instanceof FormGroup) {
                if (_control.controls['TerminalId'].value > 0 && _control.controls['TerminalId'].value && _control.controls['OrderId'].value) {
                    let control = _control;
                    currentObj.invoiceService.getTerminalPriceById(
                        control.controls['TerminalId'].value,
                        control.controls['OrderId'].value,
                        currentObj._dropsForm[index].DropDate || (+date.getMonth() + 1) + '/' + date.getDate() + '/' + date.getFullYear())
                        .subscribe(data => {
                            control.controls['TerminalPrice'].setValue(data);
                        });
                }
            }
        });
    }
    toggleOpen(shouldOpen: boolean, event: any) {
        this._opened = shouldOpen;
        if (shouldOpen) {
            if (event != null) {
                let bolFormData = this.getBolFormData(event.bolDetail);
                this.setProductsToFormArray();
                this.BolDetailForm.patchValue(bolFormData);
                this.EditIndex = event.index;
            }
        } else {
            this.BolDetailForm.reset();
            (this.BolDetailForm.get('Products') as FormArray).clear();
            this.EditIndex = -1;
        }
    }

    getBolFormData(bolDetail: any): any {
        let _products = [];
        bolDetail.Products.forEach(x => {
            _products.push({
                ProductId: x.ProductId,
                ProductName:x.ProductName,
                GrossQuantity: x.GrossQuantity,
                NetQuantity: x.NetQuantity,
                TerminalId: x.TerminalId,
                TerminalName: x.TerminalName,
                DeliveredQuantity : x.DeliveredQuantity,
            });
        });
        let data = {
            BolNumber: bolDetail.BolNumber,
            BadgeNumber: bolDetail.BadgeNumber,
            LiftDate: bolDetail.LiftDate,
            Products: _products,
            LiftStartTime: bolDetail.LiftStartTime,
            LiftEndTime: bolDetail.LiftEndTime,
            CommonTerminalId: bolDetail.CommonTerminalId,
            CommonTerminalName: bolDetail.CommonTerminalName
        };
        return data;
    }

    buildForm() {
        return this.fb.group({
            Id: this.fb.control(''),
            BolNumber: this.fb.control('', Validators.required),
            LiftDate: this.fb.control('', Validators.required),
            BadgeNumber: this.fb.control(''),
            LiftStartTime: this.fb.control('', Validators.required),
            LiftEndTime: this.fb.control('', Validators.required),
            CommonTerminalId: this.fb.control(''),
            CommonTerminalName: this.fb.control(''),
            Products: this.fb.array([]),
        });
    }
    buildProduct(model: DropDetailModel): FormGroup {
        var formG = this.fb.group({
            ProductId: this.fb.control(model.FuelTypeId),
            ProductName: this.fb.control(model.FuelTypeName),
            NetQuantity: this.fb.control('', [Validators.pattern(RegExConstants.Quantity)]),
            GrossQuantity: this.fb.control('', [Validators.pattern(RegExConstants.Quantity)]),
            TerminalId: this.fb.control(model.TerminalId),
            TerminalName: this.fb.control(model.TerminalName, [this.terminalConditionallyRequiredValidator.bind(this)]),
            TerminalPrice: this.fb.control(null),
            DropDate: this.fb.control(model.DropDate),
            OrderId: this.fb.control(model.OrderId),
            Image: this.fb.control(''),
            QuantityIndicatorTypeId: this.fb.control(model.QuantityIndicatorTypeId),
            UOM: this.fb.control(''),
            DeliveredQuantity : this.fb.control('', [Validators.pattern(RegExConstants.Quantity)])
        });
        this.setUoM(model.UoM, model.JobCountryId,formG);
        return formG;
    }

    private terminalConditionallyRequiredValidator(formControl: AbstractControl) {
    if (!formControl.parent) {
        return null;
    }

        if (formControl.parent.get('TerminalId').value) {
        return Validators.required(formControl);
    }
    return null;
}

    cancelBolDetail() {
        this.BolDetailForm.reset();
        this.toggleOpen(false, null);
    }

    addBolDetail() {
        this.BolDetailForm.markAllAsTouched();
        var bolnumber = this.BolDetailForm.get('BolNumber').value.trim().toLowerCase();
        var products = this.BolDetailForm.get('Products').value;
        var prodNumber = products.length;
        var productsArr = this.BolDetailForm.get('Products') as FormArray;
        var commonTermId = this.BolDetailForm.get('CommonTerminalId').value;
        var commonTerminalName = this.BolDetailForm.get('CommonTerminalName').value;
        if (productsArr) {
            productsArr.controls.forEach(function (control) {
                control.get('TerminalId').setValue(commonTermId);
                control.get('TerminalName').setValue(commonTerminalName);
            });
        }
        var count = 0;
        products.forEach(function (product) {
            if (!(product.NetQuantity > 0 && product.GrossQuantity > 0 && product.DeliveredQuantity > 0)) {
                count++;
            }
        });
        if (this.isBadgeMandatory && !this.BolDetailForm.get('BadgeNumber').value.trim()) {
            Declarations.msgerror("Please enter badge number", undefined, undefined);
            return;
        }
        //means no bol details entered
        if (prodNumber == count) {
            Declarations.msgerror("Delivered, gross and net quantity is required for atleast one product", undefined, undefined);
            return;
        }
        var isFormValidInfo = this.validateForm();
        if (isFormValidInfo.length > 0) {
            isFormValidInfo.forEach(function (isFormValidInfo) {
                if (isFormValidInfo.isFormValid == false) {
                    Declarations.msgerror("Delivered/gross/net quantity quantity is missing for product " + isFormValidInfo.ProductName, undefined, undefined);
                }
            });
        }
		else if (this.BolDetailForm.valid && isFormValidInfo.length == 0 && prodNumber != count) 
        {
			if (this.bolnumberslist.indexOf(bolnumber) != -1) {
                Declarations.msgerror("You are trying to enter duplicate BOL Numbers", undefined, undefined);
            }
            else {
                this.onBolDetailAdded.emit(this.BolDetailForm.value);
                this.toggleOpen(false, null);
            }
        }
        else {
            this.BolDetailForm.markAllAsTouched();
        }
    }

    updatedBolDetail() {
        this.BolDetailForm.markAllAsTouched();
        var updatedBolnumber = this.BolDetailForm.get('BolNumber').value.trim().toLowerCase();
        var isFormValidInfo = this.validateForm();
        if (isFormValidInfo.length > 0) {
            isFormValidInfo.forEach(function (isFormValidInfo) {
                if (isFormValidInfo.isFormValid == false) {
                    Declarations.msgerror("Gross or Net quantity is missing for Product " + isFormValidInfo.ProductName, undefined, undefined);
                }
            });
        }
        var productsArr = this.BolDetailForm.get('Products') as FormArray;
        var commonTermId = this.BolDetailForm.get('CommonTerminalId').value;
        var commonTerminalName = this.BolDetailForm.get('CommonTerminalName').value;
        if (productsArr) {
            productsArr.controls.forEach(function (control) {
                control.get('TerminalId').setValue(commonTermId);
                control.get('TerminalName').setValue(commonTerminalName);
            });
        }
		if (this.BolDetailForm.valid && isFormValidInfo.length == 0) {
			if ((this.curBolNumber != updatedBolnumber) && (this.bolnumberslist.indexOf(updatedBolnumber) != -1)) {
                Declarations.msgerror("You are trying to add duplicate Bol numbers", undefined, undefined);
            }
            else {
                var eventData = {
                    bolDetail: this.BolDetailForm.value,
                    index: this.EditIndex
                };
                this.onBolDetailUpdated.emit(eventData);
                this.toggleOpen(false, null);
                this.EditIndex = -1;
                this.bolnumberslist.splice(this.bolnumberslist.indexOf(this.curBolNumber), 1);
                this.bolnumberslist.push(updatedBolnumber);
            }
        }

    }
    isInvalid(name: string): boolean {
        var result = this.BolDetailForm.get(name).invalid
            &&
            (
                this.BolDetailForm.get(name).dirty
                ||
                this.BolDetailForm.get(name).touched
            )
        return result;
    }

    isRequired(name: string): boolean {
        return this.BolDetailForm.get(name).errors.required;
    }

    loadTerminals(_orderId: number, _fuelTypeId: number): void {
        if (this.TerminalList[_fuelTypeId] == undefined
            || this.TerminalList[_fuelTypeId] == null
            || this.TerminalList[_fuelTypeId].length == 0) {
            this._loadingTerminals = true;
            this.invoiceService.getTerminals(_orderId)
                .subscribe((data: DropdownItem[]) => {
                    this._loadingTerminals = false;
                    this.TerminalList[_fuelTypeId] = data;
                });
        }
    }
    onTerminalSearched(event: any,orderId): void {
        var keyword = event.target.value.toLowerCase();
        let searchKeyword$ = of(keyword);
        searchKeyword$.pipe(
            debounceTime(2000),
            distinctUntilChanged(),
            tap((data) => {
                this._loadingTerminals = true
                if (data.length < 3) {
                    this.minCharRequired = true;
                } else {
                    this.minCharRequired = false;
                }
            }),
            switchMap((term) => iif(
                () => (term.length < 3),
                of([])
                , this.invoiceService.getTerminals(orderId, term).pipe(
                    tap(() => {
                        this._loadingTerminals = false;
                    }),
                    catchError(() => {
                        this._loadingTerminals = false;
                        this.searchError = true;
                        return of([]);
                    })
                ))),
            tap(() => this._loadingTerminals = false)
        ).subscribe((data) => {
            if (data.length === 0) {
                this.noTerminalFound = true;
            } else {
                this.noTerminalFound = false;
            }
            this.Terminals = data;
            this._loadingTerminals = false;
        }, (err) => {
            console.log(err);
        });
    }
    getAddedProductDetails(drops: any[]) {
        if (drops && drops.length > 0) {

            let _drops = drops.filter(drop => drop.DropDate && drop.DropDate.length > 0).map(drop => ({ DropDate: new Date(drop.DropDate), StartTime: drop.StartTime, EndTime: drop.EndTime }));

            if (_drops.length > 0) {
                let _minDate = Math.min.apply(null, _drops.map(d => d.DropDate));
                let drop = _drops.find(d => d.DropDate.toDateString() == new Date(_minDate).toDateString());

                if (drop) {
                    let formattedDate = this.getFormattedDate(drop.DropDate);

                    if (formattedDate) {
                        this.BolDetailForm.get('LiftDate').patchValue(formattedDate);
                        this.BolDetailForm.get('LiftStartTime').patchValue(drop.StartTime);
                        this.BolDetailForm.get('LiftEndTime').patchValue(drop.EndTime);
                    }
                }
            }
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
    setTerminalName(product: FormGroup, terminal: any): void {
        if (terminal.Name) {
            product.controls['TerminalName'].patchValue(terminal.Name);
            product.controls['TerminalId'].patchValue(terminal.Id);
            this.setPrice();
            this.dataService.setDropTerminalSelectedSubject(product.get('OrderId').value);
        }
    }
    setCommonTerminalName(OrderId,terminal: any): void {
        if (terminal.Name) {
            this.BolDetailForm.controls['CommonTerminalName'].patchValue(terminal.Name);
            this.BolDetailForm.controls['CommonTerminalId'].patchValue(terminal.Id);
            this.setPrice();
            this.dataService.setDropTerminalSelectedSubject(OrderId); 
        }
    }

    public validateTerminal(product, event, orderId): void {
        if ((this.Terminals.length && !(this.Terminals.filter(e => e.Name === product.controls['TerminalName'].value).length > 0)) || this.noTerminalFound) {
            product.controls['TerminalName'].patchValue('');
        }
        
    }

    public validateCommonTerminal(event, orderId): void {
        if ((this.Terminals.length && !(this.Terminals.filter(e => e.Name === this.BolDetailForm.controls['CommonTerminalName'].value).length > 0)) || this.noTerminalFound) {
            this.BolDetailForm.controls['CommonTerminalName'].patchValue('');
        }
        
    }
    //getAddedLiftDetails(liftdetails: any) {
    //    var newProds = [];
    //    liftdetails.map(item => item.Products.map(prod => newProds.push(prod)));

    //    this.clearValidators(newProds);
    //}
    //clearValidators(products: any[]) {
    //    var _bols = this.BolDetailForm.get('Products') as FormArray;
    //    products.forEach(function (x) {
    //        var _group = _bols.controls.find(function (y) {
    //            return y.get('ProductId').value == x.ProductId;
    //        });
    //        if (_group != undefined && _group != null) {
    //            _group.get('NetQuantity').clearValidators();
    //            _group.get('NetQuantity').updateValueAndValidity();
    //            _group.get('GrossQuantity').clearValidators();
    //            _group.get('GrossQuantity').updateValueAndValidity();
    //        }
    //    });
    //}

    validateForm(): any[] {
        var isFormValid = true;
        var invalidFormInfo = [];
        var products = this.BolDetailForm.get('Products').value;
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

    // bol number duplicate validation 
    getDeletedBol(items: any) {
        this.bolnumberslist.length = 0;
        this.bolnumberslist = items.map(item => item.BolNumber.trim().toLowerCase());
    }
    getEditedBol(items: any) {
        this.curBolNumber = items.BolNumber.trim().toLowerCase();
        //this.bolnumberslist.push(this.curBolNumber);
    }
    getAddedBols(items: any) {
        var bolnumbers = items.map(item => item.BolNumber);
        if (bolnumbers.length > 0 && bolnumbers[0] != null) {
            this.bolnumberslist = items.map(item => item.BolNumber.trim().toLowerCase());
        }
        //this.bolnumberslist = items.map(item => item.BolNumber.trim().toLowerCase());
    }
    setDeliveredQuantValidation(arr: number[], control: FormControl) {
        let max = Math.max(...arr);
        if (control.value > max)
            this.validationService.addError(control, 'maxQuantity');
        else
            this.validationService.removeError(control, 'maxQuantity');
    }

    public ValidateBolTime(LiftstartTime: AbstractControl, LiftEndTime: AbstractControl) {

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

    //prepopulate the bilable qty set at order level in bol delivered qty text box.
    setBolDeliveredQuantity(product : FormControl) {
        let quantityIndicatorTypeId = product.get('QuantityIndicatorTypeId').value;
        if (quantityIndicatorTypeId == 1) {
            product.get('DeliveredQuantity').setValue(product.get('NetQuantity').value);
        }
        else if (quantityIndicatorTypeId == 2) {
            product.get('DeliveredQuantity').setValue(product.get('GrossQuantity').value);
        }
    }
}

