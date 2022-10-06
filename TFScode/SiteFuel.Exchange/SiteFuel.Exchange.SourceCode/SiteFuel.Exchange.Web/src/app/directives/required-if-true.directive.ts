import { Directive, Input, OnChanges, SimpleChanges, ChangeDetectorRef, ElementRef } from '@angular/core';
import { AbstractControl, NG_VALIDATORS, Validators } from '@angular/forms';

@Directive({
    selector: '[requiredIfTrue]',
    providers: [{
        provide: NG_VALIDATORS,
        useExisting: RequiredIfTrueValidator,
        multi: true
    }]
})
export class RequiredIfTrueValidator implements OnChanges {

    pastControl: AbstractControl;

    @Input('requiredIfTrue') public requiredIfTrue: boolean;

    constructor(private cdr: ChangeDetectorRef) { }

    validate(control: AbstractControl): { [key: string]: any } | null {

        this.pastControl = control;
        if (this.requiredIfTrue && (control.value == '' || control.value == undefined || control.value == null)) {
            return { required: true };
        }
        else {
            return null;
        }
    }

    ngOnChanges(change: SimpleChanges) {
        //not first change of requiredIfTrue
        if (!change.requiredIfTrue.firstChange) {
            if (change.requiredIfTrue && !change.requiredIfTrue.currentValue) {
                this.pastControl.setErrors(null);
                this.pastControl.updateValueAndValidity();
                this.cdr.detectChanges();
            }
        }
        //chnage in requiredIfTrue
        if (change.requiredIfTrue && change.requiredIfTrue.currentValue) {
            if (this.pastControl != undefined || this.pastControl != null) {
                this.pastControl.updateValueAndValidity();
                this.cdr.detectChanges();
            }          
        }
    }
}