import { Component, OnInit, QueryList, ViewChildren, ViewEncapsulation } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { DataTableDirective } from 'angular-datatables';
import { IDropdownSettings } from 'ng-multiselect-dropdown';
import { Subject } from 'rxjs';
import { debounceTime, distinctUntilChanged } from 'rxjs/operators';
import { DefTrailerAssetType, FuelTrailerAssetType, ServiceOfferingType } from 'src/app/app.enum';
import { Declarations } from 'src/app/declarations.module';
import { InvitationService } from 'src/app/invitation/invitation.service';
import { ServiceArea } from 'src/app/invitation/third-party-invitation.model';
import { DropdownItem, StateDropdownExtendedItem, StatelistService } from 'src/app/statelist.service';
import { CarrierDetailsModel, ThirdPartyCompanyFilter } from '../third-party-network.model';
import { ThirdPartyNetworkService } from '../third-party-network.service';

@Component({
  selector: 'app-dashboard',
  templateUrl: './dashboard.component.html',
  styleUrls: ['./dashboard.component.scss'],
  encapsulation: ViewEncapsulation.None
})

export class DashboardComponent implements OnInit {
  dtTrigger1: Subject<any> = new Subject();
  dtTrigger2: Subject<any> = new Subject();
  dtOptions: any = {};
  //get accessors
  get StatesListByCountry(): any { return (countryId: number) => this.StateList.filter(x => x.CountryId == countryId); }

  PageLoader: boolean = false;
  InvitationToken: string = null;
  RegisteredCompanies: CarrierDetailsModel[] = [];
  NonRegisteredCompanies: CarrierDetailsModel[] = [];
  //PANEL
  public IsRegisteredCompany: boolean = null;
  public IsThirdPartyEnabled: boolean = null;
  public _opened: boolean = false;
  public _initiated: boolean = false;
  public SelectedCompany: any;
  public FuelTrailerAssetType: typeof FuelTrailerAssetType = FuelTrailerAssetType;
  public DefTrailerAssetType: typeof DefTrailerAssetType = DefTrailerAssetType;
  public ServiceOfferingTypes: typeof ServiceOfferingType = ServiceOfferingType;
  public ServiceOfferingTypesDisplay: { [index: number]: string } = {};

  //filter
  FilterForm: FormGroup;
  CountryList: DropdownItem[] = [];
  StateList: StateDropdownExtendedItem[] = [];
  Citylist: DropdownItem[] = [];
  ServiceList: DropdownItem[] = [];
  CitiesWithZip: ServiceArea[] = [];

  ddlSetting: IDropdownSettings = {
    idField: 'Id',
    textField: 'Name',
    itemsShowLimit: 1,
    allowSearchFilter: true,
  };
  ZipDdlSettings: IDropdownSettings = {
    singleSelection: false,
    idField: 'ZipCode',
    textField: 'ZipCode',
    selectAllText: 'Select All',
    unSelectAllText: 'UnSelect All',
    itemsShowLimit: 4,
    allowSearchFilter: true,
    enableCheckAll: true
  }
  ddlCitySettings: IDropdownSettings = {
    singleSelection: false,
    idField: 'CityId',
    textField: 'CityName',
    selectAllText: 'Select All',
    unSelectAllText: 'UnSelect All',
    itemsShowLimit: 4,
    allowSearchFilter: true,
    enableCheckAll: true
  }

  constructor(
    private thirdPartyNetworkService: ThirdPartyNetworkService,
    private fb: FormBuilder,
    private stateListService: StatelistService,
    private invitationService: InvitationService
  ) { }

  ngOnInit() {
    this.initializeCarrierCustomers();
    this.GetInvitationTokenByCompany();
    this.GetRegisteredInvitedCompanies();
    this.GetNonRegisteredInvitedCompanies();
    this.getCountryList();
    this.getStatesOfAllCountries();
    this.initilizeFilterForm();
    this.InitializeServiceDropdown();
    this.bindServiceOfferingType();
  }

