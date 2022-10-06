import { AfterViewInit, Component, OnInit, ViewChild, ViewEncapsulation, ElementRef } from '@angular/core';
import { LeftSideFilterComponent } from '../left-side-filter/left-side-filter.component';
import { LiftfiledashboardserviceService } from '../service/liftfiledashboardservice.service';
import { DropDownItem, LFBolEditModel, LFRecordGridModel, LFRecordsGridExport, LFRecordsGridViewModel, LFValidationGridViewModel, LFVValidationParameters } from '../LiftFileModels';
import { Subject } from 'rxjs';
import { DataTableDirective } from 'angular-datatables';
import { Declarations } from 'src/app/declarations.module';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { IDropdownSettings } from 'ng-multiselect-dropdown';
import { AngularCsv } from 'angular7-csv/dist/Angular-csv'
import { LFVRecordStatus } from 'src/app/app.enum';

@Component({
  selector: 'app-master',
  templateUrl: './master.component.html',
  styleUrls: ['./master.component.css'],
  encapsulation: ViewEncapsulation.None
})

export class MasterComponent implements OnInit, AfterViewInit {
  @ViewChild(LeftSideFilterComponent) filterComponent: LeftSideFilterComponent;
  viewType = 1;
  gridType = 0;
  LFValidationList: LFValidationGridViewModel[] = [];
  LFVRecordGrid: LFRecordsGridViewModel[] = [];
  IsLoading = false;
  isValidationCarrier = false;
  dtTrigger: Subject<any> = new Subject();
  dtOptions: any = {};
  public isChecked = false;
  public LFVRecordStatus = LFVRecordStatus;
  @ViewChild(DataTableDirective) datatableElement: DataTableDirective;
  //side bar related variables
  public _opened: boolean = false;
  public _animate: boolean = true;
  public _positionNum: number = 1;
  public _POSITIONS: Array<string> = ['left', 'right', 'top', 'bottom'];


  public bolResolveForm: FormGroup;
  public selectedLiftFileRecord: LFRecordGridModel; //used for display purpose only
  public TerminalList: DropDownItem[];
  public FuelTypeList: DropDownItem[];
  public multiselectSettingsById: IDropdownSettings;
  public InvoiceFtlDetailIdList: DropDownItem[];
  public SelectedBolProduct: any;
  public SelectedTerminalList: DropDownItem[] = [];
  public SelectedFuelTypeList: DropDownItem[] = [];

  //search Liftfile records variables
  public bolSearchQuery: string = "";
  public fileNameSearchQuery: string = "";
  public showSearchControls: boolean = false;
  public showSearchBtn: boolean = false;
  public LiftFilesearchResults: LFRecordGridModel[] = [];
  searchGridDtOptions: any = {};
  searchGridDtTrigger: Subject<any> = new Subject();
  @ViewChild('btnOpenModal') btnOpenModal: ElementRef<HTMLElement>;

  csvOptions = {
    fieldSeparator: ',',
    quoteStrings: '"',
    decimalseparator: '.',
    showLabels: true,
    showTitle: false,
    title: 'LFV' + new Date(),
    useBom: true,
    noDownload: false,
      headers: ["BOL", "Terminal Code", "Terminal", "Corrected Quantity", "Terminal Item Code", "Product Type", "Load Date", "Record Date", "Carrier ID", "Carrier Name", "Reasons", "Reason Code", "Reason Category", "Modified By", "Modified Date", "Resolution Time","Time to BOL"]
    };

    //LFV Record Edit variables 
    public isAdminUser: boolean;
    public LfvValidationParameters: LFVValidationParameters;
    public LFVRecordEditForm: FormGroup;
    public  QuantityPattern: string = "^(\0*[1-9]*[1-9][0-9]*(\.[0-9]+)?|[0]*\.[0-9]*[1-9][0-9]*)$";
    
