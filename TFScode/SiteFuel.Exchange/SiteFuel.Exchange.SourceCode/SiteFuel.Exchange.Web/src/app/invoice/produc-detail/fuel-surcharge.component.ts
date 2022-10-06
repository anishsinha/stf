import { Component, OnInit, Input, OnChanges, SimpleChanges } from '@angular/core';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { BulkPlantAddress, FreightCostInputViewModel, FreightTableNameModel, SurchargeModel } from '../models/DropDetail';
import * as moment from 'moment';
import { InvoiceService } from '../services/invoice.service';
import { DropdownItem } from '../../statelist.service';
import { FreightPricingMethod, FreightRateRuleType} from '../../app.enum';
import { debounceTime, distinctUntilChanged } from 'rxjs/operators';
//import { FeeService } from '../services/fee.service';

@Component({
    selector: 'app-fuel-surcharge',
    templateUrl: './fuel-surcharge.component.html',
    styleUrls: ['./fuel-surcharge.component.css']
})
export class FuelSurchargeComponent implements OnInit {
    @Input() public Parent: FormGroup;
    @Input() public Model: SurchargeModel;
    @Input() public SurchargeEnabled: boolean = false;
    @Input() public PickUpAddress: BulkPlantAddress;
    public SurchargeForm: FormGroup;
    public LoadingPrice: boolean = false;
    private dropQuantity: number = 0;
    public FuelSurchargeTableTypeList: DropdownItem[];
    public FuelSurchargeTableNameList: DropdownItem[];
    public FreightTypeList: DropdownItem[];
    public FreightTableTypeList: DropdownItem[];
    public FreightTableNameList: DropdownItem[];
    public Startdate: any;
    public IsSurchargeApplicable: boolean;
    public IsFreightCostApplicable: boolean;
    public ActualDropQuantity: number;
    public SingleSelectSettingsById = {};
    public IsDistanceVisible: boolean = true;
    public IsFreightTableTypeVisible: boolean = true;
    public IsFrieghtPricingMethodAuto: boolean = false;
    public CheckBoxFreightCost: boolean;
    public CheckBoxFuelSurcharge: boolean;
    private dateRegex = new RegExp(/^(0?[1-9]|1[012])[\/\-](0?[1-9]|[12][0-9]|3[01])[\/\-]\d{4}$/);

    constructor(private fb: FormBuilder, private invoiceService: InvoiceService) { }

