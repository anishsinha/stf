import { Component, Input, OnChanges, OnInit, SimpleChanges } from '@angular/core';
import { Router } from '@angular/router';
import { BehaviorSubject } from 'rxjs';
import { SalesUserService } from '../sales-user.service';
import { SourcingRequestDisplayStatus, UoM } from 'src/app/app.enum';
import { Form, FormArray, FormBuilder, FormGroup, Validators } from '@angular/forms';
import { IDropdownSettings } from 'ng-multiselect-dropdown';
import { FuelSurchargeService } from 'src/app/fuelsurcharge/services/fuelsurcharge.service';
import { Declarations } from 'src/app/declarations.module';
import { ConfirmDRStatus, CustomCompanyModel, CustomersAndJobs, CustomersModel, DeliveryRequestInputModel, ProductsGroup, SalesDRModel, SalesUserDRProductStatus, SalesUserDRStatus } from '../sales-user.model';
import * as moment from 'moment';
import { NgbModule } from '@ng-bootstrap/ng-bootstrap';

@Component({
  selector: 'app-sales-user-dashboard',
  templateUrl: './sales-user-dashboard.component.html',
  styleUrls: ['./sales-user-dashboard.component.scss']
})
export class SalesUserDashboardComponent implements OnInit, OnChanges {

  public IsLoading: boolean = false;
  Sourcingrequests: any = [];
    orders: any = [];
    Invoices: any = [];
    activeInvoiceDDTTab = 0;
    public minDate: Date = new Date();
    public maxDate: Date = new Date();
    @Input() private SelectedDate: Date = new Date();
    formSubmitted = false;
    selectedCompany:{CompanyId: number, CompanyName: string} = {CompanyId: 0, CompanyName: ""};
    selectedSite:{Id: number, Name: string} = {Id: 0, Name: ""};
    CompanySettings: IDropdownSettings = {};
    SiteddlSettings: IDropdownSettings = {};
    companyList = [];
    SiteList= [];
    IsEditable: boolean;
    SingleSelectSettingsById: IDropdownSettings = {};
    FuelTypeList = [];
    SalesDRForm: FormGroup;
    public isLoadingSubject: BehaviorSubject<any>;
    AllTPOCompaniesList = [];
    UserContext = {};
    postData= {};
    AdditionalProducts: FormGroup[];
    showModal = false;
    CustomersJobsParentList: CustomersModel = {regionsAndJobsModels: [], customersandJobs: []};;
    CustomersNJobsList: CustomersAndJobs[];
    salesDrModel: SalesDRModel;
    DRInput: DeliveryRequestInputModel[] = [];
    ProductStatuses: SalesUserDRProductStatus[] = [];
    Unit: UoM;
    isConfirmDisabled: boolean = false;
    QuantityRegEx = /^\d*(\.\d{1,4})?$/;
    initFormValues: any;
    uparrow = true; 


  public DisplayRequestStatus = SourcingRequestDisplayStatus;
  
  constructor(private salesUserService: SalesUserService,private router: Router, private fb: FormBuilder, private fuelsurchargeService: FuelSurchargeService) {
    this.getUserContext();
    this.AdditionalProducts = [];
    this.maxDate.setFullYear(this.maxDate.getFullYear() + 1);
   }

  ngOnInit() {
    this.multiSettings();
    this.salesDrModel = new SalesDRModel();
    this.SalesDRForm = this.initSalesDRForm();
    this.initFormValues = this.SalesDRForm.value;
    this.getSourcingRequests();
    this.getOrders();
    this.getInvoices(0);
    this.GetCustomersNJobs();
    this.getProducts(0, 0);//By Default Get All Products

  }

  ngOnChanges(change: SimpleChanges): void {
    if (change.SelectedDate && change.SelectedDate.currentValue) {
        if (this.maxDate < this.SelectedDate) {
            this.maxDate = moment(new Date(this.SelectedDate)).add(1, 'day').toDate();
            this.minDate = moment(new Date(this.SelectedDate)).toDate();
        } else {
            this.minDate = moment(new Date(this.SelectedDate)).toDate();
            this.maxDate = moment(new Date(this.SelectedDate)).add(1, 'day').toDate();
        }

    }
}

  SetStartDate(date: string, index: number) {
      let _date = ((this.SalesDRForm.controls.AdditionalProducts as FormArray).controls[index] as FormGroup).controls['StartDate'];
      if (_date.value != date) {
          _date.patchValue(date);
      }
  }

  SetStartTime(time: string, index: number) {
    let _startTime = ((this.SalesDRForm.controls.AdditionalProducts as FormArray).controls[index] as FormGroup).controls['StartTime'];
      if (_startTime.value != time) {
        _startTime.patchValue(time);
      }
  }

