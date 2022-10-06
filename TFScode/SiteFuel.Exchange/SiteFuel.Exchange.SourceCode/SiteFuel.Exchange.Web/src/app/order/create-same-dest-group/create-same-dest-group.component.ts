import { Component, OnInit, Output, EventEmitter } from '@angular/core';
import { FormGroup, FormBuilder, Validators, FormControl } from '@angular/forms';
import { DropdownItem } from '../../statelist.service';
import { OrderGroupDetails } from '../models/models';
import { OrderService } from '../services/order.service';
import { Declarations } from '../../declarations.module';
import { SharedService } from '../services/shared.service';
import { OrderGroupType } from 'src/app/app.enum';
declare var currentUserCompanyId: number;
declare var IsSupplierCompany: boolean;
declare function closeSlidePanel(): any;

@Component({
    selector: 'app-create-same-dest-group',
    templateUrl: './create-same-dest-group.component.html',
    styleUrls: ['./create-same-dest-group.component.css']
})
export class CreateSameDestGroupComponent implements OnInit {

    public SameDestGroupForm: FormGroup;

    public FuelTypeList = [];
    public SelectedFuelTypes = [];
    public CustomerList = [];
    public SelectedCustomer: any;
    public JobList = [];
    public SelectedJob: any;
    public IsLoading: boolean = false;
    public configSettings = {};
    public FuelDdlSettings = {};
    public SingleDdlSettings = {};
    public orderList: OrderGroupDetails[] = [];
    public orderListSelected: OrderGroupDetails[] = [];
    public finalSubmitData: any = {};
    public groupId: number = 0;
    public isEdit: boolean = false;

    @Output() onSubmitGroupData: EventEmitter<any> = new EventEmitter<any>();
    constructor(private fb: FormBuilder, private orderService: OrderService, private viewGroupService: SharedService) {
        this.SameDestGroupForm = this.fb.group({
            CustomerPoNumber: this.fb.control('')
        });
    }

    ngOnInit() {
        this.configSettings = {
            displayKey: 'Name',
            search: true,

        }
        this.FuelDdlSettings = {
            singleSelection: false,
            idField: 'Id',
            textField: 'Name',
            enableCheckAll: true,
            selectAllText: 'Select All',
            unSelectAllText: 'Unselect All',
            itemsShowLimit: 1,
            allowSearchFilter: true,
            closeDropDownOnSelection: true
        };
        this.SingleDdlSettings = {
            singleSelection: true,
            idField: 'Id',
            textField: 'Name',
            enableCheckAll: false,
            selectAllText: 'Select All',
            unSelectAllText: 'Unselect All',
            itemsShowLimit: 1,
            allowSearchFilter: true
        };

        if (IsSupplierCompany) {
            this.orderService.getCustomerList().subscribe(data => this.CustomerList = data);
        }
        else {
            this.orderService.getSupplierList().subscribe(data => this.CustomerList = data);
        }

        this.viewGroupService.currentGroup.subscribe(id => {
            this.groupId = id;
            if (id > 0) {
                this.clearDestGroupForm();
                this.GetGroupDetails();
                this.isEdit = true;
            }
        });
    }
    OrderClicked(orderData: OrderGroupDetails) {
        let obj = this.orderList.find(o => o.OrderId == orderData.OrderId);
        obj.IsOrderSelected = !obj.IsOrderSelected;
        this.orderListSelected = this.orderList.filter(t => t.IsOrderSelected == true);
    }
    get CustomerPoNumber(): FormControl {
        return this.SameDestGroupForm.get("CustomerPoNumber") as FormControl
    }
    get Job(): DropdownItem {
        return this.SelectedJob[0];
    }
    get Customer(): DropdownItem {
        return this.SelectedCustomer[0];
    }
    get selectedFuelTypeList(): number[] {
        return this.SelectedFuelTypes.map(x => x.Id);
    }


    buildProduct(model: OrderGroupDetails) {
        return this.fb.group({
            GroupId: this.fb.control(model.GroupId),
            OrderId: this.fb.control(model.OrderId),
            TfxPoNumber: this.fb.control(model.TfxPoNumber),
            FuelType: this.fb.control(model.FuelType),
            Quantity: this.fb.control(model.Quantity),
            DisplayPrice: this.fb.control(model.DisplayPrice),
            BlendPercentage: this.fb.control(model.BlendPercentage, [Validators.required, Validators.pattern(/^[0-9]\d*(\.\d+)?$/)]),
        });
    }

    public onFuelSelect(item: any): void {
        this.loadOrders(this.Customer.Id, this.Job.Id, this.selectedFuelTypeList);
    }

    public onAllFuelDeSelect(items: any): void {
        this.orderList = [];
    }

    public onFuelDeSelect(item: any): void {
        this.RemoveOrders(item.Name);
    }

    loadOrders(_customerId: number, _jobId: number, _fuelTypeIds: number[]): void {
        this.orderService.getFilteredOrdersList(_customerId, _jobId, _fuelTypeIds, this.groupId).subscribe((data: OrderGroupDetails[]) => {
            this.orderList = data;
            for (var i = 0; i < this.orderListSelected.length; i++) {
                this.orderList.filter(x => x.OrderId == this.orderListSelected[i].OrderId).forEach(t => t.IsOrderSelected = this.orderListSelected[i].IsOrderSelected);
            }
        });
    }

