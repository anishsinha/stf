import { Component, OnInit, OnChanges, SimpleChanges, Input, Output, EventEmitter, ViewChild, ElementRef } from '@angular/core';
import { FormGroup, FormBuilder, FormArray, Validators, FormControl, AbstractControl } from '@angular/forms';
import { map, tap, debounceTime, distinctUntilChanged, switchMap, catchError } from 'rxjs/operators';
import { Observable, of, from, iif } from 'rxjs';
import { DropDetailModel, AssetDropModel, AddressModel, BulkPlantAddress, QuantityInfo, MFNConversionRequestViewModel, MFNConversionResponseViewModel, BolDetail, LiftTicketDetail } from '../models/DropDetail';
import { InvoiceService } from '../services/invoice.service';
import { AddressService } from 'src/app/address.service';
import * as moment from 'moment';
import { DropdownItem } from 'src/app/statelist.service';
import { AssetListComponent } from '../asset-list/asset-list.component';
import { FreightPricingMethod, UoM } from 'src/app/app.enum';
import { RegExConstants } from 'src/app/app.constants';
import { DataService } from 'src/app/services/data.service';
import { groupBy } from 'src/app/my.functions';

@Component({
    selector: 'app-produc-detail',
    templateUrl: './produc-detail.component.html',
    styleUrls: ['./produc-detail.component.css']
})
export class ProducDetailComponent implements OnInit, OnChanges {

    @Input() public invoiceForm: FormGroup;
    @Input() public Drops: DropDetailModel[];
    //@Input() public AssetDrops: AssetDropModel[];
    @Output() public onBulkplantUpdated: EventEmitter<any> = new EventEmitter<any>();
    @Output() public onBulkplantPickupReceived: EventEmitter<any> = new EventEmitter<any>();
    @Output() public OnScheduleReceived: EventEmitter<any> = new EventEmitter<any>();

    @Output() public OnBdrDeleted: EventEmitter<any> = new EventEmitter<any>();
    @Output() public onBdrEdit: EventEmitter<any> = new EventEmitter<any>();

    @ViewChild(AssetListComponent) assetListComp: AssetListComponent;
    @ViewChild('actualDropQty') actualDropQty: ElementRef;

    public settings = {};
    public Schedules: {};
    public MinDropDate: Date = new Date();
    public DefaultDate: Date = new Date();
    public IsLoading: boolean = true;
    //
    public TerminalList: {};
    public Terminals = [];
    public minCharRequired = false;
    public searchError = false;
    public noTerminalFound = false;
    public _loadingTerminals: boolean = false;

    public SelectedAssets = {};

    public UoM: string;
    public Currency: string;
    public quantities = new Array<QuantityInfo>();

    public IsConvertingGravity: boolean = false;
    public CalculatedQty: any = '';
    public IsMarineLocation: boolean = false;
    public IsBdrAdded: boolean = false;

    constructor(private fb: FormBuilder, private invoiceService: InvoiceService, private addressService: AddressService, private dataService: DataService) {
        this.TerminalList = {};
    }

    ngOnInit() {
        this.invoiceForm.addControl('Drops', this.fb.array([]));
        this.settings = {
            singleSelection: false,
            idField: 'JobXAssetId',
            textField: 'AssetName',
            selectAllText: 'Select All',
            unSelectAllText: 'UnSelect All',
            itemsShowLimit: 1,
            allowSearchFilter: true
        };
        this.Schedules = {};
        this.dataService.DropTerminalSelectedSubject.subscribe(orderId => {
            this.bolTerminalChange(orderId);
        });
    }

