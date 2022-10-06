import { Component, OnInit, EventEmitter, Output } from '@angular/core';
import { FormGroup, FormBuilder } from '@angular/forms';
import { ViewOrderGroupService } from '../services/viewordergroup.service';
import { ViewOrderGroupDdlModel } from '../models/ViewOrderGroupDdlModel'
import { OrderService } from '../services/order.service';

@Component({
    selector: 'app-filter-group',
    templateUrl: './filter-group.component.html',
    styleUrls: ['./filter-group.component.css']
})
export class FilterGroupComponent implements OnInit {
    public filterGroupForm: FormGroup;
    public model: ViewOrderGroupDdlModel;

    public SelectedCompany = [];
    public SelectedGroupType = [];
    public SelectedJob = [];
    public SelectedFuelGroup = [];
    public SelectedState = [];

    public IsSupplierCompany: boolean;
    public IsMultiProduct: boolean = true;
    public IsTier: boolean = false;
    public IsBlend: boolean = false;
    public IsLoading: boolean = true;   

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
    
    @Output() onViewOrderGroupResponse: EventEmitter<any> = new EventEmitter<any>();

    constructor(private fb: FormBuilder, private viewOrderGroupService: ViewOrderGroupService, private orderService: OrderService) {
        this.model = new ViewOrderGroupDdlModel();
    }

    ngOnInit() {
        this.IsLoading = true;
        this.model.GroupTypeId = 0;
        this.filterGroupForm = this.fb.group({
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

        this.viewOrderGroupService.fillDDLByGroup(this.model.GroupTypeId)
            .subscribe(data => {
                this.GroupTypeList = data.GroupTypes;
                this.CompanyList = data.Companies;
                this.FuelGroupList = data.ProductCategories;
                this.StateList = data.States;

                this.IsSupplierCompany = data.IsSupplierCompany;
            });
        this.showHideControlsByGroupType(this.model.GroupTypeId);
        this.onOrderGroupFilterSubmit();
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
                this.IsLoading = false;
            });

        this.onOrderGroupFilterSubmit();
    }

    public onOrderGroupFilterSubmit() {
        this.IsLoading = true;
        this.model.SearchText = this.filterGroupForm.get('SearchText').value;
        this.viewOrderGroupService.getOrderGroupDetails(this.model)
            .subscribe(data => {
                this.IsLoading = false;
                this.onViewOrderGroupResponse.emit(data);
            });
    }

    public resetForm() {
        this.SelectedCompany = [];
        this.SelectedGroupType = [];
        this.SelectedJob = [];
        this.SelectedFuelGroup = [];
        this.SelectedState = [];
        this.onViewOrderGroupResponse.emit([]);
    }

    public onSearch(searchText: any) {
        var text = searchText.target.value;
        if (text != null && text != '' && text.length >= 3) {
            this.IsLoading = true;
            this.model.SearchText = text;
            this.viewOrderGroupService.getOrderGroupDetails(this.model)
                .subscribe(data => {
                    this.IsLoading = false;
                    this.onViewOrderGroupResponse.emit(data);
                });
        }
        else if (text.length == 0) {
            this.IsLoading = true;
            this.model.SearchText = null;
            this.viewOrderGroupService.getOrderGroupDetails(this.model)
                .subscribe(data => {
                    this.IsLoading = false;
                    this.onViewOrderGroupResponse.emit(data);
                });
        }
    }

    public OnCompanySelect(company: any) {
        this.model.CompanyId = company.Id;
        this.JobList = [];
        this.orderService.getCommonJobList(company.Id).subscribe(data => this.JobList = data);
    }

    public OnCompanyDeSelect(groupType: any) {
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

    //onGroupSelect(type: any) {
    //    this.onGroupTypeSelect.emit(type);
    //}
}
