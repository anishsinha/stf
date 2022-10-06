import { Component, EventEmitter, Input, OnInit, Output, OnDestroy } from '@angular/core';
import { IDropdownSettings } from 'ng-multiselect-dropdown';
import { DeliveryRequestViewModel } from '../../models/DispatchSchedulerModels';
import { Subject } from 'rxjs';
import { DropdownItemExt, DropdownItem } from '../../../statelist.service';
import { FormGroup, FormBuilder } from '@angular/forms';
import { DispatcherService } from '../../service/dispatcher.service';
import { CarrierService } from '../../service/carrier.service';
import { Declarations } from '../../../declarations.module';
import { ScheduleBuilderService } from '../../service/schedule-builder.service';
import { DataService } from 'src/app/services/data.service';
import { BrokeredDrCarrierStatus, TfxModule } from 'src/app/app.enum';
import { LoadPriorities } from 'src/app/app.constants';

@Component({
    selector: 'app-assigned-by-me',
    templateUrl: './assigned-by-me.component.html',
    styleUrls: ['./assigned-by-me.component.scss']
})
export class AssignedByMeComponent implements OnInit, OnDestroy {
    @Input() regionId: string;
    @Input() SelectedDate: string;
    public MustGoDrs: any = [];
    public ShouldGoDrs: any = [];
    public CouldGoDrs: any = [];
    public dtMustGoOptions: any = {};
    public dtShouldGoOptions: any = {};
    public dtCouldGoOptions: any = {};
    public dtMustGoTrigger: Subject<any> = new Subject();
    public dtShouldGoTrigger: Subject<any> = new Subject();
    public dtCouldGoTrigger: Subject<any> = new Subject();
    public customerList: any = [];
    public locationList: any = [];
    public priorityList: any = [];
    public SelectedLocations: any = [];
    public SelectedCustomers: any = [];
    public SelectedPriorities: any = [];
    public FilterForm: FormGroup;
    public showMustGo: boolean = true;
    public showShouldGo: boolean = true;
    public showCouldGo: boolean = true;
    public multiDropdownSettings: IDropdownSettings;
    public brokeredDrRequestedByMe: DeliveryRequestViewModel[] = [];
    public allBrokeredDrRequestedByMe: DeliveryRequestViewModel[] = [];
    public requestToUpdate: DeliveryRequestViewModel = new DeliveryRequestViewModel(false);
    public IsLoading: boolean = false;

    constructor(private fb: FormBuilder, private dispatcherService: DispatcherService, private sbService: ScheduleBuilderService, private dataService: DataService, private carrierService: CarrierService) { }

    ngOnInit() {
        this.priorityList = LoadPriorities;

        this.multiDropdownSettings = {
            singleSelection: false,
            idField: 'Id',
            textField: 'Name',
            selectAllText: 'Select All',
            unSelectAllText: 'UnSelect All',
            itemsShowLimit: 1,
            allowSearchFilter: true,
            enableCheckAll: false
        };
        this.initializeFilterForm();
        this.initializeMustGo();
        this.initializeShouldGo();
        this.initializeCouldGo();
        this.GetFilters();
    }

    ngOnDestroy(): void {
        this.dtCouldGoTrigger.unsubscribe();
        this.dtShouldGoTrigger.unsubscribe();
        this.dtMustGoTrigger.unsubscribe();
    }

    public initializeFilterForm() {
        this.FilterForm = this.fb.group({
            SelectedCustomers: this.fb.control([]),
            SelectedLocations: this.fb.control([]),
            SelectedPriorities: this.fb.control([])
        });
    }

    setGridData() {
        this.MustGoDrs = this.brokeredDrRequestedByMe.filter(t => t.Priority == 1);
        this.ShouldGoDrs = this.brokeredDrRequestedByMe.filter(t => t.Priority == 2);
        this.CouldGoDrs = this.brokeredDrRequestedByMe.filter(t => t.Priority == 3);
        this.getCustomers();
        this.getLocations();
    }

    getCustomers() {
        this.customerList = this.brokeredDrRequestedByMe.map(item => {
            const customer = new DropdownItemExt();

            customer.Id = item.CustomerCompany;
            customer.Name = item.CustomerCompany;

            return customer;
        });
        this.customerList = this.customerList.filter((loc, i, arr) => {
            return arr.indexOf(arr.find(t => t.Id === loc.Id)) === i;
        });
    }

    getLocations() {
        this.locationList = this.brokeredDrRequestedByMe.map(item => {
            const location = new DropdownItem();

            location.Id = item.JobId;
            location.Name = item.JobName;

            return location;
        });
        this.locationList = this.locationList.filter((loc, i, arr) => {
            return arr.indexOf(arr.find(t => t.Id === loc.Id)) === i;
        });
    }

