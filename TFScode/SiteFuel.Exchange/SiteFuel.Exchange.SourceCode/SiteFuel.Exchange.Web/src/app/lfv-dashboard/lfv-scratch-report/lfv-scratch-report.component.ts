import { Component, OnInit, OnDestroy } from '@angular/core';
import { Subject } from 'rxjs';
import { LFRecordGridModel, LFBolEditModel, DropDownItem } from '../LiftFileModels';
import { IDropdownSettings } from 'ng-multiselect-dropdown';
import { LiftfiledashboardserviceService } from '../service/liftfiledashboardservice.service';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { Declarations } from 'src/app/declarations.module';

@Component({
    selector: 'app-lfv-scratch-report',
    templateUrl: './lfv-scratch-report.component.html',
    styleUrls: ['./lfv-scratch-report.component.css']
})
export class LfvScratchReportComponent implements OnInit, OnDestroy {

    //side bar related variables
    public _opened: boolean = false;
    public _animate: boolean = true;
    public _positionNum: number = 1;
    public _POSITIONS: Array<string> = ['left', 'right', 'top', 'bottom'];

    //grid variables
    public LFRecords: LFRecordGridModel[] = [];
    public cancelButtonText: string = 'No';
    dtOptions: any = {};
    dtTrigger: Subject<any> = new Subject();
    public ShowGridLoader: boolean = false;
    public isChecked = false;
    public LFRecordIdsForIgnoreMatch: number[] = [];

    //partial match resolve related variables
    public bolResolveForm: FormGroup;
    public selectedLiftFileRecord: LFRecordGridModel; //used for display purpose only
    public ShowSideBarLoader: boolean = false;
    public TerminalList: DropDownItem[];
    public FuelTypeList: DropDownItem[];
    public multiselectSettingsById: IDropdownSettings;
    public InvoiceFtlDetailIdList: DropDownItem[];
    public SelectedBolProduct: any;
    public SelectedTerminalList: DropDownItem[] = [];
    public SelectedFuelTypeList: DropDownItem[] = [];

