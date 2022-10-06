import { __decorate } from "tslib";
import { Component, Input } from '@angular/core';
import { Validators } from '@angular/forms';
import { ByQuantityModel } from '../models/DropDetail';
let FeeTypeComponent = class FeeTypeComponent {
    constructor(fb, feeService) {
        this.fb = fb;
        this.feeService = feeService;
        this.DisplayFeeTypes = [];
    }
    ngOnInit() {
        this.FeeGroup.setValidators(this.feeNameRequired('FeeTypeId', 'OtherFeeDescription', 'CommonFee'));
        if (this.FeeSubTypes != null && this.FeeSubTypes != undefined)
            this.DisplayFeeTypes = this.FeeSubTypes.slice();
        this.DisplayCurrency = this.Currency;
    }
    ngOnChanges(change) {
        if (change.FeeSubTypes && change.FeeSubTypes.currentValue != null) {
            var subTypes = change.FeeSubTypes.currentValue;
            this.DisplayFeeTypes = subTypes;
        }
    }
    updateSubType(feeTypeId) {
        this.DisplayFeeTypes = this.FeeSubTypes.slice().filter(function (elem) { return elem.FeeTypeId == feeTypeId; });
    }
    getForm(_fee) {
        return this.fb.group({
            Currency: this.fb.control(_fee.Currency),
            MinQuantity: this.fb.control(_fee.MinQuantity, [Validators.required, Validators.pattern(/^([0-9]\d*(\.\d+)?)$/)]),
            MaxQuantity: this.fb.control(_fee.MaxQuantity, [Validators.pattern(/^([0-9]\d*(\.\d+)?)$/), Validators.required]),
            Fee: this.fb.control(_fee.Fee, [Validators.required, Validators.pattern(/^([0-9]\d*(\.\d+)?)$/)])
        });
    }
    addByQtyFee(fee, feeObj) {
        if (feeObj == null) {
            feeObj = new ByQuantityModel();
        }
        var _fees = fee.get('DeliveryFeeByQuantity');
        if (_fees.controls.length > 0) {
            var lastMax = _fees.controls[_fees.controls.length - 1].get('MaxQuantity');
            lastMax.setValidators([Validators.required, Validators.pattern(/^([0-9]\d*(\.\d+)?)$/)]);
            feeObj.MinQuantity = lastMax.value;
        }
        var _form = this.getForm(feeObj);
        _fees.push(_form);
    }
    removeByQtyFee(fee, index) {
        var _fees = fee.get('DeliveryFeeByQuantity');
        _fees.removeAt(index);
        if (_fees.controls.length > 0) {
            var lastMax = _fees.controls[_fees.controls.length - 1].get('MaxQuantity');
            lastMax.setValidators([Validators.pattern(/^([0-9]\d*(\.\d+)?)$/), Validators.required]);
        }
    }
    isInvalid(group, name) {
        return group.get(name).invalid &&
            (group.get(name).dirty || group.get(name).touched || group.get(name).invalid);
    }
    isRequired(group, name) {
        return group.get(name).errors.required;
    }
    isFeeNameRequired(group, name) {
        return group.get(name).errors.required;
    }
    handleByQuantity(group, subTypeId) {
        var fee = group.get('Fee');
        if (subTypeId == 3) {
            fee.setValue(0);
        }
        else {
            if (fee.value == 0) {
                fee.setValue('');
            }
            group.get('DeliveryFeeByQuantity').clear();
        }
    }
    feeNameRequired(field1Name, field2Name, field3Name) {
        let field1 = this.FeeGroup.controls[field1Name];
        let field2 = this.FeeGroup.controls[field2Name];
        let field3 = this.FeeGroup.controls[field3Name];
        if (field3.value && (field1.value == null || field1.value == '')) {
            return Validators.required(field1);
        }
        else if (!field3.value && (field2.value == null || field2.value.replace(/\s/g, '') == '')) {
            return Validators.required(field2);
        }
        else {
            return null;
        }
    }
};
__decorate([
    Input()
], FeeTypeComponent.prototype, "Parent", void 0);
__decorate([
    Input()
], FeeTypeComponent.prototype, "FeeGroup", void 0);
__decorate([
    Input()
], FeeTypeComponent.prototype, "Currency", void 0);
__decorate([
    Input()
], FeeTypeComponent.prototype, "FeeTypes", void 0);
__decorate([
    Input()
], FeeTypeComponent.prototype, "FeeSubTypes", void 0);
__decorate([
    Input()
], FeeTypeComponent.prototype, "FeeConstraintTypes", void 0);
FeeTypeComponent = __decorate([
    Component({
        selector: 'app-fee-type',
        templateUrl: './fee-type.component.html',
        styleUrls: ['./fee-type.component.css']
    })
], FeeTypeComponent);
export { FeeTypeComponent };
//# sourceMappingURL=fee-type.component.js.map