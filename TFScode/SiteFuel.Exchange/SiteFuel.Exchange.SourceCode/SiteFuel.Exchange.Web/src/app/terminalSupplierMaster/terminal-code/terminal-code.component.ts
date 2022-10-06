import { Component, ElementRef, Input, OnChanges, OnInit, SimpleChanges, ViewChild } from '@angular/core';
import { DataTableDirective } from 'angular-datatables';
import { Subject } from 'rxjs';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { TerminalSupplierModel } from '../Model/TerminalSupplierModel'
import { HttpGenericService } from 'src/app/http-generic.service';
import { first } from 'rxjs/operators';
import { Declarations } from 'src/app/declarations.module';
import { Country } from 'src/app/app.enum';

@Component({
  selector: 'app-terminal-code',
  templateUrl: './terminal-code.component.html',
  styleUrls: ['./terminal-code.component.css']
})
export class TerminalCodeComponent implements OnInit, OnChanges {
  @ViewChild('idBtnClose') public btnPopupClose: ElementRef;
  public Country: Country;
  @Input() selectedCountry: any;
  TerminalSupplierList: TerminalSupplierModel[] = [];
  TerminalSupplierModel: TerminalSupplierModel = {};
  terminalSupplierForm: FormGroup;
  @ViewChild(DataTableDirective) datatableElement: DataTableDirective;
  dtOptions: any = {};
  public dtTrigger: Subject<any> = new Subject();

  GetTerminalSupplierUrl = '/Superadmin/superadmin/GetTerminalSupplierGrid?CountryId=';
  PostSaveTerminalSupplierUrl = '/Superadmin/superadmin/SaveTerminalSupplier';
  PostDeleteTerminalSupplierUrl = '/Superadmin/superadmin/DeleteTerminalSupplier';

  AddUpdateTitle = 'Add';
  public terminalCodeModal = { modalDetails: { display: 'none', data: 'Modal Show' } };
  IsLoading = false;
  public popoverTitle: string = 'Are you sure want to delete?';
  public confirmButtonText: string = 'Yes';
  public cancelButtonText: string = 'No';

  constructor(private _fb: FormBuilder, private httpService: HttpGenericService) { }

  ngOnInit() {
    // let exportColumns = { columns: ':visible' };
    // this.dtOptions = {
    //   pagingType: 'first_last_numbers',
    //   pageLength: 10,
    //   lengthMenu: [[10, 25, 50, 100, -1], [10, 25, 50, 100, "All"]],
    //   searching: true,
    //   dom: '<"html5buttons"B>lTfgitp',
    //   autoWidth: true,
    //   ordering: false,
    //   search: false,
    //   destroy: true,
    //   rowCallback: (row: Node, data: Object, index: number) => {
    //     const self = this;
    //     $('td', row).unbind('click');
    //     $('td', row).bind('click', () => {
    //       self.tableClickFilter(data, index);
    //     });
    //     return row;
    //   },
    //   buttons: [
    //     { extend: 'colvis' },
    //     { extend: 'copy', exportOptions: exportColumns },
    //     { extend: 'csv', title: 'Dispatcher Dashboard - Locations', exportOptions: exportColumns },
    //     { extend: 'pdf', title: 'Dispatcher Dashboard - Locations', orientation: 'landscape', exportOptions: exportColumns },
    //     { extend: 'print', exportOptions: exportColumns }
    //   ]
    // };
    //}
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
    this.terminalSupplierForm = this._fb.group({
      id: [''],
      code: ['', Validators.required],
      name: ['', Validators.required]
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
    this.httpService.fetchAll(`${this.GetTerminalSupplierUrl}${this.selectedCountry}`).pipe(first()).subscribe(result => {
      this.IsLoading = false;
      this.TerminalSupplierList = result;
      this.datatableRerender();
    });
  }

  public onSubmit() {
    for (let c in this.terminalSupplierForm.controls) {
      this.terminalSupplierForm.controls[c].markAsTouched();
    }
    if (this.terminalSupplierForm.valid) {
      if (this.terminalSupplierForm && this.terminalSupplierForm.controls.id.value)
        this.updateTewrminalupplier();
      else
        this.addTerminalSupplier();
    }
  }

  private addTerminalSupplier() {
    this.TerminalSupplierModel = {};
    this.IsLoading = true;
    this.TerminalSupplierModel.Code = this.terminalSupplierForm.controls.code.value;
    this.TerminalSupplierModel.Name = this.terminalSupplierForm.controls.name.value;
    this.TerminalSupplierModel.Country = this.selectedCountry;
    this.httpService.postData(this.PostSaveTerminalSupplierUrl, this.TerminalSupplierModel).pipe(first()).subscribe(res => {
      if (res && res.StatusCode == 0) {
        Declarations.msgsuccess(res.StatusMessage, undefined, undefined);
        this.getTerminalSupplier();
      } else
        Declarations.msgerror(res.StatusMessage, undefined, undefined);
      this.IsLoading = false;
    })

    //this.modalClose();
    this.btnPopupClose.nativeElement.click();
  }

  private updateTewrminalupplier() {
    this.TerminalSupplierModel = {};
    this.IsLoading = true;
    this.TerminalSupplierModel.Id = this.terminalSupplierForm.controls.id.value;
    this.TerminalSupplierModel.Code = this.terminalSupplierForm.controls.code.value;
    this.TerminalSupplierModel.Name = this.terminalSupplierForm.controls.name.value;
    this.TerminalSupplierModel.Country = this.selectedCountry;
    this.httpService.postData(this.PostSaveTerminalSupplierUrl, this.TerminalSupplierModel).pipe(first()).subscribe(res => {
      if (res && res.StatusCode == 0) {
        Declarations.msgsuccess(res.StatusMessage, undefined, undefined);
        this.getTerminalSupplier();
      } else
        Declarations.msgerror(res.StatusMessage, undefined, undefined);
      this.IsLoading = false;
    })
    // this.modalClose();
    this.btnPopupClose.nativeElement.click();
  }

  public editTerminal(terminalSupplier: TerminalSupplierModel) {
    this.terminalSupplierForm.reset();
    this.AddUpdateTitle = 'Update';
    this.TerminalSupplierModel = {};
    this.terminalSupplierForm.controls.id.setValue(terminalSupplier.Id);
    this.terminalSupplierForm.controls.code.setValue(terminalSupplier.Code);
    this.terminalSupplierForm.controls.name.setValue(terminalSupplier.Name);
    this.modalOpen();


  }
  public addTerminal() {
    this.terminalSupplierForm.reset();
    this.AddUpdateTitle = 'Add';
    this.TerminalSupplierModel = {};
    this.modalOpen();

  }

  public deleteTerminal(terminalSupplier: TerminalSupplierModel) {
    this.IsLoading = true;
    this.httpService.postData(this.PostDeleteTerminalSupplierUrl, { id: terminalSupplier.Id }).pipe(first()).subscribe(res => {
      if (res.StatusCode == 0) {
        Declarations.msgsuccess(res.StatusMessage, undefined, undefined);
        this.getTerminalSupplier();
      } else
        Declarations.msgerror(res.StatusMessage, undefined, undefined);
      this.IsLoading = false;
    })
  }
}
