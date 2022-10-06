import { Component, OnInit, SimpleChanges, forwardRef, Input, OnChanges, SimpleChange } from '@angular/core';
import { FormGroup, NG_VALUE_ACCESSOR, FormBuilder, ControlValueAccessor } from '@angular/forms';
import { CustomerModel } from '../models/DropDetail';

@Component({
    selector: 'app-customer-detail',
    templateUrl: './customer-detail.component.html',
    styleUrls: ['./customer-detail.component.css'],
    providers: [
        {
            provide: NG_VALUE_ACCESSOR,
            useExisting: forwardRef(() => CustomerDetailComponent),
            multi: true
        }
    ]
})
export class CustomerDetailComponent implements OnInit, OnChanges, ControlValueAccessor {

    public Customer: FormGroup;

    public CustomerData: CustomerModel;

    @Input() public IsVariousOrigin: boolean;

    constructor(private fb: FormBuilder) { }

    ngOnInit() {
        this.Customer = this.fb.group({
            CompanyId: this.fb.control(''),
            CompanyName: this.fb.control(''),
            Location: this.fb.group({
                JobId: this.fb.control(''),
                SiteName: this.fb.control(''),
                Address: this.fb.control(''),
                City: this.fb.control(''),
                StateCode: this.fb.control(''),
                ZipCode: this.fb.control(''),
                CountryId: this.fb.control('')
            }),
            ContactName: this.fb.control(''),
            ContactPhone: this.fb.control(''),
            ContactEmail: this.fb.control('')
        });
        this.registerOnChange((data: CustomerModel) => {
            this.CustomerData = data;
            if (this.IsVariousOrigin) {
                this.CustomerData.Location.Address = null;
                this.CustomerData.Location.City = null;
                this.CustomerData.Location.ZipCode = null;
            }
        });
    }

    ngOnChanges(change: SimpleChanges) {
        if (change.IsVariousOrigin) {
            this.IsVariousOrigin = change.IsVariousOrigin.currentValue || false;
        }
    }

    //----------------- DO NOT MODIFY: Control Value accessor ----------------------

    public onTouched: () => void = () => { };

    writeValue(val: any): void {
        val && this.Customer.patchValue(val, { emitEvent: true });
    }
    registerOnChange(fn: any): void {
        this.Customer.valueChanges.subscribe(fn);
    }
    registerOnTouched(fn: any): void {
        this.onTouched = fn;
    }
    setDisabledState?(isDisabled: boolean): void {
        isDisabled ? this.Customer.disable() : this.Customer.enable();
    }

    //----------------- DO NOT MODIFY: Control Value accessor ----------------------
}
