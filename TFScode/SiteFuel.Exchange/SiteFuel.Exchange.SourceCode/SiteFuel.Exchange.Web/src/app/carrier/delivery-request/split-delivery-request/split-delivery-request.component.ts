import { Component, OnInit, ChangeDetectionStrategy, ChangeDetectorRef, Output, EventEmitter } from '@angular/core';

import { FormBuilder, FormGroup, Validators, FormControl, FormArray } from '@angular/forms';
import { CarrierService } from '../../service/carrier.service';
import { Declarations } from 'src/app/declarations.module';
import { DeliveryRequestViewModel, SplitBlendDRModel } from '../../models/DispatchSchedulerModels';
import { getUniqueId } from '../../../my.functions';
import { RegExConstants } from 'src/app/app.constants';
declare var jQuery: any;
@Component({
    selector: 'app-split-delivery-request',
    templateUrl: './split-delivery-request.component.html',
    styleUrls: ['./split-delivery-request.component.css'],
})
export class SplitDeliveryRequestComponent implements OnInit {
    constructor(private fb: FormBuilder, private carrierService: CarrierService) { }
    public SplitDRForm: FormGroup;
    public SplitBlendDRForm: FormGroup;
    public defaultSplitDRsValue = 2;
    public productType: string;
    public siteId: string;
    public jobName: string;
    public jobAddress: string;
    public jobCity: string;
    public totalQuantity: number;
    public originalTotalQuantity: number;
    public remainingQuantity: number;
    public UoM: string;
    public IsBlendRequest: boolean = false;
    public validateQtyStatus: boolean = true;
    public validateQtyMessage: string = '';
    public splitDrs: SplitBlendDRModel[] = [];
    public _loading: boolean = false;
    public deliveryLevelPO: string ;
    @Output() onRaiseDR: EventEmitter<any> = new EventEmitter<any>();
    ngOnInit() {
        this.SplitDRForm = this.initForm();
        this.SplitBlendDRForm = this.initSplitBlendDRForm([]);
    }
    public getDeliveryRequestInfo(deliveryReqs: DeliveryRequestViewModel[]) {
        this.IsBlendRequest = deliveryReqs.some(t => t.IsBlendedRequest);
        this.splitDrs = [];
        if (this.IsBlendRequest) {
            deliveryReqs.forEach(t => {
                this.splitDrs.push({ OrderId: t.OrderId, ParentDrId: t.Id, RequiredQuantity: null, ProductType: t.ProductType, FuelType: t.FuelType, UoM: t.UoM });
            });
            this.SplitBlendDRForm = this.initSplitBlendDRForm(this.splitDrs);
        }
        let deliveryReq = deliveryReqs[0];
        this.productType = this.IsBlendRequest ? deliveryReq.AdditiveProductName + ' ' + deliveryReq.BlendedProductName : deliveryReq.ProductType;
        this.siteId = deliveryReq.SiteId;
        this.jobName = deliveryReq.JobName;
        this.jobAddress = deliveryReq.JobAddress;
        this.jobCity = deliveryReq.JobCity;
        this.totalQuantity = this.IsBlendRequest ? deliveryReq.TotalBlendedQuantity : deliveryReq.RequiredQuantity;
        this.originalTotalQuantity = this.IsBlendRequest ? deliveryReq.TotalBlendedQuantity : deliveryReq.RequiredQuantity;
        this.remainingQuantity = this.IsBlendRequest ? deliveryReq.TotalBlendedQuantity : deliveryReq.RequiredQuantity;
        this.deliveryLevelPO = deliveryReq.DeliveryLevelPO;
        this.UoM = deliveryReq.UoM == 1 ? "G" : "L";
        this.SplitDRForm = this.initForm();
        let parentDRId = this.SplitDRForm.get('ParentDRId') as FormControl;
        parentDRId.patchValue(deliveryReq.Id);
        let requiredQtyDetais = this.SplitDRForm.get('RequiredQtyDetails') as FormArray;
        for (var i = 0; i < this.defaultSplitDRsValue; i++) {
            requiredQtyDetais.push(this.getRequiredQty());
        }
        if (requiredQtyDetais.controls.length >= this.defaultSplitDRsValue) {
            this.validateQtyStatus = true;
            this.validateQtyMessage = '';
            this.validateQtyInfo();
        }
        else {
            this.validateQtyStatus = false;
        }
    }
    initForm(): FormGroup {
        return this.fb.group({
            ParentDRId: this.fb.control(''),
            NoOfSubDRs: this.fb.control('0', [Validators.pattern("[0-9]+(\.[0-9][0-9]?)?"), Validators.min(0)]),
            SubDRType: this.fb.control('1'),
            RequiredQtyDetails: this.fb.array([])
        });
    }

