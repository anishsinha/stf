import { Component, OnInit, AfterViewInit, ViewChild, ViewChildren, QueryList, Output, EventEmitter, OnChanges, SimpleChanges, AfterContentChecked, ChangeDetectorRef, ChangeDetectionStrategy, AfterContentInit, AfterViewChecked, Input } from '@angular/core';
import { FormGroup, FormBuilder, Validators, FormArray } from '@angular/forms';
import { ActivatedRoute } from '@angular/router';
import { InvoiceService } from './services/invoice.service';
import { InvoiceDetailModel, AssetDropModel, DropDetailModel, FeeModel, OtherProductTaxModel, DeDuplicateFees, ImageModel, BlendedSchedules, BlendedScheduleDetail, AccessorialFeeTableDetailViewModel } from './models/DropDetail';
import { ProducDetailComponent } from './produc-detail/produc-detail.component';
import { ImageuploadComponent } from '../imageupload/imageupload.component';
import { BolListComponent } from './bol-detail/bol-list.component';
import { LiftTicketsComponent } from './lift-tickets/lift-tickets.component';
import { OtherProductTaxesComponent } from './taxes/other-product-taxes.component';
import { Declarations } from '../declarations.module';
import { Country } from '../carrier/models/location';
import { WaitingAction, FreightPricingMethod} from 'src/app/app.enum';
import { Subject } from 'rxjs/internal/Subject';
import { ValidationService } from '../services/validation.service';
import { groupBy } from '../my.functions';

@Component({
    selector: 'app-createinvoice',
    templateUrl: './createinvoice.component.html',
    styleUrls: ['./createinvoice.component.css']
})
export class CreateinvoiceComponent implements OnInit, AfterViewInit, AfterViewChecked {
    public DtTrigger: Subject<any> = new Subject();
    public orderId: number;
    public trackableScheduleId: number = 0;
    public invoiceId: number = 0;
    public invoiceForm: FormGroup;
    public PoList = [];
    public DriverList = [];
    public PoDdlSettings = {};
    public DriverDdlSettings = {}
    public SingleSelectSettingsById = {};
    public MultiSelectSettingsById = {};
    public SelectedDriver = [];
    //public completeInvoiceViewModel: any;
    public InvoiceModel: InvoiceDetailModel;
    //public AssetDrops: AssetDropModel[];
    public InvoiceFees: FeeModel[];
    public AccessorialTableDetails: AccessorialFeeTableDetailViewModel[];
    public OtherProductTaxes: OtherProductTaxModel[];
    public OtherProductAdded: boolean = false; 

    public IsFrieghtPricingMethodAuto: boolean = false;
    public IsSignatureRequired: boolean = false;
    public IsBOLImageRequired: boolean = false;
    public IsDropImageRequired: boolean = false;
    public IsBolDetailsRequired: boolean = false;
    //public IsFTL: boolean = false;
    public IsSupressOrderPricing: boolean = false;
    //public InvoiceTypeId: number;
    public BlendedScheduleDetail: BlendedScheduleDetail[] = [];    

    public InvalidFtlDetails: boolean = false;
    public InvalidBOLDetails: boolean = false;
    public Currency: string; // sending Currency as Input property to feelist component
    public IsDateInvalid: boolean = false;
    public InValidBolImage: boolean = false;
    public dropInfos = new Array<DropInfo>();
    public IsInvalidFtlDetailsnew: boolean = false; // new flag added here ccroding to change validation 
    public IsInvalidFtlDetailsnewlift: boolean = false; // new flag added here ccroding to changes validation 
    public InvalidDropInfos = new Array<InvalidDropDetailInfo>();
    public MissingImgInfos = new Array<MissingImageInfo>();
    public IsLoading: boolean = false;
    public IsLoadingImages: boolean = false;
    public hasDuplicateProduct: boolean;
    public disabled: boolean = false;
    public InvoiceImage: ImageModel;
    public InvoiceImages: ImageModel[] = [];
    public SignatureImage: ImageModel;
    public SignatureImages: ImageModel[] = [];
    public TaxAffidavitImage: ImageModel;
    public TaxAffidavitImages: ImageModel[] = [];
    public BDNImage: ImageModel;
    public BDNImages: ImageModel[] = [];
    public CoastGuardInspectionImage: ImageModel;
    public CoastGuardInspectionImages: ImageModel[] = [];
    public InspectionRequestVoucherImage: ImageModel;
    public InspectionRequestVoucherImages: ImageModel[] = [];
    public AdditionalImage: ImageModel;
    public AdditionalImages: ImageModel[] = [];
    public InvoiceNotes: string;
    public disableInputControls: boolean = false;
    public waitingAction: number = 0;
    public baseDetailUrl: string;
    public IsBadgeMandatory: boolean = false;
    public exceptionId: number = 0;
    public NoOrders: boolean;
    @ViewChildren(ImageuploadComponent) imageuploadComponents: QueryList<ImageuploadComponent>;
    @ViewChildren(BolListComponent) BolListComponents: QueryList<BolListComponent>;
    @ViewChildren(LiftTicketsComponent) LiftTicketsComponents: QueryList<BolListComponent>;
    @ViewChild(OtherProductTaxesComponent) otherProductTax: OtherProductTaxesComponent;
    
    constructor(
        private fb: FormBuilder, 
        private route: ActivatedRoute,
        private invoiceService: InvoiceService,
        private cdr: ChangeDetectorRef,
        private validationService: ValidationService) {
        this.invoiceForm = this.fb.group({
            PaymentTerm: this.fb.control(''),
            Customer: this.fb.control(''),
            BolDetails: this.fb.array([]),
            TicketDetails: this.fb.array([]),
            InvoiceTypeId: this.fb.control(''),
            IsVariousOrigin: this.fb.control(''),
            InvoiceNotes: this.fb.control(''),
            InvoiceImages: this.fb.control(''),
            AdditionalImages: this.fb.control(''),
            SignatureImages: this.fb.control(''),
            TaxAffidavitImages:this.fb.control(''), 
            BDNImages:this.fb.control(''),
            CoastGuardInspectionImages:this.fb.control(''),
            InspectionRequestVoucherImages:this.fb.control(''),
            FuelDropLocation: this.fb.control(''),
            OriginalInvoiceHeaderId: this.fb.control(''),
            IsRebillInvoice: this.fb.control(''),
            SupplierInvoiceNumber: this.fb.control(''),
            Driver: this.fb.control({}),
            SelectedDriver: this.fb.control({}),
            Carrier: this.fb.control(''),
            BrokerChainId: this.fb.control(''),
            ExistingHeaderId: this.fb.control(0),
            SelectedOrders: this.fb.control([]),           
        });

        this.OtherProductTaxes = [];
    }

