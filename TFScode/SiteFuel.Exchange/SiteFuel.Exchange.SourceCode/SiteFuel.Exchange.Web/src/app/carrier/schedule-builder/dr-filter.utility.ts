import { HttpClient } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { FormBuilder, FormGroup } from "@angular/forms";
import { catchError } from "rxjs/operators";
import { DeliveryRequestTypes, DeliveryShiftEnum, DrFilterPreferencesModel, OrderType, TrailerTypeEnum } from "src/app/app.enum";
import { DropDownItem } from "src/app/buyer-wally-board/Models/BuyerWallyBoard";
import { HandleError } from "src/app/errors/HandleError";
import { DeliveryRequestViewModel, DelRequestsByJobModel } from "../models/DispatchSchedulerModels";

@Injectable({
    providedIn: 'root'
})

export class DrFilterService extends HandleError {

    private urlSaveDrFilterPreferences = '/Freight/SaveDrFilterPreferences';
    private urlGetDrFilterPreferences = '/Freight/GetDrFilterPreferences';

    constructor(private fb: FormBuilder, private httpClient: HttpClient) { super(); }

    drFilterFormFromModel(drFilterModel: DrFilterModel) {

        let form = this.fb.group({
            Terminals: this.fb.control(drFilterModel.Terminals),
            BulkPlants: this.fb.control(drFilterModel.Bulkplants),
            Priority: this.fb.group({
                Missed: this.fb.control(drFilterModel.Priority.includes(DeliveryRequestTypes.Missed) ? true : false),
                MustGo: this.fb.control(drFilterModel.Priority.includes(DeliveryRequestTypes.MustGo) ? true : false),
                ShouldGo: this.fb.control(drFilterModel.Priority.includes(DeliveryRequestTypes.ShouldGo) ? true : false),
                CouldGo: this.fb.control(drFilterModel.Priority.includes(DeliveryRequestTypes.CouldGo) ? true : false),
                AssignedToMe: this.fb.control(drFilterModel.Priority.includes(DeliveryRequestTypes.AssignedToMe) ? true : false),
                AssignedByMe: this.fb.control(drFilterModel.Priority.includes(DeliveryRequestTypes.AssignedByMe) ? true : false)
            }),
            TrailerType: this.fb.group({
                Lead: this.fb.control(drFilterModel.TrailerType.includes(TrailerTypeEnum.Lead) ? true : false),
                Pup: this.fb.control(drFilterModel.TrailerType.includes(TrailerTypeEnum.Pup) ? true : false),
                Tandem: this.fb.control(drFilterModel.TrailerType.includes(TrailerTypeEnum.Tandem) ? true : false),
                Quad: this.fb.control(drFilterModel.TrailerType.includes(TrailerTypeEnum.Quad) ? true : false),
                Tridem: this.fb.control(drFilterModel.TrailerType.includes(TrailerTypeEnum.Tridem) ? true : false),
            }),
            DeliveryShift: this.fb.group({
                Morning: this.fb.control(drFilterModel.DeliveryShift.includes(DeliveryShiftEnum.Morning) ? true : false),
                Evening: this.fb.control(drFilterModel.DeliveryShift.includes(DeliveryShiftEnum.Evening) ? true : false)
            }),
            IsFilterApplied: this.fb.control(drFilterModel.IsFilterApplied),
            IsTBDRequest: this.fb.control(drFilterModel.IsTBDRequest),
            DeliveryBrokeredDR: this.fb.group({
                brokeredDrs: this.fb.control(drFilterModel.IsBrokeredDRs),
            }),
            Customers: this.fb.control(drFilterModel.Customers)
        });

        return form;
    }

    getDrFilterForm(isFilterApplied?: boolean) {

        return this.fb.group({
            Terminals: this.fb.control([]),
            BulkPlants: this.fb.control([]),
            Priority: this.fb.group({
                Missed: this.fb.control(false),
                MustGo: this.fb.control(isFilterApplied ? true : false),
                ShouldGo: this.fb.control(false),
                CouldGo: this.fb.control(false),
                AssignedToMe: this.fb.control(false),
                AssignedByMe: this.fb.control(false)
            }),
            TrailerType: this.fb.group({
                Lead: this.fb.control(false),
                Pup: this.fb.control(false),
                Tandem: this.fb.control(false),
                Quad: this.fb.control(false),
                Tridem: this.fb.control(false)
            }),
            DeliveryShift: this.fb.group({
                Morning: this.fb.control(false),
                Evening: this.fb.control(false)
            }),
            DeliveryBrokeredDR: this.fb.group({
                brokeredDrs: this.fb.control(false),
            }),
            IsTBDRequest: this.fb.control(false),
            IsFilterApplied: this.fb.control(isFilterApplied ? true : false),
            OrderType: this.fb.group({
                OrderTypeLTL: this.fb.control(false),
                OrderTypeFTL: this.fb.control(false),
                OrderTypeALL: this.fb.control(false),
            }),
            Customers: this.fb.control([])
        });
    }

