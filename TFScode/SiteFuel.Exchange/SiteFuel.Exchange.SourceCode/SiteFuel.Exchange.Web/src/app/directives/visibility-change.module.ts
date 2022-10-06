import { CommonModule } from '@angular/common';
import { ElementRef, EventEmitter, Output, Directive, AfterViewInit, Input, NgModule } from '@angular/core';

@Directive({
    selector: '[visibilityChange]'
})
export class VisibilityChangeDirective implements AfterViewInit {

    @Output() public visibilityChange: EventEmitter<any> = new EventEmitter();
    
    @Input() public threshold: number;

    private _intersectionObserver?: IntersectionObserver;

    constructor(private _element: ElementRef) { }

    private checkForIntersection = (entries: Array<IntersectionObserverEntry>) => {
        entries.forEach((entry: IntersectionObserverEntry) => {
            if (this.checkIfIntersecting(entry)) {
                this.visibilityChange.emit();
            }
        });
    }

    private registerIntersectionObserver(): void {
        if (!!this._intersectionObserver) {
            return;
        }
        this._intersectionObserver = new IntersectionObserver(entries => {
            this.checkForIntersection(entries);
        }, {
            threshold: this.threshold ? this.threshold : 0.0
        });
    }

    private checkIfIntersecting(entry: IntersectionObserverEntry) {
        return (<any>entry).isIntersecting && entry.target === this._element.nativeElement;
    }

    ngAfterViewInit(): void {
        this.registerIntersectionObserver();
        if (this._intersectionObserver && this._element.nativeElement) {
            this._intersectionObserver.observe(<Element>(this._element.nativeElement));
        }
    }
}

@NgModule({
    declarations: [VisibilityChangeDirective],
    imports: [CommonModule],
    exports: [VisibilityChangeDirective]
})
export class VisibilityChangeModule { }