    public _EditOpened: boolean = false;
    public _EditAnimate: boolean = true;
    public _EditPositionNum: number = 1;
    public _EditPOSITIONS: Array<string> = ['left', 'right', 'top', 'bottom'];

    //ignore by reason
    public preferenceSetting: any = null;
    public selectedReason = [];
    public reasonList = [];
    public dropdownSettings: IDropdownSettings = { singleSelection: true, idField: 'Id', textField: 'Name', allowSearchFilter: true };

    constructor(private _lfvService: LiftfiledashboardserviceService, private fb: FormBuilder) {

    }

  ngOnInit() {
    this.getPreferencesSetting();
    this.multiselectSettingsById = {
      singleSelection: true,
      idField: 'Id',
      textField: 'Name',
      selectAllText: 'Select All',
      unSelectAllText: 'UnSelect All',
      itemsShowLimit: 1,
      allowSearchFilter: true
    };
    this.initializeGrid();
      this.bolResolveForm = this.buildForm();
      this.LFVRecordEditForm = this.buildLFVRecordEditForm();
  }
  buildForm(): FormGroup {
    let fg = this.fb.group({
      BolNumber: this.fb.control('', [Validators.required]),
      LiftDate: this.fb.control('', [Validators.required]),
      GrossQuantity: this.fb.control('', [Validators.required]),
      NetQuantity: this.fb.control('', [Validators.required]),
      BadgeNumber: this.fb.control(''),
      SelectedTerminal: this.fb.control('', [Validators.required]),
      SelectedFuelType: this.fb.control('', [Validators.required]),
      Notes: this.fb.control(''),
      LIftFileRecordId: this.fb.control(''),
      InvoiceFtlDetailId: this.fb.control(''),
      IsBulkPlantLift: this.fb.control('')
    });
    this.SelectedFuelTypeList = [];
    this.SelectedTerminalList = [];
    return fg;
  }
  initFormData(data: LFBolEditModel) {
    this.bolResolveForm.reset();//clear previous values
    if ((this.bolResolveForm != null && this.bolResolveForm != undefined) && (data != null && data != undefined)) {
      this.bolResolveForm.get('BolNumber').setValue(data.BolNumber);
      this.bolResolveForm.get('LiftDate').setValue(data.DisplayLiftDate);
      this.bolResolveForm.get('GrossQuantity').setValue(data.GrossQuantity);
      this.bolResolveForm.get('NetQuantity').setValue(data.NetQuantity);
      this.bolResolveForm.get('Notes').setValue(data.Notes);
      this.bolResolveForm.get('LIftFileRecordId').setValue(data.LiftRecord.LiftFileRecordId);
      this.bolResolveForm.get('InvoiceFtlDetailId').setValue(data.InvoiceFtlDetailId);
      this.bolResolveForm.get('IsBulkPlantLift').setValue(data.IsBulkPlantLift);
      this.bolResolveForm.get('LIftFileRecordId').setValue(data.LiftRecord.LiftFileRecordId);
      this.bolResolveForm.get('BadgeNumber').setValue(data.BadgeNumber);
      if (data.IsBulkPlantLift == true) { // no terminal dropdown for pickup from bulk plants
        this.bolResolveForm.get('SelectedTerminal').clearValidators();
        this.bolResolveForm.get('SelectedTerminal').updateValueAndValidity();
      }
      if (data != null && data.SelectedTerminal != null) {
        this.bolResolveForm.get('SelectedTerminal').setValue(data.SelectedTerminal);
        this.SelectedTerminalList = [];
        this.SelectedTerminalList.push(data.SelectedTerminal);
      }
      if (data != null && data.SelectedFuelType != null) {
        this.bolResolveForm.get('SelectedFuelType').setValue(data.SelectedFuelType);
        this.SelectedFuelTypeList = [];
        this.SelectedFuelTypeList.push(data.SelectedFuelType);
      }
    }
  }

