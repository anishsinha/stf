import { Component, OnInit } from '@angular/core';
import { FormGroup, FormBuilder, Validators, FormArray, AbstractControl, ValidationErrors } from '@angular/forms';
import { OrderService } from './services/order.service';
import { OrderDetailModel, OrderList, CurrentUser, OrderGroupViewModel } from '../order/models/OrderDetail';
import { Declarations } from '../declarations.module';
import { SharedService } from './services/shared.service';
import * as moment from 'moment';
import { RegExConstants } from '../app.constants';
declare var currentUserCompanyId: number;
declare var IsSupplierCompany: boolean;
declare var currentUserCompanyName: string;

@Component({
	selector: 'app-term-pricing-contract',
	templateUrl: './term-pricing-contract.component.html',
	styleUrls: ['./term-pricing-contract.component.css']
})
export class TermPricingContractComponent implements OnInit {
	public TermPricingForm: FormGroup;
	public OrderGroupModel: OrderGroupViewModel;
	public CustomerList = [];
	public SupplierList = [];
	public FuelGroupList = [{ Id: 1, Name: "Gasoline" }, { Id: 2, Name: "Diesel" }];
	public JobList = [];
	public SelectedFuelGroup = [{ Id: 1, Name: "Gasoline" }];
	public SelectedCustomer = [];
	public SelectedSupplier = [];
	public groupId: number;
	public SelectedJob = [];
	public DdlSettings = {};
	public Orders: OrderDetailModel[];
	public MinStartDate = new Date();
	public MaxStartDate = new Date();
	public NextRenewalDate: string;
	public IsLoading: boolean = true;
	OrderList: OrderList[] = [];
	public DefaultTier: OrderList = { Order: new OrderDetailModel(), OrderId: null, MinVolume: null, MaxVolume: null };
	public CurrentUser: CurrentUser = { IsBuyerCompany: !IsSupplierCompany, IsSupplierCompany: IsSupplierCompany };
	public DisplayDate: string;
	public emptyOrder: OrderDetailModel = {
		OrderId: null,
		TfxPoNumber: null,
		FuelType: null,
		Quantity: null,
		DisplayPrice: null
	};

	constructor(private fb: FormBuilder, private orderService: OrderService, private viewGroupService: SharedService) {
	}

	ngOnInit() {
		this.DisplayDate = moment(new Date()).format('MM/DD/YYYY');
		this.NextRenewalDate = moment(new Date()).add(1, 'months').startOf('month').format('MM/DD/YYYY');
		this.MaxStartDate.setFullYear(this.MaxStartDate.getFullYear() + 10);
		this.TermPricingForm = this.fb.group({
			GroupType: this.fb.control('2', [Validators.required]),
			BuyerCompanyId: this.fb.control('', [Validators.required]),
			SupplierCompanyId: this.fb.control('', [Validators.required]),
			ProductType: this.fb.control(1, [Validators.required]),
			JobId: this.fb.control('', [Validators.required]),
			StartDate: this.fb.control(this.DisplayDate, [Validators.required]),
			RenewalFrequency: this.fb.control('1', [Validators.required]),
			RenewalPeriod: this.fb.control('Monthly'),
			RenewalCount: this.fb.control('', [Validators.required, Validators.pattern(RegExConstants.Integer)]),
			GroupPoNumber: this.fb.control(''),
			OrderList: this.fb.array([]),
		});
		this.DdlSettings = {
			singleSelection: true,
			idField: 'Id',
			textField: 'Name',
			closeDropDownOnSelection: true,
			enableCheckAll: false,
			selectAllText: 'Select All',
			unSelectAllText: 'UnSelect All',
			itemsShowLimit: 1,
			allowSearchFilter: true
		};
		this.viewGroupService.currentGroup.subscribe(id => this.groupId = id);

		if (IsSupplierCompany) {
			this.SupplierList = [{ Id: currentUserCompanyId, Name: currentUserCompanyName }];
			this.SelectedSupplier = [{ Id: currentUserCompanyId, Name: currentUserCompanyName }];
			this.TermPricingForm.get('SupplierCompanyId').patchValue(currentUserCompanyId);
			this.GetCustomerList();
		}
		else {
			this.CustomerList = [{ Id: currentUserCompanyId, Name: currentUserCompanyName }];
			this.SelectedCustomer = [{ Id: currentUserCompanyId, Name: currentUserCompanyName }];
			this.TermPricingForm.get('BuyerCompanyId').patchValue(currentUserCompanyId);
			this.GetSupplierList();
		}

		if (this.groupId == 0) {
			this.addNewTier(this.DefaultTier);
		}
		this.IsLoading = false;
	}


