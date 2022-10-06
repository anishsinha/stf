import { Component, EventEmitter, Input, OnInit, OnDestroy, Output, SimpleChanges, ViewEncapsulation } from '@angular/core';
import { FormArray, FormBuilder, FormGroup } from '@angular/forms';
import { Subject, Subscription } from 'rxjs';
import { DropdownItem } from 'src/app/statelist.service';
import { DeliveryRequestViewModel, DelRequestsByJobModel, DropAddressModel, Priority } from '../models/DispatchSchedulerModels';
import { CarrierService } from '../service/carrier.service';
import { NgbModal, ModalDismissReasons } from '@ng-bootstrap/ng-bootstrap';
import { IDropdownSettings } from 'ng-multiselect-dropdown';
import * as moment from 'moment';
import { Declarations } from 'src/app/declarations.module';
import { DataService } from 'src/app/services/data.service';
import { BrokeredDrCarrierStatus, DeliveryReqPriority } from 'src/app/app.enum';
import { LoadPriorities } from 'src/app/app.constants';


@Component({
    selector: 'app-assigned-to-me',
    templateUrl: './assigned-to-me.component.html',
    styleUrls: ['./assigned-to-me.component.css'],
    encapsulation: ViewEncapsulation.None
})
export class AssignedToMeComponent implements OnInit {

    @Output() OnAcceptRejectDR: EventEmitter<any> = new EventEmitter<any>();
    @Input() regionId: string;
    @Input() SelectedDate: string;
    //@Input() public Locations: DelRequestsByJobModel[] = [];
    
    public dtMustGoOptions: any = {};
    public MustGoSchedules: DeliveryRequestViewModel[] = [];
    public dtMustGoTrigger: Subject<any> = new Subject();
    public dtShouldGoOptions: any = {};
    public dtCouldGoOptions: any = {};
    closeResult = '';
    AssignedToMeForm: FormGroup;
    FilterForm: FormGroup;
    isLoading = false;
    public HeaderText: string;
    public NextRenewalDate: string;
    public AcceptRejectDRSubscription: Subscription;

    public currentRegionId: string;
    public currentSelectedDate: string;
    public brokeredDrRequestedToMe: DelRequestsByJobModel[] = [];
    public brokeredDrRequestedToMeAPI: DeliveryRequestViewModel[] = [];
    public MustGoDrRequestedToMeAPI: DeliveryRequestViewModel[] = [];
    public ShouldGoDrRequestedToMeAPI: DeliveryRequestViewModel[] = [];
    public CouldGoDrRequestedToMeAPI: DeliveryRequestViewModel[] = [];
    public requestToUpdate: DeliveryRequestViewModel = new DeliveryRequestViewModel(false);
    public BrokeredDrStatus: BrokeredDrCarrierStatus = null;

    //#region  Filters
    public SupplierCompanies: DropdownItem[] = [];
    public CustomerLocation: DropdownItem[] = [];
    public priorityList: Priority[] = [];
    public PriorityDdlSettings: IDropdownSettings;
    public MaxInputDate: Date = new Date();
    public MinInputDate: Date = new Date();
    //#endregion Filters 

    constructor(private fb: FormBuilder, private carrierService: CarrierService, public dataService: DataService,
        private modalService: NgbModal) { }

    ngOnInit() {
        this.MaxInputDate.setFullYear(this.MaxInputDate.getFullYear() + 1);
        this.MinInputDate.setDate(this.MaxInputDate.getDate());
        this.NextRenewalDate = moment(new Date()).add(1, 'months').startOf('month').format('MM/DD/YYYY');
        this.HeaderText = 'Assigned To Me';
        this.FilterForm = this.getAssignedToMeFilterForm(false);
        this.currentRegionId = this.regionId;
        this.currentSelectedDate = this.SelectedDate;
        this.Init();
        this.subscribeFormChanges();
        this.subscribeAcceptRejectDRSubject();
        //this.InitilizeGrids();
    }

    ngOnDestroy(): void {
        if (this.AcceptRejectDRSubscription) {
            this.AcceptRejectDRSubscription.unsubscribe();
        }
    }

    subscribeFormChanges() {
        this.FilterForm.controls.Suppliers.valueChanges.subscribe(chenge => {
            this.FilterData();
        });
        this.FilterForm.controls.Locations.valueChanges.subscribe(chenge => {
            this.FilterData();
        });
    }