    ngOnInit() {
        this.orderId = parseInt(this.route.snapshot.queryParamMap.get('orderId'), 10);
        if (this.route.snapshot.queryParamMap.get('existingHeaderId'))
            this.invoiceForm.controls.ExistingHeaderId.setValue(this.route.snapshot.queryParamMap.get('existingHeaderId'));
        this.trackableScheduleId = parseInt(this.route.snapshot.queryParamMap.get('trackableScheduleId'), 10);
        this.waitingAction = parseInt(this.route.snapshot.queryParamMap.get('waitingAction'), 10);
        this.exceptionId = parseInt(this.route.snapshot.queryParamMap.get('del-exceptionId'), 10);
        var invId = parseInt(this.route.snapshot.params.number, 10);
        this.baseDetailUrl = '/Supplier/Order/Details/' + this.orderId;
        if (this.trackableScheduleId > 0) {
            this.baseDetailUrl = '/Carrier/ScheduleBuilder';
        }
        if (invId > 0) {
            this.invoiceId = invId;
            if (this.waitingAction == WaitingAction.BolDetails) {
                this.disableInputControls = true;
            }
        }
                
        this.PoDdlSettings = {
            singleSelection: false,
            idField: 'OrderId',
            textField: 'DisplayPoNumber',
            enableCheckAll: false,
            selectAllText: 'Select All',
            unSelectAllText: 'UnSelect All',
            itemsShowLimit: 1,
            allowSearchFilter: true
        };
        this.DriverDdlSettings = {
            singleSelection: true,
            idField: 'Id',
            textField: 'Name',
            enableCheckAll: false,
            selectAllText: 'Select All',
            unSelectAllText: 'UnSelect All',
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
            itemsShowLimit: 1
        };
        this.MultiSelectSettingsById = {
            singleSelection: false,
            idField: 'Id',
            textField: 'Name',
            selectAllText: 'Select All',
            unSelectAllText: 'UnSelect All',
            itemsShowLimit: 5,
            allowSearchFilter: true
        };       
        this.invoiceService.getDriverList().subscribe(data => this.DriverList = data);       
    }

    ngAfterViewInit() {
        //this.invoiceForm.addControl('Drops', this.productList.DropGroup);
        //this.productList.DropGroup.setParent(this.invoiceForm);

        this.getPoDetailsList(this.orderId);
        if (this.invoiceId > 0) {
            this.getInvoiceDetails(this.invoiceId);
        } else {
            this.getDefaultDetail(this.orderId);
        }
        //this.getChildProperty();  
    }

    ngAfterViewChecked(){
        this.cdr.detectChanges();
    }
   
    public OnDriverSelect(driver: any) {
        this.invoiceForm.get('Driver').patchValue(driver);
    }

    public OnDriverDeSelect(driver: any) {
        this.invoiceForm.get('Driver').patchValue(null);
    }
   
    initFormData(model: InvoiceDetailModel): void {      
        var fuelDropLocation = null;
        if (model.FuelDropLocation != undefined && model.FuelDropLocation != null) {
            fuelDropLocation = {
                IsAddressAvailable: false,
                Address: model.FuelDropLocation.Address,
                City: model.FuelDropLocation.City,
                State: {
                    Id: model.FuelDropLocation.State.Id,
                    Code: model.FuelDropLocation.State.Code
                },
                Country: {
                    Id: model.FuelDropLocation.Country.Id,
                    Code: model.FuelDropLocation.Country.Code
                },
                Latitude: model.FuelDropLocation.Latitude,
                Longitude: model.FuelDropLocation.Longitude,
                CountyName: model.FuelDropLocation.CountyName,
                ZipCode: model.FuelDropLocation.ZipCode
            };
        }
        if (model.InvoiceImages != null && model.InvoiceImages != undefined) {
            this.InvoiceImages = model.InvoiceImages;
        }
        if (model.AdditionalImages != null && model.AdditionalImages != undefined) {
            this.AdditionalImages = model.AdditionalImages;
        }
        if (model.SignatureImages != null && model.SignatureImages != undefined) {
            this.SignatureImages = model.SignatureImages;
        }
        if (model.TaxAffidavitImages != null && model.TaxAffidavitImages != undefined) {
            this.TaxAffidavitImages = model.TaxAffidavitImages;
        }
        if (model.BDNImages != null && model.BDNImages != undefined) {
            this.BDNImages = model.BDNImages;
        }        
        if (model.CoastGuardInspectionImages != null && model.CoastGuardInspectionImages != undefined) {
            this.CoastGuardInspectionImages = model.CoastGuardInspectionImages;
        }
        if (model.InspectionRequestVoucherImages != null && model.InspectionRequestVoucherImages != undefined) {
            this.InspectionRequestVoucherImages = model.InspectionRequestVoucherImages;
        }
        if (model.InvoiceNotes != null && model.InvoiceNotes != undefined) {
            this.InvoiceNotes = model.InvoiceNotes;
        }
        this.SignatureImage = model.SignatureImage;
        this.TaxAffidavitImage=model.TaxAffidavitImage;
        this.BDNImage =model.BDNImage;
        this.CoastGuardInspectionImage=model.CoastGuardInspectionImage;
        this.InspectionRequestVoucherImage=model.InspectionRequestVoucherImage;
        this.products.IsMarineLocation = model.Customer.Location.IsMarineLocation;
        //if (model.BolDetails != null && model.BolDetails != undefined)
        //    model.BolDetails.forEach(x => {
        //        if (x.Images != null && x.Images != undefined) {
        //            x.Images.ImageData = x.Images.AzurePath;
        //            x.ImageList = [x.Images];
        //        }
        //    });

        //if (model.TicketDetails != null && model.TicketDetails != undefined)
        //    model.TicketDetails.forEach(x => {
        //        if (x.ImageList != null && x.ImageList != undefined) {
        //            x.Images.ImageData = x.Images.AzurePath;
        //            x.ImageList = [x.Images];
        //        }
        //    });

        this.invoiceForm.patchValue({
            PaymentTerm: {
                TermId: model.PaymentTerm.TermId,
                NetDays: model.PaymentTerm.NetDays
            },
            Customer: {
                CompanyId: model.Customer.CompanyId,
                CompanyName: model.Customer.CompanyName,
                Location: {
                    JobId: model.Customer.Location.JobId,
                    SiteName: model.Customer.Location.SiteName,
                    Address: model.Customer.Location.Address,
                    City: model.Customer.Location.City,
                    StateCode: model.Customer.Location.StateCode,
                    ZipCode: model.Customer.Location.ZipCode,
                    CountryId: model.Customer.Location.CountryId,
                },
                ContactName: model.Customer.ContactName,
                ContactPhone: model.Customer.ContactPhone,
                ContactEmail: model.Customer.ContactEmail
            },
            FuelDropLocation: fuelDropLocation,
            InvoiceTypeId: model.InvoiceTypeId,
            IsVariousOrigin: model.IsVariousOrigin,
            InvoiceNotes: model.InvoiceNotes,
            OriginalInvoiceHeaderId: model.OriginalInvoiceHeaderId,
            IsRebillInvoice: model.IsRebillInvoice,
            SupplierInvoiceNumber: model.SupplierInvoiceNumber,
            Driver: model.Driver,
            SelectedDriver: [model.Driver],
            Carrier: model.Carrier,
            BrokerChainId: model.BrokerChainId,
            SelectedOrders: [],
        });
    }

