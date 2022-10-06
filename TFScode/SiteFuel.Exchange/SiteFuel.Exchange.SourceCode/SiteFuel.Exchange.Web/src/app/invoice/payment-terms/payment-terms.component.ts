import { Component, OnInit, forwardRef } from '@angular/core';
import { FormGroup, NG_VALUE_ACCESSOR, FormBuilder, Validators, ControlValueAccessor } from '@angular/forms';

@Component({
    selector: 'app-payment-terms',
    templateUrl: './payment-terms.component.html',
    styleUrls: ['./payment-terms.component.css'],
    providers: [
        {
            provide: NG_VALUE_ACCESSOR,
            useExisting: forwardRef(() => PaymentTermsComponent),
            multi: true
        }
    ]
})
export class PaymentTermsComponent implements OnInit, ControlValueAccessor {

    public PaymentTerm: FormGroup;

    constructor(private fb: FormBuilder) { }

    ngOnInit() {
        this.PaymentTerm = this.fb.group({
            TermId: this.fb.control(''),
            NetDays: this.fb.control('0', Validators.required)
        });
    }

    readonlyNetDays(termId: number) {
        var netdays = this.PaymentTerm.get('NetDays');
        if (netdays.value == '') {
            netdays.setValue('0');
        }
        if (termId == 1) {
            netdays.enable();
        } else {
            netdays.disable();
        }
    }

    isInvalid(name: string): boolean {
        return this.PaymentTerm.get(name).invalid &&
            (this.PaymentTerm.get(name).dirty || this.PaymentTerm.get(name).touched);
    }

    isRequired(name: string): boolean {
        return this.PaymentTerm.get(name).errors.required;
    }

    //----------------- DO NOT MODIFY: Control Value accessor ----------------------

    public onTouched: () => void = () => { };

    writeValue(val: any): void {
        val && this.PaymentTerm.setValue(val, { emitEvent: true });
    }
    registerOnChange(fn: any): void {
        this.PaymentTerm.valueChanges.subscribe(fn);
    }
    registerOnTouched(fn: any): void {
        this.onTouched = fn;
    }
    setDisabledState?(isDisabled: boolean): void {
        isDisabled ? this.PaymentTerm.disable() : this.PaymentTerm.enable();
    }

    //----------------- DO NOT MODIFY: Control Value accessor ----------------------
}
