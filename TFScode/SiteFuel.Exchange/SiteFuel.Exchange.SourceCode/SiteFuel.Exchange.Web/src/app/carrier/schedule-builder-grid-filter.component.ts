import { Component, OnInit, Input, EventEmitter, Output } from '@angular/core';
import { SbGridViewFilterModel } from './models/DispatchSchedulerModels';
@Component({
    selector: 'app-schedule-builder-grid-filter',
    templateUrl: './schedule-builder-grid-filter.component.html',
    styleUrls: ['./schedule-builder-grid-filter.component.css']
})
export class ScheduleBuilderGridFilterComponent implements OnInit {

    constructor() { }
    @Input() public SbGridFilterData: SbGridViewFilterModel;
    @Output() public OnSbGridFilterApply: EventEmitter<any> = new EventEmitter<any>();
    @Input() public isDisableControl: boolean;

    ngOnInit() {
    }
    filterLocation() {
        this.OnSbGridFilterApply.emit({ SearchLocation: this.SbGridFilterData.SearchLocation});
    }
    resetfilterLocation() {
        this.SbGridFilterData.SearchLocation = '';
        this.OnSbGridFilterApply.emit({ SearchLocation: this.SbGridFilterData.SearchLocation });
    }
}
