import { Component, OnInit } from '@angular/core';
import { CarrierBOLReport } from '../LiftFileModels';
import { LiftfiledashboardserviceService } from '../service/liftfiledashboardservice.service';
import { Subject } from 'rxjs';
import { Declarations } from 'src/app/declarations.module';
@Component({
  selector: 'app-carrier-bol-report',
  templateUrl: './carrier-bol-report.component.html',
  styleUrls: ['./carrier-bol-report.component.css']
})
export class CarrierBolReportComponent implements OnInit {
    public ReportRecords: CarrierBOLReport [] = [];
    dtOptions: any = {};
    dtTrigger: Subject<any> = new Subject();
    public ShowGridLoader: boolean = false;
    public fromDate: any;
    public toDate: any;
    constructor(private dashboardservice: LiftfiledashboardserviceService) {
        this.fromDate = null;
        this.toDate = null;
    }

    ngOnInit() {
        this.intializeGrid();
    }
    intializeGrid() {
        this.ShowGridLoader = true;
        let exportColumns = { columns: ':visible' };
        this.dtOptions = {
            dom: '<"html5buttons"B>lTfgitp',
            buttons: [
                { extend: 'colvis' },
                { extend: 'copy', exportOptions: exportColumns },
                { extend: 'csv', title: 'Lift File Records', exportOptions: exportColumns },
                { extend: 'pdf', title: 'Lift File Records', orientation: 'landscape', exportOptions: exportColumns },
                { extend: 'print', exportOptions: exportColumns }
            ],
            pagingType: 'first_last_numbers',
            pageLength: 10,
            lengthMenu: [[10, 25, 50, 100, -1], [10, 25, 50, 100, "All"]],
        };
        this.getCarrierBOLReport();
    }

    public setFromDate(event: any): void {
        this.fromDate = event;
    }

    public setToDate(event: any): void {
        this.toDate = event;
    }
    ApplyFilter() {
        if ((this.fromDate == null || this.fromDate == undefined || this.fromDate == "")
            || (this.toDate == null || this.toDate == undefined || this.toDate == "")) {
            Declarations.msgerror("From/To date not selected", undefined, undefined);
        } else {
            this.reloadGrid();
        }
    }
    reloadGrid() {
        $("#carrierbolreport-datatable").DataTable().clear().destroy();
        this.getCarrierBOLReport();
    }

    ClearFilter() {
        this.fromDate = null;
        this.toDate = null;
        this.reloadGrid();
    }
    getCarrierBOLReport() {
        let fromDate = this.fromDate
        let toDate = this.toDate;
        this.ShowGridLoader = true;
        this.dashboardservice.getCarrierBOLReport(fromDate, toDate).subscribe((data: CarrierBOLReport[]) => {
            this.ShowGridLoader = false;
            this.ReportRecords = data;
            this.dtTrigger.next();

        });
    }


}