    loadFuelType(_customerId: number, _jobId: number): void {
        this.orderService.getFuelTypesList(_customerId, _jobId).subscribe((data: DropdownItem[]) => {
            this.FuelTypeList = data;
        });
    }

    RemoveOrders(_FuelType: string): void {
        this.orderList = this.orderList.filter(t => t.FuelType != _FuelType);
        this.orderListSelected = this.orderListSelected.filter(t => t.FuelType != _FuelType);
    }
    public OnCustomerSelect(customer: any) {
        this.SelectedJob = '';
        this.SelectedFuelTypes = [];
        this.orderService.getCommonJobList(customer.Id).subscribe(data => this.JobList = data);
    }

    public OnCustomerDeSelect(customer: any) {
        this.SelectedCustomer = '';
        this.JobList = [];
        this.SelectedJob = '';
        this.FuelTypeList = [];
        this.SelectedFuelTypes = [];
        this.orderList = [];
        this.orderListSelected = [];
    }

    public OnJobSelect(job: any) {
        this.orderList = [];
        this.SelectedFuelTypes = [];
        this.orderListSelected = [];
        this.loadFuelType(this.Customer.Id, job.Id);
    }

    public OnJobDeSelect(job: any) {
        this.FuelTypeList = [];
        this.SelectedFuelTypes = [];
        this.orderList = [];
        this.orderListSelected = [];
    }

    public OnSubmit(): void {

        if (this.orderListSelected.length < 2) {
            Declarations.msgerror('Group must contain at least 2 orders', undefined, undefined);
            this.SameDestGroupForm.setErrors({ 'invalid': true });
        }

        this.finalSubmitData = {
            Id: this.isEdit ? this.groupId : 0,
            OrderList: this.orderListSelected,
            GroupPoNumber: this.CustomerPoNumber.value,
            BuyerCompanyId: IsSupplierCompany ? this.Customer.Id : currentUserCompanyId,
            SupplierCompanyId: IsSupplierCompany ? currentUserCompanyId : this.Customer.Id,
            JobId: this.Job.Id,
            GroupType: OrderGroupType.MultiProducts
        };
        
        if (this.SameDestGroupForm.valid) {
            this.IsLoading = true;
            if (this.isEdit)
            {
                this.orderService.postEditGroup(this.finalSubmitData).subscribe(data => {
                    this.IsLoading = false;
                    if (data.StatusCode == 0) {
                        Declarations.msgsuccess(data.StatusMessage, undefined, undefined);
                        this.clearDestGroupForm();
                        closeSlidePanel();
                        this.onSubmitGroupData.emit();
                    }
                    else {
                        Declarations.msgerror(data == null ? 'Failed' : data.StatusMessage, undefined, undefined);
                    }
                });
            }
            else
            {
                this.orderService.postCreateGroup(this.finalSubmitData).subscribe(data => {
                    this.IsLoading = false;
                    if (data.StatusCode == 0) {
                        Declarations.msgsuccess(data.StatusMessage, undefined, undefined);
                        closeSlidePanel();
                        this.onSubmitGroupData.emit();
                    }
                    else {
                        Declarations.msgerror(data == null ? 'Failed' : data.StatusMessage, undefined, undefined);
                    }
                });
            }
        }
    }

    GetGroupDetails(): void {
        this.orderService.getBlendGroupDetails(this.groupId).subscribe(group => {
            this.JobList = group.Jobs;
            this.FuelTypeList = group.FuelTypes;
            this.orderList = group.FilteredOrders;
            this.CustomerPoNumber.setValue(group.GroupDetails.GroupPoNumber);
            this.SelectedCustomer = IsSupplierCompany ? this.getCustomerFromSupplier(group.GroupDetails.BuyerCompanyId) : this.getCustomerFromSupplier(group.GroupDetails.SupplierCompanyId);
            this.SelectedJob = this.JobList.filter(ele => ele.Id == group.GroupDetails.JobId);
            var _fuelTypes: string[] = group.GroupDetails.OrderList.map(x => x.Order.FuelType);
            this.SelectedFuelTypes = this.FuelTypeList.filter(ele => _fuelTypes.indexOf(ele.Name) > -1);
            this.orderList.filter(ele => _fuelTypes.indexOf(ele.FuelType) > -1).forEach(t => t.IsOrderSelected = true);
            this.orderListSelected = this.orderList.filter(x => x.IsOrderSelected == true);
            //for (var i = 0; i < group.GroupDetails.OrderList.length; i++) {
            //    var orderDetail: OrderGroupDetails = group.GroupDetails.OrderList[i];
            //    var orderToMove: OrderGroupDetails[] = this.orderList.filter(t => t.OrderId == orderDetail.OrderId);
            //    this.setSelectedOrders(orderToMove[0]);
            //}
        });
    }

    getCustomerFromSupplier(id: number) {
        return this.CustomerList.filter(e => e.Id == id);
    }

    public clearDestGroupForm(): void {
        if (this.SelectedCustomer != null) {
            this.OnCustomerDeSelect(this.SelectedCustomer);
        }
        this.CustomerPoNumber.setValue("");
        this.isEdit = false;
    }

    //setSelectedOrders(order: OrderGroupDetails) {
    //    order.IsOrderSelected = true;
    //    this.orderListSelected.push(order);
    //    this.orderList.filter(x => x.FuelType == order.FuelType).forEach(x1 => x1.IsOrderSelected = true);
    //}
}