    //to use datatable Added this functions but Inbuit dataTrigger giving error - check with anant
    InitilizeGrids() {

        let ColumnDetails = [];

        ColumnDetails = [{ title: 'Supplier Name', name: 'Supplier Name', data: 'CustomerCompany', "autoWidth": true },
        { title: 'Address', name: 'Address', data: 'JobAddress', "autoWidth": true },
        { title: 'Product', name: 'Product', data: 'ProductType', "autoWidth": true },
        { title: 'Required Qty', name: 'Required Qty', data: 'RequiredQuantity', "autoWidth": true },
        { title: 'Inventory', name: 'Inventory', data: 'CurrentInventory', "autoWidth": true },
        { title: 'Ullage', name: 'Ullage', data: 'UoM', "autoWidth": true }];


        this.dtMustGoOptions = {
            pagingType: 'first_last_numbers',
            pageLength: 10,
            lengthMenu: [[10, 25, 50, 100, 99999999], [10, 25, 50, 100, "All"]],
            serverSide: true,
            processing: true,
            fixedHeader: {
                header: true,
                headerOffset: 200,
            },
            ajax: (dataTablesParameters: any, callback) => {
                this.isLoading = true;
                // this.brokeredDrRequestedToMe = [];
                // this.carrierService.getBrokeredDrAssignedToMe(this.regionId, this.SelectedDate).subscribe(drs => {
                //   console.log(drs);
                //   this.brokeredDrRequestedToMeAPI = drs || [];
                //   this.MustGoSchedules = drs || []
                //   data: drs || [];
                //   // this.AssignedToMeForm = this.fb.group({
                //   //   DeliveryRequests: this.getDeliveryRequestFormArray(this.brokeredDrRequestedToMeAPI)
                //   // });
                //   this.isLoading = false;
                //   //this.brokeredDrRequestedToMe = this.deliveryReqService.groupDrsByJob(drs, DeliveryRequestTypes.AssignedToMe);           

                // });

            },
            dom: '<"html5buttons"B>lTfgitp',
            order: [[8, 'desc']],
            buttons: [
                { extend: 'copy', exportOptions: ColumnDetails },
                { extend: 'csv', title: 'Dispatcher Dashboard - Must Go', exportOptions: ColumnDetails },
                { extend: 'pdf', title: 'Dispatcher Dashboard - Must Go', orientation: 'landscape', exportOptions: ColumnDetails },
                { extend: 'print', exportOptions: ColumnDetails }
            ],
            columns: ColumnDetails
        };
    }

    FiltersSettingsAndPriorityList() {

        this.PriorityDdlSettings = {
            singleSelection: false,
            idField: 'Id',
            textField: 'Name',
            selectAllText: 'Select All',
            unSelectAllText: 'UnSelect All',
            itemsShowLimit: 1,
            allowSearchFilter: false,
            enableCheckAll: false
        }

        this.priorityList = LoadPriorities;

    }

    ngAfterViewInit(): void {
        //this.dtMustGoTrigger.next();
    }

    Init() {
        //this.FiltersSettingsAndPriorityList();
    }
    getBrokeredDrAssignedToMe(regionId: string, selectedDate?: string) {
        this.isLoading = true;
        this.brokeredDrRequestedToMe = [];
        this.carrierService.getBrokeredDrAssignedToMe(regionId, selectedDate).subscribe(drs => {
            this.brokeredDrRequestedToMeAPI = drs || [];
            this.GetSuplierCompanyList(this.brokeredDrRequestedToMeAPI)
            this.MustGoSchedules = drs || []
            this.brokeredDrRequestedToMeAPI.forEach(x => {
                x.StringAssignedOn = x.AssignedOn ? moment(x.AssignedOn).format('MM/DD/YYYY') : '--';
                if (x.StringAssignedOn == '01/01/0001' || x.StringAssignedOn == 'Invalid date' ||
                    x.StringAssignedOn == '12/31/0000')
                    x.StringAssignedOn = '--'
            })
            this.MustGoDrRequestedToMeAPI = [];
            this.ShouldGoDrRequestedToMeAPI = [];
            this.CouldGoDrRequestedToMeAPI = [];
            this.MustGoDrRequestedToMeAPI = drs.filter(x => x.Priority == DeliveryReqPriority.MustGo) || [];
            this.ShouldGoDrRequestedToMeAPI = drs.filter(x => x.Priority == DeliveryReqPriority.ShouldGo) || [];
            this.CouldGoDrRequestedToMeAPI = drs.filter(x => x.Priority == DeliveryReqPriority.CouldGo) || [];
            data: drs || [];
            this.isLoading = false;
        });
    }

