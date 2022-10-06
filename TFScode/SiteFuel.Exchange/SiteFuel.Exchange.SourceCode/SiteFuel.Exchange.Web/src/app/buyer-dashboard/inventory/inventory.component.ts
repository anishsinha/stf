import { Component, Input, OnInit, SimpleChanges, ViewChild } from '@angular/core';
import { Router } from '@angular/router';
import { DataTableDirective } from 'angular-datatables';
import * as moment from 'moment';
import { Subject } from 'rxjs';
import { DeliveryReqPriority } from 'src/app/app.enum';
import { DashboardService } from '../dashboard.service';
import { BuyerLoadsForDashboardInputModel, BuyerLoadsForDashboardViewModel, InventoryForDashboardInputModel, InventoryViewModel } from '../Model/DashboardModel';
declare var currentCompanyId: any;

@Component({
  selector: 'app-inventory',
  templateUrl: './inventory.component.html',
  styleUrls: ['./inventory.component.scss']
})

export class InventoryComponent implements OnInit {
  @Input() SelectedCountryId: any;
  dtOptions: any = {};
  dtTrigger: Subject<any> = new Subject();
  public IsLoading: boolean = false;
  currentCompanyId: any;
  inventories: InventoryViewModel[] = [];

  private cloneInventories: InventoryViewModel[] = [];
  activePriorityTab = DeliveryReqPriority.MustGo;
  public DeliveryReqPriority = DeliveryReqPriority;
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
      this.getDeliveries();
    }

  }
  initializeGrid() {
    let exportInvitedColumns = { columns: ':invisible' };
    this.dtOptions = {
      paging: false,
      bSort: false,
      bInfo : false,
      searching: true,
      pagingType: 'first_last_numbers',
      pageLength: 10,
      lengthMenu: [[10, 25, 50, 100, -1], [10, 25, 50, 100, "All"]]
  };
  }

  public getDeliveries() {
    this.IsLoading = true;
    if ((this.datatableElement && this.datatableElement.dtInstance)) {
      this.datatableElement.dtInstance.then((dtInstance: DataTables.Api) => { dtInstance.destroy(); });
    }
    var input = new InventoryForDashboardInputModel();
    input.CountryId = this.SelectedCountryId;
    this.cloneInventories = [];
    this.dashbpardSer.GetLocationInventory(input).subscribe(data => {
      this.IsLoading = false;
      this.cloneInventories = data;
      this.FilterDate(DeliveryReqPriority.MustGo); // Default must go
    });
  }

  private FilterDate(priority): void {
    if (this.cloneInventories)
      this.inventories = this.cloneInventories.filter(f => f.Priority == priority);
    else
      this.inventories = [];
      if ((this.datatableElement && this.datatableElement.dtInstance)) {
        this.datatableElement.dtInstance.then((dtInstance: DataTables.Api) => { dtInstance.destroy(); this.dtTrigger.next();});
      }
  }

  public changeActiveTab(priority) {
    this.activePriorityTab = priority;
    this.FilterDate(priority);
  }

  public navigate(): void {
    this.router.navigate([]).then(result => { window.open('/Buyer/Job/BuyerWallyBoard?viewTypeFromDashboard=3', '_blank'); });
  }
}


