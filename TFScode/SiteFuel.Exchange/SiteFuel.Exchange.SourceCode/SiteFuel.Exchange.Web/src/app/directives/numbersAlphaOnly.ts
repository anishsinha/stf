import { Directive, ElementRef, HostListener, Input } from "@angular/core";
import { NgControl } from '@angular/forms';
@Directive({
    selector: '[numbersAlphaOnly]'
})
export class OnlynumberAlphaDirective {
    private regex: RegExp = new RegExp(/^[a-zA-Z0-9]*$/);
    private specialKeys: Array<string> = ['Backspace', 'End', 'Home', 'ArrowLeft', 'ArrowRight', 'Del', 'Delete'];
    private specialCopyKeys: Array<string> = ['KeyC', 'KeyV','Tab'];
    constructor(public el: ElementRef, public formControl: NgControl) { }
    @HostListener('keydown', ['$event'])
    onKeyDown(event: KeyboardEvent) {
        let current: string = this.el.nativeElement.value;
        let next: string = current.concat(event.key);
        if (this.specialKeys.indexOf(event.key) !== -1) {
            return;
        }
        if (next && !String(next).match(this.regex)) {
            if (this.specialCopyKeys.indexOf(event.key) !== -1) {
                this.el.nativeElement.value = '';
                this.formControl.control.setValue('');
            }
            event.preventDefault();
        }
    }
    
}