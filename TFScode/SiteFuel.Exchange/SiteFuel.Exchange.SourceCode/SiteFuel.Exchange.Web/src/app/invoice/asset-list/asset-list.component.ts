import { Component, OnInit, Input, OnChanges, SimpleChanges, Output, EventEmitter} from '@angular/core';
import { FormGroup, FormBuilder, Validators, FormArray, ValidationErrors } from '@angular/forms';
import { AssetDropModel, DropDetailModel, DropQuantityByPrePostDipRequestModel, DropQuantityByPrePostDipResponseModel} from '../models/DropDetail';
import { DropdownItem } from 'src/app/statelist.service';
import { Declarations } from 'src/app/declarations.module';
import { InvoiceService } from '../services/invoice.service';
import { ValidationService } from 'src/app/services/validation.service';
import { convertTo24Hour, getSeconds, RegExConstants } from 'src/app/app.constants';

@Component({
    selector: 'app-asset-list',
    templateUrl: './asset-list.component.html',
    styleUrls: ['./asset-list.component.css']
})
export class AssetListComponent implements OnInit, OnChanges {

    @Input() public Parent: FormGroup;
    @Input() public Drops: DropDetailModel[];
    @Input() SelectedAssets: AssetDropModel[];
    @Input() UoM: string;
    @Input() IsSelectedMT:boolean;

    //@Output() onDropQuantityUpdate: EventEmitter<any> = new EventEmitter<any>();

    public IsDipDataRequired: boolean;

    public UOMsForDipTest: DropdownItem[] = [];
    public IsCalculating: boolean = false;

    constructor(private fb: FormBuilder, private invoiceService: InvoiceService, private validationService: ValidationService) {
    }

    ngOnInit() {
        //this.Parent.addControl('Assets', this.fb.array([]));
    }

    ngOnChanges(change: SimpleChanges) {
        if (change.SelectedAssets && change.SelectedAssets.currentValue != null) {

            if (this.SelectedAssets != undefined) 
                this.showAssets(this.SelectedAssets);           
        }
        if (change.UoM && change.UoM.currentValue != null) {             
            var uom = change.UoM.currentValue;
            this.setUoM(uom);  
            this.CreateUOMsListForDipTestCalculation();
        }
        if (change.IsSelectedMT && change.IsSelectedMT.currentValue != null) {             
            this.IsSelectedMT = change.IsSelectedMT.currentValue;
        }
        if (change.Parent && change.Parent.currentValue != null) {
            var drop = change.Parent.currentValue as FormGroup;
            this.IsDipDataRequired = drop.get('IsDipDataRequired').value;
            if (this.IsDipDataRequired && this.SelectedAssets && this.SelectedAssets.length > 0) {
                this.setDipDataValidatorForInvoiceConversion();
                
            }
        }
    }

    getNewAsset(asset: AssetDropModel): FormGroup {
        var form = this.fb.group({
            Id: this.fb.control(asset.Id),
            AssetName: this.fb.control(asset.AssetName, [Validators.required]),
            JobXAssetId: this.fb.control(asset.JobXAssetId),
            DropGallons: this.fb.control(asset.DropGallons, [(Validators.required), Validators.pattern(RegExConstants.DecimalNumber)]),
            StartTime: this.fb.control(asset.StartTime, [Validators.required]),
            EndTime: this.fb.control(asset.EndTime, [Validators.required]),
            IsNewAsset: this.fb.control(asset.IsNewAsset),
            PreDip: this.fb.control(asset.PreDip),
            PostDip: this.fb.control(asset.PostDip),
            TankMakeModel: this.fb.control(asset.TankMakeModel),
            TankScaleMeasurement: this.fb.control(asset.TankScaleMeasurement)
        });       
        var _assetForm = this.setDipDataValidators(form, this.IsDipDataRequired);
        return _assetForm;

    }