    initSplitBlendDRForm(blendDrs: SplitBlendDRModel[]): FormGroup {
        let _formGroup = this.fb.group({
            splitDrArray: this.fb.array([])
        });

        let splitDrArray = _formGroup.get('splitDrArray') as FormArray;
        for (var i = 0; i < 2; i++) {
            splitDrArray.push(this.getsplitDrArray(blendDrs));
        }
        return _formGroup;
    }

    getsplitDrArray(blendDrs: SplitBlendDRModel[]) {
        let _formGroup = this.fb.group({
            BlendDrArray: this.fb.array([]),
            BlendGroupId: this.fb.control(getUniqueId())
        });

        let blendDrArr = _formGroup.get('BlendDrArray') as FormArray;
        blendDrs.forEach(t => {
            blendDrArr.push(this.getSplitDRForm(t));
        })

        return _formGroup;
    }

    getSplitDRForm(blendDr: SplitBlendDRModel) {
        return this.fb.group({
            OrderId: this.fb.control(blendDr.OrderId, [Validators.required]),
            ParentDrId: this.fb.control(blendDr.ParentDrId, [Validators.required]),
            RequiredQuantity: this.fb.control(null, [Validators.required, Validators.pattern("[0-9]+(\.[0-9][0-9]?)?"), Validators.min(0.00001)]),
            ProductType: this.fb.control(blendDr.ProductType),
            FuelType: this.fb.control(blendDr.FuelType),
            UoM: this.fb.control(blendDr.UoM)
        });
    }

