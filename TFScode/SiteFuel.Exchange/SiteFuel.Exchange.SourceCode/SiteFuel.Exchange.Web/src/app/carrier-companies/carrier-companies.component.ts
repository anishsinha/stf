import { Component, OnInit, ViewChild, ElementRef, ViewEncapsulation, ViewChildren, QueryList } from '@angular/core';
import { IDropdownSettings } from 'ng-multiselect-dropdown';
import { startsWithPipe, startsWithJobPipe } from './search-filter';
import { FormGroup, FormBuilder, FormControl, Validators, FormArray } from '@angular/forms';
import { AssigncarrierService, DropdownItem, Carrier, CarrierJob, CarrierJobDetails, EditFreightOnlyOrder, JobWithEmails } from './service/assigncarrier.service';
import { Declarations } from '../declarations.module';
import { Subject } from 'rxjs';
import { DataTableDirective } from 'angular-datatables';

@Component({
    selector: 'app-carrier-companies',
    templateUrl: './carrier-companies.component.html',
    styleUrls: ['./carrier-companies.component.css'],
    encapsulation: ViewEncapsulation.None
})


export class CarrierCompaniesComponent implements OnInit {

    public carrierEmails = [];
    public _opened: boolean = false;
    public _animate: boolean = true;
    public _positionNum: number = 1;
    public _POSITIONS: Array<string> = ['left', 'right', 'top', 'bottom'];
    public serviceResponse: any;
    public isUpdate: boolean = false;
    public multiDropdownSettings: IDropdownSettings;
    public dropdownSettings: IDropdownSettings;
    public IsLoading: boolean = false;
    public IsEmpty: boolean = false;
    public IsSuccess: boolean = false;
    public carrier: Carrier;
    public assignedCarrierList: Carrier[] = [];
    public assignedCarriers: Carrier[] = [];
    public rcForm: FormGroup;
    public carrierList: DropdownItem[] = [];
    public jobs: DropdownItem[] = [];
    public jobs2: DropdownItem[] = [];
    public availableJobs: CarrierJob[] = [];
    public assignedJobs: CarrierJob[] = [];
    query: string = '';
    list1Search: string = '';
    list2Search: string = '';
    public popoverTitle: string = 'Delete Confirmation';
    public popoverMessage: string = 'Do you really want to delete? Deleting a location assignment will result in closure of all the existing orders for the carrier';

    public SelectedCount: number;
    public confirmClicked: boolean = false;
    public cancelClicked: boolean = false;

    public CarrierUsers: CarrierJobDetails[] = [];
    public dtTrigger: Subject<any> = new Subject();
    public dtOptions: any = {};
    @ViewChildren(DataTableDirective) dtElements: QueryList<DataTableDirective>;


    @ViewChild('btnOpenModal') btnOpenModal: ElementRef<HTMLElement>;
    @ViewChild('btnCloseModal') btnCloseModal: ElementRef<HTMLElement>;
    @ViewChild('btnCloseBulkModal') btnCloseBulkModal: ElementRef<HTMLElement>;
    @ViewChild('confirmationBox') confirmationBox: ElementRef;
    public IsDisplayLoader: boolean = false;
    public SelectedJobs: DropdownItem[] = [];
    public IsCreateFreightOrder: boolean = null;
    public UpdatedJobIds: any[];

    public IsJobDeselect: boolean = false;
    public removedJobs: DropdownItem[] = [];
    public WarningMessage: string = "Note: Removing a location assignment will result in closure of all the existing orders for the carrier";
    file: any;
    editCarrierId: string = null;
    existingJobs: any = [];
    SelectedCarrier: DropdownItem;
    selectedCarrierItem: DropdownItem;
    allJobs: CarrierJob[];
    selectCarrierModel: any = [];
    assginedJobSelectAll = false;
    availableJobSelectAll = false;
    editEmailJobId: number;
    editEmailDetails: DropdownItem[];
    selectedFile: any;
    constructor(private fb: FormBuilder, private assigncarrierService: AssigncarrierService) { }

