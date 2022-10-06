import { Component, OnInit, ViewChild } from '@angular/core';
import { Country } from '../models/location';
import { DropdownItem } from '../../statelist.service';
import { TerminalsComponent } from './terminals/terminals.component';
import { BulkPlantsComponent } from './bulk-plants/bulk-plants.component';

@Component({
  selector: 'app-pickup-location',
  templateUrl: './pickup-location.component.html',
  styleUrls: ['./pickup-location.component.css']
})
export class PickupLocationComponent implements OnInit {
    public locationViewType: any;
    public CountryFilter: any;
    public CountryEnum: typeof Country = Country;
    public CountryType: any = [];

    @ViewChild(TerminalsComponent) terminalComponent: TerminalsComponent;
    @ViewChild(BulkPlantsComponent) bulkPLantComponent: BulkPlantsComponent;

    constructor() { }

    ngOnInit() {
        this.checkWindowSelection();
        this.CountryType = (Object.keys(this.CountryEnum).filter(k => typeof this.CountryEnum[k] === "number") as string[]).map(x => { return { Id: this.CountryEnum[x], Name: x, Code: "" } as DropdownItem });
    }

    public onCountryChange() {
        localStorage.setItem('countryFilterType', <string>this.CountryFilter);
        if (this.locationViewType == 2) {
            this.bulkPLantComponent.loadDataTable();
        } else {
            this.terminalComponent.loadDataTable();
        }
    }

    public changeViewType(type: any): void {
        localStorage.setItem('locationViewType', <string>type);
        this.locationViewType = type;
    }
    private checkWindowSelection(): void {
        this.locationViewType = (localStorage.getItem('locationViewType')) ? (localStorage.getItem('locationViewType')) : 1;
        this.CountryFilter = (localStorage.getItem('countryFilterType')) ? (localStorage.getItem('countryFilterType')) : (localStorage.getItem('countryIdForDashboard') ? localStorage.getItem('countryIdForDashboard') : 1);
    }
}
