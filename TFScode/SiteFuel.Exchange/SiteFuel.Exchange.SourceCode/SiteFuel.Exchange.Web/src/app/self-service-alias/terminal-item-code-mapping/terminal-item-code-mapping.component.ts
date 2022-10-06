import { Component, OnInit, ViewChild, ElementRef, Input, AfterViewInit, OnChanges, SimpleChanges } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { DataTableDirective } from 'angular-datatables';
import * as moment from 'moment';
import { Subject, from } from 'rxjs';
import { first } from 'rxjs/operators';
import { Declarations } from 'src/app/declarations.module';
import { HttpGenericService } from 'src/app/http-generic.service';
import { CreateTerminalItemCodeComponent } from '../create-terminal-item-code/create-terminal-item-code.component';
import { TerminalItemCodeMappingModel } from '../models/TerminalItemCodeMappingModel';
import { CarrierService } from 'src/app/carrier/service/carrier.service';

@Component({
    selector: 'app-terminal-item-code-mapping',
    templateUrl: './terminal-item-code-mapping.component.html',
    styleUrls: ['./terminal-item-code-mapping.component.css']
})
export class TerminalItemCodeMappingComponent implements OnInit, AfterViewInit, OnChanges {
    @Input() countryId: any;
    title = 'Create';
    @ViewChild(DataTableDirective) datatableElement: DataTableDirective;
    @ViewChild('openSidePannel') public openSidePannel: ElementRef;
    @ViewChild('btnCloseBulkUploadPopup') CloseBulkUploadPopup: ElementRef;
    @ViewChild(CreateTerminalItemCodeComponent) public createTerminalItemCodeComponent: CreateTerminalItemCodeComponent;
    terminalMappingList: TerminalItemCodeMappingModel[] = [];

    public dtOptions: any = {};
    public dtTrigger: Subject<any> = new Subject();
    public IsLoading: boolean;
    public IsValidForm: boolean = true;
    public popoverSaveTitle: string = 'Save the change(s)?';
    public popoverDeleteTitle: string = 'Are you sure, want to delete?';
    public confirmButtonText: string = 'Yes';
    public cancelButtonText: string = 'No';
    public IsShowBulkUploadPopup: boolean = false;
    public SelectedFiles: File[] = [];
    public MaxFileUploadSize: number = 1048576; // 1MB
    GetTerminalSupplierUrl = '/Carrier/SelfServiceAlias/GetTerminalItemCodeMappings'
    DeleteTerminalItemCodeMappingUrl = '/Carrier/SelfServiceAlias/DeleteTerminalItemCodeMapping'

    constructor(private httpService: HttpGenericService, private carrierService: CarrierService) { }

    ngOnInit() {
        let exportInvitedColumns = { columns: ':visible' };
        this.dtOptions = {
            dom: '<"html5buttons"B>lTfgitp',
            aaSorting: [],
            orderable: false,
            buttons: [
                { extend: 'colvis' },
                { extend: 'copy', exportOptions: exportInvitedColumns },
                { extend: 'csv', title: 'Terminal Item Code Mapping', exportOptions: exportInvitedColumns },
                { extend: 'pdf', title: 'Terminal Item Code Mapping', orientation: 'landscape', exportOptions: exportInvitedColumns },
                { extend: 'print', exportOptions: exportInvitedColumns }
            ],
            pagingType: 'first_last_numbers',
            pageLength: 10,
            lengthMenu: [[10, 25, 50, 100, -1], [10, 25, 50, 100, "All"]]
        };

        this.getTerminalItemCodeMapping();
    }

    ngOnChanges(change: SimpleChanges) {
        if (change.countryId && change.countryId.currentValue) {
            this.countryId = change.countryId.currentValue;
            if (change.countryId.previousValue && change.countryId.currentValue != change.countryId.previousValue) {
                this.getTerminalItemCodeMapping();
            }
        }

    }


    filterGrid() {
        $("#terminal-item-code-datatable").DataTable().clear().destroy();
    }
    ngAfterViewInit(): void {
        this.dtTrigger.next();
    }

    private getTerminalItemCodeMapping() {
        this.IsLoading = true;
        this.httpService.postData(`${this.GetTerminalSupplierUrl}`, { CountryId: this.countryId }).pipe(first()).subscribe(result => {
            this.IsLoading = false;
            this.terminalMappingList = result;
            this.datatableRerender();
        });
    }

    private datatableRerender(): void {
        if (this.datatableElement.dtInstance) {
            this.datatableElement.dtInstance.then((dtInstance: DataTables.Api) => {
                dtInstance.destroy();
                this.dtTrigger.next();
            });
        }
    }