    @ViewChild(ProducDetailComponent) products: ProducDetailComponent;

    public recieveChildImages(entireInvoiceModel, sas) {
        var allPromises = [];

        this.imageuploadComponents.forEach(imageComponent => { allPromises.push(imageComponent.UploadFile(entireInvoiceModel, sas)) });

        this.BolListComponents.forEach(a => a.children.forEach(y => allPromises.push(y.UploadFile(entireInvoiceModel, sas))));

        this.LiftTicketsComponents.forEach(a => a.children.forEach(y => allPromises.push(y.UploadFile(entireInvoiceModel, sas))));

        return Promise.all(allPromises);
    }

    public onOrderSelect(item: any, trackableScheduleId: number = 0, blendedScheduleId: string = '', deliveryLevelPO:string=''): void {
        this.getAnotherProduct(item.OrderId, trackableScheduleId, blendedScheduleId, deliveryLevelPO);
        this.NoOrders = false;
    }

    public onOrderDeSelect(item: any, isAvoidBlend: boolean = false): void {
        if (!isAvoidBlend) {
            let existBlendDetail = this.BlendedScheduleDetail.find(t => t && t.Schedules && t.Schedules.some(x => x.OrderId == item.OrderId));
            if (existBlendDetail && existBlendDetail.BlendId && existBlendDetail.BlendId != '') {
                this.RemoveBlendSchedules(existBlendDetail.BlendId, true);
            }
        }
        this.products.removeProduct(item);
        this.clearDropInfo(item);
        var dropIndex = this.InvoiceModel.Drops.findIndex(function (elem, idx) {
            return elem.OrderId == item.OrderId;
        });
        if (dropIndex >= 0) {
            this.InvoiceModel.Drops.splice(dropIndex, 1);
            this.OtherProductTaxes = this.OtherProductTaxes.filter(function (elem) {
                return elem.OrderId != item.OrderId
            });
            if (this.otherProductTax != null) {
                this.otherProductTax.removeOrderTaxes(item.OrderId);
            }
            if (!this.InvoiceModel.Drops.some(t1 => t1.TypeOfFuel == 10 || t1.TypeOfFuel == 26) && this.OtherProductTaxes.length == 0) {
                this.OtherProductAdded = false;
            }
        }

        if (this.IsFrieghtPricingMethodAuto) {
            let combinedIds = null;
            this.NoOrders = true;
            let items = this.invoiceForm.get('SelectedOrders').value;
            let ids = items.map(s => s.OrderId);
            if (ids.length == 0) {
                 combinedIds = this.orderId
            } else {
                 combinedIds = this.orderId + "," + ids.join(',');
            }
            this.invoiceService.GetAccessorialFeeTablesForSelectedOrder(combinedIds).subscribe(response => {
                this.AccessorialTableDetails = response;
            });
        }
    }

    private getAnotherProduct(_orderId: number, _trackableScheduleId: number, _blendedScheduleId: string, deliveryLevelPO:string) {
        this.invoiceService.getAnotherProductDetail(_orderId)
            .subscribe((data: DropDetailModel) => {
                this.setImageFlags(data);// SET ANOTHER PRODUCT DETAILS IMAGE FLAGS 
                this.setDropInfo(data);  
                if (data != null && data != undefined) {
                    if (_trackableScheduleId && _trackableScheduleId > 0) {
                        data.TrackableScheduleId = _trackableScheduleId;
                        data.BlendedScheduleId = _blendedScheduleId;
                    }
                    if (data.FreightPricingMethod == FreightPricingMethod.Manual) {
                        this.IsFrieghtPricingMethodAuto = false;
                    } else {
                        this.IsFrieghtPricingMethodAuto = true;
                        let items = this.invoiceForm.get('SelectedOrders').value;
                        if (items != null && items != undefined && items.length > 0) {
                            let ids = items.map(s => s.OrderId);
                            let combinedIds = this.orderId + "," + ids.join(',');
                            this.invoiceService.GetAccessorialFeeTablesForSelectedOrder(combinedIds).subscribe(response => {
                                this.AccessorialTableDetails = response;
                            });
                        }
                    }
                    data.DeliveryLevelPO = deliveryLevelPO;
                    this.getAnotherDropFees(_orderId);
                    this.InvoiceModel.Drops.push(data);
                    this.products.addProduct(data);
                    if ((data.TypeOfFuel == 10 || data.TypeOfFuel == 26) && !this.OtherProductAdded) {
                        this.OtherProductAdded = true;
                    }
                    if (this.OtherProductAdded && data.OtherTaxDetails != null && data.OtherTaxDetails != undefined) {
                        this.OtherProductTaxes = this.OtherProductTaxes.concat(data.OtherTaxDetails);
                    }              
                }             
            });
    }
    setCurrency(data: DropDetailModel) { //SET THE CURRENCY ACCOR. TO ENUM 
        //None = 0,
        //USD = 1,
        //CAD = 2
        var Currency = data.Currency;
        if (Currency == "1") {
            this.Currency = "USD";
        }
        else if (Currency == "2") {
            this.Currency = "CAD";
        }
    }
    setDropInfo(drop: DropDetailModel) {
        this.dropInfos.push({
            OrderId: drop.OrderId,
            ProductId: drop.FuelTypeId,
            PoNumber: drop.PoNumber,
            IsBOLImageRequired: drop.IsBOLImageRequired,
            IsDropImageRequired: drop.IsDropImageRequired,
            IsBolDetailsRequired: drop.IsBolDetailsRequired,
            IsSignatureRequired: drop.IsSignatureRequired,
            //IsFTL: drop.IsFTL
        });
    }
    clearDropInfo(item: any) {
        var index = this.dropInfos.findIndex(function (element, idx) {
            return element.OrderId == item.OrderId;
        });
        if (index >= 0) {
            this.dropInfos.splice(index, 1);
        }
    }
    private getDefaultDetail(_orderId: number) {
        this.invoiceService.getDefaultDetail(_orderId, this.trackableScheduleId)
            .subscribe(data => {
                //this.InvoiceTypeId = data.InvoiceTypeId;                
                this.InvoiceModel = data;
                this.IsBadgeMandatory = this.InvoiceModel.IsBadgeMandatory;
                this.initFormData(data);
                this.InvoiceFees = data.Fees;
                this.setImageFlags(data.Drops[0]);
                this.setCurrency(data.Drops[0]);
                this.setDropInfo(data.Drops[0]);
                if (data.Drops != undefined && data.Drops != null && data.Drops.length > 0) {
                    this.OtherProductAdded = (data.Drops[0].TypeOfFuel == 10 || data.Drops[0].TypeOfFuel == 26);
                }
                if (this.OtherProductAdded) {
                    this.OtherProductTaxes = data.Drops[0].OtherTaxDetails;
                }
                this.IsSupressOrderPricing = data.IsSupressOrderPricing;
                if (data.Drops[0].IsFreightOnlyOrder) {
                    this.baseDetailUrl = '/Carrier/Order/Details/' + this.orderId;
                }
                if (data.Drops[0].FreightPricingMethod == FreightPricingMethod.Manual) {
                    this.IsFrieghtPricingMethodAuto = false;
                } else {
                    this.IsFrieghtPricingMethodAuto = true;
                    this.AccessorialTableDetails = data.AccessorialFeeDetails;
                }
            });    
    }
    setFTLValidators() {
        this.invoiceForm.controls['BolDetails'].setValidators([Validators.required]);
        this.invoiceForm.controls['TicketDetails'].setValidators([Validators.required]);
    }
    setImageFlags(drop: DropDetailModel): void {
        this.IsBOLImageRequired = this.IsBOLImageRequired || drop.IsBOLImageRequired;
        this.IsBolDetailsRequired = this.IsBolDetailsRequired || drop.IsBolDetailsRequired;
        this.IsDropImageRequired = this.IsDropImageRequired || drop.IsDropImageRequired;
        this.IsSignatureRequired = this.IsSignatureRequired || drop.IsSignatureRequired;

        //this.IsFTL = this.IsFTL || drop.IsFTL;
        //this.IsFTL || 

        if (this.IsBOLImageRequired || this.IsBolDetailsRequired) {
            this.setFTLValidators();
        }
        //this.IsSignatureRequired = drop.IsSignatureRequired;
    }