    ngOnInit() {
        this.SurchargeForm = this.buildForm();
        this.Parent.addControl('FuelSurchargeFreightFee', this.SurchargeForm);
        if (this.Model != undefined && this.Model != null) {
            this.SurchargeForm.patchValue(this.Model); 
            this.setDefaultDetail();
            this.getInvoiceType();          
        }     
        this.Parent.controls['ActualDropQuantity'].valueChanges.pipe(debounceTime(2000), distinctUntilChanged()).subscribe(qty => {           
            if (qty > 0) {
                this.dropQuantity = qty;
                this.calculateSurcharge(qty);
                this.ActualDropQuantity = qty;            
            }       
            let selectedFreightType = this.SurchargeForm.get('FreightTableName').value;
            if (selectedFreightType.length != 0 && selectedFreightType != null && selectedFreightType != undefined ) {
                this.getFreightCostForAutoInvoice();
            }
        });
        this.SurchargeForm.get('AutoFreightDistance').setValue('');
        this.SurchargeForm.controls['AutoFreightDistance'].valueChanges.pipe(debounceTime(500), distinctUntilChanged()).subscribe(distance => {
            let selectedFreightType = this.SurchargeForm.get('FreightTableName').value
            if (distance > 0 && selectedFreightType.length != 0 && selectedFreightType != null && selectedFreightType != undefined) {
                this.getFreightCostForAutoInvoice();
                this.calculateSurcharge(this.dropQuantity);       
            }
            if (distance == null || distance == 0) {
                this.SurchargeForm.get('SurchargeFreightCost').setValue('');
                this.SurchargeForm.get('AutoSurchargeFreightCost').setValue('');
                this.SurchargeForm.get('AutoFreightDistance').setValue('');
            }
        });
        this.SurchargeForm.controls['AutoSurchargeFreightCost'].valueChanges.pipe(debounceTime(1000), distinctUntilChanged()).subscribe(cost => {
            if (cost > 0) {
                this.SurchargeForm.get('SurchargeFreightCost').setValue(cost);
                this.calculateSurcharge(this.dropQuantity);
            }
        });
        this.Parent.controls['DropDate'].valueChanges.subscribe(date => {
            this.Startdate = date;
            if (this.dateRegex.test(date) && this.SurchargeEnabled) {
                if (this.IsFrieghtPricingMethodAuto == false) {
                    this.getEiaPriceForDate(date);
                } else {
                    this.getEiaPriceAutoInvoice(date);
                }
            }
        });
        var surchargeForm = this.Parent.get('FuelSurchargeFreightFee') as FormGroup;
        if (surchargeForm != undefined && surchargeForm != null) {
            surchargeForm.get('Distance').valueChanges.subscribe(distance => {
                if (distance > 0 && this.SurchargeEnabled) {
                    this.getEiaPriceForDate(distance);
                }
            });
        }  
        this.SurchargeForm.get('IsSurchargeApplicable').setValue(this.Model.IsSurchargeApplicable);
        this.IsSurchargeApplicable = this.Model.IsSurchargeApplicable ;     
        this.CheckBoxFuelSurcharge = this.IsSurchargeApplicable;

        this.SurchargeForm.get('IsFreightCostApplicable').setValue(this.Model.IsFreightCostApplicable);
        this.IsFreightCostApplicable = this.Model.IsFreightCostApplicable;
        this.CheckBoxFreightCost = this.Model.IsFreightCostApplicable;

        if (this.SurchargeForm.get('FreightPricingMethod').value == 2 && this.SurchargeForm.get('IsFreightCostApplicable').value == false) {
            this.SurchargeForm.get('IsSurchargeApplicable').setValue(false);
        }

        this.SingleSelectSettingsById = {
            singleSelection: true,
            closeDropDownOnSelection: true,
            idField: 'Id',
            textField: 'Name',
            enableCheckAll: false,
            selectAllText: 'Select All',
            unSelectAllText: 'UnSelect All',
            itemsShowLimit: 1,
        };
        this.GetFreightType();
        this.getTableTypes();        
    }

    buildForm(): FormGroup {
        return this.fb.group({
            IsFreightCostApplicable: this.fb.control(''),
            IsSurchargeApplicable: this.fb.control(''),
            SurchargePricingType: this.fb.control(''),
            SurchargeProductType: this.fb.control(''),
            FeeSubTypeId: this.fb.control(''),
            SurchargeFreightCost: this.fb.control(''),
            SurchargePercentage: this.fb.control(''),
            SurchargeEiaPrice: this.fb.control(''),
            SurchargeTableRangeStart: this.fb.control(''),
            SurchargeTableRangeEnd: this.fb.control(''),
            IsFeeByDistance: this.fb.control(''),
            TotalFuelSurchargeFee: this.fb.control(''),
            Distance: this.fb.control(''),
            FuelSurchargeTableType: this.fb.control(''),
            FuelSurchargeTableName: this.fb.control(''),
            FreightType: this.fb.control(''),
            FreightTableType: this.fb.control(''),
            FreightTableName: this.fb.control(''),
            AutoFreightDistance: this.fb.control(''),
            AutoSurchargeFreightCost: this.fb.control(''),  
            FreightPricingMethod: this.fb.control('')  
        });     
    }

    public OncheckBoxChange(event) {
        if (event.target.checked == false) {
            this.SurchargeForm.get('IsSurchargeApplicable').setValue(false);
        }
        this.validateFreight();
    }
 
    public OnChangeSurchargeApplicable(event) {
        this.validateFuelSurcharge();
    }

    calculateSurcharge(quantity: number): void {
        var percent = this.SurchargeForm.get('SurchargePercentage').value;
        var freightCost = this.SurchargeForm.get('SurchargeFreightCost').value;

        var surchargePartial = (parseFloat(percent) / 100) * parseFloat(freightCost);
        var surchargeFee = parseFloat(surchargePartial.toFixed(4)) * quantity;
        if (surchargeFee > 0) {
            this.SurchargeForm.get('TotalFuelSurchargeFee').setValue(surchargeFee.toFixed(4));
        }
    }