    public editTerminalMapping(item: TerminalItemCodeMappingModel) {
        this.title = 'Edit';
        this.createTerminalItemCodeComponent.terminalmappingForm.controls.id.setValue(item.Id);
        var obj = this.createTerminalItemCodeComponent.TerminalSupplierList.filter(f => f.Id == item.TerminalSupplierId)
            .map(m => { delete m.Code; return m; });
        if (obj) {

            this.createTerminalItemCodeComponent.selectedTerminalSupplier = [];
            this.createTerminalItemCodeComponent.selectedTerminalSupplier.push(obj);
            this.createTerminalItemCodeComponent.terminalmappingForm.controls.terminalSupplierId.setValue(obj);
        }
        var obj1 = this.createTerminalItemCodeComponent.TerminalSupplierDescList.filter(f => f.Id == item.ItemDescriptionId)
            .map(m => { delete m.Code; return m; });;

        if (obj1) {
            this.createTerminalItemCodeComponent.selectedItemDesc = [];
            this.createTerminalItemCodeComponent.selectedItemDesc.push(obj1);
            this.createTerminalItemCodeComponent.terminalmappingForm.controls.itemDescriptionId.setValue(obj1);
        }

        this.createTerminalItemCodeComponent.terminalmappingForm.controls.effectiveDate.setValue(item.EffectiveDate);
        this.createTerminalItemCodeComponent.terminalmappingForm.controls.expiryDate.setValue(item.ExpiryDate);
        this.createTerminalItemCodeComponent.terminalmappingForm.controls.itemCode.setValue(item.ItemCode);
    }

    public addTerminalItemCode() {
        this.title = 'Create';
        this.createTerminalItemCodeComponent.terminalmappingForm.reset();
    }

    getOutput($event) {
        if ($event) {
            this.openSidePannel.nativeElement.click();
            this.getTerminalItemCodeMapping();
        }
    }

    deleteTerminalItemCode(item: TerminalItemCodeMappingModel) {
        this.IsLoading = true;
        this.httpService.postData(`${this.DeleteTerminalItemCodeMappingUrl}`, { id: item.Id }).pipe(first()).subscribe(result => {
            this.IsLoading = false;
            if (result.StatusCode == 0) {
                Declarations.msgsuccess(result.StatusMessage, undefined, undefined);
                this.getTerminalItemCodeMapping();
            } else
                Declarations.msgerror(result.StatusMessage, undefined, undefined);
        });
    }
    showBulkUploadPopup() {
        this.IsShowBulkUploadPopup = true;
    }

    closePopup() {
        this.IsShowBulkUploadPopup = false;
    }

    selectFiles(files: File[]) {
        if (files != null && files != undefined && files.length > 0)
            this.SelectedFiles = files;
    }

    uploadTerminalItemCodeMappingFile() {
        var files = this.SelectedFiles;
        if (files.length === 0)
            return;

        const formData = new FormData();
        for (var file of files) {
            if (!this.isValidFile(file)) {
                return;
            }
            formData.append(file.name, file);
        }

        this.IsLoading = true;
        this.carrierService.postBulkUploadTerminalItemCodeMappingTemplate(formData).subscribe(data => {
            this.IsLoading = false;
            if (data.StatusCode == 0) {
                this.CloseBulkUploadPopup.nativeElement.click();
                Declarations.msgsuccess(data.StatusMessage, undefined, undefined);
                this.SelectedFiles = [];
            }
            else if (data.StatusCode == 2) {
                Declarations.msgwarning(data.StatusMessage, undefined, undefined);
            }
            else {
                Declarations.msgerror(data == null ? 'Failed' : data.StatusMessage, undefined, undefined);
            }
        });
    }

    isValidFile(file: File) {
        var isValid = true;
        var extension = this.getExtension(file.name);
        if (extension == undefined || extension == null || extension == '' || extension.toLowerCase() != 'csv') {
            Declarations.msgerror('Invalid file, only csv files are allowed', undefined, undefined);
            isValid = false;
            return isValid;
        }

        if (file.size > this.MaxFileUploadSize) {
            Declarations.msgerror('Invalid file size, file size should not be greater than 1 MB', undefined, undefined);
            isValid = false;
            return isValid;
        }

        return isValid;
    }

    downloadCsvTemplate() {
        this.IsLoading = true;
        var timestamp = new Date().getTime();
        this.carrierService.downloadTerminalItemCodeMappingTemplate(timestamp).subscribe(blob => {
            const a = document.createElement('a')
            const objectUrl = URL.createObjectURL(blob)
            a.href = objectUrl
            a.download = 'TerminalItemCodeMapping_Template.csv';
            a.click();
            URL.revokeObjectURL(objectUrl);

            this.IsLoading = false;
        });
    }

    getExtension(fileName) {
        // extract file name from full path ...
        var basename = fileName.split(/[\\/]/).pop();

        // (supports `\\` and `/` separators)
        var pos = basename.lastIndexOf(".");       // get last position of `.`

        if (basename === "" || pos < 1)            // if file name is empty or ...
            return "";                             //  `.` not found (-1) or comes first (0)

        return basename.slice(pos + 1);            // extract extension ignoring `.`
    }
}
