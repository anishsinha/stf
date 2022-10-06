import { Component, ElementRef, Input, OnChanges, OnInit, SimpleChanges, ViewChild } from '@angular/core';
import { DataTableDirective } from 'angular-datatables';
import { Subject } from 'rxjs';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { TerminalSupplierModel } from '../Model/TerminalSupplierModel'
import { HttpGenericService } from 'src/app/http-generic.service';
import { first } from 'rxjs/operators';
import { Declarations } from 'src/app/declarations.module';
import { DropDownItem } from 'src/app/buyer-wally-board/Models/BuyerWallyBoard';
import { Country } from 'src/app/app.enum';

@Component({
    selector: 'app-terminal-description',
    templateUrl: './terminal-description.component.html',
    styleUrls: ['./terminal-description.component.css']
})

export class TerminalDescriptionComponent implements OnInit, OnChanges {
    @ViewChild('idBtnClose') public btnPopupClose: ElementRef;
    public Country: Country;
    @Input() selectedCountry: any;
    TerminalItemDescList: TerminalSupplierModel[] = [];
    TerminalItemDescModel: TerminalSupplierModel = {};
    terminalSupplierDescForm: FormGroup;
    @ViewChild(DataTableDirective) datatableElement: DataTableDirective;
    dtOptions: any = {};
    public dtTrigger: Subject<any> = new Subject();
    ProductTypeList:DropDownItem[]=[];

    GetTerminalItemDescGridUrl = '/Superadmin/superadmin/GetTerminalItemDescGrid?CountryId=';
    PostSaveTerminalItemDescriptionUrl = '/Superadmin/superadmin/SaveTerminalItemDescription';
    PostDeleteTerminalItemDescriptionUrl = '/Superadmin/superadmin/DeleteTerminalItemDescription';
    GetProductTypeDropdownUrl = '/Superadmin/superadmin/GetProductTypeDropDown';
    AddUpdateTitle = 'Add';
    public terminalCodeModal = { modalDetails: { display: 'none', data: 'Modal Show' } };
    IsLoading = false;
    public popoverTitle: string = 'Are you sure want to delete?';
    public confirmButtonText: string = 'Yes';
    public cancelButtonText: string = 'No';

    constructor(private _fb: FormBuilder, private httpService: HttpGenericService) { }

    ngOnInit() {
        this.getProductTypes();
        let exportInvitedColumns = { columns: ':visible' };
        this.dtOptions = {
            dom: '<"html5buttons"B>lTfgitp',
            aaSorting: [],
            orderable: false, 
            buttons: [
                { extend: 'colvis' },
                { extend: 'copy', exportOptions: exportInvitedColumns },
                { extend: 'csv', title: 'API Details', exportOptions: exportInvitedColumns },
                { extend: 'pdf', title: 'API Details', orientation: 'landscape', exportOptions: exportInvitedColumns },
                { extend: 'print', exportOptions: exportInvitedColumns }
            ],
            pagingType: 'first_last_numbers',
            pageLength: 10,
            lengthMenu: [[10, 25, 50, 100, -1], [10, 25, 50, 100, "All"]]
        };
    }


    ngOnChanges(change: SimpleChanges) {
        if (change.selectedCountry && change.selectedCountry.currentValue)
            this.init();
    }

    init() {
        this.terminalSupplierDescForm = this._fb.group({
            id: [''],
            code: ['', Validators.required],
            name: ['', Validators.required],
            productTypeId: ['',Validators.required]
        });
        this.getTerminalSupplier();
    }

    private tableClickFilter(data, index): void {
    }

    ngAfterViewInit(): void {
        this.dtTrigger.next();
    }

    private datatableRerender(): void {
        if (this.datatableElement.dtInstance) {
            this.datatableElement.dtInstance.then((dtInstance: DataTables.Api) => {
                dtInstance.destroy();
                this.dtTrigger.next();
            });
        }
    }

    ngOnDestroy(): void {
        this.dtTrigger.unsubscribe();
    }

    public modalOpen(): void {
        this.terminalCodeModal = { modalDetails: { display: 'block', data: 'Modal Show' } };
        var txt2 = $("<div class='modal-backdrop fade show'></div>");
        $("body").append(txt2);
    }


    public modalClose(): void {
        this.terminalCodeModal = { modalDetails: { display: 'none', data: 'Modal Show' } };
        $(".modal-backdrop").remove();
    }

