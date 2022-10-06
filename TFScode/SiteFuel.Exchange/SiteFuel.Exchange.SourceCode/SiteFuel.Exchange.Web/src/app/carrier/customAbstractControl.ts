import { FormGroup, FormArray, ValidationErrors, AbstractControl, FormControl } from '@angular/forms';

export class CustomAbstractControls {
    public static findRecursiveErrors(formToInvestigate: FormGroup | FormArray): string[] {
        var invalidControls: string[] = [];
        let recursiveFunc = (form: FormGroup | FormArray,nestedFormGroup=null) => {
            Object.keys(form.controls).forEach(field => {
                const control = form.get(field);
                if (control.invalid && control.errors) {
                    if (!nestedFormGroup)
                        invalidControls.push(field); 
                    else
                        invalidControls.push(nestedFormGroup + ' ' +field);
                }
                if (control instanceof FormGroup) {
                    recursiveFunc(control,field);
                } else if (control instanceof FormArray) {
                    recursiveFunc(control,null);
                }
            });
        }
        recursiveFunc(formToInvestigate);
        return invalidControls;
	}
    public static findErrors(formToInvestigate: FormGroup | FormArray): string[] {
        var invalidControls: string[] = [];
        let recursiveFunc = (form: FormGroup | FormArray, nestedFormGroup = null) => {
            Object.keys(form.controls).forEach(field => {
                const control = form.get(field);
                if (control.invalid && control.errors) {
                    if (!nestedFormGroup)
                        invalidControls.push(field);
                    else
                        invalidControls.push(field);
                }
                if (control instanceof FormGroup) {
                    recursiveFunc(control, field);
                } else if (control instanceof FormArray) {
                    recursiveFunc(control, null);
                }
            });
        }
        recursiveFunc(formToInvestigate);
        return invalidControls;
    }
	public static cloneForm<T extends AbstractControl>(control: T): T {
		let cloneControl: T;
		if (control instanceof FormGroup) {
			const formGroup = new FormGroup({}, control.validator, control.asyncValidator);
			const controls = control.controls;
			Object.keys(controls).forEach(key => {
				formGroup.addControl(key, this.cloneForm(controls[key]));
			})
			cloneControl = formGroup as any;
		}
		else if (control instanceof FormArray) {
			const formArray = new FormArray([], control.validator, control.asyncValidator);
			control.controls.forEach(formControl => formArray.push(this.cloneForm(formControl)))
			cloneControl = formArray as any;
		}
		else if (control instanceof FormControl) {
			cloneControl = new FormControl(control.value, control.validator, control.asyncValidator) as any;
		}
		else {
			throw new Error('Error: invalid control');
		}
		if (control.disabled) cloneControl.disable({ emitEvent: false });
		return cloneControl;
	}
}