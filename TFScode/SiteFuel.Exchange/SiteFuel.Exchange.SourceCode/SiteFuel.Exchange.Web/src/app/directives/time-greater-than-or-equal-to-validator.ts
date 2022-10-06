import { Directive, Input } from '@angular/core';
import { AbstractControl, NG_VALIDATORS } from '@angular/forms';

@Directive({
    selector: '[timeGreaterThanOrEqualTo]',
    providers: [{
        provide: NG_VALIDATORS,
        useExisting: TimeGreaterThanOrEqualToValidator,
        multi: true
    }]
})
export class TimeGreaterThanOrEqualToValidator {

    @Input("timeGreaterThanOrEqualTo") public startTimeControl: AbstractControl = null;
    @Input("startDate") public startDate: AbstractControl = null;
    @Input("endDate") public endDate: AbstractControl = null;

    validate(endTimeControl: AbstractControl): { [key: string]: any } | null {
        //if end time is null, required validation already there
        if (endTimeControl.value) {
            //if start time is null, then no need to set validation
            if (this.startTimeControl && this.startTimeControl.value) {
                //convert 12 hrs time to 24 hrs integer
                let _startTime = ConvertTime12to24(this.startTimeControl.value);
                let _endTime = ConvertTime12to24(endTimeControl.value);
                //if end time greater than start time, set error
            
                if (this.startDate !=null && this.endDate !=null && new Date(this.startDate.value) > new Date(this.endDate.value)) {
                    //if start time has already error, them remove it
                    if (this.startTimeControl.errors && this.startTimeControl.errors.timeSmallerThanOrEqualTo) {
                        this.startTimeControl.setErrors({ timeSmallerThanOrEqualTo: null });
                    };
                    //set error
                    endTimeControl.setErrors({ timeGreaterThanOrEqualTo: true });
                    return { timeGreaterThanOrEqualTo: true };
                } else if (this.startDate !=null && this.endDate !=null && new Date(this.startDate.value) < new Date(this.endDate.value)) {
                    return;
                }
                else if (_startTime > _endTime) {
                    //if start time has already error, them remove it
                    if (this.startTimeControl.errors && this.startTimeControl.errors.timeSmallerThanOrEqualTo) {
                        this.startTimeControl.setErrors({ timeSmallerThanOrEqualTo: null });
                    };
                    //set error
                    endTimeControl.setErrors({ timeGreaterThanOrEqualTo: true });
                    return { timeGreaterThanOrEqualTo: true };
                }
            }
        }
        return;
    }
}
//convert 12 hrs time to 24 hrs integer
function ConvertTime12to24(time12h: any) {
    const [time, modifier] = time12h.split(' ');
    let [hours, minutes, seconds] = time.split(':');
    if (hours === '12') { hours = '00'; }
    if (modifier === 'PM') { hours = parseInt(hours, 10) + 12; }
    return `${hours || '00'}${minutes || '00'}${seconds || '00'}`;
}