  @ViewChildren(DataTableDirective) dtElements: QueryList<DataTableDirective>;
  refreshDatatable(): void {
    this.dtElements.forEach((dtElement: DataTableDirective) => {
      if (dtElement.dtInstance) {
        dtElement.dtInstance.then((dtInstance: DataTables.Api) => { dtInstance.destroy(); });
      }
    });
    this.dtTrigger1.next();
    this.dtTrigger2.next();
  }
  initializeCarrierCustomers() {
    let exportInvitedColumns = { columns: ':visible' };
    this.dtOptions = {
        dom: '<"html5buttons"B>lTfgitp',
        buttons: [
            { extend: 'colvis' },
            { extend: 'copy', exportOptions: exportInvitedColumns },
            { extend: 'csv', title: 'Customer Details', exportOptions: exportInvitedColumns },
            { extend: 'pdf', title: 'Customer Details', orientation: 'landscape', exportOptions: exportInvitedColumns },
            { extend: 'print', exportOptions: exportInvitedColumns }
        ],
        pagingType: 'first_last_numbers',
        pageLength: 10,
        fixedHeader: false,
        lengthMenu: [[10, 25, 50, 100, -1], [10, 25, 50, 100, "All"]]
    };
}

  public getStateNameById(countryId: number, stateId: number) {
    return this.StateList.find(s => s.CountryId == countryId || 1 && s.Id == stateId)?.Name
  }

  public initilizeFilterForm() {
    this.FilterForm = this.fb.group({
      CountryId: this.fb.control(null, [Validators.required]),
      States: this.fb.control([]),
      Cities: this.fb.control([]),
      ZipCodes: this.fb.control([]),
      ServicesOffered: this.fb.control([]),
      IsPump: this.fb.control(false),
      IsMetered: this.fb.control(false),
      IsPackagedGoods: this.fb.control(false)
    })
  }
  InitializeServiceDropdown() {
    this.ServiceOfferingTypesDisplay[this.ServiceOfferingTypes[ServiceOfferingType.FTL]] = "FTL";
    this.ServiceOfferingTypesDisplay[this.ServiceOfferingTypes[ServiceOfferingType.LTL]] = "LTL";
    this.ServiceOfferingTypesDisplay[this.ServiceOfferingTypes[ServiceOfferingType.DEF]] = "DEF";
    this.ServiceOfferingTypesDisplay[this.ServiceOfferingTypes[ServiceOfferingType.LTLWetHosing]] = "LTL Wet Hosing";
    this.ServiceOfferingTypesDisplay[this.ServiceOfferingTypes[ServiceOfferingType.DEFWetHosing]] = "DEF Wet Hosing";
}
  bindServiceOfferingType() {
    for (let index in ServiceOfferingType) {
      if (!isNaN(Number(index))) {
        this.ServiceList.push({
          Id: +index,
          Name: this.ServiceOfferingTypesDisplay[ServiceOfferingType[+index]],
          Code: ""
        });
      }
    }
  }

  public GetInvitationTokenByCompany() {
    this.thirdPartyNetworkService.GetInvitationTokenByCompany().subscribe(response => {
      if (response && response.Status == 0) {
        this.IsThirdPartyEnabled = response.Result;
        this.InvitationToken = response.Message;
      }
    });
  }

  public GenerateInvitationToken() {
    this.PageLoader = true;
    this.thirdPartyNetworkService.GenerateInvitationToken().subscribe(response => {
      this.PageLoader = false;
      if (response && response.StatusCode == 0 && response.EntityNumber) {
        this.InvitationToken = response.EntityNumber;
      }
    });
  }

  public GetRegisteredInvitedCompanies(input?: ThirdPartyCompanyFilter) {
    this.RegisteredCompanies = [];
    this.thirdPartyNetworkService.GetRegisteredInvitedCompanies(input).subscribe(response => {
      if (response && response.length > 0) {
        this.RegisteredCompanies = response;
      }
      this.refreshDatatable();
    });
  }

