import { Input, Output, EventEmitter, ElementRef, Directive, HostListener } from "@angular/core";
import { NgControl } from '@angular/forms';
@Directive({
    selector: "[numberWithDecimal]"
})
export class NumberWithDecimal {

    private regex: RegExp = new RegExp(/^[0-9]+(\.[0-9]*){0,1}$/g);
    private specialKeys: Array<string> = ['Backspace', 'Tab', 'End', 'Home', 'Delete', 'ArrowRight', 'ArrowLeft', 'Control', 'KeyC', 'KeyV', 'KeyZ', 'KeyX','Enter'];

    constructor(public el: ElementRef, public formControl: NgControl) { }
    @HostListener('keydown', ['$event'])
    onKeyDown(event: KeyboardEvent) {
        let current: string = this.el.nativeElement.value;
        let next: string = current.concat(event.key);
        if (this.specialKeys.indexOf(event.key) !== -1) {
            return;
        }
        //Allow copy event
        if ((event.ctrlKey || event.metaKey) && (event.keyCode == 67)) {
            return;
        }
        //Allow Cut event {
        if ((event.ctrlKey || event.metaKey) && (event.keyCode == 88)) {
            return;
        }
        //Allow paste event 
        if ((event.ctrlKey || event.metaKey) && (event.keyCode == 86)) {
            return;
        }
        if (next && !String(next).match(this.regex)) {
            this.formControl.control.setErrors({ invalidinput: true });
            event.preventDefault();
        }
    }
}