  createPostObject(): LFBolEditModel {
    let inputPostObject = new LFBolEditModel();
    inputPostObject.BadgeNumber = this.bolResolveForm.get('BadgeNumber').value;
    inputPostObject.BolNumber = this.bolResolveForm.get('BolNumber').value;
    inputPostObject.GrossQuantity = this.bolResolveForm.get('GrossQuantity').value;
    inputPostObject.InvoiceFtlDetailId = this.bolResolveForm.get('InvoiceFtlDetailId').value;
    inputPostObject.IsBulkPlantLift = this.bolResolveForm.get('IsBulkPlantLift').value;
    inputPostObject.LiftRecord.LiftFileRecordId = this.bolResolveForm.get('LIftFileRecordId').value;
    inputPostObject.LiftDate = this.bolResolveForm.get('LiftDate').value;
    inputPostObject.NetQuantity = this.bolResolveForm.get('NetQuantity').value;
    inputPostObject.Notes = this.bolResolveForm.get('Notes').value;
    let SelectedFuelType = this.bolResolveForm.get('SelectedFuelType').value;
    let fuelTypeId = SelectedFuelType[0].Id;
    inputPostObject.FuelTypeId = fuelTypeId;
    let selectedTerminal = this.bolResolveForm.get('SelectedTerminal').value;
    let terminalId = inputPostObject.IsBulkPlantLift ? selectedTerminal.Id : selectedTerminal[0].Id;
    inputPostObject.TerminalId = terminalId;
    return inputPostObject;
  }
  initializeGrid() {
    let exportInvitedColumns = { columns: ':visible' };
    this.dtOptions = {
      dom: '<"html5buttons"B>lTfgitp',
      buttons: [
        { extend: 'colvis' },
        { extend: 'copy', exportOptions: exportInvitedColumns },
        { extend: 'csv', title: 'LiftFileRecords', exportOptions: exportInvitedColumns },
        { extend: 'pdf', title: 'LiftFileRecords', orientation: 'landscape', exportOptions: exportInvitedColumns },
        { extend: 'print', exportOptions: exportInvitedColumns }
      ],
        pagingType: 'first_last_numbers',
        fixedHeader: false,
      pageLength: 10,
      lengthMenu: [[10, 25, 50, 100, -1], [10, 25, 50, 100, "All"]]
    };
  }
  ngAfterViewInit(): void {
    this.getLfvData();

  }
  public changeViewType(value) {
    this.viewType = value;
    if (value == 1)
      this.isValidationCarrier = false;
    else
      this.isValidationCarrier = true;
    this.getLfvData();
  }

  private getLfvData() {
    this.LFValidationList = [];
    let ids = [];
    let carrierOrderIds = "";
    if (this.filterComponent.selectedCarrierList.length == this.filterComponent.CarrierDrpDwnList.length) {
      carrierOrderIds = "";
    } else {
      this.filterComponent.selectedCarrierList && this.filterComponent.selectedCarrierList.forEach(res => { ids.push(res.Name) });
      carrierOrderIds = ids.join();
    }
    this.IsLoading = true;
    this._lfvService.getLFValidationGrid({ fromDate: this.filterComponent.fromDate, toDate: this.filterComponent.toDate, isCarrierPerFormanceDashboard: this.isValidationCarrier, carrierIds: carrierOrderIds, isMatchingWindow: this.filterComponent.isMatchingWindow }).subscribe(async (res) => {
      if (res)
        this.LFValidationList = res;
      else
        this.LFValidationList = [];

      this.IsLoading = false;
    })
  }

  public OnSearch($event): void {
    if ($event) {
      try {
        $("#liftfilerecords-datatable").DataTable().clear().destroy();
        this.gridType = LFVRecordStatus.None;
        this.LFVRecordGrid = [];
        // this.dtTrigger.next();
        this.getLfvData();

      } catch (e) {
      }
    }
  }

  changeGridType(status) {
    this.gridType = status;
    this.getLfvFilterGrid(status);
  }

