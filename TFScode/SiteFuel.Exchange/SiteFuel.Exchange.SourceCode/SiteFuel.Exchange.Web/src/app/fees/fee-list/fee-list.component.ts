import { Component, OnInit, Input, OnChanges, SimpleChanges } from '@angular/core';
import { FormGroup, FormBuilder, FormArray, Validators } from '@angular/forms';
import { FeeModel } from '../model';
import { DropdownItem, DropdownItemExt, FeeSubType } from 'src/app/statelist.service';
import { FeeService } from '../service/fee.service';
import * as moment from 'moment';
import { ActivatedRoute } from '@angular/router';
import { InvoiceService } from '../../invoice/services/invoice.service';
import { FreightTableNameModel, AccessorialFeeTableDetailViewModel, DeDuplicateFees } from '../../invoice/models/DropDetail'; 
import { TableType } from '../../app.enum';

@Component({
    selector: 'app-fee-list',
    templateUrl: './fee-list.component.html',
    styleUrls: ['./fee-list.component.css']
})
export class FeeListComponent implements OnInit, OnChanges {

    @Input() Parent: FormGroup;
    @Input() public Fees: FeeModel[];
    @Input() public InputAccessorialFeeDetails: AccessorialFeeTableDetailViewModel[];
    @Input() public isWaitingForBol: boolean;
    @Input() public isSales: boolean = false;
    @Input() public IsFrieghtPricingMethodAuto: boolean;
    @Input() Currency: string; // Taking currency as input to display on addon 
    @Input() public NoOrders: boolean;
    //---------------Fees GROUP -----------

    public CommonFees: FormGroup[];
    public OtherFees: FormGroup[];
    public SpCommonFees: FormGroup[];
    public SpOtherFees: FormGroup[];

    public FeeTypes: DropdownItemExt[];
    public FeeSubTypes: FeeSubType[];
    public FeeConstraintTypes: DropdownItem[];
    public AccessorialFeeTableTypeList: DropdownItem[];
    public AccessorialFeeIdList: DropdownItem[];
    public SingleSelectSettingsById = {};
    public MultiSelectSettingsById = {};
    public AccessorialForm: FormGroup;
    public group: FormGroup;
    public DisplayCurrency: any;
    public OrderId: number;
    public currValues: FeeModel[];
    constructor(private fb: FormBuilder, private feeService: FeeService, private invoiceService: InvoiceService,
        private route: ActivatedRoute) {
        this.CommonFees = [];
        this.OtherFees = [];
        this.SpCommonFees = [];
        this.SpOtherFees = [];
    }

    ngOnInit() {
        this.AccessorialForm = this.buildForm();
        this.Parent.addControl('AccessorialFeeDetails', this.AccessorialForm);
        if (this.InputAccessorialFeeDetails != undefined && this.InputAccessorialFeeDetails != null) {
            this.AccessorialForm.patchValue(this.InputAccessorialFeeDetails);
        }
        this.OrderId = parseInt(this.route.snapshot.queryParamMap.get('orderId'), 10);
        this.Parent.addControl('Fees', this.fb.array([]));

        this.feeService.getFeeTypes(this.OrderId, this.Currency, this.isSales)
            .subscribe(data => { this.FeeTypes = data; });

        this.feeService.getFeeConstraintTypes()
            .subscribe((data: DropdownItem[]) => { this.FeeConstraintTypes = data; });

        this.feeService.getFeeSubTypes(this.OrderId, this.Currency, this.isSales)
            .subscribe((data: FeeSubType[]) => {
                this.FeeSubTypes = data.filter(function (elem: FeeSubType) { return elem.FeeSubTypeId != 1 });
            });
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
        this.MultiSelectSettingsById = {
            singleSelection: false,
            idField: 'Id',
            textField: 'Name',
            selectAllText: 'Select All',
            unSelectAllText: 'UnSelect All',
            itemsShowLimit: 5,
            allowSearchFilter: true
        };
        this.getTableTypes();
    }