    private getInvoiceDetails(_invoiceId: number) {
        this.invoiceService.getInvoiceDetails(_invoiceId)
            .subscribe(response => {
                if (response && response.BolDetails && response.BolDetails.length == 1 && response.BolDetails[0].BolNumber == null && response.BolDetails[0].LiftDate == null) {
                    response.BolDetails = [];
                }
                this.InvoiceModel = response;
                this.IsBadgeMandatory = this.InvoiceModel.IsBadgeMandatory;
                this.initFormData(response);
                this.InvoiceFees = response.Fees;
                this.setCurrency(response.Drops[0]);
                var currentObj = this;
                response.Drops.forEach(function (elem) {
                    if (elem != undefined && elem != null) {
                        currentObj.OtherProductAdded = (elem.TypeOfFuel == 10 || elem.TypeOfFuel == 26) ;
                    }
                    if (currentObj.OtherProductAdded) {
                        currentObj.OtherProductTaxes = elem.OtherTaxDetails;
                    }
                });
                if (response.Drops[0].IsFreightOnlyOrder) {
                    this.baseDetailUrl = '/Carrier/Order/Details/' + this.orderId;
                }
            });
    }

    private getPoDetailsList(_orderId: number): void {
        this.invoiceService.getPoList(_orderId)
            .subscribe(response => { this.PoList = response; });
    }

    public onCancel() {
        window.location.href = this.baseDetailUrl;
    }

    findDuplicates() {
        const drops = this.invoiceForm.get('Drops').value;
        const fuelTypeIds = drops.map(item => item.FuelTypeId);
        this.hasDuplicateProduct = fuelTypeIds.some(function (item, idx) {
            return fuelTypeIds.indexOf(item) != idx
        });
    }

    validateLiftDates() {
        const bolDetails = this.invoiceForm.get('BolDetails').value;
        const liftDates = bolDetails.map(item => item.LiftDate); //all lift dates added in bols

        const dropDetails = this.invoiceForm.get('Drops').value;
        const dropDates = dropDetails.map(item => item.DropDate); // all drop dates added

        const ticketdetails = this.invoiceForm.get('TicketDetails').value;
        const liftTicketDates = ticketdetails.map(item => item.LiftDate);// all liftdates added in lifttickets 

        if (dropDates.length != 0 || dropDates != undefined || dropDates != null) {
            var minDropdt = this.findMinDate(dropDates);
        }
        this.IsDateInvalid = false;
        if (liftDates.length != 0 || liftDates != undefined || liftDates != null) {
            liftDates.forEach((ltdate) => {
                var liftdt = new Date(ltdate);
                if (minDropdt != "NaN/NaN/NaN") {
                    if (liftdt > new Date(minDropdt)) {
                        this.IsDateInvalid = true;
                    }
                }
            });
        }
        if (liftTicketDates.length != 0 || liftTicketDates != undefined || liftTicketDates != null) {
            liftTicketDates.forEach((liftktdate) => {
                var liftdt = new Date(liftktdate);
                if (minDropdt != "NaN/NaN/NaN") {
                    if (liftdt > new Date(minDropdt)) {
                        this.IsDateInvalid = true;
                    }
                }

            });
        }

    }
    validateDropStartEndTime() {
        if (this.invoiceForm.invalid) {
            if (this.invoiceForm.get("Drops").value.length > 0) {
                if (this.invoiceForm.get("Drops").value[0].StartTime != null && this.invoiceForm.get("Drops").value[0].EndTime != null) {
                    let strStartTime = this.invoiceForm.get("Drops").value[0].StartTime;
                    let strEndTime = this.invoiceForm.get("Drops").value[0].EndTime;
                    let startTime = new Date().setHours(this.GetHours(strStartTime), this.GetMinutes(strStartTime), 0);
                    let endTime = new Date(startTime);
                    let endTimeD = endTime.setHours(this.GetHours(strEndTime), this.GetMinutes(strEndTime), 0);
                    if (startTime < endTimeD) {
                        if (this.invoiceForm.get(['Drops', 0, 'StartTime']).valid == false) {
                            this.invoiceForm.get(['Drops', 0, 'StartTime']).setErrors(null);
                        }
                        if (this.invoiceForm.get(['Drops', 0, 'EndTime']).valid == false ) {
                            this.invoiceForm.get(['Drops', 0, 'EndTime']).setErrors(null);
                        }
                    }
                }
            }
        }
    }
    public GetHours(d) {
        var h = parseInt(d.split(':')[0]);
        if (d.split(':')[1].split(' ')[1] == "PM") {
            h = h + 12;
        }
        return h;
    }
    public GetMinutes(d) {
        return parseInt(d.split(':')[1].split(' ')[0]);
    }
    validateBolAndLiftDetails() {
        var currObject = this;
        this.IsInvalidFtlDetailsnew = false;
        this.IsInvalidFtlDetailsnewlift = false;
        this.InvalidDropInfos.length = 0;
        this.MissingImgInfos.length = 0;
        var bolProducts = [];// all the products inside bol details
        var liftProducts = []; // all the products inside liftticket details 
        var missingbols = [];
        var missingLifts = [];
        bolProducts.length = 0;
        liftProducts.length = 0;
        var boldetails = this.invoiceForm.get('BolDetails').value;

        boldetails.map(item => item.Products.map(prod => bolProducts.push(prod)));

        const bolgroupedProducts = groupBy(bolProducts, 'ProductId');
        for (const productId in bolgroupedProducts) {
            var key = productId;
            var productsperproductId = [];
            productsperproductId = bolgroupedProducts[key];
            var count = 0;
            var prodCount = productsperproductId.length;
            productsperproductId.forEach(function (product) {
                if (product.NetQuantity == "" && product.GrossQuantity == "") {
                    count++;
                }
            });
            //Means no bol details added against that product
            if (prodCount == count) {
                missingbols.push(productsperproductId[0]);
            }
        }
        var ticketDetails = this.invoiceForm.get('TicketDetails').value;
        ticketDetails.map(item => item.Products.map(prod => liftProducts.push(prod)));
        const liftgroupedProducts = groupBy(liftProducts, 'ProductId');
        for (let productId in liftgroupedProducts) {
            var key = productId;
            var productsperproductId = [];
            productsperproductId = liftgroupedProducts[key];

            var count = 0;
            var prodCount = productsperproductId.length;
            productsperproductId.forEach(function (product) {
                if (product.NetQuantity == "" && product.GrossQuantity == "" && product.BulkPlantName == "") {
                    count++;
                }
            });
            //Means no lift details added against that product
            if (prodCount == count) {
                missingLifts.push(productsperproductId[0]);
            }
        }
        this.dropInfos.forEach(function (dropinfo) {
            if (bolProducts.length > 0) {
                missingbols.forEach(function (missingbol) {
                    if (dropinfo.ProductId == missingbol.ProductId) {
                        //dropinfo.IsFTL || 
                        if (dropinfo.IsBolDetailsRequired || dropinfo.IsBOLImageRequired) {
                            if (missingbol.NetQuantity == "" && missingbol.GrossQuantity == "") {
                                var IsBolDetailsAvailable = false;
                                var IsLiftDetailsAvailble = currObject.validateLiftTicketDetails(missingbol, dropinfo.OrderId);

                                var PoNumber = dropinfo.PoNumber;
                                if (!IsBolDetailsAvailable && !IsLiftDetailsAvailble) {
                                    currObject.InvalidDropInfos.push({
                                        ProductId: dropinfo.ProductId,
                                        PoNumber: dropinfo.PoNumber,
                                        Message: "No bol or lift details provided for order with PO number " + dropinfo.PoNumber
                                    });
                                }
                            }
                        }
                    }
                });
            }
            if (liftProducts.length > 0) {
                missingLifts.forEach(function (missingLift) {
                    if (dropinfo.ProductId == missingLift.ProductId) {
                        //dropinfo.IsFTL || 
                        if (dropinfo.IsBolDetailsRequired || dropinfo.IsBOLImageRequired) {
                            if (missingLift.NetQuantity == "" && missingLift.GrossQuantity && missingLift.BulkPlantName == "") {
                                var IsLiftDetailsAvailable = false;
                                var IsBolDetailsAvailable = currObject.validateBolDetails(missingLift, dropinfo.OrderId);
                                var PoNumber = dropinfo.PoNumber;
                                if (!IsBolDetailsAvailable && !IsLiftDetailsAvailable) {
                                    currObject.InvalidDropInfos.push({
                                        ProductId: dropinfo.ProductId,
                                        PoNumber: dropinfo.PoNumber,
                                        Message: "No bol or lift provided for order with PO number :" + dropinfo.PoNumber
                                    });


                                }
                            }
                        }
                    }
                });
            }
        })
    }