    ngOnInit() {
        this.getAssignedCarriers();
        this.getCarriers();
        this.multiDropdownSettings = {
            singleSelection: false,
            idField: 'Id',
            textField: 'Name',
            selectAllText: 'Select All',
            unSelectAllText: 'UnSelect All',
            itemsShowLimit: 1,
            allowSearchFilter: true
        };
        this.dropdownSettings = {
            singleSelection: true,
            idField: 'Id',
            textField: 'Name',
            selectAllText: 'Select All',
            unSelectAllText: 'UnSelect All',
            itemsShowLimit: 1,
            allowSearchFilter: true
        };
        //this.getAssignedCarrierUsers();
        let exportColumns = { columns: [0, 1, 2] };
        this.dtOptions = {
            pagingType: 'simple_numbers',
            pageLength: 10,
            lengthMenu: [[10, 25, 50, 100, -1], [10, 25, 50, 100, "All"]],
            searching: true,
            destroy: true,
            dom: '<"html5buttons"B>lTfgitp',
            buttons: [
                { extend: 'copy', exportOptions: exportColumns },
                { extend: 'csv', title: 'Assigned Carriers', exportOptions: exportColumns },
                { extend: 'pdf', title: 'Assigned Carriers', orientation: 'landscape', exportOptions: exportColumns },
                { extend: 'print', exportOptions: exportColumns }
            ],
        };
    }

    getCarriers() {
        this.assigncarrierService.getCarriers()
            .subscribe((carriers: DropdownItem[]) => {
                this.carrierList = carriers;
                this.carrierList.length
            });
    }
    getAssignedCarrierUsers() {
        this.assigncarrierService.getAssignedCarrierUsers()
            .subscribe((carriers: CarrierJobDetails[]) => {
                this.CarrierUsers = carriers;
                this.refreshDatatable();
            });
    }
    refreshDatatable(): void {
        this.dtElements.forEach((dtElement: DataTableDirective) => {
            if (dtElement.dtInstance) {
                dtElement.dtInstance.then((dtInstance: DataTables.Api) => { dtInstance.destroy(); });
            }
        });
        this.dtTrigger.next();
    }
    removeAssignedCarrier(carrier: Carrier) {
        this.IsSuccess = true;
        this.IsLoading = true;
        this.assigncarrierService.deleteAssignedCarrier(carrier)
            .subscribe((response: any) => {
                this.serviceResponse = response;
                if (response.StatusCode == 0) {
                    this.carrierList.push(carrier.Carrier);
                    this.closeAssignedOrdersforCarrier(carrier);
                } else {
                    this.IsLoading = false;
                }
            });
    }

    getAssignedCarriers() {
        this.IsLoading = true;
        this.assigncarrierService.getAssignedCarriers()
            .subscribe((response: Carrier[]) => {
                if (response != null && response != undefined) {
                    this.assignedCarrierList = response;
                    var existingCarriers = response.map(function (item) { return item.Carrier.Id; });
                    this.carrierList = this.carrierList.filter(function (item) { return existingCarriers.indexOf(item.Id) == -1; });
                    this.getJobs();
                    this.IsLoading = false;
                    this.refreshDatatable();
                }
            });
    }
    getJobs() {
        this.assignedCarrierList.forEach(element => {
            if (element) {
                element.assignedLocations = this.getAssignedJobs(element);
                if (element.Jobs) {
                    this.existingJobs.push(element.Jobs.map(function (item) { return item.Job; }));
                }
            }
        })
        //this.existingJobs = this.assignedCarrierList.map(function (item) { return item.Jobs;});
        this.assigncarrierService.getJobs()
            .subscribe((response: CarrierJob[]) => {
                this.availableJobs = response;
                this.allJobs = response;
                this.availableJobs && this.availableJobs.map(m => m.Job.IsSelected = false);
                if (this.existingJobs) {
                    this.existingJobs.forEach(element => {
                        if (element) {
                            element.forEach(job => {
                                let index = this.availableJobs.findIndex(t => t.Job.Id == job.Id);
                                if (index != -1) { this.availableJobs.splice(index, 1); };
                            })
                        }
                    })
                }
            });
    }
    getAssignedJobs(assignedJobs: Carrier) {
        var locations = '-';
        if (assignedJobs) {
            var jobs = assignedJobs.Jobs.map(function (item) { return item.Job.Name; });
            if (jobs.length > 4) {
                locations = jobs.slice(0, 4).join(", ") + "...";
            } else {
                locations = jobs.join(", ");
            }
        }
        return locations;
    }