    getEiaPriceForDate(date: Date): void {
        this.LoadingPrice = true;
        var _date = moment(date).format('MM/DD/YYYY');
        var EIAdata = {
            buyerCompanyId: this.Model.BuyerCompanyId,
            pricingType: this.SurchargeForm.get('SurchargePricingType').value,
            productType: this.SurchargeForm.get('SurchargeProductType').value,
            deliveryDate: _date
        }
        this.invoiceService.getEiaPrice(EIAdata).subscribe(x => {
            this.LoadingPrice = false;
            if (!isNaN(parseFloat(x.eiaResponse))) {
                this.SurchargeForm.get('SurchargeEiaPrice').setValue(x.eiaResponse);
                this.SurchargeForm.get('SurchargePercentage').setValue(x.percent)
                this.SurchargeForm.get('SurchargeTableRangeStart').setValue(x.start);
                this.SurchargeForm.get('SurchargeTableRangeEnd').setValue(x.end);
                this.calculateSurcharge(this.dropQuantity);
            }
        });
    }    
    public OnFuelSurchargeTableNameSelect() {
        this.getEiaPriceAutoInvoice(this.Startdate);
    }
    public onFuelSurchargeNameDeSelect() {
        this.SurchargeForm.get('SurchargeEiaPrice').patchValue([]);
        this.SurchargeForm.get('SurchargePercentage').patchValue([]);
        this.SurchargeForm.get('SurchargeTableRangeStart').patchValue([]);
        this.SurchargeForm.get('SurchargeTableRangeEnd').patchValue([]);
    }
    public OnFreightTableNameSelect() {
        let selectedFreightType = this.SurchargeForm.get('FreightType').value as DropdownItem[];
        let freightRateRuleTypeIds = selectedFreightType.map(s => s.Id);
        let FreightRateRuleIds = freightRateRuleTypeIds[0];
        if (FreightRateRuleIds == FreightRateRuleType.Range) {
            let distance = this.SurchargeForm.get('AutoFreightDistance').value
            if (distance) {
                this.getFreightCostForAutoInvoice();
                this.calculateSurcharge(this.dropQuantity);
            }
        }
        if (FreightRateRuleIds == FreightRateRuleType.Route) {
            let qty = this.Parent.controls['ActualDropQuantity'].value;
            if (qty) {
                this.getFreightCostForAutoInvoice();
                this.calculateSurcharge(this.dropQuantity);
            }
        }
        if (FreightRateRuleIds == FreightRateRuleType.P2P) {
            this.getFreightCostForAutoInvoice();
        }
    }
    public onFreightTableNameDeSelect() {
        this.SurchargeForm.controls['AutoSurchargeFreightCost'].setValue('');
    }
    private getEiaPriceAutoInvoice(date: Date): void {
        this.LoadingPrice = true;
        var _date;
        if (date) {
            _date = moment(date).format('MM/DD/YYYY');
        } else {
            _date = moment(new Date()).format('MM/DD/YYYY');
        }
        var selectedFuelSurchargeTableName = this.SurchargeForm.get('FuelSurchargeTableName').value as DropdownItem[];
        if (selectedFuelSurchargeTableName) {
            let surchargeTableNameId = selectedFuelSurchargeTableName.map(s => s.Id);
            let EIAdata = {
                deliveryDate: _date,
                fuelSurchargeIndexId: surchargeTableNameId[0]
            };
            this.invoiceService.getEiaPriceAutoFreightMethod(EIAdata).subscribe(x => {
                this.LoadingPrice = false;
                if (x.length != 0 && x) {
                    this.SurchargeForm.get('SurchargeEiaPrice').setValue(x.eiaResponse);
                    this.SurchargeForm.get('SurchargePercentage').setValue(x.percent)
                    this.SurchargeForm.get('SurchargeTableRangeStart').setValue(x.start);
                    this.SurchargeForm.get('SurchargeTableRangeEnd').setValue(x.end);
                    this.calculateSurcharge(this.dropQuantity);
                }
            });
        }
    }