    //To check if bols are not provided then lift details exist against that order 
    validateLiftTicketDetails(item: any, orderId: number): boolean {
        var IsLiftDetailsAvailble = true;
        var liftProducts = [];

        var ticketDetails = this.invoiceForm.get('TicketDetails').value;
        ticketDetails.map(item => item.Products.map(prod => liftProducts.push(prod)));
        const liftgroupedProducts = groupBy(liftProducts, 'ProductId');
        for (const productId in liftgroupedProducts) {
            var key = productId;
            var productsperproductId = [];
            productsperproductId = liftgroupedProducts[key];
            var count = 0;
            var prodCount = productsperproductId.length;
            productsperproductId.forEach(function (product) {
                if (product.NetQuantity == "" && product.GrossQuantity == "") {
                    count++;
                }
            });
        }
        var prodCount = 0;
        var missingInfoCount = 0;
        if (liftProducts.length > 0) {
            liftProducts.forEach(function (liftproduct) {
                if (liftproduct.ProductId == item.ProductId) {
                    prodCount++;
                    if (liftproduct.NetQuantity == "" && liftproduct.GrossQuantity == "" && liftproduct.BulkPlantName == "") {
                        //IsLiftDetailsAvailble = false;
                        missingInfoCount++;
                    }
                }
            });
            if ((prodCount == missingInfoCount) && (prodCount > 0 && missingInfoCount > 0)) {
                IsLiftDetailsAvailble = false;
                return IsLiftDetailsAvailble;
            }
        }
        else {
            IsLiftDetailsAvailble = false;
        }
        return IsLiftDetailsAvailble;

    }

    //To check if liftdetails are not provided then bol details exist against that order 
    validateBolDetails(item: any, orderId: number) {
        var IsBolDetailsAvailble = true;
        var bolProducts = [];
        var boldetails = this.invoiceForm.get('BolDetails').value;
        boldetails.map(item => item.Products.map(prod => bolProducts.push(prod)));

        const bolgroupedProducts = groupBy(bolProducts, 'ProductId');
        for (const productId in bolgroupedProducts) {
            var key = productId;
            var productsperproductId = [];
            productsperproductId = bolgroupedProducts[key];
            var count = 0;
            var prodCount = productsperproductId.length;
            productsperproductId.forEach(function (product) {
                if (product.NetQuantity == "" && product.GrossQuantity == "") {
                    count++;
                }
            });
        }
        var prodCount = 0;
        var missingInfoCount = 0;
        if (bolProducts.length > 0) {
            bolProducts.forEach(function (bolproduct) {
                if (bolproduct.ProductId == item.ProductId) {
                    prodCount++;
                    if (bolproduct.NetQuantity == "" && bolproduct.GrossQuantity == "") {
                        //IsBolDetailsAvailble = false;
                        missingInfoCount++;
                    }
                }
            });
            if ((prodCount == missingInfoCount) && (prodCount > 0 && missingInfoCount > 0)) {
                IsBolDetailsAvailble = false;
                return IsBolDetailsAvailble;
            }
        }
        else {
            IsBolDetailsAvailble = false;
        }
        return IsBolDetailsAvailble;
    }