    editForm(_carrier: Carrier) {
        this.SelectedCarrier = _carrier.Carrier;
        this.editCarrierId = _carrier.Id;
        var selectedJobs = _carrier.Jobs.map(function (item) { return item.Job; });
        this.SelectedJobs = [];
        this.SelectedJobs = selectedJobs;
        this.assignedJobs = _carrier.Jobs;
        this.assignedJobs && this.assignedJobs.map(m => m.Job.IsSelected = false);
        this._toggleOpened(true);
        this.GetCarrierUserEmails(_carrier.Carrier.Id);
        this.isUpdate = true;
    }
    public getCarrierEmailsById() {
        if (this.SelectedCarrier && this.SelectedCarrier.Id > 0) {
            var _com = this.carrierEmails.find(x => x.CompanyId == this.SelectedCarrier.Id);
            return _com ? _com.CarrierEmails : [];
        }
        else { return []; }
    }

    public GetCarrierUserEmails(companyId: number) {
        var _com = this.carrierEmails.find(x => x.CompanyId == companyId);
        if (!_com) {
            this.IsLoading = true;
            this.assigncarrierService.GetCarrierUserEmails(companyId)
                .subscribe((response: DropdownItem[]) => {
                    if (response) {
                        this.carrierEmails.push({ CompanyId: companyId, CarrierEmails: response });
                    }
                    this.IsLoading = false;
                });
        }
    }
    Validate() {
        if (!this.SelectedCarrier || this.SelectedCarrier.Id <= 0) {
            Declarations.msgerror("Atleast one carrier must be assigned", undefined, undefined);
            return false;
        }
        if (!this.assignedJobs || this.assignedJobs.length <= 0) {
            Declarations.msgerror("Atleast one job must be selected", undefined, undefined);
            return false;
        }
        return true;
    }

    public SaveCarrier() {
        if (this.Validate()) {
            this.assignedCarriers = [];
            this.assignedCarriers.push({ Id: "", Carrier: null, Jobs: [], assignedLocations: null });
            if (this.SelectedCarrier) {
                this.assignedCarriers[0].Carrier = this.SelectedCarrier;
                this.assignedCarriers[0].Jobs = this.assignedJobs;
                this.assignedCarriers[0].Id = this.editCarrierId;
                if (this.isUpdate) {
                    this.updateAssignedCarrier();
                } else {
                    this.assignCarriers();
                }
            }
        }
    }
    public SaveJobEmail() {
        var selectedJob = this.assignedJobs.find(t => t.Job.Id == this.editEmailJobId);
        if (selectedJob) {
            selectedJob.Job.Emails = [];
            if (this.editEmailDetails) {
                this.editEmailDetails.forEach(element => {
                    if (!selectedJob.Job.Emails) {
                        selectedJob.Job.Emails = [];
                    }
                    if (!selectedJob.Job.Emails.find(t => t.Id == element.Id)) {
                        selectedJob.Job.Emails.push({ Id: element.Id, Name: element.Name, Code: element.Code, IsSelected: element.IsSelected });
                    }
                })
            }
            else {
                selectedJob.Job.Emails = [];
            }
        }
    }
    assignNewForm() {
        this._toggleOpened(true);
        this.isUpdate = false;
    }

    assignCarriers() {
        this.DisplayFreightOrderConfirmationModal();
    }