    getRequiredQty(): FormGroup {
        return this.fb.group({
            RequiredQty: this.fb.control('', [Validators.required, Validators.pattern("[0-9]+(\.[0-9][0-9]?)?"), Validators.min(0.00001)]),
            DeliveryLevelPO : this.fb.control(this.deliveryLevelPO)
        })
    }
    get f() {
        return this.SplitDRForm.controls;
    }
    removeItem(j: number) {
        let requiredQtyDetails = this.SplitDRForm.get('RequiredQtyDetails') as FormArray;
        requiredQtyDetails.removeAt(j);
        if (requiredQtyDetails.controls.length >= this.defaultSplitDRsValue) {
            this.validateQtyStatus = true;
            this.validateQtyMessage = '';
            this.validateQtyInfo();
        }
        else {
            this.validateQtyStatus = false;
            this.caculateQtyDetails();
        }
        let subDRType = this.SplitDRForm.get('SubDRType') as FormControl;
        if (subDRType.value == 2) {
            let noOfSubDRs = this.SplitDRForm.get('NoOfSubDRs') as FormControl;
            if (noOfSubDRs.value != '' && noOfSubDRs.value > 0) {
                noOfSubDRs.setValue(parseFloat(noOfSubDRs.value) - 1);
            }
        }
    }
    removeBlendDr(i: number) {
        let splitDrArray = this.SplitBlendDRForm.get('splitDrArray') as FormArray;
        splitDrArray.removeAt(i);
        if (splitDrArray.controls.length >= this.defaultSplitDRsValue) {
            this.validateQtyStatus = true;
            this.validateQtyMessage = '';
            this.validateBlendDrQtyInfo();
        }
        else {
            this.validateQtyStatus = false;
            this.caculateBlendDrQtyDetails();
        }
    }
    removeBlendDrItem(i: number, j: number) {
        let splitDrArray = this.SplitBlendDRForm.get('splitDrArray') as FormArray;
        let blendDrArray = splitDrArray.controls[i].get('BlendDrArray') as FormArray;
        blendDrArray.removeAt(j);
        if (blendDrArray.controls.length == 0) {
            splitDrArray.removeAt(i);
        }
        if (splitDrArray.controls.length >= this.defaultSplitDRsValue) {
            this.validateQtyStatus = true;
            this.validateQtyMessage = '';
            this.validateBlendDrQtyInfo();
        }
        else {
            this.validateQtyStatus = false;
            this.caculateBlendDrQtyDetails();
        }
    }
    validateBlendDrQtyInfo() {
        var totalQty = 0;
        var splitDrArray = this.SplitBlendDRForm.get('splitDrArray') as FormArray;
        for (var i = 0; i < splitDrArray.controls.length; i++) {
            var blendDrArray = splitDrArray.controls[i].get('BlendDrArray') as FormArray;
            for (var j = 0; j < blendDrArray.controls.length; j++) {
                let requiredQ = blendDrArray.controls[j].get('RequiredQuantity').value;
                if (requiredQ) {
                    totalQty = totalQty + parseFloat(requiredQ);
                }
            }
        }
        if (totalQty >= this.originalTotalQuantity) {
            this.totalQuantity = totalQty
            this.remainingQuantity = 0
        }
        else
            this.remainingQuantity = this.totalQuantity - totalQty;

        if (this.remainingQuantity >= 0) {
            this.validateQtyStatus = true;
        }
        else {
            if (this.remainingQuantity <= 0) {
                this.totalQuantity = totalQty
                this.remainingQuantity = 0;
            }
            this.validateQtyStatus = true;
        }
    }
    caculateBlendDrQtyDetails() {
        var totalQty = 0;
        var splitDrArray = this.SplitBlendDRForm.get('splitDrArray') as FormArray;
        for (var i = 0; i < splitDrArray.controls.length; i++) {
            var blendDrArray = splitDrArray.controls[i].get('BlendDrArray') as FormArray;
            for (var j = 0; j < blendDrArray.controls.length; j++) {
                let requiredQ = blendDrArray.controls[j].get('RequiredQuantity').value;
                if (requiredQ) {
                    totalQty = totalQty + parseFloat(requiredQ);
                }
            }
        }
        this.remainingQuantity = this.totalQuantity - totalQty;
    }

    addBlendDR() {
        let splitDrArray = this.SplitBlendDRForm.get('splitDrArray') as FormArray;
        splitDrArray.push(this.getsplitDrArray(this.splitDrs));

        if (splitDrArray.controls.length >= this.defaultSplitDRsValue) {
            this.validateQtyStatus = true;
            this.validateQtyMessage = '';
            this.validateBlendDrQtyInfo();
        }
        else {
            this.validateQtyStatus = false;
            this.caculateBlendDrQtyDetails();
        }
    }

