import { Component, OnInit, Input, Output, EventEmitter, ViewChildren, QueryList, OnChanges, SimpleChanges } from '@angular/core';
import { FormArray, FormBuilder, Validators, FormGroup, AbstractControl } from '@angular/forms';
import { BolDetail, BolProductModel, QuantityInfo } from '../models/DropDetail';
import { ImageuploadComponent, FileInfo } from 'src/app/imageupload/imageupload.component';


@Component({
    selector: 'app-bol-list',
    templateUrl: './bol-list.component.html',
    styleUrls: ['./bol-list.component.css']
})
export class BolListComponent implements OnInit, OnChanges {
    public files = new Array<FileInfo>();
    public quantities = new Array<QuantityInfo>();

    @Input() public invoiceForm: FormGroup;
    @Input() public BolDetails: FormArray;
    @Input() public Model: BolDetail[];

    @Output() public onBolEditRequest: EventEmitter<any> = new EventEmitter<any>();

    @Input() public IsImageRequired: boolean;
    public IsBolImageRequired: boolean = false;

    @Output() public onBolAdded: EventEmitter<any> = new EventEmitter<any>();
    @Output() public OnBolDeleted: EventEmitter<any> = new EventEmitter<any>();
    @Output() public onBolEdit: EventEmitter<any> = new EventEmitter<any>();

    @Output() public OnBolQuantitiesAdded: EventEmitter<any> = new EventEmitter<any>();

    @ViewChildren(ImageuploadComponent) children: QueryList<ImageuploadComponent>;

    constructor(private fb: FormBuilder) { }

    ngOnInit() {
    }
    ngOnChanges(change: SimpleChanges) {
        if (change.IsImageRequired && change.IsImageRequired.currentValue != null) {
            this.IsBolImageRequired = change.IsImageRequired.currentValue;
        }
        if (change.Model && change.Model.currentValue != null) {
            var bols = change.Model.currentValue as BolDetail[];
            bols.forEach(x => this.bolDetailAdded(x));
        }
    }
    buildForm(model: BolDetail): FormGroup {

        var products = this.fb.array([]);

        var currentObj = this;
        model.Products.forEach(function (elem, idx) {
            products.push(currentObj.buildProduct(elem));
        });
        //this.IsFormBuilt = true;


        return this.fb.group({
            Id: this.fb.control(model.Id),
            BolNumber: this.fb.control(model.BolNumber),
            LiftDate: this.fb.control(model.LiftDate),
            BadgeNumber: this.fb.control(model.BadgeNumber),
            Products: products,
            BolImages: this.fb.control(''),
            LiftStartTime: this.fb.control(model.LiftStartTime),
            LiftEndTime: this.fb.control(model.LiftEndTime),
            CommonTerminalId: this.fb.control(model.Products ? model.Products[0].TerminalId : model.CommonTerminalId),
            CommonTerminalName: this.fb.control(model.Products ? model.Products[0].TerminalName : model.CommonTerminalName)
        });
    }


    buildProduct(model: BolProductModel): FormGroup {
        return this.fb.group({
            ProductId: this.fb.control(model.ProductId),
            ProductName: this.fb.control(model.ProductName),
            NetQuantity: this.fb.control(model.NetQuantity),
            GrossQuantity: this.fb.control(model.GrossQuantity),
            TerminalId: this.fb.control(model.TerminalId),
            TerminalName: this.fb.control(model.TerminalName),
            QuantityIndicatorTypeId: this.fb.control(model.QuantityIndicatorTypeId),
            DeliveredQuantity: this.fb.control(model.DeliveredQuantity),

        });
    }

    bolDetailAdded(item: BolDetail): void {
        if (item.LiftDate && item.LiftDate.indexOf('/Date(') >= 0) {
            item.LiftDate = item.DisplayLiftDate || '';
        }
        this.BolDetails.push(this.buildForm(item));
        this.onBolAdded.emit(this.BolDetails.value);
        this.OnBolQuantitiesAdded.emit();
    }

    bolDetailUpdated(item: any): void {
        var formG = this.BolDetails.controls[item.index] as FormGroup;
        formG.patchValue(item.bolDetail);       
        this.OnBolQuantitiesAdded.emit();
    }

    editBolDetail(bolDetail: FormGroup, i: number): any {
        this.onBolEditRequest.emit({ bolDetail: bolDetail.value, index: i });
        this.onBolEdit.emit(bolDetail.value);
    }

    deleteBolDetail(i: number): void {
        this.BolDetails.removeAt(i);
        this.OnBolDeleted.emit(this.BolDetails.value);
        this.OnBolQuantitiesAdded.emit();
    }
}

