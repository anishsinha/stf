import { Component, OnInit } from '@angular/core';
import { HistoricalPriceModel, HistoricalPriceDetailsModel } from '../../models/CreateFuelSurcharge';
import { FormBuilder } from '@angular/forms';
import { FuelSurchargeService } from '../../services/fuelsurcharge.service';

@Component({
    selector: 'app-view-historical-price',
    templateUrl: './view-historical-price.component.html',
    styleUrls: ['./view-historical-price.component.css']
})
export class ViewHistoricalPriceComponent implements OnInit {

    constructor(private fb: FormBuilder, private fuelsurchargeService: FuelSurchargeService) { }
    public HistoricalPrice: HistoricalPriceModel;
    public SinlgeselectSettingsById = {};
    public PeriodList: any[] = [];
    public HistoricalPriceDetailList: HistoricalPriceDetailsModel[];
    public SelectedPeriod: any[] = [];
    public SelectedFuelSurchargeIndexId: any;
  

    ngOnInit() {
        this.SinlgeselectSettingsById = {
            singleSelection: true,
            closeDropDownOnSelection: true,
            idField: 'Id',
            textField: 'Name',
            enableCheckAll: false,
            selectAllText: 'Select All',
            unSelectAllText: 'UnSelect All',
            itemsShowLimit: 1,
            allowSearchFilter: true
        };
        this.getPeriod();
    }

    private getPeriod(): void {
        for (let i = 1; i <= 12; i++) {
            this.PeriodList.push({ Id: i, Name: i });
        }
    }

    getHistoricalPriceDetails(fuelSurchargeIndexId: number) {

        this.SelectedPeriod = [];
        this.SelectedPeriod.push({ Id: 6, Name: 6 });


        this.fuelsurchargeService.getHistoricalPrice(fuelSurchargeIndexId, 6).subscribe(data => {
            this.HistoricalPrice = data as HistoricalPriceModel;
            this.HistoricalPriceDetailList = this.HistoricalPrice.HistoricalPriceDetails;
            this.SelectedFuelSurchargeIndexId = fuelSurchargeIndexId;
        });


    }

    fetchHistoricalPrice() {
        this.fuelsurchargeService.getHistoricalPrice(this.SelectedFuelSurchargeIndexId, this.SelectedPeriod[0].Id).subscribe(data => {
            this.HistoricalPrice = data as HistoricalPriceModel;
            this.HistoricalPriceDetailList = this.HistoricalPrice.HistoricalPriceDetails;
        });
    }
}
