import { Component, OnInit, ViewChildren, QueryList, Input, ViewChild, ElementRef } from '@angular/core';
import { Subject } from 'rxjs';
import { ExternalMappingsService } from '../../service/externalmappings.service';
import { DataTableDirective } from 'angular-datatables';
import { ExternalBulkPlantMappingViewModel } from '../../models/ExternalMappingModel';
import { Declarations } from 'src/app/declarations.module';
declare function closeSlidePanel(): any;

@Component({
  selector: 'app-external-bulk-plant-mappings',
  templateUrl: './external-bulk-plant-mappings.component.html',
  styleUrls: ['./external-bulk-plant-mappings.component.css']
})
export class ExternalBulkPlantMappingsComponent implements OnInit {

    isShowLoader = false;
    dtOptions: any = {};
    dtTrigger: Subject<any> = new Subject();
    externalBulkPlantMappings: ExternalBulkPlantMappingViewModel[] = [];
    editBulkPlantDetails: ExternalBulkPlantMappingViewModel;
    @ViewChildren(DataTableDirective) dtElements: QueryList<DataTableDirective>;
    @Input() thirdPartyCompanyId: number;
    file: any;
    selectedFile: any;
    @ViewChild('btnCloseBulkModal') btnCloseBulkModal: ElementRef<HTMLElement>;
    constructor(private externalMappingsService: ExternalMappingsService) { }

    ngOnInit() {
        this.initializeBulkPlantCustomers();
        this.getBulkPlantsData();
    }
    getBulkPlantsData() {
        this.isShowLoader = true;
        this.externalMappingsService.getBulkPlantsForExternalMapping().subscribe(data => {
            this.externalBulkPlantMappings = data;
            this.isShowLoader = false;
            this.refreshDatatable();
        });
    }

    initializeBulkPlantCustomers() {
        let exportInvitedColumns = { columns: ':visible' };
        this.dtOptions = {
            dom: '<"html5buttons"B>lTfgitp',
            buttons: [
                { extend: 'colvis' },
                { extend: 'copy', exportOptions: exportInvitedColumns },
                { extend: 'csv', title: 'BulkPlant Details', exportOptions: exportInvitedColumns },
                { extend: 'pdf', title: 'BulkPlant Details', orientation: 'landscape', exportOptions: exportInvitedColumns },
                { extend: 'print', exportOptions: exportInvitedColumns }
            ],
            pagingType: 'first_last_numbers',
            pageLength: 10,
            lengthMenu: [[10, 25, 50, 100, -1], [10, 25, 50, 100, "All"]]
        };
    }
    refreshDatatable(): void {
        this.dtElements.forEach((dtElement: DataTableDirective) => {
            if (dtElement.dtInstance) {
                dtElement.dtInstance.then((dtInstance: DataTables.Api) => { dtInstance.destroy(); });
            }
        });
        this.dtTrigger.next();
    }
    editBulkPlant(bulkPlant: ExternalBulkPlantMappingViewModel) {
        this.editBulkPlantDetails = JSON.parse(JSON.stringify(bulkPlant));
    }
    SaveExternalBulkPlantMappings(bulkPlant: ExternalBulkPlantMappingViewModel) {
        bulkPlant.ThirdPartyId = this.thirdPartyCompanyId;
        this.isShowLoader = true;
        this.externalMappingsService.SaveExternalBulkPlantMappings(bulkPlant).subscribe(data => {
            if (data.StatusCode == 0) {
                Declarations.msgsuccess(data.StatusMessage, undefined, undefined);
                closeSlidePanel();
                this.clearForm();
            }
            else if (data.StatusCode == 2) {
                Declarations.msgwarning(data.StatusMessage, undefined, undefined);
            }
            else {
                Declarations.msgerror(data == null ? 'Failed' : data.StatusMessage, undefined, undefined);
            }
            this.isShowLoader = false;
            this.getBulkPlantsData();
        });
    }

    clearForm() {
        this.editBulkPlantDetails = null;
    }

    onFileChange(event) {
        this.file = event.target.files[0];
    }
    bulkUpload() {
        this.selectedFile = null;
        this.file = null;
    }
    onFileUpload() {
        if (!this.file) {
            Declarations.msgerror("Please select file", undefined, undefined);
            return;
        }
        let element: HTMLElement = this.btnCloseBulkModal.nativeElement;
        element.click();
        this.isShowLoader = true;
        this.externalMappingsService.BulkUploadBulkPlantMapping(this.file).subscribe(
            (response: any) => {
                if (response.StatusCode == 1) {
                    Declarations.msgerror(response == null ? 'Failed' : response.StatusMessage, undefined, undefined);
                    this.isShowLoader = false;
                } else {
                    Declarations.msgsuccess(response.StatusMessage, undefined, undefined);
                    this.getBulkPlantsData();
                }
                this.file = null;
            }
        );
    }
}