    updateAssignedCarrier() {
        var updatedJobIds = this.ValidateIfNewJobAdded();
        if (updatedJobIds.length > 0) {
            if (updatedJobIds[0].InsertedJobs.length > 0) {
                //show modal as new jobs are added when editing;FO will be created only for newly assigned jobs
                this.DisplayFreightOrderConfirmationModal();
            }
            else {
                this.IsSuccess = true;
                this.assigncarrierService.updateAssignedCarrier(this.assignedCarriers[0])// First Update Existing carrier assignment
                    .subscribe((response: any) => {
                        this.serviceResponse = response;
                        if (response.StatusCode == 0) {
                            this.IsSuccess = false;
                            this.EditFreightOnlyOrders(false);// Edit FO according to new job assignment
                        }
                        else {
                            this.IsSuccess = false;
                            Declarations.msgerror("Carrier Assignment Failed", undefined, undefined);
                            this.getAssignedCarriers();
                            this.IsDisplayLoader = false;
                            let element: HTMLElement = this.btnCloseModal.nativeElement;
                            element.click();
                            this._toggleOpened(false);
                        }
                    });
            }
        }
    }


    _toggleOpened(shouldOpen: boolean) {
        if (shouldOpen) {
            this._opened = true;
        }
        else {
            this._opened = !this._opened;
            this.clearForm();
            this.isUpdate = false;
        }
    }
    clearForm() {
        this.availableJobs = this.allJobs
        this.availableJobs && this.availableJobs.map(m => m.Job.IsSelected = false);
        this.assignedJobs = [];
        this.SelectedCarrier = null;
        this.selectCarrierModel = [];
        this.list1Search = '';
        this.list2Search = '';
        this.assginedJobSelectAll = false;
        this.availableJobSelectAll = false;
        this.assignedCarriers.push({ Id: "", Carrier: null, Jobs: [], assignedLocations: null });
        this.editEmailDetails = [];
        this.selectedCarrierItem = null;
        this.existingJobs = [];
    }
    isInvalid(name: string, i: number): boolean {
        var carrierControls = this.getCarriersFormArray();
        var result = carrierControls.controls[i].get(name).invalid
            &&
            (
                carrierControls.controls[i].get(name).dirty
                ||
                carrierControls.controls[i].get(name).touched
            )
        return result;
    }

    getCarriersFormArray(): FormArray {
        return this.rcForm.get('Carriers') as FormArray;
    }

    public OnCarrierSelect(carrier: any) {
        this.SelectedCarrier = carrier;
        this.GetCarrierUserEmails(carrier.Id);
    }

    public OnCarrierDeSelect(carrier: any) {
        this.SelectedCarrier = null;
    }
    public OnEmailSelect(email: any, job: CarrierJob) {
        if (!job.Job.Emails) {
            job.Job.Emails = [];
        }
        job.Job.Emails.push({ Id: email.Id, Name: email.Name, Code: null, IsSelected: true });
    }
    public OnEmailDeSelect(email: any, job: CarrierJob) {
        let index = job.Job.Emails.findIndex(a => a.Id == email.Id);
        if (index != -1) { job.Job.Emails.splice(index, 1); }
    }
    public editEmail(email: any, job: CarrierJob) {
        job.Job.IsEmailEdit = true;
        this.editEmailJobId = job.Job.Id;
        this.editEmailDetails = job.Job.Emails;
    }
    public toggleSelect(availableJob) {
        if (availableJob.Job.IsSelected) {
            availableJob.Job.IsSelected = false;
        } else {
            availableJob.Job.IsSelected = true;
        }
        if (this.availableJobs.find(t => !t.Job.IsSelected)) {
            this.availableJobSelectAll = false;
        }
        if (this.assignedJobs.find(t => !t.Job.IsSelected)) {
            this.assginedJobSelectAll = false;
        }
        this.SelectedCount = Object.keys(this.availableJobs.filter((data) => data.Job.IsSelected === true)).length;
    }
    public toogleSelectAll(name: string, isChecked: boolean) {
        if (isChecked) {
            (name == 'availableJob') ? this.availableJobs.filter(t => t.Job.Name.toLowerCase().indexOf(this.list1Search.toLowerCase()) >= 0).map(m => m.Job.IsSelected = true) : this.assignedJobs.filter(t => t.Job.Name.toLowerCase().indexOf(this.list2Search.toLowerCase()) >= 0).map(m => m.Job.IsSelected = true);
        } else {
            (name == 'availableJob') ? this.availableJobs.filter(t => t.Job.Name.toLowerCase().indexOf(this.list1Search.toLowerCase()) >= 0).map(m => m.Job.IsSelected = false) : this.assignedJobs.filter(t => t.Job.Name.toLowerCase().indexOf(this.list2Search.toLowerCase()) >= 0).map(m => m.Job.IsSelected = false);
        }
        this.SelectedCount = Object.keys(this.availableJobs.filter((data) => data.Job.IsSelected === true)).length;
    }

