import { Component, ElementRef, OnInit, ViewChild, ViewChildren } from '@angular/core';
import { FormGroup, FormBuilder } from '@angular/forms';
import { ViewOrderGroupService } from '../services/viewordergroup.service';
import { OrderGroupDetailModel } from '../models/ViewOrderGroupModel'
import { SharedService } from '../services/shared.service';
import { CreateBlendGroupComponent } from '../create-blend-group/create-blend-group.component';
import { Declarations } from 'src/app/declarations.module';
import { ViewOrderGroupDdlModel } from '../models/ViewOrderGroupDdlModel';
import { OrderService } from '../services/order.service';
import { CreateSameDestGroupComponent } from '../create-same-dest-group/create-same-dest-group.component';
import { OrderGroupType } from 'src/app/app.enum';

@Component({
	selector: 'app-view-order-group',
	templateUrl: './view-order-group.component.html',
	styleUrls: ['./view-order-group.component.css']
})
export class ViewOrderGroupComponent implements OnInit {

	public viewOrderGroupForm: FormGroup;
	public groups: OrderGroupDetailModel[];
	SelectedButton: string = "";
	ModalText: string = "";
	ModalTextData: string[] = ['', 'Create Same Destination Group', 'Term Pricing Contract', 'Create Blend Group'];
	ModalEditTextData: string[] = ['', 'Edit Same Destination Group', 'Edit Pricing Contract', 'Edit Blend Group'];

    public IsLoading: boolean = false;

    public ShowCount: number = 0;
    public TotalGroupCount: number = 0;

    //child 
    public model: ViewOrderGroupDdlModel = new ViewOrderGroupDdlModel();

    public SelectedCompany = [];
    public SelectedGroupType = [];
    public SelectedJob = [];
    public SelectedFuelGroup = [];
    public SelectedState = [];

    public IsSupplierCompany: boolean;
    public IsMultiProduct: boolean = true;
    public IsTier: boolean = false;
    public IsBlend: boolean = false;

   // public SearchText: string;
    public GroupTypeList = [];
    public CompanyList = [];
    public JobList = [];
    public FuelGroupList = [];
    public StateList = [];

    public GroupTypeDdlSettings = {}; 
    public CompanyDdlSettings = {};
    public JobDdlSettings = {};
    public FuelGroupDdlSettings = {};
    public StateDdlSettings = {};
    SelectedOrderGroupIdToDelete: number = 0;
    @ViewChild(CreateBlendGroupComponent) BlendComponent: CreateBlendGroupComponent;
    @ViewChild(CreateSameDestGroupComponent) SameDestComponent: CreateSameDestGroupComponent;

    constructor(private fb: FormBuilder, private viewOrderGroupService: ViewOrderGroupService, private sharedService: SharedService, private orderService: OrderService) {
	}

    ngOnInit() {

        this.model.GroupTypeId = 0;
        this.viewOrderGroupForm = this.fb.group({
            OrderGroupDetails: this.fb.control(''),
            GroupType: this.fb.control(null),
            Customer: this.fb.control(null),
            Job: this.fb.control(null),
            ProductCategory: this.fb.control(null),
            State: this.fb.control(null),
            GroupTypeId: this.fb.control(0),
            CompanyId: this.fb.control(null),
            JobId: this.fb.control(null),
            FuelGroupId: this.fb.control(null),
            StateId: this.fb.control(null),
            SearchText: this.fb.control(null),
        });

        this.GroupTypeDdlSettings = {
            singleSelection: true,
            idField: 'Id',
            textField: 'Name',
            enableCheckAll: false,
            selectAllText: '',
            unSelectAllText: '',
            itemsShowLimit: 1,
            allowSearchFilter: true
        };
        this.CompanyDdlSettings = {
            singleSelection: true,
            idField: 'Id',
            textField: 'Name',
            enableCheckAll: false,
            selectAllText: '',
            unSelectAllText: '',
            itemsShowLimit: 1,
            allowSearchFilter: true
        };
        this.JobDdlSettings = {
            singleSelection: true,
            idField: 'Id',
            textField: 'Name',
            enableCheckAll: false,
            selectAllText: '',
            unSelectAllText: '',
            itemsShowLimit: 1,
            allowSearchFilter: true
        };
        this.FuelGroupDdlSettings = {
            singleSelection: true,
            idField: 'Id',
            textField: 'Name',
            enableCheckAll: false,
            selectAllText: '',
            unSelectAllText: '',
            itemsShowLimit: 1,
            allowSearchFilter: true
        };
        this.StateDdlSettings = {
            singleSelection: true,
            idField: 'Id',
            textField: 'Name',
            enableCheckAll: false,
            selectAllText: '',
            unSelectAllText: '',
            itemsShowLimit: 1,
            allowSearchFilter: true
        };

        this.showHideControlsByGroupType(this.model.GroupTypeId);
        this.fillDDLByGroup();
    }


    fillDDLByGroup() {
        this.IsLoading = true;
        this.viewOrderGroupService.fillDDLByGroup(this.model.GroupTypeId)
            .subscribe(data => {
                this.GroupTypeList = data.GroupTypes;
                this.CompanyList = data.Companies;
                this.FuelGroupList = data.ProductCategories;
                this.StateList = data.States;
                this.IsSupplierCompany = data.IsSupplierCompany;

                this.onOrderGroupFilterSubmit();
            });
    }

    //onViewOrderGroupChildResponse(response: ViewOrderGroupModel) {
	//	//this.model = response;
	//	this.groups = response.OrderGroupDetails as OrderGroupDetailModel[];
	//	console.log(response);
	//}

