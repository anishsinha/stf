import { Component, OnInit, Input, SimpleChanges } from '@angular/core';
import { BehaviorSubject } from 'rxjs';

import { HttpGenericService } from '../http-generic.service';
import * as moment from 'moment';
declare var ApexCharts;
@Component({
  selector: 'app-tank-chart',
  templateUrl: './tank-chart.component.html',
  styleUrls: ['./tank-chart.component.css']
})
export class TankChartComponent implements OnInit {
  @Input() JobId: any;
  @Input() SiteId: any;
  @Input() TankId: any;
  @Input() isSupplierView:boolean;
  NoOfDays: number = 3;
  chart: any;
  cData: any[] = [];
  showChart = false;
  // isLoading = false;
  showCustomDateText = false;
  customDays = 0;
  dateFilterList: any[] = [];
  noChartData = false;
  public toogleMap: Boolean = true;
  public isLoadingSubject: BehaviorSubject<any>;
  public GetSalesDataForGraphUrl = "/Supplier/Sales/GetSalesDataForGraph";
  constructor(private httpSer: HttpGenericService) {
    this.isLoadingSubject = new BehaviorSubject(false);
  }

  ngOnInit() {
    if(!this.isSupplierView){
      this.GetSalesDataForGraphUrl = "/buyer/Sales/GetSalesDataForGraph";
    }
    this.loadChart();
  }

  ngAfterViewInit() {
    if (this.chart)
      this.chart.render();
  }

  public toggleMapView() {
    this.toogleMap = !this.toogleMap;
  }

  ngOnChanges(change: SimpleChanges) {
    if (change.TankId && change.TankId.currentValue) {
      this.TankId = change.TankId.currentValue;
      if (this.chart) {
        this.chart.destroy();
      }
      if (this.SiteId && this.NoOfDays && this.JobId) {
        this.isLoadingSubject.next(true);
        this.rendor(this.cData);
        this.getDateList();
      }
      this.noChartData = false;
    }
    else if (change.JobId && change.JobId.currentValue && !change.JobId.firstChange) {
      if (this.chart) {
        this.chart.destroy();
      }
      this.loadChart();
      this.noChartData = false;
    }

  }


  async loadChart() {
    if (this.SiteId && this.NoOfDays && this.JobId) {
      this.isLoadingSubject.next(true);
      this.getTankChartData(this.SiteId, this.NoOfDays, this.JobId);
      this.getDateList();
    }
  }
  public getDateList() {
    //return this.dateFilterList = [{ Id: 3, Value: 'Last 3 Days' }, { Id: 7, Value: 'Last 7 Days' }, { Id: 10, Value: 'Last 10 Days' }, { Id: 0, Value: 'Custom' }]
    return this.dateFilterList = [{ Id: 3, Value: 'Last 3 Days' }, { Id: 7, Value: 'Last 7 Days' }, { Id: 10, Value: 'Last 10 Days' }]
  }

  async onSelection($event) {

    if ($event == 0) {
      this.showCustomDateText = true;
    } else {
      this.showCustomDateText = false;
      this.showChart = false;
      this.NoOfDays = $event;
      this.chart.destroy();
      this.noChartData = false;
      this.getTankChartData(this.SiteId, $event, this.JobId);
    }

  }
  async getTankChartData(siteId, noOfDays, tfxJobId) {
    this.isLoadingSubject.next(true);
    this.httpSer.fetchAll(`${this.GetSalesDataForGraphUrl}?jobId=${tfxJobId}&noOfDays=${noOfDays}`).subscribe(op => {
      if (op && op.length > 0) {
        this.cData = op;
        this.rendor(op);
      }
      else {
        this.noChartData = true;
        this.isLoadingSubject.next(false);
      }
    })
  }

