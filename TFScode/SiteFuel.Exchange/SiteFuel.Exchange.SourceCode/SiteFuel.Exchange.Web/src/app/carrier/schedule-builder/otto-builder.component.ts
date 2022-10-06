import { Component, OnInit, ChangeDetectionStrategy, AfterViewInit, ChangeDetectorRef, Input } from '@angular/core';
import { ShiftViewModel, DeliveryRequestViewModel, OttoTripModel, OttoBuilder } from '../models/DispatchSchedulerModels';
import { ScheduleBuilderService } from '../service/schedule-builder.service';
import { MyLocalStorage } from 'src/app/my.localstorage';
import { DataService } from 'src/app/services/data.service';
import { Declarations } from 'src/app/declarations.module';

@Component({
    selector: 'app-otto-builder',
    templateUrl: './otto-builder.component.html',
    styleUrls: ['./otto-builder.component.css'],
    changeDetection: ChangeDetectionStrategy.OnPush
})
export class OttoBuilderComponent implements OnInit, AfterViewInit {
    public _loadingOttoDRs: boolean = true;
    public _loadingShifts: boolean = true;
    public _EnableScheduleDR: boolean = false;

    @Input() public SelectedDate: string;
    @Input() public DSBFilter: number;

    public SelectedRegionId: string;
    public SelectedShiftId: string;
    private ShiftStartTime: string = null;
    private ShiftEndTime: string = null;

    public MinInputDate: Date = new Date();
    public MaxInputDate: Date = new Date();

    public Shifts: ShiftViewModel[] = [];
    public Loads: OttoTripModel[] = [];
    public AllDeliveryRequests: DeliveryRequestViewModel[] = [];
    public SelectedDeliveryRequests: DeliveryRequestViewModel[] = [];
    public SelectedLoads: Map<number, DeliveryRequestViewModel[]> = new Map<number, DeliveryRequestViewModel[]>();

    constructor(private sbService: ScheduleBuilderService, private dataService: DataService,
        private changeDetectorRef: ChangeDetectorRef) {
        this.MaxInputDate.setDate(this.MinInputDate.getDate() + 30);
        this.SelectedRegionId = MyLocalStorage.getData(MyLocalStorage.DSB_REGION_KEY);
        //this.SelectedDate = MyLocalStorage.getData(MyLocalStorage.DSB_DATE_KEY);
        this.SelectedShiftId = null;
    }

    ngOnInit() {
        if (this.DSBFilter == 1) {
            this.sbService.getShifts(this.SelectedRegionId).subscribe(x => {
                if (x) {
                    this.Shifts = x;
                    if (this.Shifts && this.Shifts.length > 0) {
                        this.SelectedShiftId = this.Shifts[0].Id;
                        this.ShiftStartTime = this.Shifts[0].StartTime;
                        this.ShiftEndTime = this.Shifts[0].EndTime;
                    }
                }
                this._loadingShifts = false;
                this.changeDetectorRef.detectChanges();
                if (this.ShiftStartTime && this.ShiftEndTime) {
                    this.getScheduleDetails(this.SelectedRegionId, this.ShiftStartTime, this.ShiftEndTime, this.SelectedDate);
                }
                else {
                    this._loadingOttoDRs = false;
                    this.changeDetectorRef.detectChanges();
                }
            });
        }
        else {
            this.sbService.getDriversShifts(this.SelectedRegionId, this.SelectedDate).subscribe(x => {
                if (x) {
                    this.Shifts = x;
                    if (this.Shifts && this.Shifts.length > 0) {
                        this.SelectedShiftId = this.Shifts[0].Id;
                        this.ShiftStartTime = this.Shifts[0].StartTime;
                        this.ShiftEndTime = this.Shifts[0].EndTime;
                    }
                }
                this._loadingShifts = false;
                this.changeDetectorRef.detectChanges();
                if (this.ShiftStartTime && this.ShiftEndTime) {
                    this.getScheduleDetails(this.SelectedRegionId, this.ShiftStartTime, this.ShiftEndTime, this.SelectedDate);
                }
                else {
                    this._loadingOttoDRs = false;
                    this.changeDetectorRef.detectChanges();
                }
            });
        }
    }

    ngAfterViewInit(): void {
        //this.getScheduleDetails(this.SelectedRegionId, this.SelectedDate);
    }

    public closeSlider(): void {
        this.dataService.setOpenDsbOttoBuilderSubject(false);
    }

