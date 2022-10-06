import { Component, OnInit, Input, OnChanges, SimpleChanges, Pipe, PipeTransform } from '@angular/core';
import { FormGroup, FormBuilder, FormArray, Validators } from '@angular/forms';
import { FeeModel } from '../../../invoice/models/DropDetail';
import { DropdownItem, DropdownItemExt, FeeSubType } from 'src/app/statelist.service';
import { FeeService } from '../../../invoice/services/fee.service';
import * as moment from 'moment';
import { ActivatedRoute } from '@angular/router';
import { DataService } from 'src/app/services/data.service';

@Component({
  selector: 'app-fee-list',
  templateUrl: './fee-list.component.html',
  styleUrls: ['./fee-list.component.css']
})
export class FeeListComponent implements OnInit {
    @Input() Parent: FormGroup;
    @Input() CountryId: any;
    @Input() public Fees: FeeModel[];

    //---------------Fees GROUP -----------

    public CommonFees: FormGroup[];
    public OtherFees: FormGroup[];
    public SpCommonFees: FormGroup[];
    public SpOtherFees: FormGroup[];

    public FeeTypes: DropdownItemExt[];
    public FeeSubTypes: FeeSubType[];
    public FeeConstraintTypes: DropdownItem[];

    public IsLoading: boolean = false; 
    public DisplayCurrency: any;
    //public OrderId: number;
    constructor(private fb: FormBuilder, private feeService: FeeService, private route: ActivatedRoute, private dataService: DataService) {
        this.CommonFees = [];
        this.OtherFees = [];
        this.SpCommonFees = [];
        this.SpOtherFees = [];
    }

    ngOnInit() {
        //this.OrderId = parseInt(this.route.snapshot.queryParamMap.get('orderId'), 10);
        this.Parent.addControl('Fees', this.fb.array([]));


        this.IsLoading = true
        this.feeService.getFeeTypes(0, true).subscribe(data => {
            this.IsLoading = false
            this.FeeTypes = data;

        });

        this.feeService.getFeeConstraintTypes()
            .subscribe((data: DropdownItem[]) => { this.FeeConstraintTypes = data; });

        this.feeService.getFeeSubTypes(0)
            .subscribe((data: FeeSubType[]) => {
                this.FeeSubTypes = data.filter(function (elem: FeeSubType) { return elem.FeeSubTypeId != 1 });
            });

        this.dataService.RemoveFeesSubject.subscribe(x => {
            this.removeFeesOnCreateNew();
        });
    }

    ngOnChanges(change: SimpleChanges) {
        if (change.CountryId && change.CountryId.currentValue) {
            var currency = change.CountryId.currentValue;
            if (currency == 1) {
                this.DisplayCurrency = "USD";
            } else if (currency == 2) {
                this.DisplayCurrency = "CAD";
            }           
        }

        if (change.Fees && change.Fees.currentValue) {
            this.CommonFees = [];
            this.OtherFees = [];
            this.SpCommonFees = [];
            this.SpOtherFees = [];
          
            let fees = this.Parent.get('Fees') as FormArray;
            if (fees) {
                fees.clear();
            }
            var currValues = change.Fees.currentValue as FeeModel[];
            currValues.forEach((x: FeeModel) => {
                if (x.FeeConstraintTypeId == null) {
                    this.addGeneralFee(x.CommonFee, x);
                } else {
                    this.addSpecialFee(x.CommonFee, x.FeeConstraintTypeId, x);
                }
            });
        }
    }

    getForm(model: FeeModel): FormGroup {
        var byQtyModel = model.DeliveryFeeByQuantity;
        var byQuantity = []; var _fb = this.fb;
        if (byQtyModel != undefined && byQtyModel != null) {
            byQtyModel.forEach(function (elem, idx) {
                byQuantity.push(_fb.group({
                    Currency: _fb.control(elem.Currency),
                    MinQuantity: _fb.control(elem.MinQuantity, [Validators.required]),
                    MaxQuantity: _fb.control(elem.MaxQuantity, [Validators.required]),
                    Fee: _fb.control(elem.Fee, [Validators.required])
                }));
            });
        }

        var _specialDate = '';
        if (model.SpecialDate != null && model.SpecialDate != undefined) {
            _specialDate = moment(model.SpecialDate).format('MM/DD/YYYY');
            _specialDate = _specialDate == '01/01/0001' ? '' : _specialDate;
        }
        var group = this.fb.group({
            OrderId: this.fb.control(model.OrderId),
            Currency: this.fb.control(this.DisplayCurrency),
            TruckLoadType: this.fb.control(model.TruckLoadType),
            TruckLoadCategoryId: this.fb.control(model.TruckLoadCategoryId),
            IncludeInPPG: this.fb.control(model.IncludeInPPG),
            CommonFee: this.fb.control(model.CommonFee),
            FeeConstraintTypeId: this.fb.control(model.FeeConstraintTypeId),
            SpecialDate: this.fb.control(_specialDate),
            FeeTypeId: this.fb.control(model.FeeTypeId, [Validators.required]),
            FeeSubTypeId: this.fb.control(model.FeeSubTypeId, [Validators.required]),
            OtherFeeDescription: this.fb.control(model.OtherFeeDescription),
            MinimumGallons: this.fb.control(model.MinimumGallons),
            Fee: this.fb.control(model.Fee, [Validators.required]),
            DeliveryFeeByQuantity: this.fb.array(byQuantity),
        });
        return group;
    }

