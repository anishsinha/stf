import { __decorate } from "tslib";
import { Component, Input } from '@angular/core';
import { Validators } from '@angular/forms';
import { FeeModel } from '../models/DropDetail';
import * as moment from 'moment';
let FeeListComponent = class FeeListComponent {
    constructor(fb, feeService, route) {
        this.fb = fb;
        this.feeService = feeService;
        this.route = route;
        this.CommonFees = [];
        this.OtherFees = [];
        this.SpCommonFees = [];
        this.SpOtherFees = [];
    }
    ngOnInit() {
        this.OrderId = parseInt(this.route.snapshot.queryParamMap.get('orderId'), 10);
        this.Parent.addControl('Fees', this.fb.array([]));
        this.feeService.getFeeTypes(this.OrderId)
            .subscribe(data => { this.FeeTypes = data; });
        this.feeService.getFeeConstraintTypes()
            .subscribe((data) => { this.FeeConstraintTypes = data; });
        this.feeService.getFeeSubTypes(this.OrderId)
            .subscribe((data) => {
            this.FeeSubTypes = data.filter(function (elem) { return elem.FeeSubTypeId != 1; });
        });
    }
    ngOnChanges(change) {
        if (change.Fees && change.Fees.currentValue) {
            this.CommonFees = [];
            this.OtherFees = [];
            this.SpCommonFees = [];
            this.SpOtherFees = [];
            this.Parent.get('Fees').clear();
            var currValues = change.Fees.currentValue;
            currValues.forEach((x) => {
                if (x.FeeConstraintTypeId == null) {
                    this.addGeneralFee(x.CommonFee, x);
                }
                else {
                    this.addSpecialFee(x.CommonFee, x.FeeConstraintTypeId, x);
                }
            });
        }
        if (change.Currency && change.Currency.currentValue) {
            var currency = change.Currency.currentValue;
            this.DisplayCurrency = currency;
        }
    }
    getForm(model) {
        var byQtyModel = model.DeliveryFeeByQuantity;
        var byQuantity = [];
        var _fb = this.fb;
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
    addGeneralFee(_commonFee, feeObj) {
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
        }
        else {
            this.OtherFees.push(feeGroup);
        }
        this.Parent.get('Fees').push(feeGroup);
    }
    removeGeneralFee(_commonFee, fee) {
        var _fees = this.Parent.get('Fees');
        _fees.removeAt(_fees.controls.indexOf(fee));
        if (_commonFee) {
            this.CommonFees.splice(this.CommonFees.indexOf(fee), 1);
        }
        else {
            this.OtherFees.splice(this.OtherFees.indexOf(fee), 1);
        }
    }
    addSpecialFee(_commonFee, typeId, feeObj) {
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
        }
        else {
            this.SpOtherFees.push(feeGroup);
        }
        this.Parent.get('Fees').push(feeGroup);
    }
    removeSpecialFee(_commonFee, fee) {
        var _fees = this.Parent.get('Fees');
        _fees.removeAt(_fees.controls.indexOf(fee));
        if (_commonFee) {
            this.SpCommonFees.splice(this.SpCommonFees.indexOf(fee), 1);
        }
        else {
            this.SpOtherFees.splice(this.SpOtherFees.indexOf(fee), 1);
        }
    }
    addByQtyFee(fee) {
        var _fees = fee.get('DeliveryFeeByQuantity');
        var lastFee = _fees.controls[_fees.controls.length - 1].get('Fee').value;
        var feeObj = new FeeModel();
        feeObj.Fee = lastFee;
        _fees.push(this.getForm(feeObj));
    }
    removeByQtyFee(fee, index) {
        var _fees = fee.get('DeliveryFeeByQuantity');
        _fees.removeAt(index);
    }
    //--------------------------------------------------------------
    isInvalid(drop, name) {
        return drop.get(name).invalid &&
            (drop.get(name).dirty || drop.get(name).touched);
    }
    isRequired(drop, name) {
        return drop.get(name).errors.required;
    }
    isMin(drop, name) {
        return drop.get(name).errors.min;
    }
};
__decorate([
    Input()
], FeeListComponent.prototype, "Parent", void 0);
__decorate([
    Input()
], FeeListComponent.prototype, "Fees", void 0);
__decorate([
    Input()
], FeeListComponent.prototype, "isWaitingForBol", void 0);
__decorate([
    Input()
], FeeListComponent.prototype, "Currency", void 0);
FeeListComponent = __decorate([
    Component({
        selector: 'app-fee-list',
        templateUrl: './fee-list.component.html',
        styleUrls: ['./fee-list.component.css']
    })
], FeeListComponent);
export { FeeListComponent };
function requiredIfValidator(predicate) {
    return (formControl => {
        if (!formControl.parent) {
            return null;
        }
        if (predicate()) {
            return Validators.required(formControl);
        }
        return null;
    });
}
//# sourceMappingURL=fee-list.component.js.map