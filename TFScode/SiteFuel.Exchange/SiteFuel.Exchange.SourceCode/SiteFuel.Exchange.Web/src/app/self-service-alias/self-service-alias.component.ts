import { Component, OnInit, ViewChild } from '@angular/core';
import { DropdownItem } from 'src/app/statelist.service';
import { ProductMappingComponent } from './product-mapping/product-mapping.component';
import { Country } from './models/location';
import { TerminalMappingComponent } from './terminal-mapping/terminal-mapping.component';
import { CarrierService } from '../carrier/service/carrier.service';
import { ExternalMappingsService } from '../self-service-alias/service/externalmappings.service';
declare var defaultCountryId: any;
declare var currentCompanyId: any;
declare var carrierMappingVisible: any;

@Component({
    selector: 'app-self-service-alias',
    templateUrl: './self-service-alias.component.html',
    styleUrls: ['./self-service-alias.component.css']
})
export class SelfServiceAliasComponent implements OnInit {
    public locationViewType: any;
    public CountryFilter: any;
    public CountryEnum: typeof Country = Country;
    public CountryType: any = [];
    public isShow: boolean = false;
    public isShowCarrier: boolean = false;
    public isShowCountryDDL: boolean = true;
    public SelectedCountryId: number;
    public CurrentCompanyId: number;
    public CountryList: DropdownItem[] = [];
    public IsShowProductMappingComponent: boolean = true;
    public IsShowCarrierMappingComponent: boolean = false;
    isShowTerminalmappingCode = false;
    public isShowFuelGroup = false;
    public SelectedCompany: DropdownItem = new DropdownItem;
    @ViewChild(ProductMappingComponent) ProductMappingComponent: ProductMappingComponent;
    public ServingCountries: any = {};
    public externalCompanies: DropdownItem[] = [];
    public pdiTabName: string = 'CUSTOMERS';
    constructor(private carrierService: CarrierService, private externalMappingsService: ExternalMappingsService) { }

    ngOnInit() {
        this.CurrentCompanyId = Number(currentCompanyId);
        this.SelectedCountryId = Number(defaultCountryId);
        this.getExternalCompanies();
        this.getCountries();
        if (!carrierMappingVisible) {
            this.IsShowCarrierMappingComponent = false;
        }
        else {
            this.IsShowCarrierMappingComponent = true;
           // this.isShowCarrier = true;
        }

    }
   
    getExternalCompanies() {
        this.externalMappingsService.getExternalCompanies().subscribe(data => {
            if (data != null && data.length > 0) {
               
                this.externalCompanies = data;
                this.SelectedCompany = this.externalCompanies[0];
            }
        });
    }
    public changeTab(currencyDdlHide: string): void
    {
        if (currencyDdlHide === "Carrier") {
            this.isShowCarrier = true;
            this.isShowTerminalmappingCode = false;
        }
        else if (currencyDdlHide === "Customer") {
            this.isShow = true;
            this.isShowTerminalmappingCode = false;
            this.isShowCarrier = false;
        }
        else if (currencyDdlHide === "TerminalCode") {
            this.isShowTerminalmappingCode = true;
            this.isShowCarrier = false
        }
        else if (currencyDdlHide === "FuelGroup") {
           // this.isShow = false;
            this.isShowCarrier = false;
            this.isShowTerminalmappingCode = false;
            this.isShowFuelGroup = true;
            
        }
        else {
            this.isShow = !this.isShow;
            this.isShowTerminalmappingCode = false;
            this.isShowCarrier = false
            this.isShowFuelGroup = false;

        }
    }

    public onCountryChange() {
    localStorage.setItem('countryFilterType', <string>this.CountryFilter);
}

countryChanged(event: any) {
    this.IsShowProductMappingComponent = false;
    this.SelectedCountryId = (event.target.value == "null" || event.target.value == null) ? 1 : Number(event.target.value);
    this.IsShowProductMappingComponent = true;
}
    companyChanged(event: any) {
        if (event != null) {
            this.SelectedCompany = this.externalCompanies.find(t => t.Id == event.target.value);
        }     
    }
    private checkWindowSelection(): void {
    this.locationViewType = (localStorage.getItem('locationViewType')) ? (localStorage.getItem('locationViewType')) : 1;
    this.CountryFilter = (localStorage.getItem('countryFilterType')) ? (localStorage.getItem('countryFilterType')) : (localStorage.getItem('countryIdForDashboard') ? localStorage.getItem('countryIdForDashboard') : 1);
}

getCountries() {
    this.carrierService.GetCountries(this.CurrentCompanyId).subscribe(data => {
        if (data != null) {
            if (data.ServingCountries != null && data.ServingCountries.length > 1) {
                this.ServingCountries = data.ServingCountries;
                this.CountryList = data.CountryList as DropdownItem[];
                if (isNaN(this.SelectedCountryId) || this.SelectedCountryId == 0) {
                    defaultCountryId = data.DefaultCountryId;
                    this.SelectedCountryId = Number(data.DefaultCountryId);
                }
            }
            else {
                this.isShowCountryDDL = false;
            }
        }
    });
}
}
