import { Component, EventEmitter, OnInit, Output } from '@angular/core';
import { IDropdownSettings } from 'ng-multiselect-dropdown';
import * as moment from 'moment';
import { LiftfiledashboardserviceService } from '../service/liftfiledashboardservice.service';
import { DropDownItem } from '../LiftFileModels';
import { Declarations } from 'src/app/declarations.module';
import { LFVRecordStatus } from 'src/app/app.enum';
declare var matchingWindowDays;

@Component({
  selector: 'app-left-side-filter',
  templateUrl: './left-side-filter.component.html',
  styleUrls: ['./left-side-filter.component.css']
})

export class LeftSideFilterComponent implements OnInit {
  DateType = 3;
  CarrierDrpDwnList: DropDownItem[] = [];
  //min max date
  @Output() search = new EventEmitter<any>();
  @Output() export = new EventEmitter<any>();
  public minfromdate: Date = new Date();
  matchingWindowDays = matchingWindowDays;
  public fromDate: any;
  public toDate: any;
  public selectedCarrierList: DropDownItem[] = [];
  public multiselectSettingsById: IDropdownSettings;
  public isMatchingWindow = false;
  selectedStatus=LFVRecordStatus.Clean;
  public LFVRecordStatus=LFVRecordStatus;

    constructor(private _lfvSevice: LiftfiledashboardserviceService) {
        this.toDate = moment().format('MM/DD/YYYY');
        this.fromDate = moment(this.toDate, "MM/DD/YYYY").add(-7, 'days').format('MM/DD/YYYY');
        this.minfromdate = moment(this.toDate).add(-3, 'months').startOf('month').toDate(); //moment(this.toDate, "MM/DD/YYYY").add(-3, 'months').format('MM/DD/YYYY');
    }

  ngOnInit() {
    this.getLFCarrier();
    this.multiselectSettingsById = {
      singleSelection: false,
      idField: 'DisplayName',
      textField: 'DisplayName',
      itemsShowLimit: 2,
      allowSearchFilter: true,
      enableCheckAll: true
    }

  }

    private getLFCarrier() {
        let fromDate = this.fromDate
        let toDate = this.toDate;
        this._lfvSevice.getLFCarrier(fromDate, toDate).subscribe(res => {
            if (res) {
                this.CarrierDrpDwnList = res;
                this.CarrierDrpDwnList && this.CarrierDrpDwnList.map(m => { m.DisplayName = `${m.Name}-${m.Code}` })
                //code=>carrierId
                //name=CarrierName
            }
            else
                this.CarrierDrpDwnList = [];
        })
    }

  public changeDateType(value): void {
    this.DateType = value;
    if (this.DateType == 1) {
      this.toDate = moment().format('MM/DD/YYYY');
      this.fromDate = moment(this.toDate, "MM/DD/YYYY").add(-1, 'days').format('MM/DD/YYYY');
      this.isMatchingWindow = false;
    } else if (this.DateType == 3) {
      this.toDate = moment().format('MM/DD/YYYY');
      this.fromDate = moment(this.toDate, "MM/DD/YYYY").add(-7, 'days').format('MM/DD/YYYY');
      this.isMatchingWindow = false;
    } else {
      this.isMatchingWindow = true;
      var day = this.matchingWindowDays ? this.matchingWindowDays : 3;//3 is default 
      this.fromDate = moment(this.toDate, "MM/DD/YYYY").add(-day, 'days').format('MM/DD/YYYY');

    }

    this.onSearch();
  }

  public setFromDate(event: any): void {
    this.fromDate = event;

  }

    public setToDate(event: any): void {
        this.toDate = event;
        this.fromDate = moment(this.toDate, "MM/DD/YYYY").add(-7, 'days').format('MM/DD/YYYY');
        if (moment(this.fromDate).toDate() <= this.minfromdate)
            this.fromDate = this.toDate;
    }

  async onCarrierSelect($event) {

    await this.selectedCarrierList.map(m => { m.Name = this.CarrierDrpDwnList.find(f => f.DisplayName == m.DisplayName).Code })
  }
  async onCarrierDeSelect($event) {

    await this.selectedCarrierList.map(m => { m.Name = this.CarrierDrpDwnList.find(f => f.DisplayName == m.DisplayName).Code })
  }
  public onSearch(): void {
    var startDate = moment(this.fromDate, "MM/DD/YYYY");
    var endDate = moment(this.toDate, "MM/DD/YYYY");
    var result = endDate.diff(startDate, 'days');
    if (result > 8) {
      Declarations.msgwarning("Date Difference should be less than 7 days", undefined, undefined);
    } else
      this.search.emit(true);
  }

  public onExport() {
    this.export.emit(this.selectedStatus);
  }
}
