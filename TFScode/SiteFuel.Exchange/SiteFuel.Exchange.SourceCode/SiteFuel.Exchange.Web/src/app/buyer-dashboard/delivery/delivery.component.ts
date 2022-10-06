import { Component, Input, OnChanges, OnInit, SimpleChanges, ViewChild } from '@angular/core';
import { Router } from '@angular/router';
import { DataTableDirective } from 'angular-datatables';
import * as moment from 'moment';
import { Subject } from 'rxjs';
import { DeliveryReqPriority } from 'src/app/app.enum';
import { DashboardService } from '../dashboard.service';
import { BuyerLoadsForDashboardInputModel, BuyerLoadsForDashboardViewModel } from '../Model/DashboardModel';
declare var currentCompanyId: any;

@Component({
    selector: 'app-delivery',
    templateUrl: './delivery.component.html',
    styleUrls: ['./delivery.component.scss']
})
export class DeliveryComponent implements OnInit, OnChanges {
    @Input() SelectedCountryId: any;
    dtOptions: any = {};
    dtTrigger: Subject<any> = new Subject();
    public IsLoading: boolean = false;
    currentCompanyId: any;
    deliveries: BuyerLoadsForDashboardViewModel[] = [];
    public type = 1;
    private cloneDeliveries: BuyerLoadsForDashboardViewModel[] = [];
    maxDate: Date;
    minDate: Date;
    fromDate: string;
    toDate: string;
    activePriorityTab = DeliveryReqPriority.MustGo;
    public DeliveryReqPriority = DeliveryReqPriority;
    //min max date

    @ViewChild(DataTableDirective) datatableElement: DataTableDirective;

    constructor(private dashbpardSer: DashboardService, private router: Router) {
        this.toDate = moment().format('MM/DD/YYYY');
        this.minDate = new Date(new Date().setMonth(new Date().getMonth() - 10));
        this.maxDate = new Date(new Date().setMonth(new Date().getMonth() + 10));
        //this.fromDate = moment(new Date(new Date().setDate(new Date().getDate() - 1))).format('MM/DD/YYYY');
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
        let exportInvitedColumns = { columns: ':visible' };
        this.dtOptions = {
            paging: false,
            bSort: false,
            bInfo: false,
            searching: true,
            pagingType: 'first_last_numbers',
            pageLength: 10,
            lengthMenu: [[10, 25, 50, 100, -1], [10, 25, 50, 100, "All"]]
        };
    }
    public initializeDate(type): void {
        this.type = type;
        if (type == 1)//today
        {
            this.toDate = moment().format('MM/DD/YYYY');
            //  this.fromDate = moment(new Date(new Date().setDate(new Date().getDate() - 1))).format('MM/DD/YYYY');
        } else {
            this.toDate = moment(new Date(new Date().setDate(new Date().getDate() + 1))).format('MM/DD/YYYY');
            // this.fromDate = moment().format('MM/DD/YYYY');
        }
        this.getDeliveries();
    }
    public getDeliveries() {
        this.IsLoading = true;
        if ((this.datatableElement && this.datatableElement.dtInstance)) {
            this.datatableElement.dtInstance.then((dtInstance: DataTables.Api) => { dtInstance.destroy(); });
        }
        var input = new BuyerLoadsForDashboardInputModel();
        input.CountryId = this.SelectedCountryId;
        input.FromDate = new Date(this.toDate);
        input.ToDate = new Date(this.toDate);
        this.cloneDeliveries = [];
        this.dashbpardSer.getDeliveries(input).subscribe(data => {
            this.IsLoading = false;
            this.cloneDeliveries = data;
            this.FilterDate(DeliveryReqPriority.MustGo); // Default must go
        });
    }

    private FilterDate(priority): void {
        if (this.cloneDeliveries)
            this.deliveries = this.cloneDeliveries.filter(f => f.Priority == priority);
        else
            this.deliveries = [];
        this.dtTrigger.next();

    }

    public changeActiveTab(priority) {
        this.activePriorityTab = priority;
        this.FilterDate(priority);
    }

    public navigate(): void {
        this.router.navigate([]).then(result => { window.open('/Buyer/Job/BuyerWallyBoard?viewTypeFromDashboard=2', '_blank'); });
    }

    setFromDate(event: any): void {
        if (event) {
            this.type = 0;//not today and tomorrow
            this.toDate = (event);
            if (moment().format('MM/DD/YYYY') == moment(this.toDate).format('MM/DD/YYYY'))
                this.type = 1;
            else if (moment(new Date().setDate(new Date().getDate() + 1)).format('MM/DD/YYYY') == moment(this.toDate).format('MM/DD/YYYY'))
                this.type = 2;
            // this.fromDate = moment(new Date(new Date().setDate(new Date().getDate() - 1))).format('MM/DD/YYYY');
            this.getDeliveries();
        }

    }
}




