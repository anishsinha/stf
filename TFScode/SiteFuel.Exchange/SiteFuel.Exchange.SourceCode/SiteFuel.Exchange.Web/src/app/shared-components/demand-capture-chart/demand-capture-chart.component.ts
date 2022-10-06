import { Component, OnInit, Input, SimpleChanges } from '@angular/core';
import { BehaviorSubject } from 'rxjs';
import { CarrierService } from 'src/app/carrier/service/carrier.service';
declare var ApexCharts;

@Component({
    selector: 'app-demand-capture-chart',
    templateUrl: './demand-capture-chart.component.html',
    styleUrls: ['./demand-capture-chart.component.css']
})

export class DemandCaptureChartComponent implements OnInit {
    @Input() data: any;
    demandCapChart: any;
    showChart = false;
    // isLoading = false;
    showCustomDateText = false;
    customDays = 0;
    dateFilterList: any[] = [];
    noChartData = false;
    public isLoadingSubject: BehaviorSubject<any>;
    constructor(private carrierService: CarrierService) {
        this.isLoadingSubject = new BehaviorSubject(false);
    }

    ngOnInit() {
        this.loadTrends();
    }

    ngAfterViewInit() {
        if (this.demandCapChart)
            this.demandCapChart.render();
    }

    ngOnChanges(change: SimpleChanges) {
        if (change.data.currentValue && change.data.currentValue.tfxJobId && !change.data.firstChange) {
            if (this.demandCapChart) {
                this.demandCapChart.destroy();
            }
            this.loadTrends();
            this.noChartData = false;
        }
    }


    async loadTrends() {
        if (this.data.siteId && this.data.noOfDays && this.data.tfxJobId) {
            this.isLoadingSubject.next(true);
            this.getDemandCapChartData(this.data.siteId, this.data.noOfDays, this.data.tfxJobId);
            this.getDateList();
        }
    }
    public getDateList() {
        return this.dateFilterList = [{ Id: 3, Value: 'Last 3 Days' }, { Id: 7, Value: 'Last 7 Days' }, { Id: 10, Value: 'Last 10 Days' }, { Id: 0, Value: 'Custom' }]
    }

    async onSelection($event) {

        if ($event == 0) {
            this.showCustomDateText = true;
        } else {
            this.showCustomDateText = false;
            this.showChart = false;
            this.data.noOfDays = $event;
            this.demandCapChart.destroy();
            this.noChartData = false;
            this.getDemandCapChartData(this.data.siteId, $event, this.data.tfxJobId);
        }

    }
    async getDemandCapChartData(siteId, noOfDays, tfxJobId) {
        this.isLoadingSubject.next(true);
        this.carrierService.getDemandCapChartData(siteId, noOfDays, tfxJobId).subscribe(op => {
            if (op.Result && op.Result.length > 0) this.rendor(op.Result)
            else {
                this.noChartData = true;
                this.isLoadingSubject.next(false);
            }
        })
    }

    async rendor(data) {
        var mapList = [];
        var filterkeys = [];
        await data && data.map(m => {
            if (!filterkeys.find(f => f && f.tankid == m.TankId && f.storageid == m.StorageId)) {
                filterkeys.push({ tankid: m.TankId, storageid: m.StorageId });
                mapList.push({ name: `[Tank: ${m.TankName} , Product Category: ${m.ProductName}]`, data: data.filter(f => f.TankId == m.TankId && f.StorageId == m.StorageId).map(re => { return [new Date(re.CaptureTime).getTime(), re.NetVolume.toFixed(1)] }) });
            }
        })

        var date = new Date();
        var previousDate = date.setDate(date.getDate() - this.data.noOfDays);
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
                        text: "Fuel (In Gallons)",
                        style: {
                            color: '#008FFB',
                        }
                    },
                    text: 'Fuel (In Gallons)',
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
                        text: 'Gallons',
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
        //if (this.demandCapChart)
        //    this.demandCapChart.resetSeries();
        this.demandCapChart = new ApexCharts(document.querySelector("#chart-timeline"), options);
        if (this.demandCapChart)
            this.demandCapChart.render();

        this.showChart = true;


        //var resetCssClasses = function (activeEl) {
        //    var els = document.querySelectorAll("button");
        //    Array.prototype.forEach.call(els, function (el) {
        //        el.classList.remove('active');
        //    });
        //                activeEl.target.classList.add('active')
        //}




    }

}
