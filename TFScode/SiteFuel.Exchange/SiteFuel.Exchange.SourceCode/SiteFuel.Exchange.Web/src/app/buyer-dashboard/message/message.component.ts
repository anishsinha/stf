import { Component, Input, OnInit, SimpleChanges, ViewChild } from '@angular/core';
import { Router } from '@angular/router';
import { DataTableDirective } from 'angular-datatables';
import { Subject } from 'rxjs';
import { DashboardService } from '../dashboard.service';

@Component({
  selector: 'app-message',
  templateUrl: './message.component.html',
  styleUrls: ['./message.component.scss']
})
export class MessageComponent implements OnInit {
  dtOptions: any = {};
  dtTrigger: Subject<any> = new Subject();
  public IsLoading: boolean = false;
  @ViewChild(DataTableDirective) datatableElement: DataTableDirective;
  Messages: any;
  constructor(private dashbpardSer: DashboardService, private router: Router) { }

  ngOnInit() {
    this.initializeGrid();
    this.getMessages();
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
  public getMessages() {
    this.IsLoading = true;
    if ((this.datatableElement && this.datatableElement.dtInstance)) {
      this.datatableElement.dtInstance.then((dtInstance: DataTables.Api) => { dtInstance.destroy(); });
    }
    this.dashbpardSer.GetMessages().subscribe(data => {
      this.IsLoading = false;
      this.Messages = data;
    });
  }
  public navigate(): void {
    this.router.navigate([]).then(result => { window.open('/Messages/Mailbox', '_blank'); });
  }
}
