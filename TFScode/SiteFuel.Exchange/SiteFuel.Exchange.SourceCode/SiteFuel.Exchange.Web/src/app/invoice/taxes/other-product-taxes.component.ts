import { Component, OnInit, Input, OnChanges, SimpleChanges } from '@angular/core';
import { FormGroup, FormBuilder, FormArray, Validators } from '@angular/forms';
import { OtherProductTaxModel } from '../models/DropDetail';
import { DropdownItem } from 'src/app/statelist.service';
import { InvoiceService } from '../services/invoice.service';
import { ActivatedRoute } from '@angular/router';

@Component({
    selector: 'app-other-product-taxes',
    templateUrl: './other-product-taxes.component.html',
    styleUrls: ['./other-product-taxes.component.css']
})
export class OtherProductTaxesComponent implements OnInit, OnChanges {

    @Input() invoiceForm: FormGroup;
    @Input() Taxes: OtherProductTaxModel[];
    public TaxPricingTypes: DropdownItem[];
    public OrderId: number;
    @Input() Currency: any;

    public DisplayCurrency: any;

    constructor(private fb: FormBuilder, private invoiceService: InvoiceService, private route: ActivatedRoute) {
        this.Taxes = [];
        this.TaxPricingTypes = [];
    }

    ngOnInit() {
        this.OrderId = parseInt(this.route.snapshot.queryParamMap.get('orderId'), 10);
        this.invoiceForm.addControl('OtherProductTaxes', this.fb.array([]));

        this.invoiceService.getTaxePricingTypes(this.OrderId).subscribe(data => {
            this.TaxPricingTypes = data as DropdownItem[];
        });
    }

    ngOnChanges(change: SimpleChanges) {
        if (change.Taxes && change.Taxes.currentValue != null) {
            var currValues = change.Taxes.currentValue as OtherProductTaxModel[];
            var prevValues = change.Taxes.previousValue as OtherProductTaxModel[];
            var newTaxes = prevValues == undefined || prevValues == null ? currValues :
                currValues.filter(item => prevValues.indexOf(item) < 0);
            newTaxes.forEach((x: OtherProductTaxModel) => {
                this.addTax(x);
            });
        }
        if (change.Currency && change.Currency.currentValue) {
            var currency = change.Currency.currentValue;
            this.DisplayCurrency = currency;
        }
    }

    getForm(model: OtherProductTaxModel): FormGroup {
        var group = this.fb.group({
            OrderId: this.fb.control(model.OrderId),
            TaxId: this.fb.control(model.TaxId),
            TaxPricingTypeId: this.fb.control(model.TaxPricingTypeId, [Validators.required]),
            TaxAmountSymbol: this.fb.control(""),
            TaxDescription: this.fb.control(model.TaxDescription, [Validators.required]),
            TaxAmount: this.fb.control(model.TaxAmount, [Validators.required, Validators.pattern(/^[0-9]\d*(\.\d+)?$/)])
        });
        return group;
    }

    addTax(taxObj: OtherProductTaxModel): void {
        if (taxObj == null) {
            taxObj = new OtherProductTaxModel();
        }
        var taxGroup = this.getForm(taxObj);
        var taxArray = this.invoiceForm.get('OtherProductTaxes') as FormArray;
        if (taxArray == null || taxArray == undefined) {
            this.invoiceForm.addControl('OtherProductTaxes', this.fb.array([]));
            taxArray = this.invoiceForm.get('OtherProductTaxes') as FormArray;
        }
        taxArray.push(taxGroup);
    }

    removeTax(i: number) {
        var taxArray = this.invoiceForm.get('OtherProductTaxes') as FormArray;
        taxArray.removeAt(i);
    }

    removeOrderTaxes(_orderId: number): void {
        var taxArray = this.invoiceForm.get('OtherProductTaxes') as FormArray;
        var removedTaxIndexes = [];
        taxArray.controls.forEach(function (elem: FormGroup, idx: number) {
            if (elem.get('OrderId').value == _orderId) {
                removedTaxIndexes.push(idx);
            }
        });
        removedTaxIndexes.forEach(function (index: number) {
            taxArray.removeAt(index);
        });
    }

    isInvalid(tax: FormGroup, name: string): boolean {
        var result = tax.get(name).invalid
            &&
            (
                tax.get(name).dirty
                ||
                tax.get(name).touched
            )
        return result;
    }

    isRequired(tax: FormGroup, name: string): boolean {
        return tax.get(name).errors.required;
    }

    isPattern(tax: FormGroup, name: string): boolean {
        return tax.get(name).errors.pattern;
    }

    onTaxTypeSelect(event, tax: FormGroup): void {
        let currentObj = this;
        if (event.target.value > 0) {
            let selectedTaxType = currentObj.TaxPricingTypes[event.target.value - 1].Name;
            let displaySymbol = selectedTaxType[0];
            if (displaySymbol === '$') {
                tax.controls['TaxAmountSymbol'].setValue(this.DisplayCurrency);
            }
            else
            {
                tax.controls['TaxAmountSymbol'].setValue(displaySymbol);
            }
        }
        else {
            tax.controls['TaxAmountSymbol'].setValue("");
        }
    }
}
