import { Input, Output, EventEmitter, ElementRef, Directive, Renderer2, SimpleChanges } from "@angular/core";
import * as moment from "moment";
import { WeekDays } from "src/app/app.enum";

declare var jQuery: any;
@Directive({
    selector: "[myDatePicker]"
})
export class DatePicker {
    @Input() public value: string = "";
    @Input() public format: string = "MM/DD/YYYY";
    @Input() public minDate: Date;
    @Input() public maxDate: Date = new Date();
    @Output() public onDateChange: EventEmitter<any> = new EventEmitter<any>();


    constructor(public el: ElementRef, public renderer: Renderer2) { }
    ngOnInit() {
        var currentObject = this;
        if (this.minDate == undefined || this.minDate == null || this.minDate.toString() == "") {
            var todayDate = new Date().getDate();
            this.minDate = new Date(new Date().setDate(todayDate - 365 * 10));
        }

        
        jQuery(this.el.nativeElement).datetimepicker({
            format: currentObject.format,
            minDate: currentObject.minDate.setHours(0, 0, 0),
            maxDate: currentObject.maxDate.setHours(23, 59, 59),
            useCurrent: false
        });
        jQuery(this.el.nativeElement).on("dp.change", function (e) {
            if (e.date != false) {
                currentObject.value = e.date.format(currentObject.format);
                currentObject.onDateChange.emit(e.date.format(currentObject.format));
            }
        });
        jQuery(this.el.nativeElement).on("dp.show", function (e) {
         
            let dateTimePicker = jQuery(currentObject.el.nativeElement).data("DateTimePicker");
            try {
                if (currentObject.maxDate) { dateTimePicker.maxDate(currentObject.maxDate); }
                if (currentObject.minDate) { dateTimePicker.minDate(currentObject.minDate); }
            } catch (e) {
                if (currentObject.minDate) { dateTimePicker.minDate(currentObject.minDate); }
                if (currentObject.maxDate) { dateTimePicker.maxDate(currentObject.maxDate); }
            }
            dateTimePicker.date(null);
            currentObject.onDateChange.emit('');
        });
    }


    //ngOnChanges(change: SimpleChanges): void {
    //    if (change.minDate && change.minDate.currentValue) {
    //        this.minDate.setHours(0, 0, 0);
    //        let cal = jQuery(this.el.nativeElement).data("DateTimePicker");
    //        if (cal) { cal.minDate(this.minDate); }
    //    }
    //    if (change.maxDate && change.maxDate.currentValue) {
    //        this.maxDate.setHours(23, 59, 59);
    //        let cal = jQuery(this.el.nativeElement).data("DateTimePicker");
    //        if (cal) { cal.maxDate(this.maxDate); }
    //    }
    //}
}


@Directive({
    selector: "[CustomDatePicker]"
})
export class CustomDatePicker {
    @Input() public value: string = "";
    @Input() public format: string = "MM/DD/YYYY";
    @Input() public minDate: Date;
    @Input() public maxDate: Date = new Date();
    @Input() public mode: string = "";
    @Input() public daysOfWeekEnable: string = "";
    @Input() public dateOfMonthEnable: string = "";
    @Input() public dateOfYearEnable: string = "";
    @Output() public onDateChange: EventEmitter<any> = new EventEmitter<any>();


    constructor(public el: ElementRef, public renderer: Renderer2) { }
    ngOnInit() {

        var currentObject = this;
        if (this.minDate == undefined || this.minDate == null || this.minDate.toString() == "") {
            var todayDate = new Date().getDate();
            this.minDate = new Date(new Date().setDate(todayDate - 365 * 10));
        }
        let daysOfWeekDisabled = [];
        let enabledDates = [];
        if (this.mode != null && this.mode != "") {

            if (this.mode.toUpperCase() == "WEEKLY") {
                const source = [0, 1, 2, 3, 4, 5, 6];
                let input = [];
                currentObject.daysOfWeekEnable.split(',').forEach(res => { input.push(WeekDays[res]) });
                const filterArray = (source, input) => {
                    const filtered = source.filter(el => {
                        return input.indexOf(el) === -1;
                    });
                    return filtered;
                };
                daysOfWeekDisabled = filterArray(source, input);
            }
            else if (this.mode.toUpperCase() == "MONTHLY") {

                let start = moment().startOf('month').subtract(new Date().getFullYear() - this.minDate.getFullYear(), 'years');
                let end = moment().startOf('month').add(this.maxDate.getFullYear() - new Date().getFullYear(), 'years');
                while (start.isBefore(end)) {
                    enabledDates.push(start.clone());
                    start.add(1, 'month');
                }
            }

            else if (this.mode.toUpperCase() == "ANNUALY") {
                let start = moment().startOf('year').subtract(new Date().getFullYear() - this.minDate.getFullYear(), 'years');
                let end = moment().startOf('year').add(this.maxDate.getFullYear() - new Date().getFullYear(), 'years');
                while (start.isBefore(end)) {
                    enabledDates.push(start.clone());
                    start.add(1, 'year');
                }


            }
        }

        jQuery(this.el.nativeElement).datetimepicker({
            format: currentObject.format,
            minDate: currentObject.minDate.setHours(0, 0, 0),
            maxDate: currentObject.maxDate.setHours(23, 59, 59),
            daysOfWeekDisabled: daysOfWeekDisabled,
            keepInvalid: true,
            enabledDates: enabledDates
        });
        jQuery(this.el.nativeElement).on("dp.change", function (e) {
            if (e.date != false) {
                currentObject.value = e.date.format(currentObject.format);
                currentObject.onDateChange.emit(e.date.format(currentObject.format));
            }
        });
        jQuery(this.el.nativeElement).on("dp.show", function (e) {

            let dateTimePicker = jQuery(currentObject.el.nativeElement).data("DateTimePicker");
            try {
                if (currentObject.maxDate) { dateTimePicker.maxDate(currentObject.maxDate); }
                if (currentObject.minDate) { dateTimePicker.minDate(currentObject.minDate); }

            } catch (e) {
                if (currentObject.minDate) { dateTimePicker.minDate(currentObject.minDate); }
                if (currentObject.maxDate) { dateTimePicker.maxDate(currentObject.maxDate); }
            }
            dateTimePicker.date(null);
            currentObject.onDateChange.emit('');
        });
    }
   
}

@Directive({
    selector: "[myTimePicker]"
})
export class TimePicker {
    @Input() public value: string = "";
    @Input() public format: string = "hh:mm:ss A";
    @Output() public onTimeChange: EventEmitter<any> = new EventEmitter<any>();

    constructor(public el: ElementRef, public renderer: Renderer2) { }
    ngOnInit() {
        var currentObject = this;
        // jQuery(this.el.nativeElement).keydown(function (e) {
        //     return false;
        // })
        jQuery(this.el.nativeElement).datetimepicker({
            format: currentObject.format,
        });
        jQuery(this.el.nativeElement).on("dp.change", function (e) {
            if (e.date != false) {
                currentObject.value = e.date.format(currentObject.format);
                currentObject.onTimeChange.emit(e.date.format(currentObject.format));
            }
        });
    }
}