  private getLfvFilterGrid(status) {
    // if ((this.datatableElement && this.datatableElement.dtInstance)) {
    //   this.datatableElement.dtInstance.then((dtInstance: DataTables.Api) => { dtInstance.destroy(); });
    // }
    try {
      $("#liftfilerecords-datatable").DataTable().clear().destroy();
    } catch (e) {
    }
    this.IsLoading = true;
    this.LFVRecordGrid = [];
    let ids = [];
    let carrierOrderIds = "";
    if (this.filterComponent.selectedCarrierList.length == this.filterComponent.CarrierDrpDwnList.length) {
      carrierOrderIds = "";
    } else {
      this.filterComponent.selectedCarrierList && this.filterComponent.selectedCarrierList.forEach(res => { ids.push(res.Name) });
      carrierOrderIds = ids.join();
    }
    this._lfvService.getLFVRecordGrid({ fromDate: this.filterComponent.fromDate, toDate: this.filterComponent.toDate, recordStatus: status, isMatchingWindow: this.filterComponent.isMatchingWindow, carrierIds: carrierOrderIds }).subscribe(async (res) => {
        if (res) {
            this.LFVRecordGrid = await res;
            if (this.LFVRecordGrid != null && this.LFVRecordGrid.length > 0) {
                this.isAdminUser = this.LFVRecordGrid[0].IsAdminUser;
                this.LfvValidationParameters = this.LFVRecordGrid[0].LfvValidationParameters;
            }
        }      
      else
        this.LFVRecordGrid = [];
      this.dtTrigger.next();

      this.IsLoading = false;
    })
  }
  public openLFVScratchReportGrid(): void {
    window.open("Supplier/LiftFile/LFVScratchReport", "_blank");
  }
  public openAccrualReportGrid(): void {
     window.open("Supplier/LiftFile/LFVAccrualReport", "_blank");
  }
  public selectAllRecords(eventData: any) {
    if (eventData != null && eventData != undefined) {
      if (eventData.target.checked) {
        this.isChecked = true;
      }
      else {
        this.isChecked = false;
      }
    }
  }
  public getLFRecordIds() {
    let LFRecordIds = [];
    let table = $('#liftfilerecords-datatable').DataTable();
    var rowcollection = table.$(".dt-checkbox", { "page": "all" });
    rowcollection.each(function (index, elem) {
      if ($(this).is(":checked")) {
        LFRecordIds.push(parseInt($(this).attr('id')));
      }
    });
    return LFRecordIds;
    }

    public getPreferencesSetting() {
        if (!this.preferenceSetting) {
            this._lfvService.getPreferencesSetting().subscribe(response => {
                this.preferenceSetting = response;
            });
        }
    }