    public recallBrokeredDrRequest(delReq: DeliveryRequestViewModel) {
        this.requestToUpdate = Object.assign({}, delReq);
        let element = document.getElementById("recallBrokeredDrBtn");
        element ? element.click() : null;
    }

    onCustomerChange() {
        this.SelectedCustomers = this.FilterForm.get('SelectedCustomers').value;
        var selectedCustomers = this.SelectedCustomers.map(t => t.Name);
        if (this.SelectedCustomers.length > 0) {
            this.locationList = this.brokeredDrRequestedByMe.filter(t => selectedCustomers.some(t1 => t1 == t.CustomerCompany)).map(item => {
                const location = new DropdownItem();

                location.Id = item.JobId;
                location.Name = item.JobName;

                return location;
            });
            this.locationList = this.locationList.filter((loc, i, arr) => {
                return arr.indexOf(arr.find(t => t.Id === loc.Id)) === i;
            });
        }
        else {
            this.getLocations();
        }
    }

    recallBrokeredDr(dr: any): void {
        this.IsLoading = true;
        this.sbService.recallDrFromCarrier(dr).subscribe(data => {
            this.IsLoading = false;
            if (data.StatusCode != 1) {
                var recalledDrs = [dr];
                if (dr.BlendedGroupId) {
                    recalledDrs = this.allBrokeredDrRequestedByMe.filter(t => t.BlendedGroupId == dr.BlendedGroupId);
                }
                recalledDrs.forEach(t => { t.CarrierStatus = BrokeredDrCarrierStatus.Recalled; t.WindowMode = 1; t.QueueMode = 1; t.Compartments = t.Compartments || []; });
                Declarations.msgsuccess(data.StatusMessage, undefined, undefined);
                Declarations.hideModal('#recallBrokeredDrModal');
                this.dataService.setRestoreDeletedRequestSubject(recalledDrs);
                this.getBrokeredDrAssignedByMe(this.regionId, this.SelectedDate);
                this.dataService.setDrQueueChangesForFilter(true);
            } else {
                Declarations.msgerror(data.StatusMessage, undefined, undefined);
            }
        });
    }

    getBrokeredDrAssignedByMe(regionId: string, selectedDate?: string) {
        this.IsLoading = true;
        this.carrierService.getBrokeredDrAssignedByMe(regionId, selectedDate).subscribe(drs => {
            this.IsLoading = false;
            this.allBrokeredDrRequestedByMe = drs || [];
            //group DRs based on BlendedDRs
            this.brokeredDrRequestedByMe = drs.filter(x => x.IsBlendedRequest == false || x.IsBlendedDrParent == true);
            this.getCustomers();
            this.getLocations();
            this.ApplyFilters(true);
        });
    }

    onLocationChange() {
        this.SelectedLocations = this.FilterForm.get('SelectedLocations').value;
    }

    onPriorityChange() {
        this.SelectedPriorities = this.FilterForm.get('SelectedPriorities').value;
    }

    openAssignedByMePanel() {
        this.getBrokeredDrAssignedByMe(this.regionId, this.SelectedDate);
    }

    ResetFilters() {
        this.SelectedCustomers = [];
        this.SelectedLocations = [];
        this.SelectedPriorities = [];
        this.initializeFilterForm();
        this.showCouldGo = this.showMustGo = this.showShouldGo = true;
        this.MustGoDrs = this.brokeredDrRequestedByMe.filter(t => t.Priority == 1);
        this.ShouldGoDrs = this.brokeredDrRequestedByMe.filter(t => t.Priority == 2);
        this.CouldGoDrs = this.brokeredDrRequestedByMe.filter(t => t.Priority == 3);
        this.SaveFilters();
    }

