import { Component, OnInit, ViewChild } from '@angular/core';
import { ApiDashboardService } from '../service/api-dashboard.service';
import { Subject } from 'rxjs';
import * as moment from 'moment';
import { DataTableDirective } from 'angular-datatables';
import { ApiResultType, ReqResType } from 'src/app/app.enum';
declare var currentCompanyId;
@Component({
  selector: 'app-dashboard',
  templateUrl: './dashboard.component.html',
  styleUrls: ['./dashboard.component.css']
})
export class DashboardComponent implements OnInit {
    public ApiResultType = ApiResultType;
    public ReqResType = ReqResType;
    dtOptions: any = {};
  
    public Log: ApiDetailModel = {};
    dtTrigger: Subject<any> = new Subject();
    public IsLoading: boolean = false;
    currentCompanyId: any;
    selectedReqRes: any;
    modelHeader: any;
    fromDate: string;3
    toDate: string;
    selectedApi: any = '';
    ApiList = ['Select API','Invoice-Create', 'Invoice-UpdateImages', 'Schedule-Create', 'Customer-Create','Location-Create'];
    viewType: any = ApiResultType.Total;
    //min max date
    MinStartDate = moment(new Date(new Date().setMonth(new Date().getMonth() - 1)));
    MaxStartDate = new Date();
    @ViewChild(DataTableDirective) datatableElement: DataTableDirective;

    constructor(private dashbpardSer: ApiDashboardService) {
        this.toDate = moment().format('MM/DD/YYYY');
        this.fromDate = moment(new Date(new Date().setMonth(new Date().getMonth() - 1))).format('MM/DD/YYYY'); 
    }

    ngOnInit() {
        this.currentCompanyId = currentCompanyId;
        this.initializeGrid();
        this.getAPILogs();
    }

    initializeGrid() {
        let exportInvitedColumns = { columns: ':visible' };
        this.dtOptions = {
            dom: '<"html5buttons"B>lTfgitp',
            buttons: [
              /*  { extend: 'colvis' }*/
                //{ extend: 'copy', exportOptions: exportInvitedColumns },
                //{ extend: 'csv', title: 'API Details', exportOptions: exportInvitedColumns },
                //{ extend: 'pdf', title: 'API Details', orientation: 'landscape', exportOptions: exportInvitedColumns },
                //{ extend: 'print', exportOptions: exportInvitedColumns }
            ],
            pagingType: 'first_last_numbers',
            pageLength: 10,
            lengthMenu: [[10, 25, 50, 100, -1], [10, 25, 50, 100, "All"]]
        };
    }

    ReloadDataTable() {
        this.getAPILogs();
    }
   public getAPILogs() {
       this.IsLoading = true;
       if ((this.datatableElement && this.datatableElement.dtInstance)) {
        this.datatableElement.dtInstance.then((dtInstance: DataTables.Api) => { dtInstance.destroy(); });
      }
       this.Log = {};
       if(this.fromDate == undefined) this.fromDate = '' ;
       if (this.toDate == undefined) this.toDate = '';
       if (this.selectedApi == undefined || this.selectedApi == 'Select API')
       {
           this.selectedApi = '';
       }
       this.dashbpardSer.getApiLogs(this.currentCompanyId, this.fromDate, this.toDate, this.selectedApi, this.viewType).subscribe(data => {
           this.IsLoading = false;
           this.Log = data as ApiDetailModel;
           this.Log && this.Log.ApiLogList.map(m => {
               m.CreatedDate=  moment(m.CreatedDate).format('MM/DD/YYYY hh:mm a');
           });
           this.dtTrigger.next();
        });
   }

    public showReqRes(log: ApiDashBoardModel, reqResType) {
        this.IsLoading = true;
        this.selectedReqRes = null;
        this.dashbpardSer.getApiReqRes(log.Id, reqResType).subscribe(data => {
            this.IsLoading = false;
            reqResType == ReqResType.Request ? this.selectedReqRes = JSON.parse(data.Request) : this.selectedReqRes = JSON.parse(data.Response);
            reqResType == ReqResType.Request ? this.modelHeader = 'Request (' + log.Url + " )" : this.modelHeader = 'Response (' + log.Url + " )";
            });
       // reqResType == 2 ? this.selectedReqRes = JSON.parse(log.Request) : this.selectedReqRes = JSON.parse(log.Response);
    }

    setFromDate(event: any): void {
        this.fromDate=(event);
        let d = moment(new Date(new Date().setMonth(new Date().getMonth() + 1))).toDate();
        !this.toDate ? this.toDate =(moment(d).format('MM/DD/YYYY')) : '';
        if (this.fromDate != '' && this.toDate!= '') {
            let _fromDate = moment(this.fromDate).toDate();
            let _toDate = moment(this.toDate).toDate();
            if (_toDate < _fromDate) {
                this.toDate=(event);
            }
        }
    }

    setToDate(event: any) {
        this.toDate = (event);
        if (this.fromDate != '' && this.toDate != '') {
            let _fromDate = moment(this.fromDate).toDate();
            let _toDate = moment(this.toDate).toDate();
            if (_fromDate > _toDate) {
                this.fromDate = (event);
            }
        }
    }
    apiChanged($event) {
        this.selectedApi = $event.target.value;
    }

    getView(type) {
        this.viewType = type;
        this.getAPILogs();
    }
}


export class ApiDashBoardModel {
    Id?: number;
    Request?: string;
    Response?: string;
    Url?: string;
    ExternalRefID?: string;
    Message?: string;
    CreatedDate?: string;
    CreatedBy?: number;
    CompanyId?: number;
}

export class ApiDetailModel {
    TotalCall?: number;
    SuccessCall?: number;
    FailedCall?: number;
    ApiLogList?: ApiDashBoardModel[] = [];
}