    public moveToLeft() {
        this.availableJobs.map(m => m.Job.IsSelected = false);
        var ls = this.assignedJobs.filter(f => f.Job.IsSelected == true);
       // this.availableJobs = this.availableJobs.concat(ls);
        this.availableJobs = ls.concat(this.availableJobs);
        this.assignedJobs = this.assignedJobs.filter(f => f.Job.IsSelected == false);
        this.assignedJobs.map(m => m.Job.IsSelected = false);
    }
    public moveToRight() {
        this.assignedJobs.map(m => m.Job.IsSelected = false);
        var ls = this.availableJobs.filter(f => f.Job.IsSelected == true);
       // this.assignedJobs = this.assignedJobs.concat(ls);
       this.assignedJobs=ls.concat(this.assignedJobs);
        this.availableJobs = this.availableJobs.filter(f => f.Job.IsSelected == false);
       
    }


    bulkUpload() {
        this.IsCreateFreightOrder = null;
        this.selectedFile = null;
    }
    onFileChange(event) {
        this.file = event.target.files[0];
    }
    onFileUpload() {
        if (!this.file) {
            Declarations.msgerror("Please select file", undefined, undefined);
            return;
        }
        let element: HTMLElement = this.btnCloseBulkModal.nativeElement;
        element.click();
        this.IsLoading = true;
        this.assigncarrierService.upload(this.file, this.IsCreateFreightOrder).subscribe(
            (response: any) => {
                if (response.StatusCode == 1) {
                    Declarations.msgerror(response == null ? 'Failed' : response.StatusMessage, undefined, undefined);
                    this.IsLoading = false;
                } else {
                    Declarations.msgsuccess(response.StatusMessage, undefined, undefined);
                    this.getAssignedCarriers();
                }
                this.file = null;

            }
        );
    }
    public DisplayFreightOrderConfirmationModal() {
        this.IsDisplayLoader = false;
        let element: HTMLElement = this.btnOpenModal.nativeElement;
        element.click();
    }
    public IscreateFreightOrders(IsCreateOrder: boolean) {
        if (!this.isUpdate) {
            this.IsDisplayLoader = true;
            this.assigncarrierService.assignCarriers(this.assignedCarriers)
                .subscribe((response: any) => {
                    if (response.StatusCode == 0) {
                        this.IsSuccess = false;
                        if (IsCreateOrder) {
                            this.assigncarrierService.createFreightOrder(this.assignedCarriers)
                                .subscribe((response: any) => {
                                    if (response.StatusCode == 0) {
                                        Declarations.msgsuccess("Order(s) Assigned Successfully to Carrier", undefined, undefined);
                                    } else if (response.StatusCode == 1) {
                                        Declarations.msgerror(response.StatusMessage, undefined, undefined);
                                    }

                                    this.IsDisplayLoader = false;
                                    let element: HTMLElement = this.btnCloseModal.nativeElement;
                                    element.click();
                                    this._toggleOpened(false);
                                    this.getAssignedCarriers();
                                });
                        } else {
                            let element: HTMLElement = this.btnCloseModal.nativeElement;
                            element.click();
                            this._toggleOpened(false);
                            this.getAssignedCarriers();
                            this.IsDisplayLoader = false;
                        }
                    } else if (response.StatusCode == 1) {
                        Declarations.msgerror(response == null ? 'Failed' : response.StatusMessage, undefined, undefined);
                    }
                });
        }
        else if (this.isUpdate) {
            this.IsDisplayLoader = true;
            this.assigncarrierService.updateAssignedCarrier(this.assignedCarriers[0])// First Update Existing carrier assignment
                .subscribe((response: any) => {
                    this.serviceResponse = response;
                    if (response.StatusCode == 0) {
                        this.IsSuccess = false;
                        this.IsDisplayLoader = false;
                        this.EditFreightOnlyOrders(IsCreateOrder);// Edit FO according to new job assignment
                    }
                    else {
                        this.IsDisplayLoader = false;
                        let element: HTMLElement = this.btnCloseModal.nativeElement;
                        element.click();
                        this._toggleOpened(false);
                        Declarations.msgerror("Carrier Assignment Failed", undefined, undefined);
                    }
                    //this.IsDisplayLoader = false;
                });
        }
    }
    public ValidateIfNewJobAdded(): any[] {
        this.UpdatedJobIds = [];
        var prevAssignedJobIds = this.SelectedJobs.map(function (item) { return item.Id });
        var newlyAssignedJobIds = this.assignedCarriers[0].Jobs.map(function (item) { return item.Job.Id });
        var keys1 = {};
        var keys2 = {};
        var inserted = [];
        var deleted = [];
        prevAssignedJobIds.forEach(function (item) {
            keys1[item] = item;
        });
        newlyAssignedJobIds.forEach(function (item) {
            keys2[item] = item;
        });
        prevAssignedJobIds.forEach(function (item) {
            if (!keys2[item]) {
                deleted.push(item);
            }
        });
        newlyAssignedJobIds.forEach(function (item) {
            if (!keys1[item]) {
                inserted.push(item);
            }
        });
        this.UpdatedJobIds.push({
            InsertedJobs: inserted,
            DeletedJobs: deleted
        });
        return this.UpdatedJobIds;
    }