    ngOnChanges(change: SimpleChanges) {
        if (change.Drops && change.Drops.currentValue) {
            var currValues = change.Drops.currentValue as DropDetailModel[];
            var UoM = currValues.map(item => item.UoM);
            this.setUoM(UoM[0]);
            var currency = currValues.map(item => item.Currency);
            this.setCurrency(currency[0]);
            var _drops = this.invoiceForm.get('Drops') as FormArray;
            var _selectedAssets = {};
            currValues.forEach((x: DropDetailModel, idx: number) => {
                _selectedAssets[x.OrderId] = x.Assets.filter(y => y.InvoiceId > 0 && y.OrderId > 0 && y.OrderId == x.OrderId);
                _drops.push(this.getForm(x, _selectedAssets[x.OrderId], UoM[0]));
                this.getSchedules(x.OrderId, x.TrackableScheduleId);
                this.sendBulkplantUpdate(x, x.PickUpAddress);
            });
            this.SelectedAssets = _selectedAssets;
        }
    }

    sendBulkplantUpdate(x: DropDetailModel, address: BulkPlantAddress) {
        if (x.PickupLocationType == 2) {
            var bulkplantInfo = this.bulkPlantMapper(address, x.FuelTypeId, x.FuelTypeName);
            this.onBulkplantPickupReceived.emit(bulkplantInfo);
        }
    }