  SetEndTime(time: string, index: number) {
    let _endTime = ((this.SalesDRForm.controls.AdditionalProducts as FormArray).controls[index] as FormGroup).controls['EndTime'];
      if (_endTime.value != time) {
        _endTime.patchValue(time);
      }
  }
  GetUoMString(Uom:UoM): string {
    let UoMString:string = UoM[Uom];
    return UoMString;
  }

  GetToolTip(prodStatus: SalesUserDRProductStatus) {
    if(prodStatus.Status.State != SalesUserDRStatus.Success) {
        return "This customer is not set up to receive\n this product at this location; please \ncontact accounting for assistance.";
    }
    else {
        return "";
    }
  }


  RemoveToolTip(event: MouseEvent) {
    (<HTMLSpanElement>event.target).title = "";
    //(<HTMLSpanElement>event.target).title = "";
  }
    getBGColor(prodStatus: SalesUserDRProductStatus) {
    return (prodStatus.Status.State == SalesUserDRStatus.Success) ? 'green' : 'red';
  }
  GetCustomersNJobs() {
    this.IsLoading = true;
    this.salesUserService.GetCustomersAndLocations().subscribe(customersAndLocations => {
      if (customersAndLocations) {
        var _cusAndLocations = customersAndLocations;
        var _joblist = customersAndLocations.customersandJobs.map(item => {
          return {
            Id: item.JobId,
            Name: item.JobName
          } 
        });

        var _custlist = customersAndLocations.customersandJobs.map(item => {
          return {
            CompanyId: item.CustomerId,
            CompanyName: item.CustomerName
          } 
        });
      }
      this.SiteList = _joblist;

      //Filter Unique Customers
      this.AllTPOCompaniesList = this.FilterUniqueCustomers(_custlist);
      //Sort Customers by Name
      this.AllTPOCompaniesList.sort((a,b) => (a.CompanyName.toLowerCase() > b.CompanyName.toLowerCase()) ? 1 : ((b.CompanyName.toLowerCase() > a.CompanyName.toLowerCase()) ? -1 : 0));
      
      //Sort Job By Names
      this.SiteList.sort((a,b) => (a.Name.toLowerCase() > b.Name.toLowerCase()) ? 1 : ((b.Name.toLowerCase() > a.Name.toLowerCase()) ? -1 : 0));

      this.CustomersJobsParentList = _cusAndLocations;
      this.IsLoading = false;
    });

   
  }

  FilterUniqueCustomers(custList: CustomCompanyModel[]): CustomCompanyModel[] {
    const res = [];
    const map = new Map();
    for(const item of custList) {
        if(!map.has(item.CompanyId)) {
            map.set(item.CompanyId, true);
            res.push({
                CompanyId: item.CompanyId,
                CompanyName: item.CompanyName
            });
        }
    }
    return res;
  }
  getProducts(companyId: number, jobId: number) {
    this.IsLoading = true;
    this.salesUserService.GetProducts(companyId, jobId).subscribe(data =>{
      this.FuelTypeList = data; 
      this.IsLoading = false;
    })
    
  }
  addProducts() {
    this.formSubmitted = false;
    this.AdditionalProducts.push(this.initAdditionalProducts());
    (this.SalesDRForm.get('AdditionalProducts') as FormArray).push(this.initAdditionalProducts());
    this.setProducts();
  }

  setProducts() {
    if(this.selectedSite.Id > 0 && this.selectedCompany.CompanyId > 0) {
      this.getProducts(this.selectedCompany.CompanyId, this.selectedSite.Id);
    }
    else {
      this.getProducts(0, 0);
    }
  }

  removeProduct(index) {
    this.formSubmitted = false;
    (this.SalesDRForm.controls.AdditionalProducts as FormArray).removeAt(index);
    this.AdditionalProducts.splice(index, 1);
  }

  initAdditionalProducts(): FormGroup {
    let _addProd = this.fb.group({
        Quantity: this.fb.control('', [Validators.required, Validators.pattern(this.QuantityRegEx)]),
        UoM: this.fb.control(1, Validators.required),
        StartDate: this.fb.control(''),
        StartTime: this.fb.control(''),
        EndTime: this.fb.control(''),
        FuelTypes: this.fb.control([], Validators.required),
        DRPO: this.fb.control('')
    });
    return _addProd;
  }

  initSalesDRForm(): FormGroup {
    this.AdditionalProducts.push(this.initAdditionalProducts());
    let _form = this.fb.group({
      CompanyList: this.fb.control([], Validators.required),
      SiteList: this.fb.control([], Validators.required),
      DRNotes: this.fb.control(''),
      AdditionalProducts: this.fb.array(this.AdditionalProducts)
     });
     return _form;
  }

  getCompanies() {
    //this.isLoadingSubject.next(true);
    this.fuelsurchargeService.getSupplierCustomers().subscribe(async (data) => {
      this.companyList = await (data);
      //this.isLoadingSubject.next(false);
  });
  }

