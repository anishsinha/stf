import { Component, Input, OnInit, SimpleChanges } from '@angular/core';
import { Subject } from 'rxjs';
import { DropdownItem } from 'src/app/carrier-companies/service/assigncarrier.service';
import { CarrierDetailsViewModel } from 'src/app/carrier/models/CarrierDetailsViewModel';
import { TerminalMappingModel } from 'src/app/carrier/models/location';
import { CarrierService } from 'src/app/carrier/service/carrier.service';
import { DispatcherService } from 'src/app/carrier/service/dispatcher.service';
import { Declarations } from 'src/app/declarations.module';
import { MyLocalStorage } from 'src/app/my.localstorage'

@Component({
    selector: 'app-company-carrier-mapping',
    templateUrl: './company-carrier-mapping.component.html',
    styleUrls: ['./company-carrier-mapping.component.css']
})
export class CompanyCarrierMappingComponent implements OnInit {
    @Input() countryId: number;
    isShowLoader = false;
    dtOptions: any = {};
    dtTrigger: Subject<any> = new Subject();
    CarrierDataToBeSend: CarrierDetailsViewModel;
    public isShowCountryDDL: boolean = false;
    public CarrierDetails: CarrierDetailsViewModel[] = [];
    public HeaderText: string;
    public IsLoading: boolean = false;
    ddlSettingsById: any = {};
    //TerminalList = [];  
    carrierList = [];    
    //Carriermapping: CarrierDetailsViewModel = {};
    SelectedCarrierList = [];
    SelectedTerminalList = [];
    public popoverTitle: string = 'Are you sure?';
    public confirmButtonText: string = 'Yes';
    public cancelButtonText: string = 'No';

    
    //
    public SelectedCarrierName: string;
    public selectedCarrierId: number;
    public AssignedTerminalIdList = [];
    public carriers = [];
    public AssignedCarrierId: string;
    public CountryId: number;
  
    constructor(private carrierService: CarrierService, private dispatcherService: DispatcherService) {

    }

    ngOnInit() {
        this.ddlSettingsById = {
            singleSelection: true,
            idField: 'Id',
            textField: 'Name',
            selectAllText: 'Select All',
            unSelectAllText: 'UnSelect All',
            itemsShowLimit: 5,
            allowSearchFilter: true
        };
        
        this.initializeCarrierGridInfo();
       // this.getCarrierMappingsData();
        this.isShowCountryDDL = false;
        this.getTerminalIdsForMapping();
        this.getCarriers();        
    }

    ngOnChanges(changes: SimpleChanges): void {
        // if (isNaN(this.SelectedCountryId) || this.SelectedCountryId == 0) {
        //     this.getDefaultServingCountry();
        // }
        // else {
        if (changes.countryId && changes.countryId.currentValue) {
            
            this.clearForm();
        }
        //}
    }

    private getTerminalIdsForMapping() {
        this.IsLoading = true;
        this.AssignedTerminalIdList = [];
        this.carrierService.getAssignedTerminalIdsForMapping().subscribe((response) => {
            this.AssignedTerminalIdList = response;
            this.IsLoading = false;
        });
    }
    onCarrierNameSearched(event: any): void {
        let keyword = event.target.value.toLowerCase();
        this.carriers = this.carrierList.slice().filter(function (elem) {
            return elem.Name && elem.Name.toLowerCase().indexOf(keyword) >= 0;
        });       
    }

    onCarrierSelected(event: any) {
        this.SelectedCarrierName = event.Name;
        this.selectedCarrierId = event.Id;
        this.carriers = this.carrierList.slice();
    }

    private getCarriers() {
        this.dispatcherService.GetCarriersForSupplier().subscribe(data => {
            this.carrierList = data;
        });
    }
    getCarrierMappingsData() {
        this.isShowLoader = true;
        this.carrierService.getCarrierData(this.countryId).subscribe(data => {
            this.CarrierDetails = data;
            this.isShowLoader = false;
            this.dtTrigger.next();
           
        });
    }

    deleteMapping(carrier: CarrierDetailsViewModel) {
        let mappingId = carrier.Id;
        this.carrierService.deleteCarrierMapping(mappingId).subscribe(response => {
            if (response.StatusCode == 0) {
                Declarations.msgsuccess(response.StatusMessage, undefined, undefined);
                $("#carrier-grid-datatable").DataTable().clear().destroy();
                this.getCarrierMappingsData();

            }
            else if (response.StatusCode == 1) {
                Declarations.msgerror(response.StatusMessage, undefined, undefined);
            }
        });
      
    }

    setPanelHeader(headerText: string) {
        this.HeaderText = headerText;
    }

    cancelMapping(carrier: CarrierDetailsViewModel) {
        this.getCarrierMappingsData();
    }