	ngAfterViewInit() {
		if (this.groupId != null && this.groupId != undefined && this.groupId > 0) {
			this.getDefaultGroupDetail(this.groupId);
		}
	}

	private getDefaultGroupDetail(_groupId: number) {
		this.orderService.getGroupDetails(_groupId)
			.subscribe(response => {
				this.OrderGroupModel = response;
				this.initFormData(response);
			});
	}

	initFormData(model: OrderGroupViewModel): void {
		if (model != null && model != undefined && model.Id > 0) {
			this.TermPricingForm.patchValue({
				GroupType: model.GroupType,
				BuyerCompanyId: model.BuyerCompanyId,
				SupplierCompanyId: model.SupplierCompanyId,
				ProductType: model.ProductType,
				JobId: model.JobId,
				StartDate: model.StartDate,
				RenewalFrequency: model.RenewalFrequency,
				RenewalPeriod: 'Monthly',
				RenewalCount: model.RenewalCount,
				GroupPoNumber: model.GroupPoNumber,
				OrderList: model.OrderList
			});
			this.SelectedCustomer = this.CustomerList.filter(function (item) { return item.Id == model.BuyerCompanyId });
			this.SelectedSupplier = this.SupplierList.filter(function (item) { return item.Id == model.SupplierCompanyId });
			this.SelectedJob = this.JobList.filter(function (item) { return item.Id == model.JobId });
			this.SelectedFuelGroup = this.FuelGroupList.filter(function (item) { return item.Id == model.ProductType });
			//this.Tiers = model.OrderList;
			this.Orders = this.Orders.filter(function (item) {
				return model.OrderList.filter(function (t) { return t.OrderId == item.OrderId }).length == 0;
			})
		}
	}

	public GetCustomerList() {
		this.orderService.getCustomerList().subscribe(data => {
			this.CustomerList = data;
			if (data.length > 0) {
				this.SelectedCustomer = [data[0]];
				this.TermPricingForm.get('BuyerCompanyId').patchValue(data[0].Id);
				this.OnCustomerSelect(data[0]);
			}
		});
	}

	public GetSupplierList() {
		this.orderService.getSupplierList().subscribe(data => {
			this.SupplierList = data;
			if (data.length > 0) {
				this.SelectedSupplier = [data[0]];
				this.TermPricingForm.get('SupplierCompanyId').patchValue(data[0].Id);
				this.OnSupplierSelect(data[0]);
			}
		});
	}

	public OnCustomerSelect(customer: any) {
		this.TermPricingForm.get('BuyerCompanyId').patchValue(customer.Id);
		var productType = this.TermPricingForm.get('ProductType').value;
		var selectedsupplier = this.TermPricingForm.get('SupplierCompanyId').value;
		var formOrderList = this.TermPricingForm.get('OrderList') as FormArray;
		if (customer != null && customer != undefined && customer.Id > 0 && productType > 0 && selectedsupplier > 0) {
			this.orderService.getJobListByFuelGroupType(customer.Id, selectedsupplier, productType).subscribe(data => {
				this.JobList = data;
				if (data.length > 0) {
					this.SelectedJob = [data[0]];
					this.TermPricingForm.get('JobId').patchValue(data[0].Id);
					this.OnJobSelect(data[0]);
				} else {
					this.SelectedJob = [];
					formOrderList.clear();
					this.OrderList = [];
					this.addNewTier(this.DefaultTier);
					this.TermPricingForm.get('JobId').patchValue(null);
					this.Orders = [];
				}
			});
		}
		else {
			this.JobList = [];
			this.SelectedJob = [];
			formOrderList.clear();
			this.OrderList = [];
			this.addNewTier(this.DefaultTier);
			this.TermPricingForm.get('JobId').patchValue(null);
			this.Orders = [];
		}
	}

