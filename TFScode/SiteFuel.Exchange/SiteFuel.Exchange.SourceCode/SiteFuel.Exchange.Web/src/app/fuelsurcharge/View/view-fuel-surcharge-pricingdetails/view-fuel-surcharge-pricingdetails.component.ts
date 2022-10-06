import { Component, OnInit } from '@angular/core';
import { FuelSurchargeService } from '../../services/fuelsurcharge.service';
import { FormBuilder } from '@angular/forms';
import { FuelSurchargePricingModel } from '../../models/CreateFuelSurcharge';

@Component({
    selector: 'app-view-fuel-surcharge-pricingdetails',
    templateUrl: './view-fuel-surcharge-pricingdetails.component.html',
    styleUrls: ['./view-fuel-surcharge-pricingdetails.component.css']
})
export class ViewFuelSurchargePricingdetailsComponent implements OnInit {

    constructor(private fb: FormBuilder, private fuelsurchargeService: FuelSurchargeService) { }
    public FuelSurchargePricingList: FuelSurchargePricingModel[] = [];

    ngOnInit() {
    }

    getFuelSurchargePricingDetails(fuelSurchargeIndexId: number) {
        this.fuelsurchargeService.getSurchargeTableNew(fuelSurchargeIndexId).subscribe(data => {
            this.FuelSurchargePricingList = data as FuelSurchargePricingModel[];
        });
    }
}