  async rendor(data) {


    if (this.TankId) {
      try {
        let obj = this.TankId.split("_");
        let tankId = obj[0];
        let storageId = obj[1];
        data = data.filter(f => f.TankId == tankId && f.StorageId == storageId);
      } catch (e) {
        console.log("split issue related tankid and storage id (tank-chart component)");
      }

    }

    var mapList = [];
    var filterkeys = [];
    await data && data.map(m => {
      if (!filterkeys.find(f => f && f.tankid == m.TankId && f.storageid == m.StorageId)) {
        filterkeys.push({ tankid: m.TankId, storageid: m.StorageId });
        //mapList.push({ name: `[Tank: ${m.TankName}]`, data: data.filter(f => f.TankId == m.TankId && f.StorageId == m.StorageId).map(re => { return [new Date(re.CreatedDate).getTime(), 74] }) });
        mapList.push({ name: `[Tank: ${m.TankName}]`, data: data.filter(f => f.TankId == m.TankId && f.StorageId == m.StorageId).map(re => { return [moment(re.CreatedDate).valueOf(), re.Sale.toFixed(1)] }) });
      }
    })

    var date = new Date();
    var previousDate = date.setDate(date.getDate() - this.NoOfDays);
    var options = {
      series: mapList,
      chart: {
        type: 'line',
        height: 350,
        zoom: {
          enabled: true,
          type: 'x',
          autoScaleYaxis: false,
          zoomedArea: {
            fill: {
              color: '#90CAF9',
              opacity: 0.4
            },
            stroke: {
              color: '#0D47A1',
              opacity: 0.4,
              width: 1
            }
          }
        },
        toolbar: {
          show: true,
          offsetX: 0,
          offsetY: 0,
          tools: {
            download: false,
            selection: true,
            zoom: true,
            zoomin: true,
            zoomout: true,
            pan: true,
            reset: true,
            customIcons: []
          },
          autoSelected: 'zoom'
        },
      },
      annotations: {
        yaxis: [{
          show: true,
          y: 30,
          borderColor: '#999',
          title: {
            text: "Fuel (In Gallons / Litres)",
            style: {
              color: '#008FFB',
            }
          },
          text: 'Fuel (In Gallons / Litres)',
          tooltip: {
            enabled: true
          },
          axisTicks: {
            show: true,
          },
          axisBorder: {
            show: true,
            color: '#00E396'
          },
          label: {
            show: true,
            text: 'Date',
            style: {
              color: "#fff",
              background: '#00E396'
            }
          }
        }],
        xaxis: [{
          x: new Date().getTime(),
          borderColor: '#999',
          yAxisIndex: 0,
          type: "datetime",
          label: {
            show: true,
            text: 'Gallons/Litres',
            style: {
              color: "#fff",
              background: '#775DD0'
            },
            tooltip: {
              enabled: true
            },
            axisTicks: {
              show: true,
            },
            axisBorder: {
              show: true,
              color: '#00E396'
            },
          }
        }]
      },
      dataLabels: {
        enabled: false
      },
      markers: {
        size: 0,
        style: 'hollow',
      },
      xaxis: {
        type: 'datetime',
        labels: {
          datetimeUTC: false
        },
        min: new Date(previousDate).getTime(),
        tickAmount: 6,
      },
      tooltip: {
        x: {
          format: 'dd MMM yyyy hh:mm tt  '
        }
      },
      stroke: {
        show: true,
        curve: 'smooth',
        lineCap: 'butt',
        colors: undefined,
        width: 1,
        dashArray: 0,
      },
      fill: {
        type: 'gradient',
        gradient: {
          shadeIntensity: 1,
          opacityFrom: 0.7,
          opacityTo: 0.9,
          stops: [0, 100]
        }
      },
    };
    this.isLoadingSubject.next(false);
    //if (this.chart)
    //    this.chart.resetSeries();
    this.chart = new ApexCharts(document.querySelector("#chart-timeline"), options);
    try {
      if (this.chart)
        this.chart.render();
    } catch (e) {
      this.chart.destroy();
      this.chart.render();
    }


    this.showChart = true;

  }

}