    constructor(private fb: FormBuilder, private dashboardservice: LiftfiledashboardserviceService) { }
    ngOnInit() {

        this.multiselectSettingsById = {
            singleSelection: true,
            idField: 'Id',
            textField: 'Name',
            selectAllText: 'Select All',
            unSelectAllText: 'UnSelect All',
            itemsShowLimit: 1,
            allowSearchFilter: true
        };

        this.intializeGrid();
        this.bolResolveForm = this.buildForm();
        this.getPreferencesSetting();
    }
    intializeGrid() {
        this.ShowGridLoader = true;
        let exportColumns = { columns:[ 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10,11,12,13,14,15,16]};
        this.dtOptions = {
            dom: '<"html5buttons"B>lTfgitp',
            buttons: [
                { extend: 'colvis' },
                { extend: 'copy', exportOptions: exportColumns },
                { extend: 'csv', title: 'Scratch Report', exportOptions: exportColumns },
                { extend: 'pdf', title: 'Scratch Report', orientation: 'landscape', exportOptions: exportColumns },
                { extend: 'print', exportOptions: exportColumns }
            ],
            pagingType: 'first_last_numbers',
            pageLength: 10,
            lengthMenu: [[10, 25, 50, 100, -1], [10, 25, 50, 100, "All"]],
        };
        this.getLFRecords();
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
    getLFRecords() {
        this.ShowGridLoader = true;
        this.dashboardservice.getLFRecords().subscribe((data: LFRecordGridModel[]) => {
            this.ShowGridLoader = false;
            this.LFRecords = data;
            this.dtTrigger.next();

        });
    }
    reloadGrid() {
        $("#liftfilerecords-datatable").DataTable().clear().destroy();
        this.getLFRecords();
    }

    getBolDetailsForResolve(lfRecord: LFRecordGridModel) {
        this.ShowSideBarLoader = true;
        lfRecord.IsFromScratchReport = true;
        this.dashboardservice.getBolDetailsForResolve(lfRecord).subscribe((data: LFBolEditModel) => {
            if (data) {
                this._toggleOpened(true);
                this.selectedLiftFileRecord = data.LiftRecord;
                this.TerminalList = data.TerminalList;
                this.FuelTypeList = data.FuelTypeList;
                this.InvoiceFtlDetailIdList = data.InvoiceFtlDetailsList;
                this.initFormData(data)
                this.ShowSideBarLoader = false;
            }
        });
    }
    selectAllRecords(eventData: any) {
        if (eventData != null && eventData != undefined) {
            if (eventData.target.checked) {
                this.isChecked = true;
            }
            else {
                this.isChecked = false;
            }
        }
    }
    ValidateForIgnoreMatchProcessing() {
        let LFRecordIds = this.getLFRecordIds();
        
        this.selectedReason = [];

        if (LFRecordIds != null && LFRecordIds != undefined && LFRecordIds.length > 0) {

            if (this.preferenceSetting && this.preferenceSetting.IsLiftFileValidationEnabled && this.preferenceSetting.IsReasonCodesEnabled) {
                this.GetReasonDescriptionList();
                document.getElementById('openIgnoreModal2').click();
              } else {
                this.addRecordsForForcedIgnoreMatchProcessing(LFRecordIds);
              }
        }
        else{ Declarations.msgerror("No Records selected", undefined, undefined);}

    }
    addRecordsForForcedIgnoreMatchProcessing(LFRecordIds: any[]) {

        let descriptionId: number = 0;
        let descriptionText: string = '';
    
        if (this.selectedReason && this.selectedReason.length > 0) {
          descriptionId = this.selectedReason[0].Id;
          descriptionText = this.selectedReason[0].Name;
        }

        this.ShowSideBarLoader = true;
        this.dashboardservice.addRecordsForForcedIgnoreMatchProcessing(LFRecordIds, descriptionId, descriptionText).subscribe((response: any) => {
            if (response.StatusCode == 0) {
                Declarations.msgsuccess(response.StatusMessage, undefined, undefined);
                this.reloadGrid();
            } else {
                Declarations.msgerror(response.StatusMessage, undefined, undefined);
            }
        });
        this.ShowSideBarLoader = false;

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

    initFormData(data: LFBolEditModel) {
        var currObj = this;
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
    createPostObject(): LFBolEditModel{
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

    //resetSelections(isReset: boolean) {
    //    if (isReset) {
    //        this.isChecked = false;
    //    }
    //    else {
    //        this.isChecked = true;
    //    }

    //}
    redirectToMyApprovalTab() {
        window.open("Supplier/Exception/Manage", "_blank");
    }

    onSubmit() {
        this.ShowSideBarLoader = true;
        this.bolResolveForm.markAsTouched();
        if (this.bolResolveForm.valid) {
            let requestObj = this.createPostObject();
            if (requestObj != null) {
                this.dashboardservice.saveBolDetailsForResolve(requestObj).subscribe((response:any) => {
                    if (response.StatusCode == 0) {
                        this.ShowSideBarLoader = false;
                        Declarations.msgsuccess(response.StatusMessage, undefined, undefined);
                        this._toggleOpened(false);
                        this.reloadGrid();
                    } else {
                        this.ShowSideBarLoader = false;
                        Declarations.msgerror(response.StatusMessage, undefined, undefined);
                    }
                });
            }
        }
        this.ShowSideBarLoader = false;
    }

    _toggleOpened(shouldOpen: boolean) {
        if(shouldOpen) {
            this._opened = true;
        }
        else {
            this._opened = !this._opened;
            this.bolResolveForm.reset();
        }
    }

    ngOnDestroy(){
        this.dtTrigger.unsubscribe();
    }

    //ignore by reason
    public preferenceSetting: any = null;
    public selectedReason = [];
    public reasonList = [];
    public dropdownSettings: IDropdownSettings = { singleSelection: true, idField: 'Id', textField: 'Name', allowSearchFilter: true };
    
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
                this.dashboardservice.getPreferencesSetting().subscribe(response => {
                    this.preferenceSetting = response;
                });
            }
        }
    
      public GetReasonDescriptionList() {
        if (this.reasonList && this.reasonList.length == 0) {
          this.ShowGridLoader = true;
          this.dashboardservice.GetReasonDescriptionList().subscribe((response: any) => {
            if (response && response.length > 0) {
              this.reasonList = response;
            }
            this.ShowGridLoader = false;
          });
        }
      }
      public submitIgnoreDescription() {
        this.addRecordsForForcedIgnoreMatchProcessing(this.getLFRecordIds());
      }

}
