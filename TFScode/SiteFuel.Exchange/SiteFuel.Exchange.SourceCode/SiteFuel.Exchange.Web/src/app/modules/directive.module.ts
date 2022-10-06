
import { NgModule } from '@angular/core';
import { DatePicker, CustomDatePicker, TimePicker } from '../directives/myDateTimePicker';
import { startsWithPipe, startsWithJobPipe } from '../carrier-companies/search-filter';
import { RequiredIfTrueValidator } from '../directives/required-if-true.directive';
import { RequiredIfNotEmptyValidator } from '../directives/required-if-not-empty.directive';
import { TimeSmallerThanOrEqualToValidator } from '../directives/time-smaller-than-or-equal-to-validator';
import { TimeGreaterThanOrEqualToValidator } from '../directives/time-greater-than-or-equal-to-validator';
import { NumberWithDecimal } from '../directives/numberWithDecimal';
import { OnlynumberDirective } from '../directives/numbersOnly';
import { OnlynumberAlphaDirective } from '../directives/numbersAlphaOnly';
import { ClickOutsideDirective } from '../directives/click-outside.directive';
import { DisableControlDirective } from '../directives/disable-control.directive';
import { CopyDirective } from '../directives/copy.directive';
import { SortingPipe } from '../directives/sorting.pipe';


@NgModule({

    declarations: [
        DatePicker,
        CustomDatePicker,
        TimePicker,
        startsWithPipe,
        startsWithJobPipe,
        RequiredIfTrueValidator,
        RequiredIfNotEmptyValidator,
        TimeSmallerThanOrEqualToValidator,
        TimeGreaterThanOrEqualToValidator,
        NumberWithDecimal,
        ClickOutsideDirective,
        DisableControlDirective,
        CopyDirective,
        SortingPipe,
        OnlynumberDirective,
        OnlynumberAlphaDirective
    ],

    imports: [

    ],

    exports: [
        DatePicker,
        CustomDatePicker,
        TimePicker,
        startsWithPipe,
        startsWithJobPipe,
        RequiredIfTrueValidator,
        RequiredIfNotEmptyValidator,
        TimeSmallerThanOrEqualToValidator,
        TimeGreaterThanOrEqualToValidator,
        NumberWithDecimal,
        ClickOutsideDirective,
        DisableControlDirective,
        CopyDirective,
        SortingPipe,
        OnlynumberDirective,
        OnlynumberAlphaDirective
    ]
})

export class DirectiveModule { }