  public GetReasonDescriptionList() {
    if (this.reasonList && this.reasonList.length == 0) {
      this.IsLoading = true;
      this._lfvService.GetReasonDescriptionList().subscribe((response: any) => {
        if (response && response.length > 0) {
          this.reasonList = response;
        }
        this.IsLoading = false;
      });
    }
  }
  public submitIgnoreDescription() {
    this.addRecordsForForcedIgnoreMatchProcessing(this.getLFRecordIds());
  }
  public ValidateForIgnoreMatchProcessing(status: string) { //ignore or reprocess

    let LFRecordIds = this.getLFRecordIds();
    this.selectedReason = [];

    if (LFRecordIds != null && LFRecordIds != undefined && LFRecordIds.length > 0) {
      if (status == 'ignore') {
         if (this.preferenceSetting && this.preferenceSetting.IsLiftFileValidationEnabled && this.preferenceSetting.IsReasonCodesEnabled) {
           this.GetReasonDescriptionList();
           document.getElementById('openIgnoreModal').click();
         } else {
           this.addRecordsForForcedIgnoreMatchProcessing(LFRecordIds);
         }
      }
      else if (status == 'reprocess')
        this.addUnmatchedRecordForReProcessing(LFRecordIds);
    }
    else { Declarations.msgerror("No Records selected", undefined, undefined); }

  }
  addRecordsForForcedIgnoreMatchProcessing(LFRecordIds: any[]) {
    let descriptionId: number = 0;
    let descriptionText: string = '';

    if (this.selectedReason && this.selectedReason.length > 0) {
      descriptionId = this.selectedReason[0].Id;
      descriptionText = this.selectedReason[0].Name;
    }

    this.IsLoading = true;
    this._lfvService.addRecordsForForcedIgnoreMatchProcessing(LFRecordIds, descriptionId, descriptionText).subscribe((response: any) => {
      if (response.StatusCode == 0) {
        Declarations.msgsuccess(response.StatusMessage, undefined, undefined);
        this.getLfvFilterGrid(this.gridType);
      } else {
        Declarations.msgerror(response.StatusMessage, undefined, undefined);
      }
    });
    this.IsLoading = true;

  }
  addUnmatchedRecordForReProcessing(LFRecordIds: any[]) {
    this.IsLoading = true;
    this._lfvService.addUnmatchedRecordForReProcessing(LFRecordIds).subscribe((response: any) => {
      if (response.StatusCode == 0) {
        Declarations.msgsuccess(response.StatusMessage, undefined, undefined);
        this.getLfvFilterGrid(this.gridType);
      } else {
        Declarations.msgerror(response.StatusMessage, undefined, undefined);
      }
    });
    this.IsLoading = true;

  }
  getBolDetailsForResolve(lfRecord: LFRecordGridModel) {
    this.IsLoading = true;
    lfRecord.IsFromScratchReport = true;
    this._lfvService.getBolDetailsForResolve(lfRecord).subscribe((data: LFBolEditModel) => {
      if (data) {
        this._toggleOpened(true);
        this.selectedLiftFileRecord = data.LiftRecord;
        this.TerminalList = data.TerminalList;
        this.FuelTypeList = data.FuelTypeList;
        this.InvoiceFtlDetailIdList = data.InvoiceFtlDetailsList;
        this.initFormData(data)
        this.IsLoading = false;
      }
    });
  }
  onSubmit() {
    this.IsLoading = true;
    this.bolResolveForm.markAsTouched();
    if (this.bolResolveForm.valid) {
      let requestObj = this.createPostObject();
      if (requestObj != null) {
        this._lfvService.saveBolDetailsForResolve(requestObj).subscribe((response: any) => {
          if (response.StatusCode == 0) {
            this.IsLoading = false;
            Declarations.msgsuccess(response.StatusMessage, undefined, undefined);
            this._toggleOpened(false);
            this.getLfvFilterGrid(this.gridType);
          } else {
            this.IsLoading = false;
            Declarations.msgerror(response.StatusMessage, undefined, undefined);
          }
        });
      }
    }
    this.IsLoading = false;
  }
  _toggleOpened(shouldOpen: boolean) {
    if (shouldOpen) {
      this._opened = true;
    }
    else {
      this._opened = !this._opened;
      this.bolResolveForm.reset();
    }
    }