    addDR() {
        let requiredQtyDetails = this.SplitDRForm.get('RequiredQtyDetails') as FormArray;
        requiredQtyDetails.push(this.getRequiredQty());
        if (requiredQtyDetails.controls.length >= this.defaultSplitDRsValue) {
            this.validateQtyStatus = true;
            this.validateQtyMessage = '';
            this.validateQtyInfo();
        }
        else {
            this.validateQtyStatus = false;
            this.caculateQtyDetails();
        }
        let subDRType = this.SplitDRForm.get('SubDRType') as FormControl;
        if (subDRType.value == 2) {
            let noOfSubDRs = this.SplitDRForm.get('NoOfSubDRs') as FormControl;
            if (noOfSubDRs.value != '' && noOfSubDRs.value > 0) {
                noOfSubDRs.setValue(parseFloat(noOfSubDRs.value) + 1);
            }
        }
    }
    validateQty(tfxQty: any) {
        this.validateQtyMessage = '';

        var currentQty = tfxQty.controls['RequiredQty'].value;
        if (RegExConstants.Integer.test(currentQty) || RegExConstants.Float.test(currentQty)) {
            var requiredQtyDetails = this.SplitDRForm.get('RequiredQtyDetails') as FormArray;
            if (requiredQtyDetails.controls.length < this.defaultSplitDRsValue) {
                this.validateQtyStatus = false;
                this.validateQtyMessage = 'Atleast 2 sub delivery request required.';
                return;
            }
            this.validateQtyInfo();
        }
    }
    validateBlendDrQty(tfxQty: any) {
        this.validateQtyMessage = '';

        var currentQty = tfxQty.controls['RequiredQuantity'].value;
        if (RegExConstants.Integer.test(currentQty) || RegExConstants.Float.test(currentQty)) {
            var splitDrArray = this.SplitBlendDRForm.get('splitDrArray') as FormArray;
            if (splitDrArray.controls.length < this.defaultSplitDRsValue) {
                this.validateQtyStatus = false;
                this.validateQtyMessage = 'Atleast 2 sub delivery request required.';
                return;
            }
            this.validateBlendDrQtyInfo();
        }
    }
    validateQtyInfo() {
        var totalQty = 0;
        var requiredQtyDetails = this.SplitDRForm.get('RequiredQtyDetails') as FormArray;
        for (var i = 0; i < requiredQtyDetails.controls.length; i++) {
            var requiredQ = requiredQtyDetails.controls[i].get('RequiredQty').value;
            if (requiredQ != '') {
                totalQty = totalQty + parseFloat(requiredQ);
            }
        }
        if (totalQty >= this.originalTotalQuantity) {
            this.totalQuantity = totalQty
            this.remainingQuantity = 0
        }
        else
            this.remainingQuantity = this.totalQuantity - totalQty;

        if (this.remainingQuantity >= 0) {
            this.validateQtyStatus = true;
        }
        else {
            if (this.remainingQuantity <= 0) {
                this.totalQuantity = totalQty
                this.remainingQuantity = 0;
            }
            this.validateQtyStatus = true;
        }
    }
    caculateQtyDetails() {
        var totalQty = 0;
        var requiredQtyDetails = this.SplitDRForm.get('RequiredQtyDetails') as FormArray;
        for (var i = 0; i < requiredQtyDetails.controls.length; i++) {
            var requiredQ = requiredQtyDetails.controls[i].get('RequiredQty').value;
            if (requiredQ != '') {
                totalQty = totalQty + parseFloat(requiredQ);
            }
        }
        this.remainingQuantity = this.totalQuantity - totalQty;
    }
    submitSplitInfo() {
        this._loading = true;
        this.carrierService.splitDeliveryRequests(this.SplitDRForm.value).subscribe(detail => {
            this._loading = false;
            $('#btnSplitCancel').click();
            if (detail.StatusCode == 0) {
                this.onRaiseDR.emit();
                Declarations.msgsuccess("Split DRs created successfully.", undefined, undefined);
                if (this.remainingQuantity > 0) {
                    Declarations.msgwarning("The total quantity of sub DRs created is less than the requested quantity.", undefined, undefined);
                }
            }
            else {
                Declarations.msgerror(detail.StatusMessage, undefined, undefined);
            }
        });
    }