    private getFreightCostForAutoInvoice() {
        var input = new FreightCostInputViewModel();
        if (this.PickUpAddress) {
            input.BulkPlantId = this.PickUpAddress.BulkPlantId;
        } else {
            if (this.Parent) {
                input.TerminalId = this.Parent.get('TerminalId').value;
            }
        }
        if (this.Parent) {            
            input.OrderId = this.Parent.get('OrderId').value;
        }    
        let selectedFreightTableName = this.SurchargeForm.get('FreightTableName').value as DropdownItem[];
        if (selectedFreightTableName) {
            let freightRateRuleId = selectedFreightTableName.map(s => s.Id);
            input.FreightRateRuleId = freightRateRuleId[0];
            input.DeliveredQuantity = this.ActualDropQuantity;
            input.Distance = this.SurchargeForm.get('AutoFreightDistance').value;
            this.invoiceService.GetFreightCostForAutoInvoice(input).subscribe(async (data) => {
                if (data) {
                    var surchargeFreightCost = await (data);
                    this.SurchargeForm.get('SurchargeFreightCost').setValue(surchargeFreightCost);
                    this.SurchargeForm.controls['AutoSurchargeFreightCost'].setValue(surchargeFreightCost);
                }
            });
        }
    }

    public setAutoSurchargeFreightCost(event: any): void {
        this.SurchargeForm.controls['SurchargeFreightCost'].setValue(event.target.value);
    }
        
    public setDistanceBasedFreightCost(distance: number): void {
        if (this.SurchargeForm.get('IsFeeByDistance').value) {
            var feeByQty = this.Model.DeliveryFeeByQuantity.find(function (x) {
                return x.MinQuantity <= distance && x.MaxQuantity >= distance;
            });
            if (feeByQty == undefined && this.Model.DeliveryFeeByQuantity.length > 0) {
                feeByQty = this.Model.DeliveryFeeByQuantity[this.Model.DeliveryFeeByQuantity.length - 1];
            }
            if (feeByQty != undefined) {
                this.SurchargeForm.get('SurchargeFreightCost').setValue(feeByQty.Fee);
            } else {
                this.SurchargeForm.get('SurchargeFreightCost').setValue('');
            }
        }
    }

    private GetFreightType(): void {
        this.invoiceService.getFreightTable().subscribe(async (data) => {
            if (data) {
                this.FreightTypeList = await (data);
            }
        });
    }

    private getTableTypes(): void {
        this.invoiceService.getTableTypes().subscribe(async (data) => {
            if (data) {
                this.FuelSurchargeTableTypeList = await (data);
                this.FuelSurchargeTableTypeList = this.FuelSurchargeTableTypeList.splice(0, 2);
                this.FreightTableTypeList = this.FuelSurchargeTableTypeList;
            }
        });
    }

    //FuelSurcharge
    public OnFuelSurchargeTableTypeSelect(item: any): void {
        this.SurchargeForm.get('FuelSurchargeTableName').patchValue([]);
        switch (item.Id) {
            case 1: //Master
                this.SurchargeForm.controls['FuelSurchargeTableType'].patchValue([{ Id: 1, Name: "Master" }]);
                break;
            case 2: // Customer Specific
                this.SurchargeForm.controls['FuelSurchargeTableType'].patchValue([{ Id: 2, Name: "CustomerSpecific" }]);
                break;
        }
        this.GetFuelSurchargeTableName(null);
    }

