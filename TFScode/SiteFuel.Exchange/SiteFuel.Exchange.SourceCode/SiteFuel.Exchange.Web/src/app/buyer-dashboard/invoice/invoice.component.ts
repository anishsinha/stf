import { Component, Input, OnInit, SimpleChanges, ViewChild } from '@angular/core';
import { Router } from '@angular/router';
import { DataTableDirective } from 'angular-datatables';
import * as moment from 'moment';
import { Subject } from 'rxjs';
import { DashboardService } from '../dashboard.service';
import { InvoiceGridBuyerDashboardModel, InvoiceGridBuyerDashboardInputModel } from '../Model/DashboardModel';
declare var currentCompanyId: any;
@Component({
  selector: 'app-invoice',
  templateUrl: './invoice.component.html',
  styleUrls: ['./invoice.component.scss']
})
export class InvoiceComponent implements OnInit {

  @Input() SelectedCountryId: any;
  dtOptions: any = {};
  dtTrigger: Subject<any> = new Subject();
  public IsLoading: boolean = false;
  currentCompanyId: any;
  Invoices: InvoiceGridBuyerDashboardModel[] = [];
  temp_string: string =`DDTs`

  activeInvoiceDDTTab = 0;

  //min max date
  MinStartDate = moment(new Date(new Date().setMonth(new Date().getMonth() - 1)));
  MaxStartDate = new Date();
  @ViewChild(DataTableDirective) datatableElement: DataTableDirective;

  constructor(private dashbpardSer: DashboardService, private router: Router) {
  }

  ngOnInit() {
    this.currentCompanyId = currentCompanyId;
    this.initializeGrid();
  }

  ngOnChanges(changes: SimpleChanges): void {
    if (changes.SelectedCountryId && changes.SelectedCountryId.currentValue) {
      this.getInvoices();
    }

  }
  initializeGrid() {
    let exportInvitedColumns = { columns: ':invisible' };
    this.dtOptions = {
      paging: false,
      bSort: false,
      bInfo : false,
      pagingType: 'first_last_numbers',
      pageLength: 10,
      lengthMenu: [[10, 25, 50, 100, -1], [10, 25, 50, 100, "All"]]
    };
  }

  public getInvoices() {
    this.IsLoading = true;
    if ((this.datatableElement && this.datatableElement.dtInstance)) {
      this.datatableElement.dtInstance.then((dtInstance: DataTables.Api) => { dtInstance.destroy(); });
    }
    var input = new InvoiceGridBuyerDashboardInputModel();
    input.CountryId = this.SelectedCountryId;
    input.InvoiceTypeId = this.activeInvoiceDDTTab;
    let CurrencyTypeId = localStorage.getItem("currencyTypeForDashboard");
    if (CurrencyTypeId)
      input.CurrencyTypeId = +CurrencyTypeId;
    this.dashbpardSer.GetInvoices(input).subscribe(data => {
      this.IsLoading = false;
      this.Invoices = data;
      this.FilterDate(this.activeInvoiceDDTTab); // Default must go
    });
  }

  private FilterDate(type): void {
    //this.Invoices = this.cloneInventories.filter(f => f.type == type);
    this.dtTrigger.next();

  }

  public changeActiveTab(type) {
    this.activeInvoiceDDTTab = type;
    this.getInvoices();
  }

  public navigate(): void {
    this.router.navigate([]).then(result => { window.open('/Buyer/Invoice/View', '_blank'); });
  }

}