    ApplyFilters(isFromGetFilter: boolean = false) {
        this.showMustGo = this.SelectedPriorities.length == 0 || this.SelectedPriorities.some(t => t.Id == 1);
        this.showShouldGo = this.SelectedPriorities.length == 0 || this.SelectedPriorities.some(t => t.Id == 2);
        this.showCouldGo = this.SelectedPriorities.length == 0 || this.SelectedPriorities.some(t => t.Id == 3);


        if (this.SelectedCustomers.length > 0) {
            this.MustGoDrs = this.brokeredDrRequestedByMe.filter(t => t.Priority == 1 && this.SelectedCustomers.some(t1 => t1.Id == t.CustomerCompany));
            this.ShouldGoDrs = this.brokeredDrRequestedByMe.filter(t => t.Priority == 2 && this.SelectedCustomers.some(t1 => t1.Id == t.CustomerCompany));
            this.CouldGoDrs = this.brokeredDrRequestedByMe.filter(t => t.Priority == 3 && this.SelectedCustomers.some(t1 => t1.Id == t.CustomerCompany));
            if (this.SelectedLocations.length > 0) {
                this.MustGoDrs = this.MustGoDrs.filter(t => this.SelectedLocations.some(t1 => t1.Id == t.JobId));
                this.ShouldGoDrs = this.ShouldGoDrs.filter(t => this.SelectedLocations.some(t1 => t1.Id == t.JobId));
                this.CouldGoDrs = this.CouldGoDrs.filter(t => this.SelectedLocations.some(t1 => t1.Id == t.JobId));
            }
        }
        else if (this.SelectedLocations.length > 0) {
            this.MustGoDrs = this.brokeredDrRequestedByMe.filter(t => t.Priority == 1 && this.SelectedLocations.some(t1 => t1.Id == t.JobId));
            this.ShouldGoDrs = this.brokeredDrRequestedByMe.filter(t => t.Priority == 2 && this.SelectedLocations.some(t1 => t1.Id == t.JobId));
            this.CouldGoDrs = this.brokeredDrRequestedByMe.filter(t => t.Priority == 3 && this.SelectedLocations.some(t1 => t1.Id == t.JobId));
        }
        else {
            this.MustGoDrs = this.brokeredDrRequestedByMe.filter(t => t.Priority == 1);
            this.ShouldGoDrs = this.brokeredDrRequestedByMe.filter(t => t.Priority == 2);
            this.CouldGoDrs = this.brokeredDrRequestedByMe.filter(t => t.Priority == 3);
        }
        if (!isFromGetFilter) {
            this.SaveFilters();
        }
    }

    public GetFilters() {
        this.dispatcherService.getFilters(TfxModule.AssignedByMeDeliveryRequests).subscribe(res => {
            if (res && res.length > 0) {
                var filterData = JSON.parse(res);
                this.FilterForm.patchValue(filterData);
                this.SelectedCustomers = filterData.SelectedCustomers;
                this.SelectedLocations = filterData.SelectedLocations;
                this.SelectedPriorities = filterData.SelectedPriorities;
                this.getCustomers();
                this.getLocations();
                this.ApplyFilters(true);
            }
            else {
                this.setGridData();
            }
        });
    }

    public SaveFilters() {
        var filterData = this.FilterForm.value;
        var filterModel = JSON.stringify(filterData);
        this.dispatcherService.postFiltersData(TfxModule.AssignedByMeDeliveryRequests, filterModel).subscribe(res => {
            if (res) {

            }
        });
    }

    initializeMustGo() {

        let exportInvitedColumns = { columns: ':visible' };
        this.dtMustGoOptions = {
            dom: '<"html5buttons"B>lTfgitp',
            buttons: [
                { extend: 'colvis' },
                { extend: 'copy', exportOptions: exportInvitedColumns },
                { extend: 'csv', title: 'MustGo', exportOptions: exportInvitedColumns },
                { extend: 'pdf', title: 'MustGo', orientation: 'landscape', exportOptions: exportInvitedColumns },
                { extend: 'print', exportOptions: exportInvitedColumns }
            ],
            pagingType: 'first_last_numbers',
            pageLength: 10,
            fixedHeader: false,
            lengthMenu: [[10, 25, 50, 100, -1], [10, 25, 50, 100, "All"]]
        };
    }

    initializeCouldGo() {
        let exportInvitedColumns = { columns: ':visible' };
        this.dtCouldGoOptions = {
            dom: '<"html5buttons"B>lTfgitp',
            buttons: [
                { extend: 'colvis' },
                { extend: 'copy', exportOptions: exportInvitedColumns },
                { extend: 'csv', title: 'CouldGo', exportOptions: exportInvitedColumns },
                { extend: 'pdf', title: 'CouldGo', orientation: 'landscape', exportOptions: exportInvitedColumns },
                { extend: 'print', exportOptions: exportInvitedColumns }
            ],
            pagingType: 'first_last_numbers',
            pageLength: 10,
            fixedHeader: false,
            lengthMenu: [[10, 25, 50, 100, -1], [10, 25, 50, 100, "All"]]
        };
    }

    initializeShouldGo() {
        let exportInvitedColumns = { columns: ':visible' };
        this.dtShouldGoOptions = {
            dom: '<"html5buttons"B>lTfgitp',
            buttons: [
                { extend: 'colvis' },
                { extend: 'copy', exportOptions: exportInvitedColumns },
                { extend: 'csv', title: 'ShouldGo', exportOptions: exportInvitedColumns },
                { extend: 'pdf', title: 'ShouldGo', orientation: 'landscape', exportOptions: exportInvitedColumns },
                { extend: 'print', exportOptions: exportInvitedColumns }
            ],
            pagingType: 'first_last_numbers',
            fixedHeader: false,
            pageLength: 10,
            lengthMenu: [[10, 25, 50, 100, -1], [10, 25, 50, 100, "All"]]
        };

    }
}
