import { Directive, Input } from '@angular/core';
import { AbstractControl, NG_VALIDATORS } from '@angular/forms';

@Directive({
    selector: '[requiredIfNotEmpty]',
    providers: [{
        provide: NG_VALIDATORS,
        useExisting: RequiredIfNotEmptyValidator,
        multi: true
    }]
})
export class RequiredIfNotEmptyValidator {

    @Input('requiredIfNotEmpty') public requiredIfNotEmpty: AbstractControl = null;

    validate(control: AbstractControl): { [key: string]: any } | null {

        let secondControl = this.requiredIfNotEmpty;
        let baseControl = control;
        //both controls has values or both controls are empty
        if ((secondControl.value && baseControl.value) || (!secondControl.value && !baseControl.value)) {
            //if already required validation ==> remove required validation
            if (baseControl.errors && baseControl.errors.required) {
                baseControl.setErrors({ required: null });
                baseControl.updateValueAndValidity();
            }
            //if already required validation ==> remove required validation
            if (secondControl.errors && secondControl.errors.required) {
                secondControl.setErrors({ required: null });
                secondControl.updateValueAndValidity();
            }
        }
        //value && empty
        else if (secondControl.value && !baseControl.value) {
            //if already required validation ==> no need to set required validation
            if (baseControl.errors && baseControl.errors.required) { return; }
            else {
                //set required validation
                baseControl.setErrors({ required: true });
                return { required: true };
            }
        }
        //empty && value
        else if (!secondControl.value && baseControl.value) {
            //if already required ==> no need to set required
            if (secondControl.errors && secondControl.errors.required) { return; }
            else {
                //set validation
                secondControl.setErrors({ required: true });
            }
        }
    }
}