  getUserContext() {
    this.salesUserService.GetUserContext().subscribe(data => {
        this.UserContext = data;
    })
}

  clearProducts() {
    if(this.SalesDRForm.controls.AdditionalProducts != undefined && this.SalesDRForm.controls.AdditionalProducts != null && (this.SalesDRForm.controls.AdditionalProducts as FormArray).length > 1) {
      for(let i = ((this.SalesDRForm.controls.AdditionalProducts as FormArray).length - 1); i > 0; i--) {
        this.removeProduct(i);
      }
    }
  }

  clearSalesDRForm(){
    this.clearProducts();
    this.SalesDRForm.reset(this.initFormValues);
    this.formSubmitted = false;

  }

updateFormControlValidators(control: any, validators: any[]) {
  control.setValidators(validators);
  control.updateValueAndValidity();
}

FindSuccessStatus(status, index) {
  if(status.Status.State ==  SalesUserDRStatus.Success) {
    return status.Status.State == SalesUserDRStatus.Success;
  }
}

onValidate() {
  this.IsLoading = true;
  //Copy Form values to model
  this.formSubmitted = true;
  if(!this.SalesDRForm.valid) {
    this.IsLoading = false;
    return;
  }
  this.SalesDRformToModel();

  //Post the model for validations
  this.salesUserService.ValidateDREntryForm(this.salesDrModel).subscribe(res => {
    if(res) {
      var _inputDR = res;
    }

    this.DRInput = _inputDR.RaiseDeliveryRequestInput;
    this.ProductStatuses = _inputDR.ProductStatuses;
    if(this.ProductStatuses && this.ProductStatuses.length > 0) {
      let successStatus = this.ProductStatuses.find((status, index) => this.FindSuccessStatus(status, index));
      this.isConfirmDisabled = successStatus ? false : true;
    }
    this.IsLoading = false;
  });
  
  this.showModal = true;
}

  onSubmit() {
   this.IsLoading = true;

    this.salesUserService.CreateDREntryForm(this.DRInput).subscribe(data => {
      if (data && data.StatusCode == ConfirmDRStatus.Success) {
          var _DRstatus = data;

          Declarations.msgsuccess("DR/Order Creation was Successful.", undefined, undefined);
          this.IsLoading = false;
      } else {
          Declarations.msgerror(data.StatusMessage, undefined, undefined);
          this.IsLoading = false;
          return;
      }

    })
    
    this.clearForm();
  }

  clearForm() {
    this.salesDrModel = new SalesDRModel();
    this.clearSalesDRForm();
    this.SalesDRForm.reset(this.initFormValues);
    this.showModal = false;
    this.formSubmitted = false;
  }

  multiSettings() {
    this.CompanySettings = {
      singleSelection: true,
      closeDropDownOnSelection: true,
      idField: 'CompanyId',
      textField: 'CompanyName',
      enableCheckAll: false,
      itemsShowLimit: 1,
      allowSearchFilter: true
    };
    this.SingleSelectSettingsById = {
      singleSelection: true,
      closeDropDownOnSelection: true,
      idField: 'Id',
      textField: 'Name',
      enableCheckAll: false,
      selectAllText: 'Select All',
      unSelectAllText: 'UnSelect All',
      itemsShowLimit: 1,
      allowSearchFilter: true
    };
  }

  onCompanySelect(company: any): void {
        this.SalesDRForm.controls.SiteList.setValue([]);
        (this.SalesDRForm.controls.AdditionalProducts as FormArray).controls.forEach(x=> x.reset(this.initAdditionalProducts().value));
        this.selectedCompany.CompanyId = company.CompanyId;
        this.selectedCompany.CompanyName = company.CompanyName;
        this.SalesDRForm.controls.CompanyList.setValue([{ CompanyId: company.CompanyId, CompanyName: company.CompanyName }]);
        
        var _filteredSites = this.CustomersJobsParentList.customersandJobs.filter(x => x.CustomerId == company.CompanyId).map(item => {
          return {
            Id: item.JobId,
            Name: item.JobName
          }
        });

        this.SiteList = _filteredSites;
        this.SiteList.sort((a,b) => (a.Name.toLowerCase() > b.Name.toLowerCase()) ? 1 : ((b.Name.toLowerCase() > a.Name.toLowerCase()) ? -1 : 0));
        if(this.selectedSite.Id > 0 && this.selectedCompany.CompanyId > 0) {
          this.getProducts(this.selectedCompany.CompanyId, this.selectedSite.Id);
        }
        else {
          this.getProducts(0, 0);
        }
        
      /*this.selectedOrder = [];
      this.SelectedCustomerId = item.CompanyId;
      this.SiteList = this.drOrders.filter(x => x.CompanyId == item.CompanyId).map((element) => ({ Id: element.JobId, Name: element.JobName }));
      this.SiteList = this.GetUniqueLocations(this.SiteList.reduce((p, n) => p.concat(n), [])); */
  }