    drFilterFormToModel(drFilterForm: FormGroup, IsCarrierCompany: boolean) {

        let formValue = drFilterForm.value;
        let model = new DrFilterModel();

        //SHIFTS
        if (formValue.DeliveryShift.Evening)
            model.DeliveryShift.push(DeliveryShiftEnum.Evening)
        if (formValue.DeliveryShift.Morning)
            model.DeliveryShift.push(DeliveryShiftEnum.Morning)
        //PRIORITY
        if (formValue.Priority.MustGo)
            model.Priority.push(DeliveryRequestTypes.MustGo);
        if (formValue.Priority.ShouldGo)
            model.Priority.push(DeliveryRequestTypes.ShouldGo);
        if (formValue.Priority.CouldGo)
            model.Priority.push(DeliveryRequestTypes.CouldGo);
        if (formValue.Priority.Missed)
            model.Priority.push(DeliveryRequestTypes.Missed);
        if (formValue.Priority.AssignedToMe)
            model.Priority.push(DeliveryRequestTypes.AssignedToMe);
        if (formValue.Priority.AssignedByMe)
            model.Priority.push(DeliveryRequestTypes.AssignedByMe);
        //TRAILER
        if (formValue.TrailerType.Lead)
            model.TrailerType.push(TrailerTypeEnum.Lead);
        if (formValue.TrailerType.Pup)
            model.TrailerType.push(TrailerTypeEnum.Pup);
        if (formValue.TrailerType.Quad)
            model.TrailerType.push(TrailerTypeEnum.Quad);
        if (formValue.TrailerType.Tandem)
            model.TrailerType.push(TrailerTypeEnum.Tandem);
        if (formValue.TrailerType.Tridem)
            model.TrailerType.push(TrailerTypeEnum.Tridem);

        //LOCATIONS
        model.Terminals = formValue.Terminals;
        model.Bulkplants = formValue.BulkPlants;
        model.IsFilterApplied = formValue.IsFilterApplied;
        model.IsTBDRequest = formValue.IsTBDRequest;
        if (IsCarrierCompany) {
            model.IsBrokeredDRs = false;
            if (formValue.DeliveryBrokeredDR.brokeredDrs) {
                model.IsBrokeredDRs = true;
            }
        }
        else {
            model.IsBrokeredDRs = false;
        }

        //ORDER TYPE.
        if (formValue.OrderType) {
            if (formValue.OrderType.OrderTypeLTL)
                model.OrderType.push(OrderType.LTL);
            if (formValue.OrderType.OrderTypeFTL)
                model.OrderType.push(OrderType.FTL);
            if (formValue.Priority.OrderTypeALL)
                model.OrderType.push(OrderType.ALL);
        }
        model.Customers = formValue.Customers;
        return model;
    }

    validateFilterForm(formValue: any) {
        if ((formValue.Priority) && (formValue.Priority.AssignedByMe || formValue.Priority.AssignedToMe || formValue.Priority.CouldGo || formValue.Priority.Missed || formValue.Priority.MustGo || formValue.Priority.ShouldGo))
            return true;
        else
            return false;
    }

    getLocationsFromDr(drs: DeliveryRequestViewModel[]) {

        let _terminals: DropDownItem[] = [];
        let _bulkPlants: DropDownItem[] = [];
        let _cusomterCompanies: DropDownItem[] = [];

        for (let i = 0; i < drs.length; i++) {

            let _dr = drs[i];

            //terminal
            if (_dr.PickupLocationType != 2 && _dr.Terminal && _dr.Terminal.Id > 0 && !_terminals.some((term) => term.Id == _dr.Terminal.Id)) {
                _terminals.push(
                    { Name: _dr.Terminal.Name, Id: _dr.Terminal.Id }
                );
            }
            //bulk plant
            else if (_dr.PickupLocationType == 2 && _dr.BulkPlant && _dr.BulkPlant.SiteName && !_bulkPlants.some((bulk) => bulk.Name.toLocaleLowerCase() == _dr.BulkPlant.SiteName.toLocaleLowerCase())) {
                _bulkPlants.push(
                    { Name: _dr.BulkPlant.SiteName, Id: 0 }
                );
            }
            //Customers
            if (_dr.CustomerCompany && _dr.CustomerCompany.length > 0 && _dr.AssignedToCompanyId > 0) {
                _cusomterCompanies.push(
                    {
                        Name: _dr.CustomerCompany,
                        Id: _dr.AssignedToCompanyId
                    }
                )
            }
        }
        return { terminals: _terminals, bulkPlants: _bulkPlants, customerCompanies: _cusomterCompanies};
    }


    getDrsFromJob(deliveryRequests: DelRequestsByJobModel[]) {

        let _drRequests: DeliveryRequestViewModel[] = [];

        deliveryRequests && deliveryRequests.length && deliveryRequests.forEach(job => {
            if (job && job.DeliveryRequests) {
                job.DeliveryRequests.forEach(dr => { dr && _drRequests.push(dr); });
            }
        });

        return _drRequests;
    }