    findMinDate(dropdates: any[]) {
        var dates = [];
        dropdates.forEach((dropdate) => {
            if (dropdate != null) {
                var date = new Date(dropdate);
                dates.push(date);
            }
        });
        //var maximumDate = new Date(Math.max.apply(null, dates)); 
        var minimumDate = new Date(Math.min.apply(null, dates));
        var minDate = minimumDate.toDateString();
        var date = this.getFormattedDate(minDate);
        if (date != null || date != undefined) {
            return date;
        }
    }
    getFormattedDate(date: any) {
        var dt = new Date(date);
        var year = dt.getFullYear();
        var month = (1 + dt.getMonth()).toString();
        month = month.length > 1 ? month : '0' + month;
        var day = dt.getDate().toString();
        day = day.length > 1 ? day : '0' + day;
        return month + '/' + day + '/' + year;
    }

    private getAnotherDropFees(_orderId: number) {
            this.invoiceService.getInvoiceDropFees(_orderId)
                .subscribe(response => {
                    //var existing = this.InvoiceFees.slice();
                    //response.forEach((model, idx) => {
                    //    var isExists = existing.find(function (elem) { return IsDuplicate(elem, model); });
                    //    if (isExists == undefined) {
                    //        existing.push(model);
                    //    }
                    //});
                    //this.InvoiceFees = existing;

                    var fees = DeDuplicateFees(this.InvoiceFees, response);
                    this.InvoiceFees = fees;
                });
        }
    checkValidityforFtl(): boolean {
        var isBolLiftNotProvided = false;
        var boldetails = this.invoiceForm.get('BolDetails').value;
        var ticketDetails = this.invoiceForm.get('TicketDetails').value;
        //this.IsFTL ||
        if (this.IsBolDetailsRequired || this.IsBOLImageRequired) {
            if (boldetails.length == 0 && ticketDetails.length == 0) {
                isBolLiftNotProvided = true;
                return isBolLiftNotProvided;
            }
            else {
                isBolLiftNotProvided = false;
                this.invoiceForm.controls['TicketDetails'].clearValidators();
                this.invoiceForm.get('TicketDetails').updateValueAndValidity();
                this.invoiceForm.controls['BolDetails'].clearValidators();
                this.invoiceForm.get('BolDetails').updateValueAndValidity();
                return isBolLiftNotProvided;
            }
        } else {
            this.invoiceForm.controls['TicketDetails'].clearValidators();
            this.invoiceForm.get('TicketDetails').updateValueAndValidity();
            this.invoiceForm.controls['BolDetails'].clearValidators();
            this.invoiceForm.get('BolDetails').updateValueAndValidity();
            return isBolLiftNotProvided = false;
        }
    }    
    checkValidityForImages() {
        var isBolImagenotprovided = false;
        var isLiftImagenotProvided = false;
        this.MissingImgInfos.length = 0;
        var boldetails = this.invoiceForm.get('BolDetails').value;
        var ticketDetails = this.invoiceForm.get('TicketDetails').value;
        var signatureImages = this.invoiceForm.get('SignatureImages').value;

        var liftTicketImage = ticketDetails.map(item => item.LiftImages);
        var bolImages = boldetails.map(item => item.BolImages);

        var bolimgCount = 0;
        var bollength = boldetails.length;
        if (this.IsBOLImageRequired) {
            //Means boldetails are added
            if (boldetails.length > 0) {
                bolImages.forEach(function (bolimg) {
                    if (bolimg == "") {
                        bolimgCount++;
                    }
                });
            }
        }
        //Means no bolimg provided but required
        if ((bolimgCount == bollength) && boldetails.length > 0) {
            isBolImagenotprovided = true;
            var ImgMisingMsg = "Please Provide Bol Image";
            this.MissingImgInfos.push({
                isImageNotProvided: isBolImagenotprovided,
                imgMissingMsg: ImgMisingMsg
            });
        }
        var liftImgCount = 0;
        var liftLength = ticketDetails.length;
        if (this.IsBOLImageRequired && liftLength > 0) {
            liftTicketImage.forEach(function (liftimg) {
                if (liftimg == "") {
                    liftImgCount++;
                }
            });
        }
        //means no liftimage provided but required
        if ((liftImgCount == liftLength) && liftLength > 0) {
            isLiftImagenotProvided = true;
            var liftImgMissingMsg = "Please Provide Lift ticket Image";
            this.MissingImgInfos.push({
                isImageNotProvided: isLiftImagenotProvided,
                imgMissingMsg: liftImgMissingMsg
            });
        }

        // check for signature image
        if (this.IsSignatureRequired) {           
            if (signatureImages == "" || signatureImages == null || signatureImages == undefined) {
                //Means no signature provided but image is required
                var imgMisingMsg = "Please Provide Signature Image";
                this.MissingImgInfos.push({
                    isImageNotProvided: true,
                    imgMissingMsg: imgMisingMsg
                });
            }
        }
    }
    OnScheduleReceived(schedules: any) {
        let prevBlendDetail = this.BlendedScheduleDetail.find(t => t && t.Schedules && t.Schedules.some(x => x.OrderId == schedules.OrderId));
        if (prevBlendDetail && prevBlendDetail.BlendId && prevBlendDetail.BlendId != '') {
            this.RemoveBlendSchedules(prevBlendDetail.BlendId);
        }
        if (parseInt(schedules.ScheduleId) > 0) {
            this.invoiceService.getAssignedDriverForSchedule(parseInt(schedules.ScheduleId), schedules.OrderId)
                .subscribe(data => {
                    if (data.Id > 0) {
                        this.SelectedDriver = [data];// Set NgModel for UI display
                        this.invoiceForm.get('Driver').patchValue(data);
                    }
                });

            if (schedules.BlendedScheduleId && schedules.BlendedScheduleId != '') {
                this.AddBlendSchedules(schedules.BlendedScheduleId, parseInt(schedules.ScheduleId));
            }

        }
        else if (schedules.ScheduleId == "null" || schedules.ScheduleId == "undefined") {
            this.invoiceForm.get('Driver').patchValue(null);
            this.SelectedDriver = [];
        }
    }
    private AddBlendSchedules(blendedScheduleId: string, trackableScheduleId: number) {
        this.products.IsLoading = true;
        this.invoiceService.getBlendedProducts(blendedScheduleId).subscribe((data : BlendedSchedules[]) => {
            if (data && data.length > 0) {
                this.BlendedScheduleDetail.push({ BlendId: blendedScheduleId, Schedules: data });
                let formOrders = this.invoiceForm.get('SelectedOrders').value as any[];
                let _drops = this.invoiceForm.get('Drops') as FormArray;
                data.forEach(t => {
                    var poItem = this.PoList.find(x => x.OrderId == t.OrderId);
                    let scheduleId = t.Id;
                    let deliveryLevelPO = t.DeliveryLevelPO;
                    let selOrderId = t.OrderId;
                    if (poItem && !formOrders.some(t => t.OrderId == selOrderId)) {
                        formOrders.push(poItem);
                        this.onOrderSelect(poItem, scheduleId, blendedScheduleId, deliveryLevelPO);
                    } else {
                        let dropIndex = _drops.controls.findIndex(z => z.get('OrderId').value == selOrderId);
                        let scheduledrop = ((this.invoiceForm.get('Drops') as FormArray).at(dropIndex) as FormGroup);
                        scheduledrop.get('DeliveryLevelPO').patchValue(deliveryLevelPO);
                        scheduledrop.get('TrackableScheduleId').patchValue(scheduleId, { emitEvent: false });
                        if (poItem)
                            scheduledrop.get('BlendedScheduleId').patchValue(blendedScheduleId, { emitEvent: false });
                    }
                });
                this.invoiceForm.get('SelectedOrders').setValue(formOrders);
            }
            this.products.IsLoading = false;
        });
    }
    private RemoveBlendSchedules(_prevScheduleBlendId: string, _isOrderDeselect: boolean = false) {
        var rmvOrders = this.BlendedScheduleDetail.find(t => t.BlendId == _prevScheduleBlendId)?.Schedules.map(t => t.OrderId);
        //Remove blended products
        this.BlendedScheduleDetail.splice(this.BlendedScheduleDetail.findIndex(t => t && t.BlendId == _prevScheduleBlendId), 1);
        let formOrders = this.invoiceForm.get('SelectedOrders').value as any[];
        //var rmvOrders = this.InvoiceModel.Drops.filter(t => t.BlendedScheduleId == _prevScheduleBlendId).map(t => t.OrderId);
        if (rmvOrders) {
            rmvOrders.forEach(t => {
                let _curOrderId = t;
                let rmvPO = this.PoList.find(x => x.OrderId == _curOrderId);
                if (rmvPO) {
                    if (formOrders.some(t => t.OrderId == rmvPO.OrderId)) {
                        this.onOrderDeSelect(rmvPO, true);
                    }
                } else if (_isOrderDeselect) {
                    // set trackable schedule Id as null        
                    let _drops = this.invoiceForm.get('Drops') as FormArray;
                    let dropIndex = _drops.controls.findIndex(z => z.get('OrderId').value == _curOrderId);
                    ((this.invoiceForm.get('Drops') as FormArray).at(dropIndex) as FormGroup).get('TrackableScheduleId').patchValue(null);
                }
            });
            formOrders = formOrders.filter(t => !rmvOrders.includes(t.OrderId));
        }
        this.invoiceForm.get('SelectedOrders').setValue(formOrders);
    }
    public IsLiftTicketDetailsMissing(): boolean {
        var IsLiftTicketDetailsMissing = false;
        var ticketDetails = [];
        ticketDetails = this.invoiceForm.get('TicketDetails').value;      
        if (ticketDetails.length > 0) {
            var tktNumbers = ticketDetails.map(function (item) { return item.LiftTicketNumber });            
            for (var i = 0; i < tktNumbers.length; i++) {      
                if (tktNumbers[i] =='' || tktNumbers[i] == null || tktNumbers[i] == undefined)
                {
                    IsLiftTicketDetailsMissing = true;
                    break;
                }
            }
            return IsLiftTicketDetailsMissing;
        }
        else {
            IsLiftTicketDetailsMissing = false;
            return IsLiftTicketDetailsMissing;
        }       
    }
    public validateBDRDetails() {
        var isBdrDetailsAdded = true;
        this.invoiceForm.get("Drops").value.forEach(function (drop) {

            if (drop.IsMarineLocation && (drop.BdrDetails == null || drop.BdrDetails == undefined || Object.keys(drop.BdrDetails).length === 0)) {              
                isBdrDetailsAdded = false;
                return isBdrDetailsAdded;
            }
        });
        return isBdrDetailsAdded;
    }

