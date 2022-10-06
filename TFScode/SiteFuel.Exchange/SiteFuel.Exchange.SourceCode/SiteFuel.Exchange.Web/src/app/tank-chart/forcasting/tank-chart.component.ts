import { Component, OnInit, ViewChild, Input, SimpleChanges } from '@angular/core';
import { ForecastingTankChartViewModel, TankDetailsForChartModel, TankLevelModel } from 'src/app/carrier/models/DispatchSchedulerModels';
import { DispatcherService } from 'src/app/carrier/service/dispatcher.service';
import * as moment from 'moment';
import { ChartDataSets, ChartOptions, ChartType } from 'chart.js';
declare var IsBuyerCompany: boolean;
import { Chart } from 'chart.js';
import { ColorConstants } from 'src/app/app.constants';

@Component({
    selector: 'app-forcasting-tank-chart',
    templateUrl: './tank-chart.component.html',
    styleUrls: ['./tank-chart.component.css']
})
export class ForcastingTankChartComponent implements OnInit {
    @Input() JobId: any;
    @Input() SiteId: any;
    @Input() TankId: any;
    @Input() StorageId: any;
    public selectedJobId: any;
    public selectedSiteId: any;
    public selectedTankId: any;
    public selectedStorageId: any;
    public MinInputDate: Date = new Date();
    public IsChartLoading: boolean = false;
    public currentDate: string = '';

    constructor(private dispatcherService: DispatcherService) {
        // this.chartOptions = this.getChartOptions([], [], []);
    }
    ngOnInit() {
        this.currentDate = moment(this.MinInputDate).format("MM-DD-YYYY HH:mm:ss");
    }
    ngAfterViewInit() {
        // console.log("chart-obj-ngAfterViewInit", this.chart);
    }
    ngOnChanges(change: SimpleChanges) {
        //this.IsChartLoading = true;
        this.currentDate = moment(this.MinInputDate).format("MM-DD-YYYY HH:mm:ss");
        if (change.JobId && change.JobId.currentValue && change.JobId.currentValue != change.JobId.previousValue) {
            this.selectedJobId = change.JobId.currentValue;
        }
        if (change.SiteId && change.SiteId.currentValue && change.SiteId.currentValue != change.SiteId.previousValue) {
            this.selectedSiteId = change.SiteId.currentValue;
        }
        if (change.TankId && change.TankId.currentValue && change.TankId.currentValue != change.TankId.previousValue) {
            this.selectedTankId = change.TankId.currentValue;
        }
        if (change.StorageId && change.StorageId.currentValue && change.StorageId.currentValue != change.StorageId.previousValue) {
            this.selectedStorageId = change.StorageId.currentValue;
        }
        if (this.selectedJobId || this.selectedTankId || this.selectedStorageId) {
            if (this.selectedTankId == 'NONE') {
                this.selectedTankId = '';
            }
            if (this.selectedStorageId == 'NONE') {
                this.selectedStorageId = '';
            }
            if (IsBuyerCompany == false) {
                this.IsChartLoading = true;
                this.dispatcherService.getForcastingTankChartDetails(this.selectedJobId, this.selectedTankId, this.selectedStorageId, this.currentDate).subscribe((resp: ForecastingTankChartViewModel) => {
                    this.updateChartData(resp);
                    this.IsChartLoading = false;
                });
            }
            else {
                this.IsChartLoading = true;
                this.dispatcherService.getBuyerForcastingTankChartDetails(this.selectedJobId, this.selectedTankId, this.selectedStorageId, this.currentDate).subscribe((resp: ForecastingTankChartViewModel) => {
                    this.updateChartData(resp);
                    this.IsChartLoading = false;
                });
            }
        }
    }
    updateChartData(resp: ForecastingTankChartViewModel) {
        if (resp != null) {
            // var _xAxisTimeSpan = resp["XAxisTimeSpan"];
            // var _tankDetailsForChart = resp["TankDetailsForChart"] as TankDetailsForChartModel[];
            // var _tankLevels = resp["TankLevels"] as TankLevelModel[];
            // var _seriesData = _tankDetailsForChart.map(x => { return { name: x.TankName, data: x.Data }; });

            let _labels = resp.XAxisTimeSpan;
            let _lineChartData: ChartDataSets[] = [];

            if (resp.TankDetailsForChart && resp.TankDetailsForChart.length > 0) {
                resp.TankDetailsForChart.forEach((tankDetails, i) => {
                    _lineChartData.push({
                        data: tankDetails.Data,
                        label: tankDetails.TankName,
                        fill: false,
                        borderColor: ColorConstants[i] || 'gray'
                    })
                });
            }

            // Line chart:
            const ctx = document.getElementById('myChart') as HTMLCanvasElement;

            new Chart(ctx, {
                type: 'line',
                data: {
                    labels: _labels,
                    datasets: _lineChartData,
                },
                options: {
                    scales: {
                        xAxes: [{
                            type: 'time'
                        }]
                    },
                },
            });
        }

    }


}