    subscribeAcceptRejectDRSubject(): void {
        this.AcceptRejectDRSubscription = this.dataService.AcceptRejectDRSubject.subscribe(x => {
            if (x) {
                this.getBrokeredDrAssignedToMe(x.RegionId, x.Date);
            }
        });
    }

    GetSuplierCompanyList(dr: DeliveryRequestViewModel[]) {
        this.SupplierCompanies = [];
        this.CustomerLocation = [];
        dr.forEach(ele => {
            if (!this.SupplierCompanies.find(x => ele.SupplierCompanyId == x.Id)) {
                let obj = new DropdownItem;
                obj.Id = ele.SupplierCompanyId;
                obj.Name = ele.CustomerCompany;
                this.SupplierCompanies.push(obj)
            }
            if (!this.CustomerLocation.find(x => ele.JobId == x.Id)) {
                let objLocation = new DropdownItem;
                objLocation.Id = ele.JobId;
                objLocation.Name = ele.JobName;
                this.CustomerLocation.push(objLocation)
            }
        });
        if (this.SupplierCompanies.length == 1) {
            this.FilterForm.get('Suppliers').patchValue(this.SupplierCompanies);
        }
        if (this.CustomerLocation.length == 1) {
            this.FilterForm.get('Locations').patchValue(this.CustomerLocation);
        }

    }

    public confirmChangeBrokeredDrStatus(status: BrokeredDrCarrierStatus, dr: any,) {
        this.isLoading = true;
        var delReqForm = dr;
        let delReq = delReqForm as DeliveryRequestViewModel;
        this.BrokeredDrStatus = status;
        var allDrs = this.ShouldGoDrRequestedToMeAPI;
        var drToupdate = allDrs.find(t => t.Id == delReq.Id);
        this.requestToUpdate = Object.assign({}, drToupdate);
        // this.AssignDrForm.get('DispatcherNote').setValue('');        

        this.requestToUpdate = Object.assign({}, delReq);
        let element = document.getElementById("openConfirmProceedBrokeredDrModal");
        element ? element.click() : null;
        this.isLoading = false;

        // let celement = document.getElementsByClassName("modal-backdrop");    
        // celement['className'] = 'modal show';
    }

    public changeBrokeredDrStatus(DrId: string, BlendedGroupId:string, status: BrokeredDrCarrierStatus) {
        this.isLoading = true;
        this.OnAcceptRejectDR.emit({ drId: DrId, blendedGroupId: BlendedGroupId, status: status == 2 ? BrokeredDrCarrierStatus.Accepted : BrokeredDrCarrierStatus.Rejected });
        this.currentRegionId = this.regionId;
        let that = this;
        setTimeout(function () {
            that.getBrokeredDrAssignedToMe(that.currentRegionId, that.currentSelectedDate);
            this.isLoading = false;
        }, 3500);
    }
    open(content) {
        this.modalService.open(content, { ariaLabelledBy: 'modal-basic-title' }).result.then((result) => {
            this.closeResult = `Closed with: ${result}`;
        }, (reason) => {
            this.closeResult = `Dismissed ${''}`;
        });
    }


    getAssignedToMeFilterForm(isFilterApplied?: boolean) {
        return this.fb.group({
            Suppliers: this.fb.control([]),
            Priority: this.fb.control([]),
            Locations: this.fb.control([]),
            FromDate: this.fb.control(''),
            ToDate: this.fb.control(''),
            IsFilterApplied: this.fb.control(isFilterApplied ? true : false)
        });
    }

