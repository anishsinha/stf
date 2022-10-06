import { Injectable } from '@angular/core';
import { FormBuilder, Validators, FormGroup, FormArray, AbstractControl } from '@angular/forms';


@Injectable({
    providedIn: 'root'
})
export class ValidationService {

    constructor(private readonly fb: FormBuilder) { }

    removeError(control: AbstractControl, error: string) {
        const existingErrors = control.errors;
        if (existingErrors) {
            delete existingErrors[error];
            if (!Object.keys(existingErrors).length) {
                control.setErrors(null);
            } else {
                control.setErrors(existingErrors);
            }
        }
    }

    addError(control: AbstractControl, error: string) {
        let errorToSet = {};
        errorToSet[error] = true;
        control.setErrors({ ...control.errors, ...errorToSet });
    }
}