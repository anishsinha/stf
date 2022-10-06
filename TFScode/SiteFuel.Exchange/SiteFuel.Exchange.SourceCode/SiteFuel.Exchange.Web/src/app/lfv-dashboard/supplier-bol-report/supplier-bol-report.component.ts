import { Component, OnInit } from '@angular/core';
import { SupplierBOLReport } from '../LiftFileModels';
import { LiftfiledashboardserviceService } from '../service/liftfiledashboardservice.service';
import { Subject } from 'rxjs';
@Component({
  selector: 'app-supplier-bol-report',
  templateUrl: './supplier-bol-report.component.html',
  styleUrls: ['./supplier-bol-report.component.css']
})
export class SupplierBolReportComponent implements OnInit {
    public ReportRecords: SupplierBOLReport[] = [];
    dtOptions: any = {};
    dtTrigger: Subject<any> = new Subject();
    public ShowGridLoader: boolean = false;

    constructor(private dashboardservice: LiftfiledashboardserviceService) { }

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
        this.getSupplierBOLReport();
    }
    getSupplierBOLReport() {
        this.ShowGridLoader = true;
        this.dashboardservice.getSupplierBOLReport().subscribe((data: SupplierBOLReport[]) => {
            this.ShowGridLoader = false;
            this.ReportRecords = data;
            this.dtTrigger.next();

        });
    }
    reloadGrid() {
        $("#supplierbolreport-datatable").DataTable().clear().destroy();
        this.getSupplierBOLReport();
    }
}