    setDipDataValidators(assetForm: FormGroup, IsDipDataRequired: boolean): FormGroup {
        if (IsDipDataRequired) {
            assetForm.get('PreDip').setValidators([Validators.required,Validators.pattern(RegExConstants.DecimalNumber)]);
            assetForm.get('PostDip').setValidators([Validators.required, Validators.pattern(RegExConstants.DecimalNumber)]);           
        }
        return assetForm;
    }
    setDipDataValidatorForInvoiceConversion() {
        var assetArray = this.Parent.get('Assets') as FormArray;
        var _dropQuantityFormControls = assetArray.controls;
        if (_dropQuantityFormControls != undefined && _dropQuantityFormControls != null) {
            _dropQuantityFormControls.forEach(function (formControl) {
                formControl.get('PreDip').setValidators([Validators.required, Validators.pattern(RegExConstants.DecimalNumber)]);
                formControl.get('PostDip').setValidators([Validators.required, Validators.pattern(RegExConstants.DecimalNumber)]);
            });
        }
    }
    setUoM(UOM: string) {
        this.UoM = UOM;
    }
    //setActualDropQuantity() {
        // var totalQuantity = 0;
        // var assets = <FormArray>this.Parent.get('Assets');
        // assets.controls.forEach((x: FormGroup) => {
        //     var dropGallons = x.get('DropGallons');
        //     if (dropGallons.value != null && dropGallons.value != undefined && dropGallons.value != '') {
        //         totalQuantity = totalQuantity + parseFloat(dropGallons.value);
        //     }
        // });
        // totalQuantity = parseFloat(totalQuantity.toFixed(8));
        // this.Parent.get('ActualDropQuantity').setValue(totalQuantity);
        // this.onDropQuantityUpdate.emit(totalQuantity);
    //}

    addNewAsset(): void {
        var asset = new AssetDropModel();
        asset.IsNewAsset = true;
        (<FormArray>this.Parent.get('Assets')).push(this.getNewAsset(asset));
    }

    removeAsset(idx: number) {
        (<FormArray>this.Parent.get('Assets')).removeAt(idx);
        //this.setActualDropQuantity();
    }

    showAssets(assets: AssetDropModel[]): void {        
        var assetArray = this.Parent.get('Assets') as FormArray;
        var current = this; var existingAssets = [];
        assetArray.controls.forEach(function (control, idx) {
            var asset = assets.find(function (elem, idx2) {
                return elem.JobXAssetId == control.get('JobXAssetId').value
                    && elem.AssetName == control.get('AssetName').value;
            });
            if (asset != undefined) {
                existingAssets.push(asset)
            }
        });
        var newAssets = assets.filter(function (el) {
            return existingAssets.indexOf(el) < 0;
        });
        newAssets.forEach(function (model, idx) {
            assetArray.push(current.getNewAsset(model));
        });
    }

    removeAssets(assets: AssetDropModel[]): void {
        var assetArray = this.Parent.get('Assets') as FormArray;
        var removedIndex;
        assetArray.controls.forEach(function (control, idx) {
            var asset = assets.find(function (elem, idx2) {
                return elem.JobXAssetId == control.get('JobXAssetId').value
                    && elem.AssetName == control.get('AssetName').value;
            });
            if (asset != undefined) {
                removedIndex = idx;
            }
        });
        if (removedIndex >= 0) {
            assetArray.removeAt(removedIndex);
        }
        //this.setActualDropQuantity();
    }

    //------------------------------------------

    public showSelectedAsset(item: any) {
        let assets: AssetDropModel[] = [];
        let thisDrop = this.Drops.find(x => x.OrderId == this.Parent.controls['OrderId'].value);
        if (thisDrop) {
            //this.Drops.forEach(function (e) {
            thisDrop.Assets.forEach(function (e1) {
                if (e1.JobXAssetId == item.JobXAssetId && e1.AssetName == item.AssetName && assets.indexOf(e1) == -1) {
                    assets.push(e1);
                }
            });
            //});
        }
        assets = Array.from(new Set(assets))
        this.showAssets(assets);
    }

    public showSelectedAssets(item: AssetDropModel[]) {
        let orderId = (item != null && item != undefined) ? item[0].OrderId : 0;
        let assets: AssetDropModel[] = [];
        this.Drops.forEach(function (e) {
            if (e.OrderId == orderId) {
                e.Assets.forEach(function (ast) {
                    assets.push(ast);
                })
            }
        });
        assets = Array.from(new Set(assets))
        this.showAssets(assets);
    }

    public removeUnselected(item: any) {
        let assets: AssetDropModel[] = [];
        this.Drops.forEach(function (e) {
            e.Assets.forEach(function (e1) {
                if (e1.JobXAssetId == item.JobXAssetId && e1.AssetName == item.AssetName) {
                    assets.push(e1);
                }
            });
        });
        this.removeAssets(assets);
        //this.setActualDropQuantity();
    }

    public removeAllUnselected(item: any) {
        var assetArray = this.Parent.get('Assets') as FormArray;
        assetArray.clear();
        //this.setActualDropQuantity();
    }