    isPriorityVisible(priority: number) {
        let selectedArray = this.FilterForm.controls['Priority'].value as any[] || [];
        return (selectedArray.length == 0 || selectedArray.find(x => x.Id == priority)) || false;
    }
    setPanelHeader(headerText: string) {
        this.HeaderText = headerText;
        this.FiltersSettingsAndPriorityList();
        this.getBrokeredDrAssignedToMe(this.regionId, this.SelectedDate);
    }
    clearPanelControls() {
        this.brokeredDrRequestedToMe = [];
        this.brokeredDrRequestedToMeAPI = [];
        this.MustGoDrRequestedToMeAPI = [];
        this.ShouldGoDrRequestedToMeAPI = [];
        this.CouldGoDrRequestedToMeAPI = [];
        this.requestToUpdate = new DeliveryRequestViewModel(false);
        this.BrokeredDrStatus = null;
        this.SupplierCompanies = [];
        this.priorityList = [];
        this.FilterForm.reset();
    }



    FilterData() {
        if (this.FilterForm.controls['Suppliers'].value != null && this.FilterForm.controls['Suppliers'].value.length > 0) {
            let supplierIds = [];
            this.FilterForm.controls['Suppliers'].value.forEach(ele => supplierIds.push(ele.Id));
            let mustGo = [];
            let shouldGo = [];
            let couldGo = [];
            this.MustGoDrRequestedToMeAPI = this.brokeredDrRequestedToMeAPI.filter(x => x.Priority == DeliveryReqPriority.MustGo);
            this.ShouldGoDrRequestedToMeAPI = this.brokeredDrRequestedToMeAPI.filter(x => x.Priority == DeliveryReqPriority.ShouldGo);
            this.CouldGoDrRequestedToMeAPI = this.brokeredDrRequestedToMeAPI.filter(x => x.Priority == DeliveryReqPriority.CouldGo);
            supplierIds.forEach(x => {
                let oMustGo = this.MustGoDrRequestedToMeAPI.filter(res => res.SupplierCompanyId == x);
                oMustGo.forEach(x => mustGo.push(x));
                let oShouldGo = this.ShouldGoDrRequestedToMeAPI.filter(res => res.SupplierCompanyId == x);
                oShouldGo.forEach(x => shouldGo.push(x));
                let oCouldGo = this.CouldGoDrRequestedToMeAPI.filter(res => res.SupplierCompanyId == x);
                oCouldGo.forEach(x => couldGo.push(x));
            });
            this.MustGoDrRequestedToMeAPI = mustGo;
            this.ShouldGoDrRequestedToMeAPI = shouldGo;
            this.CouldGoDrRequestedToMeAPI = couldGo;
        }
        else {
            this.MustGoDrRequestedToMeAPI = this.brokeredDrRequestedToMeAPI.filter(x => x.Priority == DeliveryReqPriority.MustGo);
            this.ShouldGoDrRequestedToMeAPI = this.brokeredDrRequestedToMeAPI.filter(x => x.Priority == DeliveryReqPriority.ShouldGo);
            this.CouldGoDrRequestedToMeAPI = this.brokeredDrRequestedToMeAPI.filter(x => x.Priority == DeliveryReqPriority.CouldGo);
        }

        if (this.FilterForm.controls['Locations'].value != null && this.FilterForm.controls['Locations'].value.length > 0) {
            let LocationIds = [];
            this.FilterForm.controls['Locations'].value.forEach(ele => LocationIds.push(ele.Id));
            let mustGo = [];
            let shouldGo = [];
            let couldGo = [];
            LocationIds.forEach(x => {
                let oMustGo = this.MustGoDrRequestedToMeAPI.filter(res => res.JobId == x);
                oMustGo.forEach(x => mustGo.push(x));
                let oShouldGo = this.ShouldGoDrRequestedToMeAPI.filter(res => res.JobId == x);
                oShouldGo.forEach(x => shouldGo.push(x));
                let oCouldGo = this.CouldGoDrRequestedToMeAPI.filter(res => res.JobId == x);
                oCouldGo.forEach(x => couldGo.push(x));
            });
            this.MustGoDrRequestedToMeAPI = mustGo;
            this.ShouldGoDrRequestedToMeAPI = shouldGo;
            this.CouldGoDrRequestedToMeAPI = couldGo;
        }

        if (this.FilterForm.controls.FromDate.value && this.FilterForm.controls.ToDate.value) {
            if (moment(this.FilterForm.controls.ToDate.value).toDate() < moment(this.FilterForm.controls.FromDate.value).toDate()) {
                Declarations.msgerror('FromDate should be greater than ToDate', undefined, undefined);
                return;
            }
            let mstGo = []
            mstGo = this.MustGoDrRequestedToMeAPI.filter(x => x.StringAssignedOn == '--' || (moment(this.FilterForm.controls.FromDate.value).toDate() <= moment(moment(x.StringAssignedOn).format('MM/DD/YYYY')).toDate() &&
                moment(moment(x.StringAssignedOn).format('MM/DD/YYYY')).toDate() <= moment(this.FilterForm.controls.ToDate.value).toDate()));
            this.MustGoDrRequestedToMeAPI = mstGo;

            let shldGo = []
            shldGo = this.ShouldGoDrRequestedToMeAPI.filter(x => x.StringAssignedOn == '--' || (moment(this.FilterForm.controls.FromDate.value).toDate() <= moment(moment(x.StringAssignedOn).format('MM/DD/YYYY')).toDate() &&
                moment(moment(x.StringAssignedOn).format('MM/DD/YYYY')).toDate() <= moment(this.FilterForm.controls.ToDate.value).toDate()));
            this.ShouldGoDrRequestedToMeAPI = shldGo;

            let coldGo = []
            coldGo = this.CouldGoDrRequestedToMeAPI.filter(x => x.StringAssignedOn == '--' || (moment(this.FilterForm.controls.FromDate.value).toDate() <= moment(moment(x.StringAssignedOn).format('MM/DD/YYYY')).toDate() &&
                moment(moment(x.StringAssignedOn).format('MM/DD/YYYY')).toDate() <= moment(this.FilterForm.controls.ToDate.value).toDate()));
            this.CouldGoDrRequestedToMeAPI = coldGo;
        }
        this.RemoveSelectedSuppliers();
    }
    public ShowAllDrs() {
        this.FilterForm.reset();
        this.MustGoDrRequestedToMeAPI = this.brokeredDrRequestedToMeAPI.filter(x => x.Priority == DeliveryReqPriority.MustGo);
        this.ShouldGoDrRequestedToMeAPI = this.brokeredDrRequestedToMeAPI.filter(x => x.Priority == DeliveryReqPriority.ShouldGo);
        this.CouldGoDrRequestedToMeAPI = this.brokeredDrRequestedToMeAPI.filter(x => x.Priority == DeliveryReqPriority.CouldGo);

    }