	public OnCustomerDeSelect(customer: any) {
		this.TermPricingForm.get('BuyerCompanyId').patchValue(null);
		this.JobList = [];
		this.SelectedJob = [];
		this.Orders = [];
		var formOrderList = this.TermPricingForm.get('OrderList') as FormArray;
		formOrderList.clear();
		this.OrderList = [];
		this.addNewTier(this.DefaultTier);
		this.TermPricingForm.get('JobId').patchValue(null);
	}

	public OnSupplierSelect(supplier: any) {
		this.TermPricingForm.get('SupplierCompanyId').patchValue(supplier.Id);
		var productType = this.TermPricingForm.get('ProductType').value;
		var customer = this.TermPricingForm.get('BuyerCompanyId').value;
		var formOrderList = this.TermPricingForm.get('OrderList') as FormArray;
		if (supplier != null && supplier != undefined && supplier.Id > 0 && productType > 0 && customer > 0) {
			this.orderService.getJobListByFuelGroupType(customer, supplier.Id, productType).subscribe(data => {
				this.JobList = data;
				if (data.length > 0) {
					this.SelectedJob = [data[0]];
					this.TermPricingForm.get('JobId').patchValue(data[0].Id);
					this.OnJobSelect(data[0]);
				} else {
					this.SelectedJob = [];
					this.Orders = [];
					formOrderList.clear();
					this.OrderList = [];
					this.addNewTier(this.DefaultTier);
					this.TermPricingForm.get('JobId').patchValue(null);
				}
			});
		}
		else {
			this.JobList = [];
			this.SelectedJob = [];
			this.Orders = [];
			formOrderList.clear();
			this.OrderList = [];
			this.addNewTier(this.DefaultTier);
			this.TermPricingForm.get('JobId').patchValue(null);
		}
	}

	public OnSupplierDeSelect(supplier: any) {
		this.TermPricingForm.get('SupplierCompanyId').patchValue(null);
		this.JobList = [];
		this.SelectedJob = [];
		this.Orders = [];
		var formOrderList = this.TermPricingForm.get('OrderList') as FormArray;
		formOrderList.clear();
		this.OrderList = [];
		this.addNewTier(this.DefaultTier);
		this.TermPricingForm.get('JobId').patchValue(null);
	}


	public OnFuelGroupSelect(fuelGroup: any) {
		this.TermPricingForm.get('ProductType').patchValue(fuelGroup.Id);
		var customer = this.TermPricingForm.get('BuyerCompanyId').value;
		var selectedsupplier = this.TermPricingForm.get('SupplierCompanyId').value;
		var formOrderList = this.TermPricingForm.get('OrderList') as FormArray;

		if (customer != null && customer != undefined && customer > 0 && fuelGroup.Id > 0 && selectedsupplier > 0) {
			this.orderService.getJobListByFuelGroupType(customer, selectedsupplier, fuelGroup.Id).subscribe(data => {
				this.JobList = data;
				if (data.length > 0) {
					this.SelectedJob = [data[0]];
					this.TermPricingForm.get('JobId').patchValue(data[0].Id);
					this.OnJobSelect(data[0]);
				}
				else {
					this.SelectedJob = [];
					this.Orders = [];
					formOrderList.clear();
					this.OrderList = [];
					this.addNewTier(this.DefaultTier);
					this.TermPricingForm.get('JobId').patchValue(null);
				}
			});
		}
		else {
			this.JobList = [];
			this.SelectedJob = [];
			this.Orders = [];
			formOrderList.clear();
			this.OrderList = [];
			this.addNewTier(this.DefaultTier);
			this.TermPricingForm.get('JobId').patchValue(null);
		}
	}

	public OnFuelGroupDeSelect(fuelGroup: any) {
		this.TermPricingForm.get('ProductType').patchValue(null);
		this.JobList = [];
		this.SelectedJob = [];
		this.Orders = [];
		var formOrderList = this.TermPricingForm.get('OrderList') as FormArray;
		formOrderList.clear();
		this.OrderList = [];
		this.addNewTier(this.DefaultTier);
		this.TermPricingForm.get('JobId').patchValue(null);

	}