    addGeneralFee(_commonFee: boolean, feeObj: FeeModel): void {
        if (feeObj == null) {
            feeObj = new FeeModel();
            feeObj.CommonFee = _commonFee;
        }
        if (!_commonFee && (feeObj.FeeTypeId == undefined || feeObj.FeeTypeId == null || feeObj.FeeTypeId.indexOf('14') < 0)) {
            feeObj.FeeTypeId = '14';
        }
        var feeGroup = this.getForm(feeObj);
        if (_commonFee) {
            this.CommonFees.push(feeGroup);
        } else {
            this.OtherFees.push(feeGroup);
        }
        (this.Parent.get('Fees') as FormArray).push(feeGroup);
    }

    removeGeneralFee(_commonFee: boolean, fee: FormGroup): void {
        var _fees = this.Parent.get('Fees') as FormArray;
        _fees.removeAt(_fees.controls.indexOf(fee));
        if (_commonFee) {
            this.CommonFees.splice(this.CommonFees.indexOf(fee), 1);
        } else {
            this.OtherFees.splice(this.OtherFees.indexOf(fee), 1);
        }
    }

    addSpecialFee(_commonFee: boolean, typeId: number, feeObj: FeeModel): void {
        if (feeObj == null) {
            feeObj = new FeeModel();
            feeObj.CommonFee = _commonFee;
        }
        if (!_commonFee && (feeObj.FeeTypeId == undefined || feeObj.FeeTypeId == null || feeObj.FeeTypeId.indexOf('14') < 0)) {
            feeObj.FeeTypeId = '14';
        }
        feeObj.FeeConstraintTypeId = typeId;
        var feeGroup = this.getForm(feeObj);
        if (_commonFee) {
            this.SpCommonFees.push(feeGroup);
        } else {
            this.SpOtherFees.push(feeGroup);
        }
        (this.Parent.get('Fees') as FormArray).push(feeGroup);
    }

    removeSpecialFee(_commonFee: boolean, fee: FormGroup): void {
        var _fees = this.Parent.get('Fees') as FormArray;
        _fees.removeAt(_fees.controls.indexOf(fee));
        if (_commonFee) {
            this.SpCommonFees.splice(this.SpCommonFees.indexOf(fee), 1);
        } else {
            this.SpOtherFees.splice(this.SpOtherFees.indexOf(fee), 1);
        }
    }

    addByQtyFee(fee: FormGroup): void {
        var _fees = fee.get('DeliveryFeeByQuantity') as FormArray;
        var lastFee = _fees.controls[_fees.controls.length - 1].get('Fee').value;
        var feeObj = new FeeModel(); feeObj.Fee = lastFee;
        _fees.push(this.getForm(feeObj));
    }

    removeByQtyFee(fee: FormGroup, index: number): void {
        var _fees = fee.get('DeliveryFeeByQuantity') as FormArray;
        _fees.removeAt(index);
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
     requiredIfValidator(predicate) {
        return (formControl => {
            if (!formControl.parent) {
                return null;
            }
            if (predicate()) {
                return Validators.required(formControl);
            }
            return null;
        })
    }

    removeFeesOnCreateNew() {
        this.CommonFees.forEach(commonFee => {
            this.removeGeneralFee(true, commonFee);
        })
        this.OtherFees.forEach(OtherFee => {
            this.removeGeneralFee(false, OtherFee);
        })
        this.SpCommonFees.forEach(SpCommonFee => {
            this.removeSpecialFee(true, SpCommonFee);
        })
        this.SpOtherFees.forEach(SpOtherFee => {
            this.removeSpecialFee(false, SpOtherFee);
        })
    }
}