  onCompanyDeSelect(event: any) {
    this.selectedCompany.CompanyId = 0;
    this.selectedSite.Id = 0;
    this.SiteList = [];
    this.FuelTypeList = [];
    this.SalesDRForm.controls.SiteList.setValue([]);
    this.getProducts(0, 0);
    this.clearForm();
  }

  onSiteSelect(sites: any) {
    (this.SalesDRForm.controls.AdditionalProducts as FormArray).controls.forEach(x=> x.reset(this.initAdditionalProducts().value));
      this.selectedSite.Id = sites.Id;
      this.selectedSite.Name = sites.Name;
      this.SalesDRForm.controls.SiteList.setValue([{ Id: sites.Id, Name: sites.Name }]);

      if(this.selectedSite.Id > 0 && this.selectedCompany.CompanyId > 0) {
        this.getProducts(this.selectedCompany.CompanyId, this.selectedSite.Id);
      }
      else {
        this.getProducts(0, 0);
      }
  }

  onSiteDeSelect(event: any) {
    this.FuelTypeList = []; 
    (this.SalesDRForm.controls.AdditionalProducts as FormArray).controls.forEach(x=> x.reset(this.initAdditionalProducts().value));
    this.getProducts(0, 0);
  }


SalesDRformToModel() {
  this.salesDrModel = new SalesDRModel();
  if(this.SalesDRForm.valid) {
    this.salesDrModel.CompanyId = this.SalesDRForm.controls.CompanyList.value[0].CompanyId;
  this.salesDrModel.CompanyName = this.SalesDRForm.controls.CompanyList.value[0].CompanyName;
  this.salesDrModel.JobId = this.SalesDRForm.controls.SiteList.value[0].Id;
  this.salesDrModel.JobName = this.SalesDRForm.controls.SiteList.value[0].Name;
  this.salesDrModel.DRNotes = this.SalesDRForm.controls.DRNotes.value;
  this.salesDrModel.Products = [];
  var addProducts = this.SalesDRForm.controls.AdditionalProducts as FormArray;


  for(let control of addProducts.controls) {
    if(control instanceof FormGroup) {
      this.salesDrModel.Products.push({
        Quantity: control.value.Quantity,
        UoM: control.value.UoM,
        StartDate: control.value.StartDate,
        StartTime: control.value.StartTime,
        EndTime: control.value.EndTime,
        DRPO: control.value.DRPO,
        FuelTypes: {Id: control.value.FuelTypes[0].Id, Name: control.value.FuelTypes[0].Name},
        FuelTypeId: control.value.FuelTypes[0].Id,
        FuelName: control.value.FuelTypes[0].Name

      });
    }
  }
  }
  
}

public getJobLists(companyId, isFtl, foAsTerminal) {
  let companyName = this.AllTPOCompaniesList.find(t => t.CompanyId == companyId).CompanyName;
  let ftlvalue = isFtl == "FullTruckLoad" ? true : false;
  let tervalue = foAsTerminal == "Terminal" ? true : false;
  this.salesUserService.GetJobLists(companyName, ftlvalue, tervalue).subscribe(data => {
      if (data) {
          let joblistdata = data.map(item => {
              return {
                  Id: item.Id,
                  Name: item.Name
              }
          });
          this.SiteList = joblistdata;
      }
  });
}

    public toggleArrow() {
        this.uparrow = !this.uparrow;
    }
  public getSourcingRequests() 
  {
    this.IsLoading = true;
    var isFromDashboard = true;
      this.salesUserService.GetSourcingRequests(this.DisplayRequestStatus.All ,isFromDashboard).subscribe(data => {
      this.IsLoading = false;
      this.Sourcingrequests = data;
    });
  }
  public getOrders() 
  {
    this.IsLoading = true;
    this.salesUserService.GetOrdersForDashboard().subscribe(data => {
      this.IsLoading = false;
      this.orders = data;
    });
  }
    public getInvoices(type) {
        this.IsLoading = true;
        this.salesUserService.GetInvoicesForDashboard(type).subscribe(data => {
            this.IsLoading = false;
            this.Invoices = data;
        });
    }
    public changeActiveTab(type) {
        this.activeInvoiceDDTTab = type;
        this.getInvoices(type);
    }
  public navigateToSourcing(): void {
    this.router.navigate([]).then(result => { window.open('/SalesUser/SourcingRequest/Index', '_blank'); });
  }
  public navigateToOrders(): void {
    this.router.navigate([]).then(result => { window.open('/Supplier/Order/View', '_blank'); });
  }
    public navigateToInvoice(): void {
        this.router.navigate([]).then(result => { window.open('Supplier/Invoice/View', '_blank'); });
    }
}

