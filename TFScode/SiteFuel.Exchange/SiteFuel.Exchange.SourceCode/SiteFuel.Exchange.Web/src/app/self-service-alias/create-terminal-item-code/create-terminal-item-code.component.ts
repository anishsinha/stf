import { Component, OnInit, ViewChild, ElementRef, Input, AfterViewInit, OnChanges, SimpleChanges, Output, EventEmitter, ViewEncapsulation } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';

import { first } from 'rxjs/operators';
import { DropdownItem } from 'src/app/carrier-companies/service/assigncarrier.service';
import { Declarations } from 'src/app/declarations.module';
import { HttpGenericService } from 'src/app/http-generic.service';
import { TerminalItemCodeMappingModel } from '../models/TerminalItemCodeMappingModel';
@Component({
    selector: 'app-create-terminal-item-code',
    templateUrl: './create-terminal-item-code.component.html',
    styleUrls: ['./create-terminal-item-code.component.css'],
    encapsulation: ViewEncapsulation.None
})
export class CreateTerminalItemCodeComponent implements OnInit, OnChanges {
  @Input() countryId: any;
  @Output() result = new EventEmitter();
  public IsLoading: boolean;
  TerminalSupplierList: DropdownItem[] = [];
  TerminalSupplierDescList: DropdownItem[] = [];
  public ddlSettingsById = {};
  terminalmappingForm: FormGroup;
  TerminalItemCodeMappingModel: TerminalItemCodeMappingModel = {};
  GetTerminalSupplierUrl = '/Carrier/SelfServiceAlias/GetTerminalSupplierAndDesc?CountryId=';
  PostSaveTerminalMappingUrl = '/Carrier/SelfServiceAlias/SaveTerminalItemCodeMapping';
  PostUpdateTerminalMappingUrl = '/Carrier/SelfServiceAlias/UpdateTerminalItemCodeMapping';

  selectedTerminalSupplier = [];
  selectedItemDesc = [];
  constructor(private httpService: HttpGenericService, private _fb: FormBuilder) { }
minDate=new Date();
maxDate=new Date();
  ngOnInit() {
    this.maxDate.setFullYear(this.maxDate.getFullYear()+20);
    this.ddlSettingsById = {
      singleSelection: true,
      idField: 'Id',
      textField: 'Name',
      // selectAllText: 'Select All',
      // unSelectAllText: 'UnSelect All',
      itemsShowLimit: 5,
      allowSearchFilter: true
    };
    this.init();
  }

  init() {
    this.terminalmappingForm = this._fb.group({
      id: [''],
      terminalSupplierId: ['', Validators.required],
      itemDescriptionId: ['', Validators.required],
      itemCode: ['', Validators.required],
      effectiveDate: ['', Validators.required],
      expiryDate: ['']
    });
    this.getTerminalSupplier();
  }

  ngOnChanges(change: SimpleChanges) {
    if (change.countryId && change.countryId.currentValue) {
      this.countryId = change.countryId.currentValue;
      if (change.countryId.previousValue && change.countryId.currentValue != change.countryId.previousValue) {
        this.getTerminalSupplier();
      }
    }

  }

  getTerminalSupplier() {
    this.IsLoading = true;
    this.httpService.fetchAll(`${this.GetTerminalSupplierUrl}${this.countryId}`).pipe(first()).subscribe(result => {
      this.IsLoading = false;
      this.TerminalSupplierList = result.TerminalSupplierList;
      this.TerminalSupplierDescList = result.TerminalDescriptionList;
    });
  }

  onSubmit() {
    for (let c in this.terminalmappingForm.controls) {
      this.terminalmappingForm.controls[c].markAsTouched();
    }
    if (this.terminalmappingForm.valid) {
      if (this.terminalmappingForm && this.terminalmappingForm.controls.id.value){
        var x= this.terminalmappingForm.controls.itemCode.value.split(",");
        if(x && x.length > 1){
          Declarations.msgerror('Comma Seperated item code for update is not allowed', undefined, undefined);
          return false;
        }
        this.updateTerminalMapping();
      }
       
      else
        this.addTerminalMapping();
    }
  }

  private addTerminalMapping() {
    this.TerminalItemCodeMappingModel = {};
    this.IsLoading = true;
    this.TerminalItemCodeMappingModel.TerminalSupplierId = this.terminalmappingForm.controls.terminalSupplierId.value[0].Id;
    this.TerminalItemCodeMappingModel.ItemDescriptionId = this.terminalmappingForm.controls.itemDescriptionId.value[0].Id;
    this.TerminalItemCodeMappingModel.ItemCode = this.terminalmappingForm.controls.itemCode.value;
    this.TerminalItemCodeMappingModel.EffectiveDate = this.terminalmappingForm.controls.effectiveDate.value;
    this.TerminalItemCodeMappingModel.ExpiryDate = this.terminalmappingForm.controls.expiryDate.value;

    this.httpService.postData(this.PostSaveTerminalMappingUrl, this.TerminalItemCodeMappingModel).pipe(first()).subscribe(res => {
      if (res.StatusCode == 0) {
        this.result.emit(true);
        Declarations.msgsuccess(res.StatusMessage, undefined, undefined);

      } else
        Declarations.msgerror(res.StatusMessage, undefined, undefined);
      this.IsLoading = false;
    })
  }

  private updateTerminalMapping() {
    this.TerminalItemCodeMappingModel = {};
    this.IsLoading = true;
    this.TerminalItemCodeMappingModel.Id = this.terminalmappingForm.controls.id.value;
    this.TerminalItemCodeMappingModel.TerminalSupplierId = this.terminalmappingForm.controls.terminalSupplierId.value[0].Id;
    this.TerminalItemCodeMappingModel.ItemDescriptionId = this.terminalmappingForm.controls.itemDescriptionId.value[0].Id;
    this.TerminalItemCodeMappingModel.ItemCode = this.terminalmappingForm.controls.itemCode.value;
    this.TerminalItemCodeMappingModel.EffectiveDate = this.terminalmappingForm.controls.effectiveDate.value;
    this.TerminalItemCodeMappingModel.ExpiryDate = this.terminalmappingForm.controls.expiryDate.value;

    this.httpService.postData(this.PostUpdateTerminalMappingUrl, this.TerminalItemCodeMappingModel).pipe(first()).subscribe(res => {
      if (res.StatusCode == 0) {
        Declarations.msgsuccess(res.StatusMessage, undefined, undefined);
        this.result.emit(true);
      } else
        Declarations.msgerror(res.StatusMessage, undefined, undefined);
      this.IsLoading = false;
    })

  }
  setexpiryDate($event) {
    this.terminalmappingForm.controls.expiryDate.setValue($event);

  }
  seteffectiveDate($event) {
    this.terminalmappingForm.controls.effectiveDate.setValue($event);

  }

}