    getTerminalSupplier() {
        this.IsLoading = true;
        this.httpService.fetchAll(`${this.GetTerminalItemDescGridUrl}${this.selectedCountry}`).pipe(first()).subscribe(result => {
            this.IsLoading = false;
            this.TerminalItemDescList = result;
            this.datatableRerender();
        });
    }

    getProductTypes() {
        this.httpService.fetchAll(`${this.GetProductTypeDropdownUrl}`).pipe(first()).subscribe(result => {
            this.IsLoading = false;
            this.ProductTypeList = result;
          
        });
    }

    public onSubmit() {
        for (let c in this.terminalSupplierDescForm.controls) {
            this.terminalSupplierDescForm.controls[c].markAsTouched();
          }
        if (this.terminalSupplierDescForm.valid) {
            if (this.terminalSupplierDescForm && this.terminalSupplierDescForm.controls.id.value)
                this.updateTewrminalupplier();
            else
                this.addTerminalSupplier();
        }
    }

    private addTerminalSupplier() {
        this.TerminalItemDescModel = {};
        this.IsLoading = true;
        this.TerminalItemDescModel.Code = this.terminalSupplierDescForm.controls.code.value;
        this.TerminalItemDescModel.Name = this.terminalSupplierDescForm.controls.name.value;
        this.TerminalItemDescModel.ProductTypeId = this.terminalSupplierDescForm.controls.productTypeId.value;
        this.TerminalItemDescModel.Country = this.selectedCountry;
        this.httpService.postData(this.PostSaveTerminalItemDescriptionUrl, this.TerminalItemDescModel).pipe(first()).subscribe(res => {
            if (res && res.StatusCode == 0) {
                Declarations.msgsuccess(res.StatusMessage, undefined, undefined);
                this.getTerminalSupplier();
            } else
                Declarations.msgerror(res.StatusMessage, undefined, undefined);
            this.IsLoading = false;
        })

        this.btnPopupClose.nativeElement.click();
    }

    private updateTewrminalupplier() {
        this.TerminalItemDescModel = {};
        this.IsLoading = true;
        this.TerminalItemDescModel.Id = this.terminalSupplierDescForm.controls.id.value;
        this.TerminalItemDescModel.Code = this.terminalSupplierDescForm.controls.code.value;
        this.TerminalItemDescModel.Name = this.terminalSupplierDescForm.controls.name.value;
        this.TerminalItemDescModel.Country = this.selectedCountry;
        this.TerminalItemDescModel.ProductTypeId = this.terminalSupplierDescForm.controls.productTypeId.value;
        this.httpService.postData(this.PostSaveTerminalItemDescriptionUrl, this.TerminalItemDescModel).pipe(first()).subscribe(res => {
            if (res && res.StatusCode == 0) {
                Declarations.msgsuccess(res.StatusMessage, undefined, undefined);
                this.getTerminalSupplier();
            } else
                Declarations.msgerror(res.StatusMessage, undefined, undefined);
            this.IsLoading = false;
        })
        this.btnPopupClose.nativeElement.click();
    }

    public editTerminal(terminalSupplier: TerminalSupplierModel) {
        this.terminalSupplierDescForm.reset();
        this.AddUpdateTitle = 'Update';
        this.TerminalItemDescModel = {};
        this.terminalSupplierDescForm.controls.id.setValue(terminalSupplier.Id);
        this.terminalSupplierDescForm.controls.code.setValue(terminalSupplier.Code);
        this.terminalSupplierDescForm.controls.name.setValue(terminalSupplier.Name);
        this.terminalSupplierDescForm.controls.productTypeId.setValue(terminalSupplier.ProductTypeId);
        this.modalOpen();


    }
    public addTerminal() {
        this.terminalSupplierDescForm.reset();
        this.AddUpdateTitle = 'Add';
        this.TerminalItemDescModel = {};
        this.modalOpen();

    }

    public deleteTerminal(terminalSupplier: TerminalSupplierModel) {
        this.IsLoading = true;
        this.httpService.postData(this.PostDeleteTerminalItemDescriptionUrl, { id: terminalSupplier.Id }).pipe(first()).subscribe(res => {
            if (res.StatusCode == 0) {
                Declarations.msgsuccess(res.StatusMessage, undefined, undefined);
                this.getTerminalSupplier();
            } else
                Declarations.msgerror(res.StatusMessage, undefined, undefined);
            this.IsLoading = false;
        })
    }
}