  GetBolRecord(InvoiceFtlDetailId: any) {
    if (InvoiceFtlDetailId != null && InvoiceFtlDetailId != undefined && InvoiceFtlDetailId != '') {
      let selectedLiftFileRecordId = this.selectedLiftFileRecord.LiftFileRecordId;
      let invoiceFtlDetailId = parseInt(InvoiceFtlDetailId);
      let LFRecord = new LFRecordGridModel();

      LFRecord.LiftFileRecordId = selectedLiftFileRecordId;
      LFRecord.InvFtlDetailId = invoiceFtlDetailId;
      LFRecord.bol = this.selectedLiftFileRecord.bol;
      LFRecord.TerminalName = this.selectedLiftFileRecord.TerminalName;
      LFRecord.TerminalItemCode = this.selectedLiftFileRecord.TerminalItemCode;
      LFRecord.LoadDate = this.selectedLiftFileRecord.LoadDate;
      LFRecord.ProductType = this.selectedLiftFileRecord.ProductType;
      LFRecord.correctedQuantity = this.selectedLiftFileRecord.correctedQuantity;
      LFRecord.IsFromScratchReport = true;
      this.getBolDetailsForResolve(LFRecord);
    }
  }
  openSupplierBOLReportGrid() {
    window.open("Supplier/LiftFile/SupplierBolReport", "_blank");
  }
  openCarrierBOLReportGrid() {
    window.open("Supplier/LiftFile/CarrierBolReport", "_blank");
  }

 
  searchLiftFileRecords() {
    let bolQuery = this.bolSearchQuery;
    let fileNameQuery = this.fileNameSearchQuery;
    if ((bolQuery == "" || bolQuery == null || bolQuery == undefined) &&
      (fileNameQuery == "" || fileNameQuery == undefined || fileNameQuery == null)) {
      Declarations.msgerror("Please provide either Bol# or Filename", undefined, undefined);
    }
    else {
      let exportColumns = { columns: ':visible' };
      this.searchGridDtOptions = {
        dom: '<"html5buttons"B>lTfgitp',
        buttons: [
          { extend: 'colvis' },
          { extend: 'copy', exportOptions: exportColumns },
          { extend: 'csv', title: 'Lift File Records', exportOptions: exportColumns },
          { extend: 'pdf', title: 'Lift File Records', orientation: 'landscape', exportOptions: exportColumns },
          { extend: 'print', exportOptions: exportColumns }
        ],
          pagingType: 'first_last_numbers',
          fixedHeader:false,
        pageLength: 10,
        lengthMenu: [[10, 25, 50, 100, -1], [10, 25, 50, 100, "All"]],
      };
      let bolNumber = (bolQuery == null || bolQuery == undefined) ? "" : bolQuery.trim();
      let fileName = (fileNameQuery == null || fileNameQuery == undefined) ? "" : fileNameQuery.trim();
      this.IsLoading = true;
      this._lfvService.getLFSearchRecords(bolNumber, fileName).subscribe(data => {
        let el: HTMLElement = this.btnOpenModal.nativeElement;
        el.click();
        $("#liftfileSearchrecords-datatable").DataTable().clear().destroy();
        this.LiftFilesearchResults = data;
        this.searchGridDtTrigger.next();
        this.IsLoading = false;
      });
    }

  }
  toggleRecordSearchControls(shouldShow: boolean) {
    this.bolSearchQuery = "";
    this.fileNameSearchQuery = "";
    this.showSearchControls = shouldShow;
    this.showSearchBtn = shouldShow;

  }
  async OnExport(status) {
    this.generateCSV(status);
  }

  public generateCSV(status) {
    this.IsLoading = true;
    var exportData: LFRecordsGridExport[] = [];
    let ids = [];
    let carrierOrderIds = "";
    if (this.filterComponent.selectedCarrierList.length == this.filterComponent.CarrierDrpDwnList.length) {
      carrierOrderIds = "";
    } else {
      this.filterComponent.selectedCarrierList && this.filterComponent.selectedCarrierList.forEach(res => { ids.push(res.Name) });
      carrierOrderIds = ids.join();
    }
    this._lfvService.getLFVRecordGrid({ fromDate: this.filterComponent.fromDate, toDate: this.filterComponent.toDate, recordStatus: status, isMatchingWindow: this.filterComponent.isMatchingWindow, carrierIds: carrierOrderIds }).subscribe(async (res) => {
      if (res)
          exportData = res.map(m => {
              return {
                  bol: m.bol, TerminalName: m.TerminalName, Terminal: m.Terminals,correctedQuantity:m.correctedQuantity,
        TerminalItemCode:m.TerminalItemCode,  ProductType:m.ProductType,   LoadDate:m.LoadDate,RecordDate:m.RecordDate,
          CarrierID: m.CarrierID, CarrierName: m.CarrierName, Reason: m.Reason, 
                  ReasonCode: m.ReasonCode, ReasonCategory: m.ReasonCategory, Username: m.Username, ModifiedDate: m.ModifiedDate, LFVResolutionTime: m.LFVResolutionTime, TimeToBol: m.TimeToBol    }
      })
      else
        exportData = [];
      new AngularCsv(exportData, 'LFV_' + new Date(), this.csvOptions);
      this.IsLoading = false;

    })
    }