    submitSplitBlendDrInfo() {
        this._loading = true;
        this.carrierService.splitBlendDeliveryRequests(this.SplitBlendDRForm.value).subscribe(detail => {
            this._loading = false;
            $('#btnSplitCancel').click();
            if (detail.StatusCode == 0) {
                this.onRaiseDR.emit();
                Declarations.msgsuccess("Split DRs created successfully.", undefined, undefined);
                if (this.remainingQuantity > 0) {
                    Declarations.msgwarning("The total quantity of sub DRs created is less than the requested quantity.", undefined, undefined);
                }
            }
            else {
                Declarations.msgerror(detail.StatusMessage, undefined, undefined);
            }
        });
    }
    GenerateSubDRs() {
        let subDRType = this.SplitDRForm.get('SubDRType') as FormArray;

        let requiredQtyDetails = this.SplitDRForm.get('RequiredQtyDetails') as FormArray;
        requiredQtyDetails.clear();
        if (subDRType.value == 2) {
            let noOfSubDRs = this.SplitDRForm.get('NoOfSubDRs') as FormControl;
            if (noOfSubDRs.value != '' && noOfSubDRs.value > 0) {
                var subDFDRsQty = parseFloat((this.totalQuantity / noOfSubDRs.value).toFixed(2));
                var subDRsQty = parseFloat((this.totalQuantity / noOfSubDRs.value).toFixed(0));
                var finalDRAdditions = subDFDRsQty - subDRsQty;
                finalDRAdditions = finalDRAdditions * noOfSubDRs.value;
                for (var i = 0; i < noOfSubDRs.value; i++) {
                    if (noOfSubDRs.value - 1 == i) {
                        subDRsQty = subDRsQty + Math.round(finalDRAdditions);
                        requiredQtyDetails.push(this.getRequiredQtyWithValue(subDRsQty));
                    }
                    else {
                        requiredQtyDetails.push(this.getRequiredQtyWithValue(subDRsQty));
                    }
                }
            }
            else {
                for (var i = 0; i < this.defaultSplitDRsValue; i++) {
                    requiredQtyDetails.push(this.getRequiredQty());
                }
            }
        }
        else {
            var requiredfQtyDetails = this.SplitDRForm.get('RequiredQtyDetails') as FormArray;
            let noOfSubDRs = this.SplitDRForm.get('NoOfSubDRs') as FormControl;
            if (noOfSubDRs.value != '' && noOfSubDRs.value > 0) {
                var fQty = noOfSubDRs.value;
                var totalNoOfDRs = Math.round(this.totalQuantity / fQty);
                for (var i = 0; i < totalNoOfDRs; i++) {
                    var totalSubDRQty = this.caculateQty();
                    if (totalSubDRQty > 0) {
                        var finalQty = this.totalQuantity - totalSubDRQty;
                        if (finalQty > fQty) {
                            requiredQtyDetails.push(this.getRequiredQtyWithValue(fQty));
                        }
                        else {
                            requiredQtyDetails.push(this.getRequiredQtyWithValue(finalQty));
                        }
                    }
                    else {
                        requiredQtyDetails.push(this.getRequiredQtyWithValue(fQty));
                    }
                }
                var totalQty = 0;
                for (var i = 0; i < requiredfQtyDetails.controls.length; i++) {
                    var requiredQ = requiredfQtyDetails.controls[i].get('RequiredQty').value;
                    if (requiredQ != '') {
                        totalQty = totalQty + parseFloat(requiredQ);
                    }
                }
                var finalQty = this.totalQuantity - totalQty;
                if (finalQty > 0) {
                    requiredQtyDetails.push(this.getRequiredQtyWithValue(finalQty));
                }
            }
            else {
                for (var i = 0; i < this.defaultSplitDRsValue; i++) {
                    requiredQtyDetails.push(this.getRequiredQty());
                }
            }
        }
        this.validateQtyInfo();
    }
    getRequiredQtyWithValue(subDRsQty: number): FormGroup {
        return this.fb.group({
            RequiredQty: this.fb.control(subDRsQty, [Validators.required, Validators.pattern("[0-9]+(\.[0-9][0-9]?)?"), Validators.min(0.00001)]),
            DeliveryLevelPO: this.fb.control(this.deliveryLevelPO)
        })
    }
    clearForm() {
        let requiredQtyDetails = this.SplitDRForm.get('RequiredQtyDetails') as FormArray;
        requiredQtyDetails.clear();
        let noOfSubDRs = this.SplitDRForm.get('NoOfSubDRs') as FormControl;
        noOfSubDRs.setValue(0);
        this.validateQtyInfo();
        for (var i = 0; i < this.defaultSplitDRsValue; i++) {
            requiredQtyDetails.push(this.getRequiredQty());
        }
    }
    caculateQty(): number {
        var requiredfQtyDetails = this.SplitDRForm.get('RequiredQtyDetails') as FormArray;
        var totalQty = 0;
        for (var i = 0; i < requiredfQtyDetails.controls.length; i++) {
            var requiredQ = requiredfQtyDetails.controls[i].get('RequiredQty').value;
            if (requiredQ != '') {
                totalQty = totalQty + parseFloat(requiredQ);
            }
        }
        return totalQty;
    }
}
