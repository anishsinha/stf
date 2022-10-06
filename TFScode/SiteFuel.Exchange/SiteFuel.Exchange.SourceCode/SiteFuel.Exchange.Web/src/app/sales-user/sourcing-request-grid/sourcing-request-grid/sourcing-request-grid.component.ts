import { Component, OnInit, QueryList, ViewChild, ViewChildren } from '@angular/core';
import { SalesUserService } from '../../sales-user.service';
import { DataTableDirective } from 'angular-datatables';
import { Subject } from 'rxjs';
import { SourcingRequestDisplayStatus} from 'src/app/app.enum';
@Component({
  selector: 'app-sourcing-request-grid',
  templateUrl: './sourcing-request-grid.component.html',
  styleUrls: ['./sourcing-request-grid.component.scss']
})
export class SourcingRequestGridComponent implements OnInit {

  dtOptions: any = {};
  dtTrigger: Subject<any> = new Subject();
    public IsLoading: boolean = false;
    public RequestStatusdata: SourcingRequestDisplayStatus = SourcingRequestDisplayStatus.All; 
  Sourcingrequests: any = [];
    UserContext: any;
    public DisplayRequestStatus = SourcingRequestDisplayStatus;
    public _opened: boolean = false;
    public DispatchRegion = [];
  constructor(private salesUserService: SalesUserService) { 
      this.getUserContext();
  }

  ngOnInit() {
    this.initializeGrid();

  }
    ngAfterViewInit(): void {
        let requestStatusdata = localStorage.getItem('SelectedSRStatus');
        let requestStatus = requestStatusdata == undefined || requestStatusdata == "" ? this.DisplayRequestStatus.All : requestStatusdata;
        this.getRequests(requestStatus);
  }

  @ViewChildren(DataTableDirective) dtElements: QueryList<DataTableDirective>;
  refreshDatatable(): void {
    this.dtElements.forEach((dtElement: DataTableDirective) => {
      if (dtElement.dtInstance) {
        dtElement.dtInstance.then((dtInstance: DataTables.Api) => { dtInstance.destroy(); });
      }
    });
    this.dtTrigger.next();
  }

  getUserContext() {
    this.salesUserService.GetUserContext().subscribe(data => {
      this.UserContext = data;
    })  
  }
  initializeGrid() {
    let exportInvitedColumns = { columns: ':visible' };
    this.dtOptions = {
        dom: '<"html5buttons"B>lTfgitp',
        buttons: [
            { extend: 'colvis' },
            { extend: 'copy', exportOptions: exportInvitedColumns },
            { extend: 'csv', title: 'Sourcing Requests', exportOptions: exportInvitedColumns },
            { extend: 'pdf', title: 'Sourcing Requests', orientation: 'landscape', exportOptions: exportInvitedColumns },
            { extend: 'print', exportOptions: exportInvitedColumns }
        ],
        pagingType: 'first_last_numbers',
        order: [[0, 'desc']],
        pageLength: 10,
        lengthMenu: [[10, 25, 50, 100, -1], [10, 25, 50, 100, "All"]]
    };
  }

    public getRequests(status) 
    {
        this.RequestStatusdata = status;
        localStorage.setItem('SelectedSRStatus', status);
        this.IsLoading = true;
          this.salesUserService.GetSourcingRequests(status, false).subscribe(data => {
          this.IsLoading = false;
              this.Sourcingrequests = data;
              this.refreshDatatable();
        });
    }
  
}