    public onSubmit(): void {
        this.invoiceForm.markAllAsTouched();
        this.findDuplicates();
        var isBolLiftNotProvided = this.checkValidityforFtl();
        this.validateLiftDates();
        this.validateBolAndLiftDetails();
        this.checkValidityForImages();
        var isLiftTicketDetails = this.IsLiftTicketDetailsMissing();
        //var isValidBDR = this.validateBDRDetails();
        //if (!isValidBDR) {
        //    Declarations.msgerror("BDR details are required", undefined, undefined);
        //    return;
        //}
        // validate terminal assign on order
        var invoiceViewModel = this.invoiceForm.getRawValue();
        var customer = invoiceViewModel.Customer;
        if (!this.IsSupressOrderPricing && customer.Location.CountryId != Country.CAR) {
            this.isTerminalAssignedOnOrder();
        }           
        if (this.IsDateInvalid) {
            Declarations.msgerror("Lift date should be less than or equal to drop date", undefined, undefined);
        }
        else if (this.hasDuplicateProduct) {
            Declarations.msgerror('You are trying to add multiple drops for same product ', undefined, undefined);
        }
        else if (isBolLiftNotProvided) {
            Declarations.msgerror('Please Enter Bol or Lift ticket details', undefined, undefined);
        }
        else if (isLiftTicketDetails) {
            Declarations.msgerror('Lift Ticket Details are missing ', undefined, undefined);
        }
        else if (this.InvalidDropInfos.length > 0) {
            this.InvalidDropInfos.forEach(function (invaliddropinfo) {
                if (invaliddropinfo.Message != "") {
                    Declarations.msgerror(invaliddropinfo.Message, undefined, undefined);
                }
            });
        }
        else if (this.MissingImgInfos.length > 0) {
            this.MissingImgInfos.forEach(function (missingimginfo) {
                if (missingimginfo.isImageNotProvided == true) {
                    Declarations.msgerror(missingimginfo.imgMissingMsg, undefined, undefined);
                }
            });
        }
        else if (this.InValidBolImage) {
            Declarations.msgerror("Bol Image or Lift Image is required", undefined, undefined);
        }
        else {
            this.validateDropStartEndTime();
            this.validateAssetQuantityForInvoice();
            if (this.invoiceForm.valid) {
                this.IsLoadingImages = true;
                if (invoiceViewModel.BolDetails && invoiceViewModel.BolDetails.length > 0 || invoiceViewModel.TicketDetails && invoiceViewModel.TicketDetails.length > 0) {
                    invoiceViewModel.Drops.forEach(drop => {
                        drop.TerminalId = 0;
                        drop.TerminalName = ""                        
                    });
                }
                
                if (invoiceViewModel.Drops && invoiceViewModel.Drops.length>0) {
                    invoiceViewModel.Drops.forEach(drop => {
                        drop.FuelSurchargeFreightFee.FreightPricingMethod = drop.FuelSurchargeFreightFee.FreightPricingMethod;
                        drop.FreightPricingMethod = drop.FuelSurchargeFreightFee.FreightPricingMethod;
                        if (drop.FuelSurchargeFreightFee.FreightType && drop.FuelSurchargeFreightFee.FreightType != null
                            && drop.FuelSurchargeFreightFee.FreightType.length > 0) {
                            drop.FuelSurchargeFreightFee.FreightRateRuleType = drop.FuelSurchargeFreightFee.FreightType[0].Id;
                        }
                        if (drop.FuelSurchargeFreightFee.FreightTableType && drop.FuelSurchargeFreightFee.FreightTableType.length > 0) {
                            drop.FuelSurchargeFreightFee.FreightRateTableType = drop.FuelSurchargeFreightFee.FreightTableType[0].Id;
                        }
                        if (drop.FuelSurchargeFreightFee.FreightTableName && drop.FuelSurchargeFreightFee.FreightTableName.length > 0) {
                            drop.FuelSurchargeFreightFee.FreightRateRuleId = drop.FuelSurchargeFreightFee.FreightTableName[0].Id;
                        }
                        if (drop.FuelSurchargeFreightFee.FuelSurchargeTableType && drop.FuelSurchargeFreightFee.FuelSurchargeTableType != null
                            && drop.FuelSurchargeFreightFee.FuelSurchargeTableType.length > 0) {
                            drop.FuelSurchargeFreightFee.FuelSurchargeTableType = drop.FuelSurchargeFreightFee.FuelSurchargeTableType[0].Id;
                        }
                        if (drop.FuelSurchargeFreightFee.FuelSurchargeTableName && drop.FuelSurchargeFreightFee.FuelSurchargeTableName.length > 0) {
                            drop.FuelSurchargeFreightFee.FuelSurchargeTableId = drop.FuelSurchargeFreightFee.FuelSurchargeTableName[0].Id;
                        } 
                    });
                }
                let _AccessorialFeeDetails = [];                  
                if (invoiceViewModel.AccessorialFeeDetails.AccessorialFeeTableType && invoiceViewModel.AccessorialFeeDetails.AccessorialFeeTableType.length > 0) {
                    invoiceViewModel.AccessorialFeeDetails.AccessorialFeeTableType = invoiceViewModel.AccessorialFeeDetails.AccessorialFeeTableType[0].Id;
                    if (invoiceViewModel.AccessorialFeeDetails.AccessorialFeeId && invoiceViewModel.AccessorialFeeDetails.AccessorialFeeId.length > 0) {
                        invoiceViewModel.AccessorialFeeDetails.AccessorialFeeId.forEach(_element => {
                            _AccessorialFeeDetails.push({
                                AccessorialFeeId: _element.Id,
                                AccessorialFeeTableType: invoiceViewModel.AccessorialFeeDetails.AccessorialFeeTableType
                            })
                        })
                    }
                    invoiceViewModel.AccessorialFeeDetails = _AccessorialFeeDetails;
                }                   
                                        
                if (!isNaN(this.exceptionId) && this.exceptionId > 0 && invoiceViewModel.Drops.length > 0 && invoiceViewModel.Drops[0].OrderId == this.orderId) {
                    invoiceViewModel.Drops[0].ExceptionId = this.exceptionId;
                }        
                         
                this.imageuploadComponents.first.GetSasForBlob().then(sas => this.recieveChildImages(invoiceViewModel, sas).then(() => {
                    this.IsLoadingImages = false;
                    this.IsLoading = true;
                    this.invoiceService.postCreateInvoice(invoiceViewModel, this.invoiceId).subscribe((data: any) => {
                            if (data != null && data.StatusCode == 0) {
                                Declarations.msgsuccess(data.StatusMessage, undefined, undefined);
                                this.IsLoading = false;
                                this.invoiceForm.reset();
                                window.location.href = this.baseDetailUrl;

                            } else {
                                this.IsLoading = false;
                                Declarations.msgerror(data == null ? 'Failed' : data.StatusMessage, undefined, undefined);
                            }
                        });
                }));
            }
            else {
                this.IsLoading = false;
                this.invoiceForm.markAllAsTouched();
                let invalidControls: string[] = this.findInvalidControlsRecursive(this.invoiceForm);
                if ((invalidControls.indexOf("BolDetails") != -1 && invalidControls.indexOf("BolImages") != -1) || ((invalidControls.indexOf("TicketDetails") != -1 && invalidControls.indexOf("LiftImages") != -1))) {
                    Declarations.msgerror("Bol or Lift Image is required", undefined, undefined);
                }
                if (invalidControls.indexOf("InvoiceImages") != -1) {
                    Declarations.msgerror("Drop/Invoice Images are required", undefined, undefined);
                }
            }
        }
    }