    RemoveSelectedSuppliers() {
        let SuppliersList = [];
        let LocationList = [];
        this.MustGoDrRequestedToMeAPI.forEach(x => {
            SuppliersList.push(x.SupplierCompanyId);
            LocationList.push(x.JobId);
        })
        this.ShouldGoDrRequestedToMeAPI.forEach(x => {
            SuppliersList.push(x.SupplierCompanyId);
            LocationList.push(x.JobId);
        })
        this.ShouldGoDrRequestedToMeAPI.forEach(x => {
            SuppliersList.push(x.SupplierCompanyId);
            LocationList.push(x.JobId);
        })
        let removeSupplier = [];
        if (this.FilterForm.controls.Suppliers.value != null) {
            this.FilterForm.controls.Suppliers.value.forEach(sup => {
                let check = SuppliersList.indexOf(c => c == sup.Id)
                if (check < 0) {
                    removeSupplier.push(sup);
                }
            });

            if (removeSupplier != null) {
                removeSupplier.forEach(t => {
                    let index = this.FilterForm.controls.Suppliers.value.indexOf(c => c.Id == t.Id);
                    if (index >= 0)
                        delete this.FilterForm.controls.Suppliers.value[index];
                })
            }
        }

        let RemoveLocations = [];
        if (this.FilterForm.controls.Locations.value != null) {
            this.FilterForm.controls.Locations.value.forEach(sup => {
                let check = LocationList.indexOf(c => c == sup.Id)
                if (check < 0) {
                    RemoveLocations.push(sup);
                }
            });

            if (RemoveLocations != null) {
                RemoveLocations.forEach(t => {
                    let index = this.FilterForm.controls.Locations.value.indexOf(c => c.Id == t.Id);
                    if (index >= 0)
                        delete this.FilterForm.controls.Locations.value[index];
                })
            }
        }
    }
    getNextRenewalDate(date: Date) {
        this.NextRenewalDate = moment(date).add(1, 'months').startOf('month').format('MM/DD/YYYY');
    }
}