    public EditFreightOnlyOrders(IsCreateOrder: boolean) {
        this.IsDisplayLoader = true;
        var InsertedJobIds: number[] = [];
        var DeletedJobIds: number[] = [];

        InsertedJobIds = this.UpdatedJobIds[0].InsertedJobs;
        DeletedJobIds = this.UpdatedJobIds[0].DeletedJobs;
        let editfreightOrder = new EditFreightOnlyOrder();
        editfreightOrder.newJobsIds = InsertedJobIds;
        editfreightOrder.removedJobsIds = DeletedJobIds;
        editfreightOrder.CarrierCompanyId = this.assignedCarriers[0].Carrier.Id;
        editfreightOrder.IsCreateOrder = IsCreateOrder;
        this.assigncarrierService.editFreightOnlyOrders(editfreightOrder)
            .subscribe((response: any) => {
                if (response.StatusCode == 0) {
                    Declarations.msgsuccess("Carrier - Location assignment made successfully", undefined, undefined);
                    this.getAssignedCarriers();
                }
                else {
                    Declarations.msgerror("Failed", undefined, undefined);
                }
                this.IsDisplayLoader = false;
                let element: HTMLElement = this.btnCloseModal.nativeElement;
                element.click();
                this._toggleOpened(false);
            });
    }

    public closeAssignedOrdersforCarrier(carrier: Carrier) {
        this.IsSuccess = true;
        var removedJobsIds = carrier.Jobs.map(function (item) { return item.Job.Id });
        let editfreightOrder = new EditFreightOnlyOrder();
        editfreightOrder.newJobsIds = [];
        editfreightOrder.removedJobsIds = removedJobsIds;
        editfreightOrder.CarrierCompanyId = carrier.Carrier.Id;
        editfreightOrder.IsCreateOrder = false;
        this.assigncarrierService.closeAssignedOrdersforCarrier(editfreightOrder)
            .subscribe((response: any) => {
                this.serviceResponse = response;
                if (response.StatusCode == 0) {
                    this.IsSuccess = false;
                } else {

                }
                this.IsLoading = false;
                this._opened = false;
                this.getAssignedCarriers();
            });
    }

}
