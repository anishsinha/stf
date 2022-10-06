import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { FormBuilder } from '@angular/forms';

@Injectable({
    providedIn: 'root'
})
export class WallyUtilService {

    constructor(private readonly fb: FormBuilder) { }

    getSalesTabFilterForm() {
        return this.fb.group({
            IsShowCarrierManaged: this.fb.control(false),
            RateOfConsumption: this.fb.control(false),
            SalesViewType: this.fb.control(1),
            SelectedCarriers: this.fb.control([]),
            SelectedRegions: this.fb.control([]),
            SelectedCustomerList: this.fb.control([]),
            SelectedlocationList: this.fb.control([]),
            SelectedPriorities: this.fb.control([]),
            SelectedLocAttributeList: this.fb.control([]),
            //to triger apply filter 
            IsApplyFilter: this.fb.control(false),
            IsApplyFilterOnPageLoad: this.fb.control(false),
        })
    }

    getIdsByList(array: any[]) {
        let responseString = '';
        if (array && array.length > 0) {
            let ids = [];
            array.forEach(res => { ids.push(res.Id) });
            if (ids && ids.length > 0) {
                responseString = ids.join();
            }
        }
        return responseString;
    }
    getCompanyIdsByList(array: any[]) {
        let responseString = '';
        if (array && array.length > 0) {
            let ids = [];
            array.forEach(res => { ids.push(res.CompanyId) });
            if (ids && ids.length > 0) {
                responseString = ids.join();
            }
        }
        return responseString;
    }
}

export class LocationTankDetailsModel {
    JobId?: number;
    SiteId?: string;
    SiteIdDetails?: string;
    Tanks?: TankDetailModel[];
    LocationName?: string;
    Status: string;
    DaysRemaining: number;
    CustomerInfo?: string;
}

export class TankDetailModel {
    TankId?: string;
    StorageId?: string;
    Name?: string;
    IsUnknowDeliveryOrMissDelivery?: boolean;
    TankInventoryDiffinHrs?: number;
    DaysRemaining: number
    Status: string
}