    applyFilterToDrs(deliveryRequests: DeliveryRequestViewModel[], filterModel: DrFilterModel) {

        let searchedRecords = deliveryRequests.filter((dr: DeliveryRequestViewModel) => (
            //1.day/night delivery
            ((filterModel.DeliveryShift.length == 0)
                || (dr.IsAcceptNightDeliveries && filterModel.DeliveryShift.includes(DeliveryShiftEnum.Evening))
                || (!dr.IsAcceptNightDeliveries && filterModel.DeliveryShift.includes(DeliveryShiftEnum.Morning)))
            &&
            //2.TrailerType
            ((filterModel.TrailerType.length == 0) || dr.JobId == 0
                || (dr.TrailerTypes.some((tr) => filterModel.TrailerType.includes(tr.Id))))
            &&
            //Terminals
            ((filterModel.Terminals.length == 0 && filterModel.Bulkplants.length == 0)
                || ((filterModel.Terminals.length > 0) && (dr.PickupLocationType != 2 && dr.Terminal && dr.Terminal.Id > 0 && filterModel.Terminals.some((loc) => loc.Id == dr.Terminal.Id)))
                || ((filterModel.Bulkplants.length > 0) && (dr.PickupLocationType == 2 && dr.BulkPlant && dr.BulkPlant.SiteName && filterModel.Bulkplants.some((bulk) => bulk.Name.toLowerCase() == dr.BulkPlant.SiteName.toLowerCase()))))

            &&
            //Customers
            ((filterModel.Customers.length == 0) 
                || ((filterModel.Customers.length > 0) && filterModel.Customers.some((cus) => cus.CompanyName == dr.CustomerCompany)))
        ));
        return searchedRecords;
    }

    searchRequests(deliveryRequests: DeliveryRequestViewModel[], terms: string) {

        let searchedDrs = deliveryRequests.filter((dr: DeliveryRequestViewModel) =>
        ((dr.CustomerCompany && dr.CustomerCompany.toLowerCase().startsWith(terms))
            || (dr.JobName && dr.JobName.toLowerCase().startsWith(terms))
            || (dr.JobAddress && dr.JobAddress.toLowerCase().startsWith(terms))
            || (dr.RouteInfo && dr.RouteInfo.Name && dr.RouteInfo.Name.toLowerCase().startsWith(terms))
            || (dr.ProductType && dr.ProductType.toLowerCase().startsWith(terms))
            || dr.RequiredQuantity.toString().startsWith(terms)));

        return searchedDrs;
    }

    searchRequestsWithParams(deliveryRequests: DeliveryRequestViewModel[], terms: string, queueMode: number, windowMode: number, drType: DeliveryRequestTypes) {

        let searchedDrs = deliveryRequests.filter((dr: DeliveryRequestViewModel) =>
            ((dr.CustomerCompany && dr.CustomerCompany.toLowerCase().indexOf(terms) != -1)
                || (dr.JobName && dr.JobName.toLowerCase().indexOf(terms) != -1)
                || (dr.JobAddress && dr.JobAddress.toLowerCase().indexOf(terms) != -1)
                || (dr.JobCity && dr.JobCity.toLowerCase().indexOf(terms) != -1)
                || (dr.RouteInfo && dr.RouteInfo.Name && dr.RouteInfo.Name.toLowerCase().indexOf(terms) != -1)
                || (dr.ProductType && dr.ProductType.toLowerCase().indexOf(terms) != -1)
                || (dr.Vessel && dr.Vessel.toLowerCase().indexOf(terms) != -1)
                || dr.RequiredQuantity.toString().startsWith(terms))
            && dr.WindowMode == windowMode
            && dr.QueueMode === queueMode
            && ((drType == DeliveryRequestTypes.Missed && dr.ParentId != null) || (drType != DeliveryRequestTypes.Missed && dr.ParentId == null)));

        return searchedDrs;
    }

    saveDrFilterPreferences(filter: DrFilterPreferencesModel) {
        return this.httpClient.post<DrFilterPreferencesModel>(this.urlSaveDrFilterPreferences, filter)
            .pipe(catchError(this.handleError<DrFilterPreferencesModel>('saveDrFilterPreferences', null)));
    }

    getDrFilterPreferences() {
        return this.httpClient.get<DrFilterPreferencesModel>(this.urlGetDrFilterPreferences)
            .pipe(catchError(this.handleError<DrFilterPreferencesModel>('getDrFilterPreferences', null)));
    }
}

export class DrFilterModel {
    public Terminals: DropDownItem[];
    public Bulkplants: DropDownItem[];
    public Priority: number[];
    public TrailerType: number[];
    public DeliveryShift: number[];
    public IsFilterApplied: boolean;
    public IsBrokeredDRs: boolean;
    public IsTBDRequest: boolean;
    public OrderType: number[];
    public Customers: any[];

    constructor() {
        this.Terminals = [];
        this.Bulkplants = []
        this.Priority = [];
        this.TrailerType = [];
        this.DeliveryShift = [];
        this.IsFilterApplied = false;
        this.IsTBDRequest = false;
        this.IsBrokeredDRs = false;
        this.OrderType = [];
        this.Customers = [];
    }
}
