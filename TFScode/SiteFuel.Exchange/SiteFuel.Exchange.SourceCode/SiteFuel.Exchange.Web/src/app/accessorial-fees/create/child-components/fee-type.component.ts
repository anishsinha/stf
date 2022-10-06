import { Component, OnInit, Input, OnChanges, SimpleChanges } from '@angular/core';
import { FormGroup, FormArray, FormBuilder, Validators } from '@angular/forms';
import { DropdownItem, DropdownItemExt, FeeSubType } from 'src/app/statelist.service';
import { FeeService } from '../../../invoice/services/fee.service';
import { ByQuantityModel } from '../../../invoice/models/DropDetail';
import { RegExConstants } from 'src/app/app.constants';

@Component({
  selector: 'app-fee-type',
  templateUrl: './fee-type.component.html',
  styleUrls: ['./fee-type.component.css']
})
export class FeeTypeComponent implements OnInit {

    @Input() Parent: FormGroup;
    @Input() FeeGroup: FormGroup;

    @Input() Currency: string; // Input property from fee-list component 

    public DisplayCurrency: any;

    //----------------DDL------------------

    @Input() public FeeTypes: DropdownItemExt[];
    @Input() public FeeSubTypes: FeeSubType[];
    @Input() public FeeConstraintTypes: DropdownItem[];
    public DisplayFeeTypes: FeeSubType[];

    public maxDate = new Date();
    public minDate = new Date();


    constructor(private fb: FormBuilder, private feeService: FeeService) {
        this.DisplayFeeTypes = [];
    }

    ngOnInit() {
        this.maxDate.setFullYear(this.maxDate.getFullYear() + 20);
        this.FeeGroup.setValidators(this.feeNameRequired('FeeTypeId', 'OtherFeeDescription', 'CommonFee'));
        if (this.FeeSubTypes != null && this.FeeSubTypes != undefined)
            this.DisplayFeeTypes = this.FeeSubTypes.slice();
        this.DisplayCurrency = this.Currency;
    }

    ngOnChanges(change: SimpleChanges) {
        if (change.FeeSubTypes && change.FeeSubTypes.currentValue != null) {
            var subTypes = change.FeeSubTypes.currentValue as FeeSubType[];
            this.DisplayFeeTypes = subTypes;
        }
    }

    updateSubType(feeTypeId: string): void {
        this.DisplayFeeTypes = this.FeeSubTypes.slice().filter(function (elem) { return elem.FeeTypeId == feeTypeId; });
    }

    getForm(_fee: ByQuantityModel): FormGroup {
        return this.fb.group({
            Currency: this.fb.control(_fee.Currency),
            MinQuantity: this.fb.control(_fee.MinQuantity, [Validators.required, Validators.pattern(RegExConstants.DecimalNumber)]),
            MaxQuantity: this.fb.control(_fee.MaxQuantity, [Validators.pattern(RegExConstants.DecimalNumber), Validators.required]),
            Fee: this.fb.control(_fee.Fee, [Validators.required, Validators.pattern(RegExConstants.DecimalNumber)])
        });
    }

    addByQtyFee(fee: FormGroup, feeObj: ByQuantityModel): void {
        if (feeObj == null) {
            feeObj = new ByQuantityModel();
        }
        var _fees = fee.get('DeliveryFeeByQuantity') as FormArray;
        if (_fees.controls.length > 0) {
            var lastMax = _fees.controls[_fees.controls.length - 1].get('MaxQuantity');
            lastMax.setValidators([Validators.required, Validators.pattern(RegExConstants.DecimalNumber)]);
            feeObj.MinQuantity = lastMax.value;
        }
        var _form = this.getForm(feeObj);

        _fees.push(_form);
    }

    removeByQtyFee(fee: FormGroup, index: number): void {
        var _fees = fee.get('DeliveryFeeByQuantity') as FormArray;
        _fees.removeAt(index);
        if (_fees.controls.length > 0) {
            var lastMax = _fees.controls[_fees.controls.length - 1].get('MaxQuantity');
            lastMax.setValidators([Validators.pattern(RegExConstants.DecimalNumber), Validators.required]);
        }
    }

    isInvalid(group: FormGroup, name: string): boolean {
        return group.get(name).invalid &&
            (group.get(name).dirty || group.get(name).touched || group.get(name).invalid);
    }

    isRequired(group: FormGroup, name: string): boolean {
        return group.get(name).errors.required;
    }

    isFeeNameRequired(group: FormGroup, name: string): boolean {
        return group.get(name).errors.required;
    }

    handleByQuantity(group: FormGroup, subTypeId: number): void {
        var fee = group.get('Fee');
        if (subTypeId == 3) {
            fee.setValue(0);
        } else {
            if (fee.value == 0) {
                fee.setValue('');
            }
            (group.get('DeliveryFeeByQuantity') as FormArray).clear();
        }
    }

    feeNameRequired(field1Name: string, field2Name: string, field3Name: string): any {
        let field1 = this.FeeGroup.controls[field1Name];
        let field2 = this.FeeGroup.controls[field2Name];
        let field3 = this.FeeGroup.controls[field3Name];
        if (field3.value && (field1.value == null || field1.value == '')) {
            return Validators.required(field1);
        } else if (!field3.value && (field2.value == null || field2.value.replace(/\s/g, '') == '')) {
            return Validators.required(field2);
        }
        else {
            return null;
        }
    }
}

