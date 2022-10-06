import { Component, Input, OnChanges, OnInit, SimpleChanges } from '@angular/core';
import { LFValidationGridViewModel } from '../LiftFileModels';
import { LiftfiledashboardserviceService } from '../service/liftfiledashboardservice.service';
declare var ApexCharts;
@Component({
  selector: 'app-carrier-performace',
  templateUrl: './carrier.component.html',
  styleUrls: ['./carrier.component.css']
})
export class CarrierComponent implements OnInit, OnChanges {
  chart: any;
  @Input() LFValidationList: LFValidationGridViewModel[];
  constructor(private _lfvService: LiftfiledashboardserviceService) { }

  ngOnInit() {
  }

  ngOnChanges(changes: SimpleChanges): void {
    if (changes.LFValidationList.currentValue && !changes.LFValidationList.isFirstChange()) {
      this.createChartData();
    }

  }

  //public openLFVScratchReportGrid(): void {
  //  window.open("Supplier/LiftFile/LFVScratchReport", "_blank");
  //}

  async RendorChart(data, carrierList, chartHeight) {
    try {
      if (this.chart)
        this.chart.destroy();
    } catch (e) {

    }

    var options = {
        colors: ["#00FF00", "#ff0000", "#FF69B4", "#FFFF00", "#000080", "#00A7C6", "#800080",'#0077ff','#A9D794'],
      series: data,
      chart: {
        type: 'bar',
        height: chartHeight,
        stacked: true,
        toolbar: {
          show: true
        },
        animations: {
          enabled: false
        }

      },
      markers: {
        size: 0
      },
      responsive: [{
        breakpoint: undefined,
        options: {
        }
      }],
      plotOptions: {
        bar: {
          // borderRadius: 8,
          horizontal: true,
          dataLabels: {
            //  position: 'bottom'
          }
        },
      },
     
      xaxis: {
        type: 'category',
        categories: carrierList,
      },
      legend: {
        position: 'top',
        horizontalAlign: 'left',
        offsetX: 40
        //  offsetY: 40
      },
      fill: {
        opacity: 1
      //  colors: ["red", "#F27036", "#663F59", "#6A6E94", "#4E88B4", "#00A7C6", "#18D8D8", '#A9D794']
      }
    };
    this.chart = new ApexCharts(document.querySelector("#chart-timeline1"), options);
    try {
      if (this.chart)
        this.chart.render();
    } catch (e) {
      this.chart.destroy();
      this.chart.render();
    }
  }

  async createChartData() {
    var mapList = [];
    var carrierList = [];
    var chartHeight = 300;

    //Match Records
    var mtchRec = this.LFValidationList.map(res => { return [res.MatchedRecordCount] }).toString();
    var match = mtchRec && mtchRec.split(",").map(Number);
    //NoMatch Records
    var nomtchRec = this.LFValidationList.map(res => { return [res.NoMatchRecordCount] }).toString();
    var nomatch = nomtchRec && nomtchRec.split(",").map(Number);
    //Partial Match
    var partialRec = this.LFValidationList.map(res => { return [res.PartialMatchRecordCount] }).toString();
    var partial = partialRec && partialRec.split(",").map(Number);
    //Duplicate
    var dupRec = this.LFValidationList.map(res => { return [res.DuplicateRecordCount] }).toString();
    var duplicate = dupRec && dupRec.split(",").map(Number);
    //PendingMatchCount
    var pendingRec = this.LFValidationList.map(res => { return [res.PendingMatchCount] }).toString();
    var pending = pendingRec && pendingRec.split(",").map(Number);
    //activeException
    var activeExcRec = this.LFValidationList.map(res => { return [res.ActiveExceptionRecordCount] }).toString();
    var activeExc = activeExcRec && activeExcRec.split(",").map(Number);
    //IgnoredMatchRecordCount
    var ignoreRec = this.LFValidationList.map(res => { return [res.IgnoredMatchRecordCount] }).toString();
    var Ignore = ignoreRec && ignoreRec.split(",").map(Number);
    //UnmatchedRecordCount
    var unmstchRec = this.LFValidationList.map(res => { return [res.UnmatchedRecordCount] }).toString();
    var unMatch = unmstchRec && unmstchRec.split(",").map(Number);
      //ForcedIgnoreRecordCount
      var forcedIgnoredRec = this.LFValidationList.map(res => { return [res.ForcedIgnoredMatchRecordCount] }).toString();
      var forcedIgnored = forcedIgnoredRec && forcedIgnoredRec.split(",").map(Number);
    if (this.LFValidationList.length > 0) {
      await mapList.push({ name: 'Matched ', data: match })
      await mapList.push({ name: 'No Match ', data: nomatch })
      await mapList.push({ name: 'Partial Match ', data: partial })
      await mapList.push({ name: 'Pending Match ', data: pending })
      await mapList.push({ name: 'Duplicate  ', data: duplicate })
      await mapList.push({ name: 'Active Exception ', data: activeExc })
      await mapList.push({ name: 'Ignored Match ', data: Ignore })
      await mapList.push({ name: 'Forced Ignore', data: forcedIgnored })
      await mapList.push({ name: 'Unmatched  ', data: unMatch })      
      await this.LFValidationList && this.LFValidationList.map(lfv => {
        carrierList.push(lfv.CarrierID ? lfv.CarrierID : '-')
      })
      chartHeight = chartHeight + (carrierList.length * 40);
    } else {
      await mapList.push({ name: 'Matched ', data: [] })
      await mapList.push({ name: 'No Match ', data: [] })
      await mapList.push({ name: 'Partial Match ', data: [] })
      await mapList.push({ name: 'Pending Match ', data: [] })
      await mapList.push({ name: 'Duplicate  ', data: [] })
      await mapList.push({ name: 'Active Exception ', data: [] })
      await mapList.push({ name: 'Ignored Match ', data: [] })
      await mapList.push({ name: 'Forced Ignore  ', data: [] })
      await mapList.push({ name: 'Unmatched  ', data: [] })
    }


    await this.RendorChart(mapList, carrierList, chartHeight);
  }
  openSupplierBOLReportGrid() {
    window.open("Supplier/LiftFile/SupplierBolReport", "_blank");
  }
  openCarrierBOLReportGrid() {
    window.open("Supplier/LiftFile/CarrierBolReport", "_blank");
  }
}

