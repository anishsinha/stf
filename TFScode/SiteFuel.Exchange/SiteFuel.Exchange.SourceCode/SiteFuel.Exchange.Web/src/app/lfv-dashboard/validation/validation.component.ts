import { Component, Input, OnChanges, OnInit, SimpleChanges, ViewChild } from '@angular/core';
import * as moment from 'moment';
import { LFValidationGridViewModel } from '../LiftFileModels';
import { LiftfiledashboardserviceService } from '../service/liftfiledashboardservice.service';
declare var ApexCharts;
@Component({
  selector: 'app-validation',
  templateUrl: './validation.component.html',
  styleUrls: ['./validation.component.css']
})
export class ValidationComponent implements OnInit, OnChanges {
  @Input() LFValidationList: LFValidationGridViewModel[];
  chart: any;

  constructor(private _lfvService: LiftfiledashboardserviceService) { }

  ngOnInit() {
    this.init();
  }

  init() {

  }

  ngOnChanges(changes: SimpleChanges): void {
    if (changes.LFValidationList.currentValue && !changes.LFValidationList.isFirstChange()) {
      this.createChartData();
    }

  }


  //public openLFVScratchReportGrid(): void {
  //  window.open("Supplier/LiftFile/LFVScratchReport", "_blank");
  //}

  async RendorChart(data) {
    try {
      if (this.chart)
        this.chart.destroy();
    } catch (e) {

    }
    var options = {
        colors: ["#00FF00", "#ff0000", "#FF69B4", "#FFFF00", "#000080", "#00A7C6", "#800080", '#0077ff','#A9D794'],
      series: [{
        data: data
      }],
      chart: {
        height: 350,
        type: 'bar',
        events: {
          click: function (chart, w, e) {
             
          }
        }
      },
      // colors: colors,
      plotOptions: {
        bar: {
          columnWidth: '45%',
          distributed: true,
        }
      },
    
      dataLabels: {
        enabled: true
      },
      legend: {
        show: true
      },
      xaxis: {
        categories: [
          'Match',
          'No Match',
          'Partial Match',
          'Pending',
          'Duplicate',
         'Active Exception',
          'Ignored',
          'Forced Ignore',
          'UnMatched'          
        ],
        labels: {
          style: {
            //colors: colors,
            fontSize: '12px'
          }
        }
      },
      fill: {
        opacity: 1
        //colors: ["red", "#F27036", "#663F59", "#6A6E94", "#4E88B4", "#00A7C6", "#18D8D8", '#A9D794']
      }
    };
    this.chart = new ApexCharts(document.querySelector("#chart-timeline"), options);
    try {
      if (this.chart)
        this.chart.render();
    } catch (e) {
      this.chart.destroy();
      this.chart.render();
    }
  }

    async createChartData() {
    var lfv = new LFValidationGridViewModel();
    lfv.ActiveExceptionRecordCount = 0;
    lfv.DuplicateRecordCount = 0;
    lfv.IgnoredMatchRecordCount = 0;
    lfv.MatchedRecordCount = 0;
    lfv.NoMatchRecordCount = 0;
    lfv.PartialMatchRecordCount = 0;
    lfv.PendingMatchCount = 0;
    lfv.TotalRecordCount = 0;
    lfv.UnmatchedRecordCount = 0;
    lfv.ForcedIgnoredMatchRecordCount = 0;
    await this.LFValidationList && this.LFValidationList.map(m => {
      lfv.ActiveExceptionRecordCount += m.ActiveExceptionRecordCount;
      lfv.DuplicateRecordCount += m.DuplicateRecordCount;
      lfv.IgnoredMatchRecordCount += m.IgnoredMatchRecordCount;
      lfv.MatchedRecordCount += m.MatchedRecordCount;
      lfv.NoMatchRecordCount += m.NoMatchRecordCount;
      lfv.PartialMatchRecordCount += m.PartialMatchRecordCount;
      lfv.PendingMatchCount += m.PendingMatchCount;
      lfv.TotalRecordCount += m.TotalRecordCount;
      lfv.UnmatchedRecordCount += m.UnmatchedRecordCount;
      lfv.ForcedIgnoredMatchRecordCount += m.ForcedIgnoredMatchRecordCount;
    })
  
    let data = [lfv.MatchedRecordCount, lfv.NoMatchRecordCount, lfv.PartialMatchRecordCount
        , lfv.PendingMatchCount, lfv.DuplicateRecordCount, lfv.ActiveExceptionRecordCount
        , lfv.IgnoredMatchRecordCount,lfv.ForcedIgnoredMatchRecordCount, lfv.UnmatchedRecordCount]
    this.RendorChart(data);

  }

  // async createChartData() {
  //   debugger;
  //   var list = Array<LFValidationGridViewModel>();
  //   await this.LFValidationList && this.LFValidationList.map(m => {
  //     if (!list[m.RecordDate]) {
  //       list[m.RecordDate] = [];
  //     }
  //     list[m.RecordDate].push(m);
  //   })
  //   if (list && list.length > 0)
  //     this.RendorChart();

}