    private GetFuelSurchargeTableName(selectedFuelSurchargeId: any): void {
        var input = new FreightTableNameModel();
        var selectedFuelSurchargeTableType = this.SurchargeForm.get('FuelSurchargeTableType').value as DropdownItem[];
        if (parent) {
            input.TerminalId = this.Parent.get('TerminalId').value;
            input.OrderId = this.Parent.get('OrderId').value;
        }
        if (selectedFuelSurchargeTableType != null && selectedFuelSurchargeTableType != undefined && selectedFuelSurchargeTableType.length > 0) {
            var surchargeTableTypeIds = selectedFuelSurchargeTableType.map(s => s.Id);
            input.TableType = surchargeTableTypeIds.join(',');
            this.invoiceService.getFuelSurchargeTableName(input).subscribe(async (data) => {
                this.FuelSurchargeTableNameList = await (data);
                if (selectedFuelSurchargeId && selectedFuelSurchargeId.length != 0 && data) {
                    let FuelSurchargeTableNames = this.FuelSurchargeTableNameList.filter(t => t.Id == selectedFuelSurchargeId);
                    this.SurchargeForm.controls['FuelSurchargeTableName'].patchValue(FuelSurchargeTableNames);
                    this.getEiaPriceAutoInvoice(this.Startdate);
                }
            });
        }
    }

    private setDefaultDetail() {
        
        if (this.Model != null) {
            //FuelSurcharge 
            if (this.Model.FuelSurchargeTableType == 0 || this.Model.FuelSurchargeTableType == undefined) {
                this.SurchargeForm.controls['FuelSurchargeTableType'].patchValue([]);
            } else {
                if (this.Model.FuelSurchargeTableType == 1) {
                    this.SurchargeForm.controls['FuelSurchargeTableType'].patchValue([{ Id: 1, Name: "Master" }]);
                }
                else {
                    this.SurchargeForm.controls['FuelSurchargeTableType'].patchValue([{ Id: 2, Name: "CustomerSpecific" }]);
                }
                this.GetFuelSurchargeTableName(this.Model.FuelSurchargeTableId);
            }
            //FreightCost
            if (this.Model.FreightRateRuleType == 0 || this.Model.FreightRateRuleType == undefined || this.Model.FreightRateRuleType == null) {
                this.SurchargeForm.controls['FreightType'].patchValue([]);
                this.SurchargeForm.controls['FreightTableType'].patchValue([]);
            } else {
                if (this.Model.FreightRateRuleType == 1) {
                    this.SurchargeForm.controls['FreightType'].patchValue([{ Id: 1, Name: "P2P" }]);
                    this.SurchargeForm.controls['FreightTableType'].patchValue([{ Id: 2, Name: "CustomerSpecific" }]);
                    this.GetFreightTableName(null);
                    this.IsFreightTableTypeVisible = false;
                    this.IsDistanceVisible = false;
                }
                else if (this.Model.FreightRateRuleType == 2) {
                    this.SurchargeForm.controls['FreightType'].patchValue([{ Id: 2, Name: "Range" }]);
                    this.IsFreightTableTypeVisible = true;
                    this.IsDistanceVisible = true;
                }
                else {
                    this.SurchargeForm.controls['FreightType'].patchValue([{ Id: 3, Name: "Route" }]);
                    this.IsFreightTableTypeVisible = true;
                    this.IsDistanceVisible = false;
                }

                if (this.Model.FreightRateTableType == 1) {
                    this.SurchargeForm.controls['FreightTableType'].patchValue([{ Id: 1, Name: "Master" }]);
                }
                else {
                    this.SurchargeForm.controls['FreightTableType'].patchValue([{ Id: 2, Name: "CustomerSpecific" }]);
                }
                this.GetFreightTableName(this.Model.FreightRateRuleId);
            }           
        }
        this.validateFreight();
        this.validateFuelSurcharge();
    }