    public CreateUOMsListForDipTestCalculation() {
        let _centrimeteruom = new DropdownItem;
        _centrimeteruom.Id = 1; //CM
        _centrimeteruom.Name = 'cm';
        this.UOMsForDipTest.push(_centrimeteruom);

        let _inchuom = new DropdownItem;
        _inchuom.Id = 2; //in
        _inchuom.Name = 'in';
        this.UOMsForDipTest.push(_inchuom);

        if (this.UoM == 'Gallons') {
            let _gallonsuom = new DropdownItem;
            _gallonsuom.Id = 3; //Gallons
            _gallonsuom.Name = 'Gallons';
            this.UOMsForDipTest.push(_gallonsuom);
        }
        else if (this.UoM == 'Litres')
        {
            let litreUom = new DropdownItem;
            litreUom.Id = 3; //Gallons
            litreUom.Name = 'Litres';
            this.UOMsForDipTest.push(litreUom);
        }


    }

    public CalculateDropQuantitiesFromPrePostDip() {
        let assetsInfo: Array<AssetDropModel> = [];

        var addedAssets = this.Parent.get('Assets').value as AssetDropModel[];
        //Check for no assets added
        if (addedAssets == null || addedAssets == undefined || addedAssets.length == 0) {
            Declarations.msgerror("Assets/Tanks not added", undefined, undefined);
            return;
        }
        else
        {
            var incompleteInfoAssets = addedAssets.filter(t => (t.PreDip == null ||t.PostDip == null) && !t.IsNewAsset);
            var missingUoMAssets = addedAssets.filter(t => (t.PreDip != null && t.PostDip != null && (t.TankScaleMeasurement == null || t.TankScaleMeasurement == 0)) && !t.IsNewAsset)
            if (incompleteInfoAssets != null && incompleteInfoAssets != undefined && incompleteInfoAssets.length > 0) {
                Declarations.msgerror("Please Provide Required PreDip/PostDip", undefined, undefined);
                // To trigger validations
                let assetsFormArray = this.Parent.get('Assets') as FormArray;
                assetsFormArray.controls.forEach(function (control) {
                    let isNewAsset = control.get('IsNewAsset').value;
                    if (!isNewAsset) {
                        control.get('PreDip').markAsTouched();
                        control.get('PostDip').markAsTouched()
                    }
                });
                return;
            }
            else if (missingUoMAssets != null && missingUoMAssets != undefined && missingUoMAssets.length > 0) {
                Declarations.msgerror("Please Provide Required UoM(s)", undefined, undefined);
               // this.Parent.get('Assets').markAllAsTouched();// To trigger validations
                 
                return;
            }
            else 
            {
                assetsInfo = addedAssets.filter(t => t.PreDip != null && t.PostDip != null && t.TankScaleMeasurement != null && t.JobXAssetId > 0 && !t.IsNewAsset)
                if (assetsInfo != null && assetsInfo != undefined && assetsInfo.length > 0) {
                    this.IsCalculating = true;
                    this.invoiceService.postPrePostAssetsInfo(assetsInfo).subscribe((data) => {
                        this.IsCalculating = false;
                        let response = data as DropQuantityByPrePostDipResponseModel[];
                        if (response != undefined && response != null && response.length > 0) {
                            let assetsFormArray = this.Parent.get('Assets') as FormArray;
                            response.forEach(function (obj) {
                                var _group = assetsFormArray.controls.find(function (control) {
                                    return control.get('JobXAssetId').value == obj.JobxAssetId; 
                                });
                                if (obj.StatusCode == 0)//Success
                                {
                                    if (_group != undefined && _group != null && obj.DropQuantity != null) {
                                        _group.get('DropGallons').setValue(obj.DropQuantity);
                                    }
                                }
                                else if (obj.StatusCode == 1)//Failed 
                                {
                                    let assetName = _group.get('AssetName').value;
                                    let errMessage = "Error Ocurred when calculating quantity for asset/Tank  " + assetName + " . " + obj.StatusMessage
                                    Declarations.msgerror(errMessage, undefined, undefined);
                                }
                                else if (obj.StatusCode == 2) {
                                    let assetName = _group.get('AssetName').value;
                                    let errMessage = obj.StatusMessage + assetName + " . "
                                    Declarations.msgerror(errMessage, undefined, undefined);
                                }
                            });
                            //this.setActualDropQuantity();
                        }
                        else {
                            Declarations.msgerror("An error occurred when calculating quantities", undefined, undefined);
                        }
                    });
                }
            }

        }

    }
    public isDisable(asset: FormGroup, isdipdataRequired: any) {
        if (asset) {
            if (!asset.get('IsNewAsset').value && isdipdataRequired)
                return true;
            else 
                return false;
        }
        return false;
    }
    public ValidateAssetTime(asset: FormGroup, ValidationTriggedFrom: string, dropForm: FormGroup) {
        if (ValidationTriggedFrom === 'assetList') {
            let drop = this.Parent.value as DropDetailModel;
            let stringDropStartTime = drop.StartTime;
            let stringDropEndTime = drop.EndTime;
            let stringAssetStartTime = asset.get('StartTime').value;
            let stringAssetEndTime = asset.get('EndTime').value;
            if ((stringDropStartTime != null && stringDropStartTime != undefined && stringDropStartTime != '')
                && (stringDropEndTime != null && stringDropEndTime != undefined && stringDropEndTime != '')
                && (stringAssetStartTime != null && stringAssetStartTime != undefined && stringAssetStartTime != '')
                && (stringAssetEndTime != null && stringAssetEndTime != undefined && stringAssetEndTime != '')) {
                let dropStartTime = parseInt(getSeconds(convertTo24Hour(stringDropStartTime)));
                let dropEndTime = parseInt(getSeconds(convertTo24Hour(stringDropEndTime)));
                let assetStartTime = parseInt(getSeconds(convertTo24Hour(stringAssetStartTime)));
                let assetEndTime = parseInt(getSeconds(convertTo24Hour(stringAssetEndTime)));

                if ((assetStartTime < dropStartTime) || (assetEndTime > dropEndTime)) {
                    if (assetStartTime < dropStartTime) {
                        asset.get('StartTime').setErrors({ invalidDropTime: true });
                    }
                    if (assetEndTime > dropEndTime) {
                        asset.get('EndTime').setErrors({ invalidDropTime: true });
                    }
                }
                else {
                    if (asset.get('StartTime').hasError('invalidDropTime')) {
                        asset.get('StartTime').setErrors(null);
                    }
                    if (asset.get('EndTime').hasError('invalidDropTime')) {
                        asset.get('EndTime').setErrors(null);
                    }

                }
            }
        }
        if (ValidationTriggedFrom === 'productDetail') {
            if (dropForm != null && dropForm != undefined) {
                let stringDropStartTime = dropForm.get('StartTime').value;
                let stringDropEndTime = dropForm.get('EndTime').value;
                if ((stringDropStartTime != null && stringDropStartTime != undefined && stringDropStartTime != '')
                    && (stringDropEndTime != null && stringDropEndTime != undefined && stringDropEndTime != '')) {
                    let dropStartTime = parseInt(getSeconds(convertTo24Hour(stringDropStartTime)));
                    let dropEndTime = parseInt(getSeconds(convertTo24Hour(stringDropEndTime)));

                    let assetArray = dropForm.controls['Assets']['controls'] as FormArray;
                    if (assetArray != null && assetArray != undefined && assetArray.length > 0) {
                        for (let idx = 0; idx < assetArray.length; idx++) {
                            let stringAssetStartTime = assetArray[idx].get('StartTime').value;
                            let stringAssetEndTime = assetArray[idx].get('EndTime').value;
                            if ((stringAssetStartTime != undefined && stringAssetStartTime != null && stringAssetStartTime != '')
                                && (stringAssetEndTime != undefined && stringAssetEndTime != null && stringAssetEndTime != '')) {

                                let assetStartTime = parseInt(getSeconds(convertTo24Hour(stringAssetStartTime)));
                                let assetEndTime = parseInt(getSeconds(convertTo24Hour(stringAssetEndTime)));

                                if ((assetStartTime < dropStartTime) || (assetEndTime > dropEndTime)) {
                                    if (assetStartTime < dropStartTime) {
                                        assetArray[idx].get('StartTime').setErrors({ invalidDropTime: true });
                                    }
                                    if (assetEndTime > dropEndTime) {
                                        assetArray[idx].get('EndTime').setErrors({ invalidDropTime: true });
                                    }
                                }
                                else {
                                    if (assetArray[idx].get('StartTime').hasError('invalidDropTime')) {
                                        assetArray[idx].get('StartTime').setErrors(null);
                                    }
                                    if (assetArray[idx].get('EndTime').hasError('invalidDropTime')) {
                                        assetArray[idx].get('EndTime').setErrors(null);
                                    }

                                }
                            }
                            
                        }
                    }


                }
                
                            
            }
        }                           
    }

    @Output() assetDropGallonsChanged : EventEmitter<boolean> = new EventEmitter();
    dropGallonsChanged() {
        this.assetDropGallonsChanged.emit();
    }

}