    _EditToggleOpened(shouldOpen: boolean) {
        if (shouldOpen) {
            this._EditOpened = true;
        }
        else {
            this._EditOpened = !this._EditOpened;
            this.LFVRecordEditForm.reset();
        }
    }

    editLiftFileRecord(record: LFRecordsGridViewModel) {
        if (record != null) {
            this.IsLoading = true;
            this._EditToggleOpened(true);
            this.initRecordEditForm(record);
            this.IsLoading = false;
        }
    }
    buildLFVRecordEditForm(): FormGroup {
        let formGroup = this.fb.group({
            LIftFileRecordId: this.fb.control(''),
            BolNumber: this.fb.control(''),
            TerminalCode: this.fb.control(''),
            TerminalItemCode: this.fb.control(''),
            LoadDate: this.fb.control(''),
            CorrectedQuantity: this.fb.control(''),
            GrossQuantity: this.fb.control(''),
            ProductType: this.fb.control(''),
            RecordDate: this.fb.control(''),
            CarrierId: this.fb.control(''),
            CIN: this.fb.control(''),
            CarrierName: this.fb.control('')
        });       
        return formGroup;
    }
    initRecordEditForm(record: LFRecordsGridViewModel) {
        this.LFVRecordEditForm.reset();
        if (record != null && record != undefined) {
            this.LFVRecordEditForm.get('LIftFileRecordId').setValue(record.LiftFileRecordId);
            this.LFVRecordEditForm.get('BolNumber').setValue(record.bol);
            this.LFVRecordEditForm.get('TerminalCode').setValue(record.TerminalName);
            this.LFVRecordEditForm.get('TerminalItemCode').setValue(record.TerminalItemCode);
            this.LFVRecordEditForm.get('LoadDate').setValue(record.LoadDate);
            this.LFVRecordEditForm.get('CorrectedQuantity').setValue(record.correctedQuantity);
            this.LFVRecordEditForm.get('GrossQuantity').setValue(record.GrossQuantity);
            this.LFVRecordEditForm.get('ProductType').setValue(record.ProductType);
            this.LFVRecordEditForm.get('RecordDate').setValue(record.RecordDate);
            this.LFVRecordEditForm.get('CarrierId').setValue(record.CarrierID);
            this.LFVRecordEditForm.get('CIN').setValue(record.CIN);
            this.LFVRecordEditForm.get('CarrierName').setValue(record.CarrierName);
            if (this.LfvValidationParameters != null) {
                if (this.LfvValidationParameters.IsBolReq) {
                    this.LFVRecordEditForm.controls['BolNumber'].setValidators([Validators.required]);
                    this.LFVRecordEditForm.controls['BolNumber'].updateValueAndValidity();
                }
                if (this.LfvValidationParameters.IsCorrectedQtyRes || this.LfvValidationParameters.IsGrossReq) {
                    this.LfvValidationParameters.IsCorrectedQtyOrGrossReq = true;
                    this.LFVRecordEditForm.controls['CorrectedQuantity'].setValidators([Validators.required, Validators.pattern(this.QuantityPattern)]);
                    this.LFVRecordEditForm.controls['CorrectedQuantity'].updateValueAndValidity();
                    this.LFVRecordEditForm.controls['GrossQuantity'].setValidators([Validators.required, Validators.pattern(this.QuantityPattern)]);
                    this.LFVRecordEditForm.controls['GrossQuantity'].updateValueAndValidity();
                }
                //if (this.LfvValidationParameters.IsGrossReq) {
                   
                //}
                if (this.LfvValidationParameters.IsLoadDateReq) {
                    this.LFVRecordEditForm.controls['LoadDate'].setValidators([Validators.required]);
                    this.LFVRecordEditForm.controls['LoadDate'].updateValueAndValidity();
                }
                if (this.LfvValidationParameters.IsTerminalCodeReq) {
                    this.LFVRecordEditForm.controls['TerminalCode'].setValidators([Validators.required]);
                    this.LFVRecordEditForm.controls['TerminalCode'].updateValueAndValidity();
                }
                if (this.LfvValidationParameters.IsTermItemCodeReq) {
                    this.LFVRecordEditForm.controls['TerminalItemCode'].setValidators([Validators.required]);
                    this.LFVRecordEditForm.controls['TerminalItemCode'].updateValueAndValidity();
                }
                if (this.LfvValidationParameters.IsCINReq) {
                    this.LFVRecordEditForm.controls['CIN'].setValidators([Validators.required]);
                    this.LFVRecordEditForm.controls['CIN'].updateValueAndValidity();
                }
                if (this.LfvValidationParameters.IsCarrierNameReq) {
                    this.LFVRecordEditForm.controls['CarrierName'].setValidators([Validators.required]);
                    this.LFVRecordEditForm.controls['CarrierName'].updateValueAndValidity();
                }
            }
        }
    }
    onRecordEditSubmit() {
        this.LFVRecordEditForm.markAllAsTouched();
        if(this.LFVRecordEditForm.valid) {
            this.IsLoading = true;
            let values = this.LFVRecordEditForm.value;          
            if (values != null)
            {
                let data = new LFRecordGridModel();

                data.Terminal = this.LFVRecordEditForm.get('TerminalCode').value;
                data.bol = this.LFVRecordEditForm.get('BolNumber').value;
                data.correctedQuantity = this.LFVRecordEditForm.get('CorrectedQuantity').value;
                data.CarrierName = this.LFVRecordEditForm.get('CarrierName').value;
                data.CIN = this.LFVRecordEditForm.get('CIN').value;
                data.TerminalItemCode = this.LFVRecordEditForm.get('TerminalItemCode').value;
                data.GrossQuantity = this.LFVRecordEditForm.get('GrossQuantity').value;
                data.LoadDate = this.LFVRecordEditForm.get('LoadDate').value;
                data.LiftFileRecordId = this.LFVRecordEditForm.get('LIftFileRecordId').value;

                let requestModel = this.correctValues(data);
                this._lfvService.updateLiftFileRecord(requestModel).subscribe((response: any) => {
                    this.IsLoading = false;
                    if (response.StatusCode == 0) {
                        Declarations.msgsuccess(response.StatusMessage, undefined, undefined);

                    } else if (response.StatusCode == 1) {
                        Declarations.msgerror(response.StatusMessage, undefined, undefined);
                    }
                    this._EditToggleOpened(false); 
                    this.getLfvFilterGrid(this.gridType);
                });
            }           
        }
    }

    correctValues(data: LFRecordGridModel): LFRecordGridModel{
        if (data.Terminal === '--') {
            data.Terminal = null;
        }
        if (data.bol === '--') {
            data.bol = null;
        }
        if (data.CarrierName === '--') {
            data.CarrierName = null;
        }
        if (data.CIN === '--') {
            data.CIN = null;
        }
        if (data.TerminalItemCode === '--') {
            data.TerminalItemCode = null;
        }
        if (data.LoadDate === '--') {
            data.LoadDate = null;
        } else {
            var loadDateWithOutSlash = data.LoadDate.replace(/\//g, '')
            data.LoadDate = loadDateWithOutSlash;
        }
        return data;
       
    }
    
}
    