    validateFreight() {
        if (this.SurchargeForm.get('IsFreightCostApplicable').value && this.SurchargeForm.get('FreightPricingMethod').value == FreightPricingMethod.Auto) {

            this.SurchargeForm.get('FreightType').setValidators([Validators.required]);
            this.SurchargeForm.get('FreightType').updateValueAndValidity();

            let selectedFreightType = this.SurchargeForm.get('FreightType').value;
            if (selectedFreightType != null && selectedFreightType != undefined && selectedFreightType.length > 0) {
                var freightRateType = selectedFreightType.map(s => s.Id);

                let value = (freightRateType != FreightRateRuleType.P2P) ? [Validators.required] : [];
                this.SurchargeForm.get('FreightTableType').setValidators(value);
                this.SurchargeForm.get('FreightTableType').updateValueAndValidity();

                let distance = (freightRateType == FreightRateRuleType.Range) ? [Validators.required] : [];
                this.SurchargeForm.get('AutoFreightDistance').setValidators(distance);
                this.SurchargeForm.get('AutoFreightDistance').updateValueAndValidity();

                this.SurchargeForm.get('FreightTableName').setValidators([Validators.required]);
                this.SurchargeForm.get('FreightTableName').updateValueAndValidity();

                this.SurchargeForm.get('AutoSurchargeFreightCost').setValidators([Validators.required]);
                this.SurchargeForm.get('AutoSurchargeFreightCost').updateValueAndValidity();
            }
        } else {
            this.SurchargeForm.get('FreightType').setValidators([]);
            this.SurchargeForm.get('FreightType').updateValueAndValidity();
            this.SurchargeForm.get('FreightTableType').setValidators([]);
            this.SurchargeForm.get('FreightTableType').updateValueAndValidity();
            this.SurchargeForm.get('AutoFreightDistance').setValidators([]);
            this.SurchargeForm.get('AutoFreightDistance').updateValueAndValidity();
            this.SurchargeForm.get('FreightTableName').setValidators([]);
            this.SurchargeForm.get('FreightTableName').updateValueAndValidity();
            this.SurchargeForm.get('AutoSurchargeFreightCost').setValidators([]);
            this.SurchargeForm.get('AutoSurchargeFreightCost').updateValueAndValidity();
            this.SurchargeForm.get('AutoFreightDistance').setValidators([]);
            this.SurchargeForm.get('AutoFreightDistance').updateValueAndValidity();
        }
    } 

    validateFuelSurcharge() {
        if (this.SurchargeForm.get('IsSurchargeApplicable').value && this.SurchargeForm.get('FreightPricingMethod').value == FreightPricingMethod.Auto) {

            this.SurchargeForm.get('FuelSurchargeTableType').setValidators([Validators.required]);
            this.SurchargeForm.get('FuelSurchargeTableType').updateValueAndValidity();
            this.SurchargeForm.get('FuelSurchargeTableName').setValidators([Validators.required]);
            this.SurchargeForm.get('FuelSurchargeTableName').updateValueAndValidity();
        } else {
            this.SurchargeForm.get('FuelSurchargeTableType').setValidators([]);
            this.SurchargeForm.get('FuelSurchargeTableType').updateValueAndValidity();
            this.SurchargeForm.get('FuelSurchargeTableName').setValidators([]);
            this.SurchargeForm.get('FuelSurchargeTableName').updateValueAndValidity();
        }
    }

    //FrightCost
    public OnFreightTypeSelect(item: any): void {
        this.SurchargeForm.get('FreightTableType').patchValue([]);
        this.SurchargeForm.get('FreightTableName').patchValue([]);
        this.FreightTableNameList = [];

        switch (item.Id) {
            case 1: //P2P
                this.SurchargeForm.controls['FreightType'].patchValue([{ Id: 1, Name: "P2P" }]);
                this.SurchargeForm.controls['FreightTableType'].patchValue([{ Id: 2, Name: "CustomerSpecific" }]);
                this.GetFreightTableName(null);
                this.IsFreightTableTypeVisible = false;
                this.IsDistanceVisible = false;
                break;
            case 2: // Range
                this.SurchargeForm.controls['FreightType'].patchValue([{ Id: 2, Name: "Range" }]);
                this.IsFreightTableTypeVisible = true;
                this.IsDistanceVisible = true;
                break;

            case 3: //Route
                this.SurchargeForm.controls['FreightType'].patchValue([{ Id: 3, Name: "Route" }]);
                this.IsFreightTableTypeVisible = true;
                this.IsDistanceVisible = false;
                break;
        }
        this.validateFreight();
    }

    public OnFreightTableTypeSelect(item: any): void {
        this.SurchargeForm.get('FreightTableName').patchValue([]);
        switch (item.Id) {
            case 1: //Master
                this.SurchargeForm.controls['FreightTableType'].patchValue([{ Id: 1, Name: "Master" }]);
                break;
            case 2: // Customer Specific
                this.SurchargeForm.controls['FreightTableType'].patchValue([{ Id: 2, Name: "CustomerSpecific" }]);
                break;
        }
        this.GetFreightTableName(null);
    }
    