    updateMapping(carrier: CarrierDetailsViewModel, rowIndex: number) {
        let assignedCarrierName = jQuery('#CarrierName_'+rowIndex).text();
        let assignedCarrierId = jQuery('#CarrierID_'+rowIndex).text();
        if (assignedCarrierName && assignedCarrierId) {
            if ((assignedCarrierName.trim() == carrier.CarrierName)
                && (assignedCarrierId.trim() == carrier.AssignedCarrierId)) {
                Declarations.msgwarning("Update Assigned CarrierID/CarrierName ", undefined, undefined);
                return;
            }
            let carrierInput = new CarrierDetailsViewModel();
            carrierInput.AssignedTerminalId.Id = carrier.TerminalCompanyAliasId;
            carrierInput.AssignedTerminalId.Name = carrier.AssignedTerminalIdName;
            carrierInput.Id = carrier.Id;
            carrierInput.CountryId = this.countryId;
            carrierInput.CarrierName = carrier.CarrierName;
            carrierInput.TerminalCompanyAliasId = carrier.TerminalCompanyAliasId;
            carrierInput.AssignedCarrierId = carrier.AssignedCarrierId;
            if (assignedCarrierName.trim() != carrier.CarrierName) {
                carrierInput.CarrierName = assignedCarrierName.trim();
            }
            if (assignedCarrierId.trim() != carrier.AssignedCarrierId) {
                carrierInput.AssignedCarrierId = assignedCarrierId.trim();
            }
            this.carrierService.SaveCarrierMapping(carrierInput).subscribe(data => {
                this.IsLoading = false;
                this.isShowLoader = false;
                if (data.StatusCode == 0) {
                    Declarations.msgsuccess(data.StatusMessage, undefined, undefined);
                    $("#carrier-grid-datatable").DataTable().clear().destroy();
                    this.getCarrierMappingsData();
                    //this.updatedCarrierName = null;
                    //this.updatedCarrierId = null;
                    //this.selectedMappingId = null;
                }
                else if (data.StatusCode == 1) {
                    Declarations.msgerror(data.StatusMessage, undefined, undefined);
                }
            });
        }
        else {
            Declarations.msgwarning("Update Assigned CarrierID/CarrierName ", undefined, undefined);
            return;
        }        
    }

    //editCarrier(carrier) {
    //    this.CarrierDataToBeSend = carrier;
    //}

    clearForm() {
        $("#carrier-grid-datatable").DataTable().clear().destroy();
        this.getTerminalIdsForMapping();
        this.getCarrierMappingsData();
    }

    initializeCarrierGridInfo() {
        let exportInvitedColumns = { columns: ':visible' };
        this.dtOptions = {
            dom: '<"html5buttons"B>lTfgitp',
            buttons: [
                { extend: 'colvis' },
                { extend: 'copy', exportOptions: exportInvitedColumns },
                { extend: 'csv', title: 'Carrier Details', exportOptions: exportInvitedColumns },
                { extend: 'pdf', title: 'Carrier Details', orientation: 'landscape', exportOptions: exportInvitedColumns },
                { extend: 'print', exportOptions: exportInvitedColumns }
            ],
            pagingType: 'first_last_numbers',
            pageLength: 10,
            lengthMenu: [[10, 25, 50, 100, -1], [10, 25, 50, 100, "All"]]
        };
    }
   

    public onSubmit() {
        if (this.IsValidForm())
        {
            this.IsLoading = true;
            this.isShowLoader = true;
            let carrierMapping = new CarrierDetailsViewModel();
            carrierMapping.AssignedTerminalId = this.SelectedTerminalList[0];
            carrierMapping.CarrierName = this.SelectedCarrierName;
            carrierMapping.AssignedCarrierId = this.AssignedCarrierId;
            carrierMapping.CountryId = this.countryId;
           
            this.carrierService.SaveCarrierMapping(carrierMapping).subscribe(data => {
                this.IsLoading = false;
                this.isShowLoader = false;
                if (data.StatusCode == 0) {
                    Declarations.msgsuccess(data.StatusMessage, undefined, undefined);
                    $("#carrier-grid-datatable").DataTable().clear().destroy();
                    this.getCarrierMappingsData();
                    this.SelectedTerminalList = [];
                    this.SelectedCarrierName = "";
                    this.AssignedCarrierId = "";
                }
                else if (data.StatusCode == 1) {
                    Declarations.msgerror(data.StatusMessage, undefined, undefined);
                }
            });            
        }
        else
        {
            Declarations.msgerror("Terminal ID/CarrierName/CarrierID is required", undefined, undefined);
        }
    }

    public IsValidForm() {
        let isValid = false;
        let selectedTerminalId = this.IsValidValue (this.SelectedTerminalList[0]);
        let selectedCarrierName = this.IsValidValue(this.SelectedCarrierName);
        let assignedCarrierId = this.IsValidValue(this.AssignedCarrierId);  
        if (selectedTerminalId && selectedCarrierName && assignedCarrierId)
        {
            isValid = true;
        }
        return isValid;
        
    }
    public IsValidValue(value:any)
    {
        if (value) {
            return true;
        } else
        { return false; }
    }
}

