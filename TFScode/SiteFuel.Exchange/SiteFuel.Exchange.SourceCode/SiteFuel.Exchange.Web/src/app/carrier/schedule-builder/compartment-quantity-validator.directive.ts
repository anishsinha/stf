import { Directive, Input } from '@angular/core';
import { NG_VALIDATORS, AbstractControl } from '@angular/forms';
import { RegExConstants } from 'src/app/app.constants';


@Directive({
    selector: '[ValidateCompartmentsQuantity]',
    providers: [{
        provide: NG_VALIDATORS,
        useExisting: CompartmentQuantityValidatorDirective,
        multi: true
    }]
})
export class CompartmentQuantityValidatorDirective {

    @Input("ValidateCompartmentsQuantity")
    public DeliveryRequest: AbstractControl = null;

    validate(drControl: AbstractControl): { [key: string]: any } | null {
        if (drControl.value && this.DeliveryRequest) {
            let _compartments = this.DeliveryRequest['controls'].Compartments['controls'];
            if (_compartments && _compartments.length > 0) {
                let drQty = drControl.value; let compQty = 0;

                for (var idx = 0; idx < _compartments.length; idx++) {
                    let compartment = _compartments[idx]['controls']['Quantity'];
                    if (compartment && compartment.value && RegExConstants.DecimalNumber.test(compartment.value))
                        compQty = compQty + parseFloat(compartment.value);
                }
                if (compQty > drQty) {
                    return { CompartmentQuantity: true };
                }
            }
        }
        return null;
    }
}
