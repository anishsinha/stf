import { Component, OnInit, ViewChild, ViewChildren, QueryList } from '@angular/core';
import { Subject } from 'rxjs';
import { DataTableDirective } from 'angular-datatables';
import { LFRecordGridModel, DropDownItem } from '../LiftFileModels';
import { DataTablesResponse } from '../../shared/models/DataTable-models';
import { LiftfiledashboardserviceService } from '../service/liftfiledashboardservice.service';
import { IDropdownSettings } from 'ng-multiselect-dropdown';
import { Declarations } from 'src/app/declarations.module';

@Component({
    selector: 'app-lfv-accrual-report',
    templateUrl: './lfv-accrual-report.component.html',
    styleUrls: ['./lfv-accrual-report.component.css']
})
export class LfvAccrualReportComponent implements OnInit {
    public dtOptions: any = {};
    public dtTrigger: Subject<any> = new Subject();
    @ViewChildren(DataTableDirective) dtElements: QueryList<DataTableDirective>;
    public IsLoading: boolean = false;
    public records: LFRecordGridModel[] = [];
    public FromDate: any;
    public ToDate: any;
    public ProductTypesList: DropDownItem[] = [];
    public selectedProductTypesList: DropDownItem[] = [];
    public multiselectSettingsById: IDropdownSettings;
    public ProductTypeIds: string = "";

    constructor(private _lfvservice: LiftfiledashboardserviceService) {
        this.FromDate = null;
        this.ToDate = null;
        this.ProductTypeIds = "";
    }

    ngOnInit() {
        this.multiselectSettingsById = {
            singleSelection: false,
            idField: 'Id',
            textField: 'Name',
            itemsShowLimit: 2,
            allowSearchFilter: true,
            enableCheckAll: true
        }
        let exportColumns = { columns: ':visible' };
        let gridcolumnsDetails = [];
        gridcolumnsDetails = [
            { title: 'CallID', name: 'CallId', data: 'CallId', "autoWidth": true },
            { title: 'Record Date', name: 'RecordDate', data: 'RecordDate', "autoWidth": true },
            { title: 'BOL#', name: 'BOL', data: 'bol', "autoWidth": true },            
            { title: 'Terminal Code', name: 'TerminalName', data: 'TerminalName', "autoWidth": true },
            { title: 'Terminals', name: 'Terminal', data: 'Terminals', "autoWidth": true },
            { title: 'Terminal Item Code', name: 'TerminalItemCode', data: 'TerminalItemCode', "autoWidth": true },
            { title: 'Product Type', name: 'ProductType', data: 'ProductType', "autoWidth": true },
            { title: 'Corrected Quantity', name: 'correctedQuantity', data: 'correctedQuantity', "autoWidth": true },
            { title: 'Load Date', name: 'LoadDate', data: 'LoadDate', "autoWidth": true },
            { title: 'CarrierID', name: 'CarrierID', data: 'CarrierID', "autoWidth": true },
            { title: 'Carrier Name', name: 'CarrierName', data: 'CarrierName', "autoWidth": true },
            { title: 'FileName', name: 'FileName', data: 'FileName', "autoWidth": true },           
            //{ title: 'Reason', name: 'Reason', data: 'Reason', "autowidth": true },
            { title: 'Status', name: 'RecordStatus', data: 'recordStatus', "autowidth": true },
            { title: 'User Name', name: 'Username', data: 'Username', "autowidth": true },
            { title: 'Modified Date (MST)', name: 'ModifiedDate', data: 'ModifiedDate', "autowidth": true },
            { title: 'Resolution Time', name: 'LFVResolutionTime', data: 'LFVResolutionTime', "autowidth": true },
            { title: 'Time to BOL', name: 'TimeToBol', data: 'TimeToBol', "autowidth": true }
            //{ title: 'Reason Code', name: 'ReasonCode', data: 'ReasonCode', "autowidth": true },
            //{ title: 'Reason Category', name: 'ReasonCategory', data: 'ReasonCategory', "autowidth": true }
        ]

        this.dtOptions = {
            pagingType: 'first_last_numbers',
            pageLength: 10,
            lengthMenu: [[10, 25, 50, 100, 99999999], [10, 25, 50, 100, "All"]],
            serverSide: true,
            processing: true,
            ajax: (dataTablesParameters: any, callback) => {
                let inputs = {
                    FromDate: this.FromDate,
                    ToDate: this.ToDate,
                    ProductTypeIds: this.ProductTypeIds
                };
                let inputData = Object.assign(dataTablesParameters, inputs);
                this.IsLoading = true;
                this._lfvservice.getLFVAccrualReportGrid(inputData).subscribe((resp: DataTablesResponse) => {
                    this.records = resp.data;
                    this.IsLoading = false;
                    callback({
                        recordsTotal: resp.recordsTotal,
                        recordsFiltered: resp.recordsFiltered,
                        data: resp.data
                    });
                   // this.getLFVValidationStatsAndProductTypesDDL();
                });
            },
            dom: '<"html5buttons"B>lTfgitp',
            order: [[0, 'desc']],
            buttons: [
                { extend: 'copy', exportOptions: exportColumns },
                { extend: 'csv', title: 'LiftFile Accrual Report', exportOptions: exportColumns },
                { extend: 'pdf', title: 'LiftFile Accrual Report', orientation: 'landscape', exportOptions: exportColumns },
                { extend: 'print', exportOptions: exportColumns }
            ],
            columns: gridcolumnsDetails
        };
    }

    ngAfterViewInit(): void {
        this.getLFAccrualGrid();
        this.dtTrigger.next();
        this.getLFVValidationStatsAndProductTypesDDL();
    }
    getLFAccrualGrid(): void {
        this.IsLoading = true;
        this.refreshDatatable();
        this.IsLoading = false;
    }
    getLFVValidationStatsAndProductTypesDDL() {
        this.IsLoading = true;
            let input = {
                FromDate: this.FromDate,
                ToDate: this.ToDate
            };
            this._lfvservice.GetLFVValidationStatsAndProductTypesDDL(input).subscribe((resp: any) => {              
                this.ProductTypesList = resp;
                this.IsLoading = false;
            });
    }
    refreshDatatable(): void {
        this.dtElements.forEach((dtElement: DataTableDirective) => {
            if (dtElement.dtInstance) {
                dtElement.dtInstance.then((dtInstance: DataTables.Api) => {
                    dtInstance.draw();
                });
            }
        });
    }
    ApplyFilter() {
        if ((this.FromDate != null && this.FromDate != undefined && this.FromDate != "")
            && (this.ToDate == null || this.ToDate == undefined || this.ToDate == "")) {
            Declarations.msgerror("To Date is required", undefined, undefined);
            return;
        }
        this.getSelectedProductTypes();
        this.refreshDatatable();
    }
    ClearFilter() {
        this.FromDate = null;
        this.ToDate = null;
        this.selectedProductTypesList = [];
        this.getSelectedProductTypes();
        this.refreshDatatable();
    }
    getSelectedProductTypes(): void {
        if (this.selectedProductTypesList == null || this.selectedProductTypesList.length == 0 || this.selectedProductTypesList == undefined) {
            this.ProductTypeIds = "";
        }
        else if (this.selectedProductTypesList != null || this.selectedProductTypesList.length > 0) {
            this.ProductTypeIds = this.selectedProductTypesList.map(m => m.Id).join(',');
        }
    }


    public setFromDate(event: any): void {
        this.FromDate = event;
    }
    public setToDate(event: any): void {
        this.ToDate = event;
    }
    ngOnDestroy(): void {
        this.dtTrigger.unsubscribe();
    }

}