    ngOnChanges(change: SimpleChanges) {
        if (this.NoOrders) {
            this.AccessorialForm.controls['AccessorialFeeTableType'].patchValue([]);
            this.AccessorialForm.controls['AccessorialFeeId'].patchValue([]);
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
            this.currValues = change.Fees.currentValue as FeeModel[];
            this.currValues.forEach((x: FeeModel) => {
                if (x.FeeConstraintTypeId == null) {
                    this.addGeneralFee(x.CommonFee, x);
                } else {
                    this.addSpecialFee(x.CommonFee, x.FeeConstraintTypeId, x);
                }
            });
        }
        if (change.InputAccessorialFeeDetails && change.InputAccessorialFeeDetails.currentValue) {
            this.getDefaultDetail(change.InputAccessorialFeeDetails.currentValue as AccessorialFeeTableDetailViewModel[]);
        }
        if (change.Currency && change.Currency.currentValue) {
            var currency = change.Currency.currentValue;
            this.DisplayCurrency = currency;

            this.feeService.getFeeTypes(0, this.DisplayCurrency, this.isSales)
                .subscribe(data => { this.FeeTypes = data; });

            this.feeService.getFeeConstraintTypes()
                .subscribe((data: DropdownItem[]) => { this.FeeConstraintTypes = data; });

            this.feeService.getFeeSubTypes(0, this.DisplayCurrency, this.isSales)
                .subscribe((data: FeeSubType[]) => {
                    this.FeeSubTypes = data.filter(function (elem: FeeSubType) { return elem.FeeSubTypeId != 1 });
                });
        }
    }