    getForm(model: DropDetailModel, selectedAssets: AssetDropModel[], uom?: any): FormGroup {
        this.loadTerminals(model.OrderId, model.FuelTypeId);
        this.MinDropDate = moment(model.MinDropDate).toDate();
        var dropDate = model.DisplayDropDate == '01/01/0001' ? '' : model.DisplayDropDate;
        var quantity = model.ActualDropQuantity > 0 ? model.ActualDropQuantity : null;
        let actualQtyUoM = null;
        if (this.invoiceForm && this.invoiceForm.get('IsRebillInvoice').value && model.InvoiceId > 0 && model.UoM == '4' && model.ConversionFactor > 0 && model.ConvertedQuantity > 0) {
            quantity = model.ConvertedQuantity;
            actualQtyUoM = 'MT';
        }
        var group = this.fb.group({
            OrderId: this.fb.control(model.OrderId),
            PoNumber: this.fb.control(model.PoNumber),
            FuelTypeId: this.fb.control(model.FuelTypeId),
            FuelTypeName: this.fb.control(model.FuelTypeName),
            ActualDropQuantity: this.fb.control(quantity, [Validators.required, Validators.min(0.1), Validators.pattern(RegExConstants.Quantity)]),
            DropDate: this.fb.control(dropDate, [Validators.required]),
            StartTime: this.fb.control(model.StartTime, [Validators.required, Validators.pattern(/(?:[0][1-9]|[1][0-2]):(?:[0-5]\d):(?:[0-5]\d) ?([AaPp][Mm])/)]),
            EndTime: this.fb.control(model.EndTime, [Validators.required, Validators.pattern(/(?:[0][1-9]|[1][0-2]):(?:[0-5]\d):(?:[0-5]\d) ?([AaPp][Mm])/)]),
            Allowance: this.fb.control(model.Allowance),
            UserPriceToOverride: this.fb.control(model.UserPriceToOverride),
            MinDropDate: this.fb.control(model.MinDropDate),
            //DisplayMinDropDate: elem.,
            TrackableScheduleId: this.fb.control(model.TrackableScheduleId),
            JobCountryId: this.fb.control(model.JobCountryId),
            TerminalId: this.fb.control(model.TerminalId),
            TerminalName: this.fb.control(model.TerminalName, [this.terminalConditionallyRequiredValidator.bind(this)]),
            PickupLocationType: this.fb.control(model.PickupLocationType),
            IsAssetTracked: this.fb.control(model.IsAssetTracked),
            TerminalPrice: this.fb.control({ value: null, disabled: true }),
            SelectedAssets: selectedAssets,
            Assets: this.fb.array([]),
            Index: this.fb.control((model.Index + 1)),
            BrokerChainId: this.fb.control(model.BrokerChainId),
            IsDipDataRequired: this.fb.control(model.IsDipDataRequired),
            DropEndDate: this.fb.control(dropDate, [Validators.required]),
            Gravity: this.fb.control(model.Gravity),
            ConversionFactor: this.fb.control(model.ConversionFactor),
            ConvertedQuantity: this.fb.control(model.ConvertedQuantity),
            TypeOfFuel: this.fb.control(model.TypeOfFuel),
            BdrDetails: this.fb.group({}),
            IsMarineLocation: this.fb.control(model.IsMarineLocation),
            BlendedScheduleId: this.fb.control(model.BlendedScheduleId),
            DeliveryLevelPO: this.fb.control(model.DeliveryLevelPO),
            ActualQuantityUOM: this.fb.control(null),
        });
        this.IsLoading = false;
        this.setPrice(group, null);
        if (uom != null && uom == UoM.MetricTons || uom == UoM.Barrels) {
            this.setGravityValidatorForMFN(uom, group);

            this.setUoM(this.getUoMFromCountry(model.JobCountryId));

            group.get('ActualQuantityUOM').setValue(actualQtyUoM ?? this.UoM);
        }

        return group;
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
    setTerminalName(drop: FormGroup, terminal: any): void {
        drop.controls['TerminalName'].patchValue(terminal.Name);
        drop.controls['TerminalId'].patchValue(terminal.Id);
        this.setPrice(drop, null);
        if (drop.get('FuelSurchargeFreightFee.FreightPricingMethod').value == FreightPricingMethod.Auto) {          
            this.removeFreightFuelSurchargeForm(drop);      
        }
    }

    bolTerminalChange(orderId: number) {
        let _drops = this.invoiceForm.get('Drops') as FormArray;
        _drops.controls.forEach((drop: FormGroup) => {
            if (drop.get('OrderId').value == orderId) {
                if (drop.get('FuelSurchargeFreightFee.FreightPricingMethod').value == FreightPricingMethod.Auto) {
                    this.removeFreightFuelSurchargeForm(drop);
                }
            }
        })
    }

    removeFreightFuelSurchargeForm(x:any) {
        x.get('FuelSurchargeFreightFee.FreightType').patchValue([]);
        x.get('FuelSurchargeFreightFee.FreightTableType').patchValue([]);
        x.get('FuelSurchargeFreightFee.FreightTableName').patchValue([]);
        x.get('FuelSurchargeFreightFee.Distance').patchValue([]);
        x.get('FuelSurchargeFreightFee.AutoSurchargeFreightCost').patchValue([]);
        x.get('FuelSurchargeFreightFee.SurchargeFreightCost').patchValue([]);
        x.get('FuelSurchargeFreightFee.FuelSurchargeTableType').patchValue([]);
        x.get('FuelSurchargeFreightFee.FuelSurchargeTableName').patchValue([]);
        x.get('FuelSurchargeFreightFee.SurchargeEiaPrice').patchValue([]);
        x.get('FuelSurchargeFreightFee.SurchargePercentage').patchValue([]);
        x.get('FuelSurchargeFreightFee.SurchargeTableRangeStart').patchValue([]);
        x.get('FuelSurchargeFreightFee.SurchargeTableRangeEnd').patchValue([]);   
    }

    onTerminalSearched(event: any, orderId): void {
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
    setPrice(drop: FormGroup, event: any): void {
        var date = new Date();
        if (drop.controls['TerminalId'].value > 0) {
            this.invoiceService.getTerminalPriceById(
                drop.controls['TerminalId'].value,
                drop.controls['OrderId'].value,
                drop.controls['DropDate'].value || (+date.getMonth() + 1) + '/' + date.getDate() + '/' + date.getFullYear())
                .subscribe(data => {
                    drop.controls['TerminalPrice'].setValue(data);
                });
        }
    }

    setEndDate(drop: FormGroup, event: any): void {
        var dropDate = drop.controls['DropDate'].value;
        if (dropDate != '' && dropDate != null) {
            drop.controls['DropEndDate'].setValue(dropDate);
            drop.controls['EndTime'].setValue(null);
        }
    }
    setDropEndDate(drop: FormGroup, $event): void {
        drop.controls['DropEndDate'].setValue($event);
        drop.controls['EndTime'].setValue(null);
        //  drop.controls['StartTime'].setValue(null);
    }
    public validateTerminal(drop, event, orderId): void {
        if ((this.Terminals.length && !(this.Terminals.filter(e => e.Name === drop.controls['TerminalName'].value).length > 0)) || this.noTerminalFound) {
            drop.controls['TerminalName'].patchValue('');
        }
    }

    addProduct(model: DropDetailModel): void {
        var _drops = this.invoiceForm.get('Drops') as FormArray;
        this.setUoM(UoM);
        this.setCurrency(model.Currency);
        _drops.push(this.getForm(model, [], model.UoM));
        this.getSchedules(model.OrderId, 0);
    }

    public ValidateTime(time: any, index: any, controlName: any): void {
        let timeRegex = new RegExp(/(?:[0][1-9]|[1][0-2]):(?:[0-5]\d):(?:[0-5]\d) ?([AaPp][Mm])/);
        if (timeRegex.test(time)) {
        }
        else {
            this.DisplayInvalidTime(time, index, controlName);
        }
    }

    private DisplayInvalidTime(time: any, index: any, controlName: any) {
        var _drops = this.invoiceForm.get('Drops') as FormArray;
        let formControlName: string = controlName;
        _drops.controls[index].get(formControlName).setValue(time);
    }
    removeProduct(model: any): void {
        var removedProductIndex;
        var _drops = this.invoiceForm.get('Drops') as FormArray;
        var _currentObj = this;
        _drops.controls.forEach(function (control, idx) {
            if (control.get('OrderId').value == model.OrderId) {
                removedProductIndex = idx;
                delete _currentObj.Schedules[model.OrderId];
            }
        });
        if (removedProductIndex >= 0) {
            _drops.removeAt(removedProductIndex);
        }
    }

    onScheduleChange(event: any, orderId: number, productId: number, productName: string, drop: FormGroup): void {
        var scheduleId = event.target.selectedOptions[0].value;
        this.scheduleChangeEvent(orderId, scheduleId, drop);

        if (scheduleId == undefined || scheduleId == null || scheduleId == "null")
            return;

        this.addressService.getBulkplantAddress(scheduleId, orderId)
            .subscribe(data => {
                if (data && data.IsAddressAvailable) {
                    var bulkplantInfo = this.bulkPlantMapper(data, productId, productName);
                    this.onBulkplantUpdated.emit(bulkplantInfo);
                }
            });
    }

    private scheduleChangeEvent(orderId: number, scheduleId: any, drop: FormGroup) {
        var selectedSchedule = this.Schedules[orderId].find(t => t.Id == scheduleId);
        let blendedScheduleId = selectedSchedule && selectedSchedule.Code ? selectedSchedule.Code : '';
        let deliveryLevelPO = selectedSchedule && selectedSchedule.DeliveryLevelPO ? selectedSchedule.DeliveryLevelPO : '';
        if (drop) {
            
            if (selectedSchedule) {
                drop.controls['DeliveryLevelPO'].setValue(deliveryLevelPO);
                
            }
            else {
                drop.controls['DeliveryLevelPO'].setValue('');
            }
        }
        this.OnScheduleReceived.emit({
            ScheduleId: scheduleId,
            OrderId: orderId,
            BlendedScheduleId: blendedScheduleId
        });
    }

    bulkPlantMapper(data: any, productId: number, productName: string): any {
        data.Country.Code == 'US' || data.Country.Code == 'USA' ? data.Country.Code = 'USA' : data.Country.Code = 'CAN';
        return {
            ProductId: productId,
            ProductName: productName,
            BulkPlantId: data.SiteId,
            BulkPlantName: data.SiteName,
            Address: {
                Address: data.Address,
                City: data.City,
                CountyName: data.CountyName,
                State: {
                    Id: data.State.Id,
                    Code: data.State.Code
                },
                Country: {
                    Id: data.Country.Id,
                    Code: data.Country.Code
                },
                ZipCode: data.ZipCode,
                Latitude: data.Latitude,
                Longitude: data.Longitude
            }
        };
    }

    getSchedules(_orderId: number, _trackableScheduleId: number) {
        if (this.Schedules['' + _orderId] == undefined) {
            this.invoiceService.getSchedules(_orderId)
                .subscribe(data => {
                    this.Schedules['' + _orderId] = data;
                    if (_trackableScheduleId && _trackableScheduleId > 0) {
                        this.scheduleChangeEvent(_orderId, _trackableScheduleId, null);
                    }
                });
        }
    }

    getUoMFromCountry(countryId: number) {
        if (countryId == 2) {
            return countryId;
        }
        else {
            return 1;
        }

    }

    setUoM(UoM: any): void {
        //None =0;
        //Gallons = 1,
        //Litres = 2
        if (UoM == 1) {
            this.UoM = "Gallons";
        }
        else if (UoM == 2) {
            this.UoM = "Litres";
        }
        //else if (UoM == 3) {
        //    this.UoM = "Barrels";
        //}
        //else if (UoM == 4) {
        //    this.UoM = "MT";
        //}
    }

    setCurrency(Currency: any): void {
        //None = 0,
        //USD = 1,
        //CAD = 2
        if (Currency == 1) {
            this.Currency = "USD";
        }
        else if (Currency == 2) {
            this.Currency = "CAD";
        }
    }
    setGravityValidatorForMFN(uom: any, dropFg: FormGroup) {
        if (uom == UoM.MetricTons && dropFg != null && dropFg != undefined) {
            if (dropFg.controls.ConversionFactor.value) {
                this.OnUomChange('MT', dropFg);
            } else {
                this.OnUomChange(this.UoM, dropFg);
            }
            // dropFg.controls['Gravity'].setValidators([Validators.required, Validators.pattern(RegexPatterns.QuantityPattern)]);
            //     dropFg.controls['Gravity'].updateValueAndValidity();
        }
    }
    IsDisplayGravity(drop: any) {
        if (drop != null && drop.UoM == UoM.MetricTons) {
            return true;
        }
        return false;
    }

    ValidateGravityAndConvertForMFN(drop: FormGroup, dropModel: any) {
        if (dropModel != null && (dropModel.UoM == UoM.MetricTons || dropModel.UoM == UoM.Barrels)) {
            if (dropModel.UoM == UoM.MetricTons) {
                if (drop.get('Gravity').valid) {
                    let inputGravity = drop.get('Gravity').value;
                    let actualDropQty = drop.get('ActualDropQuantity').value;
                    // serverside api to validate gravity 
                    if ((inputGravity != "" && inputGravity != null && inputGravity != undefined)
                        && (actualDropQty != '' && actualDropQty != null && actualDropQty != undefined)) {
                        let gravity = Number(inputGravity);
                        let dropQty = Number(actualDropQty);
                        if (!isNaN(gravity) && !isNaN(dropQty)) {
                            if (dropQty > 0) {
                                let conversionRequest = new MFNConversionRequestViewModel();
                                conversionRequest.ConversionFactor = gravity;
                                conversionRequest.DroppedGallons = dropQty;
                                conversionRequest.JobCountryId = dropModel.JobCountryId;
                                conversionRequest.UoM = UoM.MetricTons;
                                this.invoiceService.ValidateGravityAndConvertForMFN(conversionRequest).subscribe((response: MFNConversionResponseViewModel) => {
                                    if (response.IsValidGravity && response.ConvertedQty > 0 && response.UoM == UoM.MetricTons) {
                                        drop.controls['Gravity'].setErrors(null);
                                        drop.get('ConvertedQuantity').setValue(response.ConvertedQty.toFixed(2));
                                    }
                                    else if (!response.IsValidGravity) {
                                        drop.controls['Gravity'].setErrors({ invalidGravity: true });
                                        drop.get('Gravity').markAsTouched();
                                        drop.get('ConvertedQuantity').setValue('');
                                    }
                                });

                            }
                            else { drop.get('ConvertedQuantity').setValue(''); }
                        } else { drop.get('ConvertedQuantity').setValue(''); }
                    } else { drop.get('ConvertedQuantity').setValue(''); }
                } else {
                    drop.get('ConvertedQuantity').setValue('');
                    drop.get('Gravity').markAsTouched();
                }
            }
            else if (dropModel.UoM == UoM.Barrels) {
                let actualDropQty = Number(drop.get('ActualDropQuantity').value);
                if (!isNaN(actualDropQty)) {
                    if (actualDropQty > 0) {
                        let conversionRequest = new MFNConversionRequestViewModel();
                        conversionRequest.ConversionFactor = 42;
                        conversionRequest.DroppedGallons = actualDropQty;
                        conversionRequest.JobCountryId = dropModel.JobCountryId;
                        conversionRequest.UoM = UoM.Barrels;
                        this.invoiceService.ValidateGravityAndConvertForMFN(conversionRequest).subscribe((response: MFNConversionResponseViewModel) => {
                            if (response.ConvertedQty > 0 && response.UoM == UoM.Barrels) {
                                drop.get('ConvertedQuantity').setValue(response.ConvertedQty.toFixed(2));
                            }
                            else { drop.get('ConvertedQuantity').setValue(''); }

                        });
                    }
                    else { drop.get('ConvertedQuantity').setValue(''); }

                } else { drop.get('ConvertedQuantity').setValue(''); }
            }

        }
    }

    //gets called after bol or lift ticket are added
    getBolLiftQuantities() {
        this.calculateQuantities();
    }


    calculateQuantities() {
        var newProds = [];
        var boldetails = this.invoiceForm.get('BolDetails').value;
        var ticketdetails = this.invoiceForm.get('TicketDetails').value;
        if (boldetails) {
            boldetails.map(item => item.Products.map(prod => newProds.push(prod)));
        }
        if (ticketdetails) {
            ticketdetails.map(item => item.Products.map(prod => newProds.push(prod)));
        }
        var _drops = this.invoiceForm.get('Drops') as FormArray;
        this.quantities.length = 0; //Empty the array 
        var products = [];
        for (var i = 0; i < newProds.length; i++) {
            var newprod = newProds[i];
            products.push({
                ProductId: newprod.ProductId,
                Quantity: newprod.DeliveredQuantity
            });
        }
        const groupedProducts = groupBy(products, 'ProductId');
        // collection returned by groupby() is [key :value] as array of [{key: prodId value:quantity}]
        for (const productId in groupedProducts) {
            var totaldroppedquantity = [];
            var key = productId;
            var values = [];
            values = groupedProducts[key];
            values.forEach(function (value) {
                totaldroppedquantity.push(parseFloat(value.Quantity));
            });

            const Totalsumperproduct = totaldroppedquantity.reduce((a, b) => a + b, 0);

            this.quantities.push({
                ProductId: parseInt(key, 10),
                TotalDroppedQuantity: parseFloat(Number(Totalsumperproduct).toFixed(6))
            });
        }
        var currObj = this;
        this.quantities.forEach(function (x) {
            var _group = _drops.controls.find(function (y) { return y.get("FuelTypeId").value == x.ProductId; });
            if (_group != undefined && _group != null) {
                if (!isNaN(x.TotalDroppedQuantity)) {
                    _group.get('ActualDropQuantity').setValue(x.TotalDroppedQuantity);
                    currObj.actualDropQty.nativeElement.dispatchEvent(new Event('input'));
                }
            }
        });
        //Implies no bol details are added so reset the actualdropquantity form control

        if (this.quantities.length == 0) {
            var _drops = this.invoiceForm.get('Drops') as FormArray;
            var _dropQuantityFormControls = _drops.controls;

            if (_dropQuantityFormControls != undefined && _dropQuantityFormControls != null) {
                _dropQuantityFormControls.forEach(function (formControl) {
                    formControl.get('ActualDropQuantity').reset();
                    currObj.actualDropQty.nativeElement.dispatchEvent(new Event('input'));
                });
            }
        }
    }

    IsBolOrTicketExistForProduct(ProductId: number) {
        if (this.invoiceForm.get('BolDetails').value.some(b => b.Products.some(p => p.ProductId == ProductId && p.DeliveredQuantity > 0)))
            return true;
        else if (this.invoiceForm.get('TicketDetails').value.some(b => b.Products.some(p => p.ProductId == ProductId && p.DeliveredQuantity > 0)))
            return true;
        return false;
    }

    assetDropGallonsChanged(drop: FormGroup) {

        if (!this.IsBolOrTicketExistForProduct(drop.get('FuelTypeId').value)) {

            let sum: number = drop.get('Assets').value.reduce((a, b) => +a + +b.DropGallons, 0);

            if (!isNaN(sum)) 
                drop.get('ActualDropQuantity').setValue(sum);
        }
    }
    // called from child compo - asset-list
    // updateConvertedQty(updatedQty: any) {
    //     if (updatedQty != null && updatedQty != undefined && updatedQty != '') {
    //         this.actualDropQty.nativeElement.dispatchEvent(new Event('input'));
    //     }
    // }


    public ValidateAssetTime(dropForm: FormGroup) {
        this.assetListComp.ValidateAssetTime(null, 'productDetail', dropForm);
    }

    public toggleAddBdrButton(dropIndex: number, isAdded: boolean) {
        if (dropIndex != null && dropIndex != undefined && dropIndex >= 0) {
            this.Drops[dropIndex].IsBdrDetailsAdded = isAdded;
        }

    }

    //--------------------------------------------------------------

    isInvalid(drop: FormGroup, name: string): boolean {
        return drop.get(name).invalid &&
            (drop.get(name).dirty || drop.get(name).touched);
    }

    isRequired(drop: FormGroup, name: string): boolean {
        return drop.get(name).errors.required;
    }

    isMin(drop: FormGroup, name: string): boolean {
        return drop.get(name).errors.min;
    }
    //isPattern(drop: FormGroup, name: string): boolean {
    //    return drop.get(name).errors.pattern;
    //}

    public OnUomChange(uom: string, drop: FormGroup) {
        if (uom == 'MT') {
            drop.controls['ConversionFactor'].setValidators([Validators.required, Validators.pattern(RegExConstants.Quantity)]);
            drop.controls['ConversionFactor'].updateValueAndValidity();
            drop.controls['Gravity'].clearValidators();
            drop.controls['Gravity'].updateValueAndValidity();
            drop.get('ConvertedQuantity').setValue('');
            if (!drop.controls['ConversionFactor'].value)
                drop.get('ConversionFactor').setValue(318);
            drop.get('Gravity').setValue('');
            drop.controls.Gravity.disable();
            drop.controls.ConversionFactor.enable();
        }
        else {
            drop.controls['Gravity'].setValidators([Validators.required, Validators.pattern(RegExConstants.Quantity)]);
            drop.controls['Gravity'].updateValueAndValidity();
            drop.controls['ConversionFactor'].clearValidators();
            drop.controls['ConversionFactor'].updateValueAndValidity();
            drop.get('ConversionFactor').setValue('');
            drop.controls.ConversionFactor.disable();
            drop.controls.Gravity.enable();

            this.calculateQuantities()
        }
    }
}