    validateAssetQuantityForInvoice() {
        let drops = this.invoiceForm.get("Drops") as FormArray;
        drops.controls.forEach((drop: FormGroup) => {
            this.validateAssetQuantityForDrop(drop);
        });
    }

    validateAssetQuantityForDrop(drop: FormGroup) {

        let assets = <FormArray>drop.get('Assets');

        if (assets.controls.length > 0) {

            let sumOfAssetQuantity = assets.value.reduce((a, b) => +a + +b.DropGallons, 0);
            let isQuantityMismatch = sumOfAssetQuantity != drop.get('ActualDropQuantity').value && drop.get('ActualDropQuantity').value > 0;

            assets.controls.forEach((asset: FormGroup) => {
                if (isQuantityMismatch)
                    this.validationService.addError(asset.get('DropGallons'), 'isQuantityMismatch');
                else
                    this.validationService.removeError(asset.get('DropGallons'), 'isQuantityMismatch');
            });
        }
    }

    public findInvalidControlsRecursive(formToInvestigate: FormGroup | FormArray): string[] {
        var invalidControls: string[] = [];
        let recursiveFunc = (form: FormGroup | FormArray) => {
            Object.keys(form.controls).forEach(field => {
                const control = form.get(field);
                if (control.invalid) invalidControls.push(field);
                if (control instanceof FormGroup) {
                    recursiveFunc(control);
                } else if (control instanceof FormArray) {
                    recursiveFunc(control);
                }
            });
        }
        recursiveFunc(formToInvestigate);
        return invalidControls;
    }

    public isTerminalAssignedOnOrder(): boolean {
        var isTerminalAssigned = true;
        var currObj = this;
        this.invoiceForm.get("Drops").value.forEach(function (drop) {
            if ((drop.TypeOfFuel != 10 && drop.TypeOfFuel != 26)  && (drop.TerminalId == 0 || drop.TerminalId == '' || drop.TerminalId == null)) {
                currObj.InvalidDropInfos.push({
                    ProductId: drop.FuelTypeId,
                    PoNumber: drop.PoNumber,
                    Message: "Terminal not assigned for PO# " + drop.PoNumber,
                });
                isTerminalAssigned = false;
                return isTerminalAssigned;
            }
        });
        return isTerminalAssigned;
    }
}

export class DropInfo {
    public ProductId: number;
    public PoNumber: string;
    public IsBOLImageRequired: boolean;
    public IsDropImageRequired: boolean;
    public IsBolDetailsRequired: boolean;
    public IsSignatureRequired: boolean;
    //public IsFTL: boolean;
    public OrderId: number;
}
export class InvalidDropDetailInfo {
    public ProductId: number;
    public PoNumber: string;
    public Message: string;
}
export class MissingImageInfo {
    public isImageNotProvided: boolean;
    public imgMissingMsg: string;
}