	public OnJobSelect(job: any) {
		this.TermPricingForm.get('JobId').patchValue(job.Id);
		var productType = this.TermPricingForm.get('ProductType').value;
		var selectedcustomer = this.TermPricingForm.get('BuyerCompanyId').value;
		var selectedsupplier = this.TermPricingForm.get('SupplierCompanyId').value;
		var formOrderList = this.TermPricingForm.get('OrderList') as FormArray;
		formOrderList.clear();
		this.OrderList = [];
		this.addNewTier(this.DefaultTier);
		this.orderService.getOrderList(selectedcustomer, selectedsupplier, productType, job.Id).subscribe(data => { this.Orders = data; });
	}

	public OnJobDeSelect(job: any) {
		this.TermPricingForm.get('JobId').patchValue(null);
		this.Orders = [];
		var formOrderList = this.TermPricingForm.get('OrderList') as FormArray;
		formOrderList.clear();
		this.OrderList = [];
		this.addNewTier(this.DefaultTier);
	}

	public onSubmit(): void {
		if (this.TermPricingForm.valid && this.validateTierControls()) {
			this.orderService.postCreateGroup(this.TermPricingForm.value)
				.subscribe((data: any) => {
					if (data != null && data.StatusCode == 0) {
						Declarations.msgsuccess(data.StatusMessage, undefined, undefined);
						this.IsLoading = false;
						window.location.href = this.CurrentUser.IsBuyerCompany ? "/Buyer/OrderGroup/View" : "/Supplier/OrderGroup/View";
						//this.TermPricingForm.reset();
					} else {
						this.IsLoading = false;
						Declarations.msgerror(data == null ? 'Failed' : data.StatusMessage, undefined, undefined);
					}
				});
		}
		else {
			this.IsLoading = false;
			this.TermPricingForm.markAllAsTouched();
			let invalidControls: string[] = this.findInvalidControlsRecursive(this.TermPricingForm);
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

	addNewTier(model: OrderList) {
		if (model == null || model == undefined) {
			model = new OrderList();
		}
		if (model.OrderId == null || model.OrderId == undefined) {
			if (this.OrderList.length == 0) {
				model.MinVolume = 0;
			}
			else {
				var formOrderList = this.TermPricingForm.get('OrderList') as FormArray;
				var lastMaxValue = formOrderList.controls[formOrderList.length - 1].get('MaxVolume').value;
				if (lastMaxValue != null && lastMaxValue != undefined && parseInt(lastMaxValue) > 0) {
					model.MinVolume = parseInt(lastMaxValue) + 1;
				}
			}
		}
		var tierForm = this.buildTier(model);
		this.OrderList.push({ Order: model.Order, OrderId: model.OrderId, MinVolume: model.MinVolume, MaxVolume: model.MaxVolume });
		var prodArray = this.TermPricingForm.get('OrderList') as FormArray;
		if (prodArray != undefined && prodArray != null) {
			prodArray.push(tierForm);
		}
	}

	buildTier(model: OrderList): FormGroup {
		return this.fb.group({
			Order: this.fb.group({
				OrderId: this.fb.control(model.Order.OrderId, [Validators.required]),
				TfxPoNumber: this.fb.control(model.Order.TfxPoNumber),
				FuelType: this.fb.control(model.Order.FuelType),
				Quantity: this.fb.control(model.Order.Quantity),
				DisplayPrice: this.fb.control(model.Order.DisplayPrice)
			}),
			OrderId: this.fb.control(model.Order.OrderId),
			MinVolume: this.fb.control(model.MinVolume, [Validators.required, Validators.pattern(RegExConstants.Integer)]),
			MaxVolume: this.fb.control(model.MaxVolume, [Validators.required, Validators.pattern(RegExConstants.Integer)])
		});
	}

	removeTier(tier: OrderList, index: number): void {
		var deletedOrder = this.OrderList[index].Order;
		if (deletedOrder != null && deletedOrder != undefined && this.OrderList[index].OrderId > 0) {
			this.Orders.push(this.OrderList[index].Order);
		}
		var _tiers = this.TermPricingForm.get('OrderList') as FormArray;
		_tiers.removeAt(index);
		this.OrderList.splice(index, 1);
	}

	onItemDropTier(index: number, event: any) {
		var tierList = this.TermPricingForm.get('OrderList') as FormArray;
		var existingOrderinTier = this.OrderList[index].Order;
		var indexOfSameOrderinAnotherTier = this.OrderList.findIndex(t => t.OrderId == event.dragData.OrderId);
		if (indexOfSameOrderinAnotherTier >= 0) {
			this.OrderList[indexOfSameOrderinAnotherTier].Order = new OrderDetailModel();
			this.OrderList[indexOfSameOrderinAnotherTier].OrderId = null;
			tierList.controls[indexOfSameOrderinAnotherTier].get('Order').patchValue(this.emptyOrder);
			tierList.controls[indexOfSameOrderinAnotherTier].get('OrderId').patchValue(null);
		}
		this.OrderList[index].Order = event.dragData;
		this.OrderList[index].OrderId = event.dragData.OrderId;
		tierList.controls[index].get('Order').patchValue(event.dragData);
		tierList.controls[index].get('OrderId').patchValue(event.dragData.OrderId);
		this.Orders = this.Orders.filter(function (element) { return element.OrderId != event.dragData.OrderId });
		if (existingOrderinTier != null && existingOrderinTier != undefined && existingOrderinTier.OrderId > 0) {
			this.Orders.push(existingOrderinTier);
		}
	}

	onItemDropOrder(event: any) {
		if (this.Orders.findIndex(t => t.OrderId == event.dragData.OrderId) < 0) {
			this.Orders.push(event.dragData);
		}
		var index = this.OrderList.findIndex(t => t.OrderId == event.dragData.OrderId);
		if (index >= 0) {
			this.OrderList[index].Order = new OrderDetailModel();
			this.OrderList[index].OrderId = null;
			var tierList = this.TermPricingForm.get('OrderList') as FormArray;
			tierList.controls[index].get('Order').patchValue(this.emptyOrder);
			tierList.controls[index].get('OrderId').patchValue(null);
		}
	}

	isInvalid(drop: FormGroup, name: string): boolean {
		return drop.get(name).invalid &&
			(drop.get(name).dirty || drop.get(name).touched);
	}

	isRequired(drop: FormGroup, name: string): boolean {
		return drop.get(name).errors.required &&
			(drop.get(name).dirty || drop.get(name).touched);
	}

	isMin(drop: FormGroup, name: string): boolean {
		return drop.get(name).errors.min;
	}

	isDropAllowed = (dragData: any) => {
		return dragData > 500;
	}

	getNextRenewalDate(date: Date) {
		this.NextRenewalDate = moment(date).add(1, 'months').startOf('month').format('MM/DD/YYYY');
	}

	comparisonValidator(group: any, index: any, controlName:string){
		const minVolume = group.get('MinVolume');
		const maxVolume = group.get('MaxVolume');
		if (minVolume.value !== null && maxVolume.value !== null && parseInt(minVolume.value) >= parseInt(maxVolume.value)) {
			group.get(controlName).setErrors({ notEquivalent: true });
		} 
	}

	validateTierControls() {
		var tierList = this.TermPricingForm.get('OrderList') as FormArray;
		var maxVolumeCtrl, minVolumeCtrl;
		var minVolume, maxVolume;
		for (var i = 0; i < tierList.length; i++) {
			maxVolumeCtrl = tierList.controls[i].get('MaxVolume');
			minVolumeCtrl = tierList.controls[i].get('MinVolume');
			if (minVolume != undefined && minVolume != null) {
				if (parseInt(maxVolume) + 1 != parseInt(minVolumeCtrl.value)) {
					minVolumeCtrl.setErrors({ notEquivalent: true });
				}
			}
			maxVolume = maxVolumeCtrl.value;
			minVolume = minVolumeCtrl.value;
		}
		return this.TermPricingForm.valid;
	}
}
