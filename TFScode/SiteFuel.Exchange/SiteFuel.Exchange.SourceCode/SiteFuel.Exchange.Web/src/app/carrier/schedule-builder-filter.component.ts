import { Component, OnInit, Input, EventEmitter, Output, ChangeDetectionStrategy } from '@angular/core';
import { SbFilterModel } from './models/DispatchSchedulerModels';

@Component({
    selector: 'app-schedule-builder-filter',
    templateUrl: './schedule-builder-filter.component.html',
    styleUrls: ['./schedule-builder-filter.component.css'],
    changeDetection: ChangeDetectionStrategy.OnPush
})
export class ScheduleBuilderFilterComponent implements OnInit {
    constructor() { }

    @Input() public SbFilterData: SbFilterModel;
    @Input() public isDisableControl: boolean;
    @Output() public OnSbFilterApply: EventEmitter<any> = new EventEmitter<any>();

    public trailerDdlSettings = {};
    public driverDdlSettings = {};
    public pickupDdlSettings = {};
    @Input() public IsTrailerExists: boolean;

    ngOnInit() {
        this.pickupDdlSettings = {
            singleSelection: false,
            idField: 'Code',
            textField: 'Name',
            selectAllText: 'Select All',
            unSelectAllText: 'UnSelect All',
            itemsShowLimit: 5,
            allowSearchFilter: true
        };
        this.driverDdlSettings = {
            singleSelection: false,
            idField: 'Id',
            textField: 'Name',
            selectAllText: 'Select All',
            unSelectAllText: 'UnSelect All',
            itemsShowLimit: 5,
            allowSearchFilter: true
        };
        this.trailerDdlSettings = {
            singleSelection: false,
            idField: 'Id',
            textField: 'TrailerId',
            selectAllText: 'Select All',
            unSelectAllText: 'UnSelect All',
            itemsShowLimit: 2,
            allowSearchFilter: true,
        };
    }

    onApplyFilter() {
        this.OnSbFilterApply.emit({ SelectedPickups: this.SbFilterData.SelectedPickups, SelectedDrivers: this.SbFilterData.SelectedDrivers, SelectedTrailers: this.SbFilterData.SelectedTrailers });
    }

    onTrailerSelect(trailer: any) {
        this.onApplyFilter();
    }
    onTrailerDeSelect(trailer: any) {
        this.onApplyFilter();
    }
    onTrailerSelectAll(trailers: any) {
        this.SbFilterData.SelectedTrailers = [];
        trailers.forEach(obj => this.SbFilterData.SelectedTrailers.push(obj));
        this.onApplyFilter();
    }
    onTrailerDeSelectAll() {
        this.SbFilterData.SelectedTrailers = [];
        this.onApplyFilter();
    }

    onDriverSelect(driver: any) {
        this.onApplyFilter();
    }
    onDriverDeSelect(driver: any) {
        this.onApplyFilter();
    }
    onDriverSelectAll(drivers: any) {
        this.SbFilterData.SelectedDrivers = [];
        drivers.forEach(obj => this.SbFilterData.SelectedDrivers.push(obj));
        this.onApplyFilter();
    }
    onDriverDeSelectAll() {
        this.SbFilterData.SelectedDrivers = [];
        this.onApplyFilter();
    }

    onPickupSelect(pickup: any) {
        this.onApplyFilter();
    }
    onPickupDeSelect(pickup: any) {
        this.onApplyFilter();
    }
    onPickupSelectAll(pikups: any) {
        this.SbFilterData.SelectedPickups = [];
        pikups.forEach(obj => this.SbFilterData.SelectedPickups.push(obj));
        this.onApplyFilter();
    }
    onPickupDeSelectAll() {
        this.SbFilterData.SelectedPickups = [];
        this.onApplyFilter();
    }

    resetFilter() {
        this.SbFilterData.SelectedDrivers = [];
        this.SbFilterData.SelectedTrailers = [];
        this.SbFilterData.SelectedPickups = [];
        this.onApplyFilter();
    }
}
