import { Component, OnInit, Output, EventEmitter } from '@angular/core';
import { FormGroup, FormBuilder, FormArray, Validators, FormControl } from '@angular/forms';
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
	selector: 'app-create-blend-group',
	templateUrl: './create-blend-group.component.html',
	styleUrls: ['./create-blend-group.component.css']
})

export class CreateBlendGroupComponent implements OnInit {

	public BlendGroupForm: FormGroup;

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
	public finalSubmitData: any = {};
	public groupId: number = 0;
	public isEdit: boolean = false;


	@Output() onSubmitGroupData: EventEmitter<any> = new EventEmitter<any>();
	constructor(private fb: FormBuilder, private orderService: OrderService, private viewGroupService: SharedService) {
		this.BlendGroupForm = this.fb.group({
			OrderBlendedGroups: this.fb.array([]),
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
			enableCheckAll: false,
			selectAllText: 'Select All',
			unSelectAllText: 'UnSelect All',
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
			unSelectAllText: 'UnSelect All',
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
				this.clearBlendGroupForm();
				this.GetGroupDetails();
				this.isEdit = true;
			}
		});
	}

	ngAfterViewInit() {
	}
	get OrderBlendedGroups(): FormArray {
		return this.BlendGroupForm.get("OrderBlendedGroups") as FormArray
	}
	get CustomerPoNumber(): FormControl {
		return this.BlendGroupForm.get("CustomerPoNumber") as FormControl
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
	isDropAllowed = (dragData: any) => {
		return dragData > 500;
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
	onItemDrop(order: any) {
		//this.usedOrderGroupDetails.push(order.dragData);
		//this.OrderBlendedGroups.patchValue(this.usedOrderGroupDetails);
		this.setSelectedOrders(order.dragData);
		this.SetDefaultBlendPercentage();
	}

	setSelectedOrders(order: any) {
		this.OrderBlendedGroups.push(this.buildProduct(order));
		this.orderList = this.orderList.filter(x => x.FuelType !== order.FuelType);
	}
	public onFuelSelect(item: any): void {
		this.loadOrders(this.Customer.Id, this.Job.Id, this.selectedFuelTypeList);
	}

	public onFuelDeSelect(item: any): void {
		this.RemoveOrder(item.Name);
		this.RemoveGroups(item.Name);
	}

	removeBlend(i: number) {
		this.OrderBlendedGroups.removeAt(i);

		var selectedFuelTypes = this.OrderBlendedGroups.controls.map(x => { return x.get('FuelType').value });
		var fuelTypeIds = this.FuelTypeList.map(t => {
			if (selectedFuelTypes.indexOf(t.Name) > -1)
				return t.Id;
		});
		var loadedFuelType = this.selectedFuelTypeList.filter(x => fuelTypeIds.indexOf(x) == -1);
		this.SetDefaultBlendPercentage();
		this.loadOrders(this.Customer.Id, this.Job.Id, loadedFuelType);
	}

	RemoveOrder(fuelTypeName: string) {
		this.orderList = this.orderList.filter(obj => obj.FuelType !== fuelTypeName);
	}

	loadOrders(_customerId: number, _jobId: number, _fuelTypeIds: number[]): void {
		this.orderService.getFilteredOrdersList(_customerId, _jobId, _fuelTypeIds, this.groupId).subscribe((data: OrderGroupDetails[]) => {
			var existingOrderList = this.OrderBlendedGroups.controls.map(x => { return x.get('OrderId').value });
			this.orderList = data.filter(ele => existingOrderList.indexOf(ele.OrderId) == -1);
		});
	}

	loadFuelType(_customerId: number, _jobId: number): void {
		this.orderService.getFuelTypesList(_customerId, _jobId).subscribe((data: DropdownItem[]) => {
			this.FuelTypeList = data;
		});
	}

	RemoveGroups(_FuelType: string): void {
		//this.usedOrderGroupDetails = this.usedOrderGroupDetails.filter(x => x.FuelType !== _FuelType);
		this.OrderBlendedGroups.setValue(this.OrderBlendedGroups.controls.filter(group => group.get('FuelType').value !== _FuelType));
		this.SetDefaultBlendPercentage();
	}

	SetDefaultBlendPercentage(): void {
		var totalFuelTypes = this.OrderBlendedGroups.length;
		var equalPercent = (100 / totalFuelTypes).toFixed(2);
		this.OrderBlendedGroups.controls.forEach((x: FormGroup) => { x.get('BlendPercentage').setValue(equalPercent) });
	}
	public OnCustomerSelect(customer: any) {
		this.SelectedJob = '';
		this.SelectedFuelTypes = [];
		this.OrderBlendedGroups.clear();
		this.orderService.getCommonJobList(customer.Id).subscribe(data => this.JobList = data);
	}

	public OnCustomerDeSelect(customer: any) {
		this.SelectedCustomer = '';
		this.JobList = [];
		this.SelectedJob = '';
		this.FuelTypeList = [];
		this.SelectedFuelTypes = [];
		this.orderList = [];
		this.OrderBlendedGroups.clear();
	}

	public OnJobSelect(job: any) {
		this.orderList = [];
		this.OrderBlendedGroups.clear();
		this.SelectedFuelTypes = [];
		this.loadFuelType(this.Customer.Id, job.Id);
	}

	public OnJobDeSelect(job: any) {
		this.FuelTypeList = [];
		this.SelectedFuelTypes = [];
		this.orderList = [];
		this.OrderBlendedGroups.clear();
	}

	public OnSubmit(): void {
		var blendValue: number[] = this.OrderBlendedGroups.controls.map(x => { return parseFloat(x.get('BlendPercentage').value) });
		var sum = 0;
		blendValue.forEach(x => sum += x);
		if (sum != 100) {
			Declarations.msgerror('Total percentage sum should be 100', undefined, undefined);
			this.BlendGroupForm.setErrors({ 'invalid': true });
		}
		this.finalSubmitData = {
			Id: this.isEdit ? this.groupId : 0,
			OrderList: this.OrderBlendedGroups.value,
			GroupPoNumber: this.CustomerPoNumber.value,
			BuyerCompanyId: IsSupplierCompany ? this.Customer.Id : currentUserCompanyId,
			SupplierCompanyId: IsSupplierCompany ? currentUserCompanyId : this.Customer.Id,
			JobId: this.Job.Id,
			GroupType: OrderGroupType.Blend
		};

		if (this.BlendGroupForm.valid) {
			this.IsLoading = true;
			if (this.isEdit) {
				this.orderService.postEditGroup(this.finalSubmitData).subscribe(data => {
					this.IsLoading = false;
					if (data.StatusCode == 0) {
						Declarations.msgsuccess(data.StatusMessage, undefined, undefined);
						this.clearBlendGroupForm();
						closeSlidePanel();
						this.onSubmitGroupData.emit();
					}
					else {
						Declarations.msgerror(data == null ? 'Failed' : data.StatusMessage, undefined, undefined);
					}
				});
			}
			else {
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
			var _fuelTypes = group.GroupDetails.OrderList.map(x => x.Order.FuelType);
			this.SelectedFuelTypes = this.FuelTypeList.filter(ele => _fuelTypes.indexOf(ele.Name) > -1);
			for (var i = 0; i < group.GroupDetails.OrderList.length; i++) {
				var orderDetail: OrderGroupDetails = group.GroupDetails.OrderList[i];
				var orderToMove: OrderGroupDetails[] = this.orderList.filter(t => t.OrderId == orderDetail.OrderId);
				this.setSelectedOrders(orderToMove[0]);
			}
		});
	}

	getCustomerFromSupplier(id: number) {
		return this.CustomerList.filter(e => e.Id == id);
	}

	public clearBlendGroupForm(): void {
		if (this.SelectedCustomer != null) {
			this.OnCustomerDeSelect(this.SelectedCustomer);
		}
		this.CustomerPoNumber.setValue("");
		this.isEdit = false;
	}
}
