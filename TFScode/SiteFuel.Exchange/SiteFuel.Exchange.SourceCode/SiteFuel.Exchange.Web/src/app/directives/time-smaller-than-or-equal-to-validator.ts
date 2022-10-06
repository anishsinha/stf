import { Directive, Input, ChangeDetectorRef } from '@angular/core';
import { AbstractControl, NG_VALIDATORS } from '@angular/forms';

@Directive({
    selector: '[timeSmallerThanOrEqualTo]',
    providers: [{
        provide: NG_VALIDATORS,
        useExisting: TimeSmallerThanOrEqualToValidator,
        multi: true
    }]
})
export class TimeSmallerThanOrEqualToValidator {

    @Input("timeSmallerThanOrEqualTo") public endTimeControl: AbstractControl = null;
    @Input("startDate") public startDate: AbstractControl = null;
    @Input("endDate") public endDate: AbstractControl = null;

    validate(startTimeControl: AbstractControl): { [key: string]: any } | null {
        //if start time is null, required validation already there
        if (startTimeControl.value) {                
            //if end time is null, then no need to set validation
            if (this.endTimeControl && this.endTimeControl.value) {
                //convert 12 hrs time to 24 hrs integer
                let _startTime = ConvertTime12to24(startTimeControl.value);
                let _endTime = ConvertTime12to24(this.endTimeControl.value);
                //if start time greater tha end time, set error
                if (this.startDate !=null && this.endDate !=null && new Date(this.startDate.value) > new Date(this.endDate.value)) {
                    if (this.endTimeControl.errors && this.endTimeControl.errors.timeGreaterThanOrEqualTo) {
                        this.endTimeControl.setErrors({ timeGreaterThanOrEqualTo: null });
                    };
                    //set error
                    startTimeControl.setErrors({ timeSmallerThanOrEqualTo: true });
                    return { timeSmallerThanOrEqualTo: true };
                } else if (this.startDate !=null && this.endDate !=null && new Date(this.startDate.value) < new Date(this.endDate.value)) {
                    return;
                }
                else if (_startTime > _endTime) {
                    //if end time has already error, them remove it
                    if (this.endTimeControl.errors && this.endTimeControl.errors.timeGreaterThanOrEqualTo) {
                        this.endTimeControl.setErrors({ timeGreaterThanOrEqualTo: null });
                    };
                    //set error
                    startTimeControl.setErrors({ timeSmallerThanOrEqualTo: true });
                    return { timeSmallerThanOrEqualTo: true };
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