    public filterByShift(): void {

        let selectedShift: ShiftViewModel = null;
        if (this.SelectedShiftId) {
            selectedShift = this.Shifts.find(x => x.Id == this.SelectedShiftId);
        }
        if (selectedShift) {
            this.ShiftStartTime = selectedShift.StartTime;
            this.ShiftEndTime = selectedShift.EndTime;
            this.getScheduleDetails(this.SelectedRegionId, this.ShiftStartTime, this.ShiftEndTime, this.SelectedDate);
            this.SelectedLoads.clear();
            this._EnableScheduleDR = false;
        }
    }

    public selectDR(loadIdx: number, drIdx: number) {
        let indexvalue = loadIdx + "_" + drIdx;
        let dr = this.Loads[loadIdx].DeliveryRequests[drIdx];
        if ($("#ottoschedule_" + indexvalue).hasClass("selected-dr")) {
            $("#ottoschedule_" + indexvalue).removeClass("selected-dr");
            let drs = this.SelectedLoads.get(loadIdx) || [];
            let drIndex = drs.findIndex(x => x == dr);
            if (drIndex >= 0) {
                drs.splice(drIndex, 1);
                this.SelectedLoads.set(loadIdx, drs);
                this._EnableScheduleDR = drs.length != 0;
            }
        } else {
            let drs = this.SelectedLoads.get(loadIdx) || [];
            drs.push(dr);
            this.SelectedLoads.set(loadIdx, drs);
            this._EnableScheduleDR = drs.length != 0;
            $("#ottoschedule_" + indexvalue).addClass("selected-dr");
        }
    }

    public filterScheduleDetails(date: string): void {
        if (date && date != '' && this.SelectedDate != date) {
            this.SelectedDate = date;
            if (this.DSBFilter == 2) {
                this.ShiftStartTime = null;
                this.ShiftEndTime = null;
                this.sbService.getDriversShifts(this.SelectedRegionId, this.SelectedDate).subscribe(x => {
                    if (x) {
                        this.Shifts = x;
                        if (this.Shifts && this.Shifts.length > 0) {
                            this.SelectedShiftId = this.Shifts[0].Id;
                            this.ShiftStartTime = this.Shifts[0].StartTime;
                            this.ShiftEndTime = this.Shifts[0].EndTime;
                        }
                    }
                    this.changeDetectorRef.detectChanges();
                    if (this.ShiftStartTime && this.ShiftEndTime) {
                        this.getScheduleDetails(this.SelectedRegionId, this.ShiftStartTime, this.ShiftEndTime, this.SelectedDate);
                        this.SelectedLoads.clear();
                        this._EnableScheduleDR = false;
                    }
                    else {
                        this._loadingOttoDRs = false;
                        this.changeDetectorRef.detectChanges();
                    }
                });
            }
            else {
                this.getScheduleDetails(this.SelectedRegionId, this.ShiftStartTime, this.ShiftEndTime, date);
                this.SelectedLoads.clear();
                this._EnableScheduleDR = false;
            }
        }
    }

    private getScheduleDetails(regionId: string, shiftStartTime: string, shiftEndTime: string, date: string) {
        this._loadingOttoDRs = true;
        this.sbService.getOttoScheduleDetails(regionId, shiftStartTime, shiftEndTime, date).subscribe(data => {
            if (data != null && data.length > 0) {
                this.Loads = data;
            }
            this._loadingOttoDRs = false;
            this.changeDetectorRef.detectChanges();
        });
    }

    public ScheduleDRs(): void {
        let ottoBuilder = new OttoBuilder();
        ottoBuilder.Date = this.SelectedDate;
        ottoBuilder.RegionId = this.SelectedRegionId;
        ottoBuilder.ShiftId = this.SelectedShiftId;
        this.SelectedLoads.forEach((value, key) => {
            let load = new OttoTripModel();
            load.DriverRowIndex = 0;
            load.DriverColIndex = key;
            load.StartTime = this.Loads[key].StartTime;
            load.EndTime = this.Loads[key].EndTime;
            load.DeliveryRequests = value;
            ottoBuilder.Loads.push(load);
        });
        this._loadingOttoDRs = true;
        this.sbService.scheduleOttoDRs(ottoBuilder).subscribe(x => {
            this._loadingOttoDRs = false;
            this.changeDetectorRef.detectChanges();
            if (x.StatusCode == 0) {
                this.dataService.setOpenDsbOttoBuilderSubject(false);
                this.dataService.refreshDsbOttoBuilderSubject(true);
                Declarations.msgsuccess(x.StatusMessage, undefined, undefined);
            } else {
                Declarations.msgerror(x.StatusMessage, undefined, undefined);
            }
        });
    }
}