  public GetNonRegisteredInvitedCompanies(input?: ThirdPartyCompanyFilter) {
    this.NonRegisteredCompanies = [];
    this.thirdPartyNetworkService.GetNonRegisteredInvitedCompanies(input).subscribe(response => {
      if (response && response.length > 0) {
        this.NonRegisteredCompanies = response;
      }
      this.refreshDatatable();
    });
  }

  public getCountryList() {
    this.stateListService.getCountries().subscribe(data => {
      if (data && data.length > 0) {
        this.CountryList = data;
      }
    });
  }

  public getStatesOfAllCountries() {
    this.stateListService.getStates().subscribe(data => {
      if (data && data.length > 0) {
        this.StateList = data;
      }
    });
  }

  copyText() {
    navigator.clipboard.writeText(window.location.origin + '/Invitation?token=' + this.InvitationToken);
    Declarations.msgsuccess("Text Copied.", undefined, undefined);
  }

  openPanel(selectedCompany: any, isRegisteredCompany: boolean) {
    this._opened = true;
    this.IsRegisteredCompany = isRegisteredCompany;
    this.SelectedCompany = selectedCompany;
  }

  getNonRegisteredInvitedCompany(entityId: number) {
    this.SelectedCompany = null;
    this.thirdPartyNetworkService.GetNonRegisteredInvitedCompany(entityId).subscribe(data => {
      if (data && data.StatusCode == 0) {
          this.SelectedCompany = data;
          this._opened = true;
      }
    });
  }

  getRegisteredInvitedCompany(companyId: number) {
    this.SelectedCompany = null;
    this.thirdPartyNetworkService.GetRegisteredInvitedCompany(companyId).subscribe(data => {
      if (data && data.StatusCode == 0) {
        this.SelectedCompany = data;
        this._opened = true;
      }
    });
  }

  countryChanged() {
    this.FilterForm.get('States').setValue([]);
    this.FilterForm.get('ZipCodes').setValue([]);
  }

  stateChanged(newStateAdded: boolean) {

    let _selectedStates = this.FilterForm.get('States').value as any[];
    var stateslist = _selectedStates.map(t => t.Id).join(",");

    this.CitiesWithZip = [];
    this.invitationService.GetCitiesFromStates(stateslist).pipe(debounceTime(1000), distinctUntilChanged()).subscribe(response => {
      if (response && response.length > 0) {
        this.CitiesWithZip = response;
      }
      if (!newStateAdded) {
        this.removeSelectedCitiesOfRemovedState();
      }
    });
  }

  removeSelectedCitiesOfRemovedState() {
    let selectedZips = this.FilterForm.get('ZipCodes').value as ServiceArea[];
    if (selectedZips.length > 0) {

      let finalCities = [];
      selectedZips.forEach(selectedCity => {
        if (this.CitiesWithZip.find(c => c.CityId == selectedCity.CityId)) {
          finalCities.push(selectedCity)
        }
      });
      this.FilterForm.get('ZipCodes').patchValue(finalCities);
    }
  }
  applyFilter() {
    //this.FilterForm.markAsTouched()
    //if (this.FilterForm.valid) {
      let filter = this.FilterForm.value;

      let input: ThirdPartyCompanyFilter = {
        CountryId: filter.CountryId,
        States: filter.States ? filter.States.map(t => t.Id).join(",") : '',
        ZipCodes: filter.ZipCodes ? filter.ZipCodes.map(t => t.ZipCode).join(",") : '',
        ServicesOffered: filter.ServicesOffered ? filter.ServicesOffered.map(t => t.Id).join(",") : '',
        IsPump: filter.IsPump,
        IsMetered: filter.IsMetered,
        IsPackagedGoods: filter.IsPackagedGoods
      }
      this.GetRegisteredInvitedCompanies(input);
      this.GetNonRegisteredInvitedCompanies(input);
    //}

  }

  resetFilter() {
    this.FilterForm.reset();
    this.GetRegisteredInvitedCompanies();
    this.GetNonRegisteredInvitedCompanies();
  }
}