    buildForm(): FormGroup {
        return this.fb.group({
            AccessorialFeeTableType: this.fb.control(''),
            AccessorialFeeId: this.fb.control([])
        });
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
        this.group = this.fb.group({
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
            Fee: this.fb.control(model.Fee, [Validators.required]),
            OtherFeeDescription: this.fb.control(model.OtherFeeDescription),
            MinimumGallons: this.fb.control(model.MinimumGallons),
            DeliveryFeeByQuantity: this.fb.array(byQuantity),
        });
        return this.group;
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

    // Accessorial
    private getTableTypes(): void {
        this.invoiceService.getTableTypes().subscribe(data => {
            this.AccessorialFeeTableTypeList = data;
            this.AccessorialFeeTableTypeList = this.AccessorialFeeTableTypeList.filter(x => x.Id != TableType.Carrier);
        });
    }

    public OnAccessorialFeeTableTypeSelect(item: any): void {
        this.AccessorialForm.get('AccessorialFeeId').patchValue([]);
        this.AccessorialForm.controls['AccessorialFeeTableType'].patchValue([{ Id: item.Id, Name: item.Id == TableType.Master ? "Master" : "CustomerSpecific" }]);
        this.GetAccessorialFeeId(null);
    }

    public GetAccessorialFeeId(selectedAccessorialId: number): void {
        let input = new FreightTableNameModel();
        let selectedAccessorialFeeTableType = this.AccessorialForm.get('AccessorialFeeTableType').value as DropdownItem[];

        if (selectedAccessorialFeeTableType && selectedAccessorialFeeTableType.length > 0) {

            let AccessorialFeeTableTypeIds = selectedAccessorialFeeTableType.map(s => s.Id);
            input.TableType = AccessorialFeeTableTypeIds.join(',');
            input.OrderId = this.OrderId;
            this.invoiceService.getAccessorialTableName(input).subscribe(data => {

                if (data && data.length > 0) {
                    this.AccessorialFeeIdList = data;
                    if (selectedAccessorialId) {
                        this.AccessorialForm.controls['AccessorialFeeId'].patchValue(this.AccessorialFeeIdList.filter(t => t.Id == selectedAccessorialId));
                    }
                }
            })
        }
    }

    public getDefaultDetail(AccessorialFees: AccessorialFeeTableDetailViewModel[]): void {
        if(AccessorialFees && AccessorialFees.length > 0){
            if (AccessorialFees.length == 1) {

                let type = AccessorialFees[0].AccessorialFeeTableType;
                this.AccessorialForm.controls['AccessorialFeeTableType'].patchValue([{ Id: type, Name: type == TableType.Master ? "Master" : "CustomerSpecific" }]);
                this.GetAccessorialFeeId(AccessorialFees[0].AccessorialFeeId);
            }
            else {
                this.invoiceService.GetAccessorialFeeTablesForConsolidated(null).subscribe((allTableNames: any[]) => {
                    this.AccessorialFeeIdList = allTableNames;
                    let AccessorialFeeIds = this.AccessorialFeeIdList.filter(this.IdInComparer(AccessorialFees));
                    this.AccessorialForm.controls['AccessorialFeeId'].setValue(AccessorialFeeIds);
                })
            }
        }
    }

    IdInComparer(otherArray) {
        return function (current) {
            return otherArray.filter(function (other) {
                return other.Id == current.Id
            }).length == 1;
        }
    }

    public AllFeeRemove() {
        if (this.CommonFees && this.CommonFees.length > 0) {
            this.CommonFees.forEach(generalFees => {
                this.removeGeneralFee(true, generalFees);
            });
            this.CommonFees = [];
        }
        if (this.OtherFees && this.OtherFees.length > 0) {
            this.OtherFees.forEach(otherFees => {
                this.removeGeneralFee(false, otherFees);
            });
            this.OtherFees = [];
        }
        if (this.SpCommonFees && this.SpCommonFees.length > 0) {
            this.SpCommonFees.forEach(spFee => {
                this.removeSpecialFee(true, spFee);
            });
            this.SpCommonFees = [];
        }
        if (this.SpOtherFees && this.SpOtherFees.length > 0) {
            this.SpOtherFees.forEach(spOtherFee => {
                this.removeSpecialFee(false, spOtherFee);
            });
            this.SpOtherFees = [];
        }
    }

    public OnAccessorialFeeTableTypeDeSelect(): void {
        let selectedAccessorialFeeTableType = this.AccessorialForm.get('AccessorialFeeTableType').value as DropdownItem[];
        if (selectedAccessorialFeeTableType.length == 0) {
            this.AccessorialForm.get('AccessorialFeeId').patchValue([]);
            this.AccessorialFeeIdList = [];
        }
        this.AllFeeRemove();
        let selectedItems = this.AccessorialForm.get("AccessorialFeeId").value as DropdownItem[];
        if (selectedItems != null && selectedItems != undefined) {
            let feeIds = selectedItems.map(s => s.Id);
            this.invoiceService.GetAccessorialFeeByAccessorialFeeId(feeIds.join(',')).subscribe(data => {
                if (data && data.length > 0) {
                    data.forEach(obj => {
                        if (obj.FeeConstraintTypeId == null) {
                            this.addGeneralFee(obj.CommonFee, obj);
                        } else {
                            this.addSpecialFee(obj.CommonFee, obj.FeeConstraintTypeId, obj);
                        }
                    });
                }
            });
        }
    }
    OnAccessorialFeeTableTypeDeSelectForSingle() {
        this.AllFeeRemove();
    }

    public OnAccessorialFeeIdSelect() {
        this.CommonFees = [];
        this.OtherFees = [];
        this.SpCommonFees = [];
        this.SpOtherFees = [];

        let fees = this.Parent.get('Fees') as FormArray;
        if (fees) {
            fees.clear();
        }
        let selectedItems = this.AccessorialForm.get("AccessorialFeeId").value as DropdownItem[];
        if (selectedItems != null && selectedItems != undefined) {
            let feeIds = selectedItems.map(s => s.Id);
            if (feeIds.length != 0) {
                this.invoiceService.GetAccessorialFeeByAccessorialFeeId(feeIds.join(',')).subscribe(data => {
                    if (feeIds.length != 1) {
                        data = DeDuplicateFees(this.currValues, data);
                    }
                    if (data && data.length > 0) {
                        data.forEach(obj => {
                            if (obj.FeeConstraintTypeId == null) {
                                this.addGeneralFee(obj.CommonFee, obj);
                            } else {
                                this.addSpecialFee(obj.CommonFee, obj.FeeConstraintTypeId, obj);
                            }
                        });
                    }
                });
            }
        }
        else {
            this.OnAccessorialFeeTableTypeDeSelect();
        }
    }
}

function requiredIfValidator(predicate) {
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