    ButtonPressed(_SelectedButton: string) {
        if (this.BlendComponent != undefined) {
            this.BlendComponent.clearBlendGroupForm();
        }
        if (this.SameDestComponent != undefined) {
            this.SameDestComponent.clearDestGroupForm();
        }
		this.SelectedButton = OrderGroupType[_SelectedButton];
		this.ModalText = this.ModalTextData[_SelectedButton];
		this.sharedService.setGroupId(0);
    }
    
    public showHideControlsByGroupType(groupTypeId) {
        if (groupTypeId == 2) {
            this.IsTier = true
            this.IsMultiProduct = false;
            this.IsBlend = false;
        }
        else if (groupTypeId == 3) {
            this.IsBlend = true;
            this.IsTier = false;
            this.IsMultiProduct = false;
        }
        else {
            this.IsMultiProduct = true;
            this.IsTier = false;
            this.IsBlend = false;
        }
    }

    public OnGroupTypeSelect(groupType: any) {
        this.IsLoading = true;
        this.resetForm();
        this.model.GroupTypeId = groupType.target.value;
        this.showHideControlsByGroupType(this.model.GroupTypeId);
        this.viewOrderGroupService.fillDDLByGroup(this.model.GroupTypeId)
            .subscribe(data => {
                this.CompanyList = data.Companies;
                this.FuelGroupList = data.ProductCategories;
                this.StateList = data.States;

                this.onOrderGroupFilterSubmit();
            });
    }

    public onOrderGroupFilterSubmit() {
        this.IsLoading = true;
        this.viewOrderGroupService.getOrderGroupDetails(this.model)
            .subscribe(data => {
                this.groups = data.OrderGroupDetails as OrderGroupDetailModel[];
                this.TotalGroupCount = data.TotalGroupCount;
                this.ShowCount = data.ShowCount;
                this.IsLoading = false;
            });
    }

    public resetForm() {
        this.SelectedCompany = [];
        this.SelectedGroupType = [];
        this.SelectedJob = [];
        this.SelectedFuelGroup = [];
        this.SelectedState = [];
        //this.onViewOrderGroupResponse.emit([]);
        this.groups = [];
    }

    public onSearch(text: string) {
        if (text != null && text != '' && text.length >= 3) {
            this.IsLoading = true;
            this.model.SearchText = text;
            this.viewOrderGroupService.getOrderGroupDetails(this.model)
                .subscribe(data => {
                    this.IsLoading = false;
                    this.groups = data.OrderGroupDetails as OrderGroupDetailModel[];
                    this.TotalGroupCount = data.TotalGroupCount;
                    this.ShowCount = data.ShowCount;
                    //this.onViewOrderGroupResponse.emit(data);
                });
        }
        else if (text.length == 0) {
            this.IsLoading = true;
            this.model.SearchText = null;
            this.viewOrderGroupService.getOrderGroupDetails(this.model)
                .subscribe(data => {
                    this.IsLoading = false;
                    this.groups = data.OrderGroupDetails as OrderGroupDetailModel[];
                    this.TotalGroupCount = data.TotalGroupCount;
                    this.ShowCount = data.ShowCount;
                   // this.onViewOrderGroupResponse.emit(data);
                });
        }
    }
    public OnCompanySelect(company: any) {
        this.model.CompanyId = company.Id;
        this.JobList = [];
        this.orderService.getCommonJobList(company.Id).subscribe(data => this.JobList = data);
    }

    public OnCompanyDeSelect(company: any) {
        this.JobList = [];
        this.model.CompanyId = null;
    }

    public OnJobSelect(job: any) {
        this.model.JobId = job.Id;
    }

    public OnJobDeSelect(groupType: any) {
        this.model.JobId = null;
    }

    public OnFuelGroupSelect(fuelGroup: any) {
        this.model.ProductCategoryId = fuelGroup.Id;
    }

    public OnFuelGroupDeSelect(fuelGroup: any) {
        this.model.ProductCategoryId = null;
    }

    public OnStateSelect(state: any) {
        this.model.StateId = state.Id;
    }

    public OnStateDeSelect(state: any) {
        this.model.StateId = null;
    }

    SetOrderGroupIdToDelete(orderGroupId: number) {
        this.SelectedOrderGroupIdToDelete = orderGroupId;
    }

    deleteOrderGroup() {
        if (this.SelectedOrderGroupIdToDelete == 0) { return; }

        this.IsLoading = true;
        this.viewOrderGroupService.deleteOrderGroup(this.SelectedOrderGroupIdToDelete)
            .subscribe(data => {                
                if (data != null && data.StatusCode == 0) {
                    Declarations.msgsuccess(data.StatusMessage, undefined, undefined);
                    this.onOrderGroupFilterSubmit();
                    this.IsLoading = false;
                } else {
                    this.IsLoading = false;
                    Declarations.msgerror(data == null ? 'Failed' : data.StatusMessage, undefined, undefined);
                }
            });
        this.SelectedOrderGroupIdToDelete = 0;
    }

	OpenEditSlider(OrderGroupId: number, _SelectedButton: string) {
		if (this.BlendComponent != undefined) {
			this.BlendComponent.isEdit = true;
		}
		this.SelectedButton = OrderGroupType[_SelectedButton];
		this.ModalText = this.ModalEditTextData[_SelectedButton];
		this.sharedService.setGroupId(OrderGroupId);
	}
}