    private GetFreightTableName(selectedFreightRateId: any): void {
        var input = new FreightTableNameModel();
        var selectedFreightType = this.SurchargeForm.get('FreightType').value as DropdownItem[];
        var selectedFreightTableType = this.SurchargeForm.get('FreightTableType').value as DropdownItem[];
        this.SurchargeForm.get('SurchargeFreightCost').setValue('');
        this.SurchargeForm.get('AutoSurchargeFreightCost').setValue('');
        if (this.PickUpAddress) {
            input.bulkPlantId = this.PickUpAddress.BulkPlantId;
        }
        if (this.Parent) {
            input.TerminalId = this.Parent.get('TerminalId').value;
            input.OrderId = this.Parent.get('OrderId').value;
        }
        if (selectedFreightType != null && selectedFreightType != undefined && selectedFreightType.length > 0) {
            var freightRateType = selectedFreightType.map(s => s.Id);
            input.FreightRateRuleType = freightRateType.join(',');
        }
        if (selectedFreightTableType != null && selectedFreightTableType != undefined && selectedFreightTableType.length > 0) {
            var freightTableType = selectedFreightTableType.map(s => s.Id);
            input.TableType = freightTableType.join(',');
        }
        if (selectedFreightTableType != null && selectedFreightTableType != undefined && selectedFreightTableType.length > 0) {
            var freightRateRuleTypeIds = selectedFreightTableType.map(s => s.Id);
            input.TableType = freightRateRuleTypeIds.join(',');
            this.invoiceService.getFreightTableName(input).subscribe(async (data) => {
                this.FreightTableNameList = await (data);
                if (selectedFreightRateId && selectedFreightRateId.length != 0 && data) {
                    let FreightTableNames = this.FreightTableNameList.filter(t => t.Id == selectedFreightRateId);
                    this.SurchargeForm.controls['FreightTableName'].patchValue(FreightTableNames);
                }
            })
        }
    }
 
    public onTypeDeSelect(item: any): void {
        let selectedType = this.SurchargeForm.get('FreightType').value as DropdownItem[];
        let selectedFreightTableType = this.SurchargeForm.get('FreightTableType').value as DropdownItem[];
        let selectedFreightTableName = this.SurchargeForm.get('FreightTableName').value as DropdownItem[];
        let selectedFuelTableType = this.SurchargeForm.get('FuelSurchargeTableType').value as DropdownItem[];
        if (selectedType.length == 0) {
            this.IsFreightTableTypeVisible = true;
            this.SurchargeForm.get('FreightType').patchValue([]);
            this.SurchargeForm.get('FreightTableType').patchValue([]);
            this.SurchargeForm.get('FreightTableName').patchValue([]);
            this.FreightTableNameList = [];
            this.SurchargeForm.controls['AutoSurchargeFreightCost'].setValue('');
        }
        if (selectedFreightTableType.length == 0) {
            this.SurchargeForm.get('FreightTableName').patchValue([]);
            this.FreightTableNameList = [];
            this.SurchargeForm.controls['AutoSurchargeFreightCost'].setValue('');
        }
        if (selectedFreightTableName.length  == 0) {
            this.SurchargeForm.controls['AutoSurchargeFreightCost'].setValue('');
        }
        if (selectedFuelTableType.length == 0) {
            this.SurchargeForm.get('FuelSurchargeTableName').patchValue([]);
            this.FuelSurchargeTableNameList = [];
        }
    }
    private getInvoiceType() {
        if (this.Model != null) {
            if (this.Model.FreightPricingMethod == FreightPricingMethod.Manual) {
                this.IsFrieghtPricingMethodAuto = false;
                this.SurchargeForm.get('FreightPricingMethod').setValue(1);
            } else {
                this.IsFrieghtPricingMethodAuto = true;
                this.SurchargeForm.get('FreightPricingMethod').setValue(2);
            }
        }
  }  
}
