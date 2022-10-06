import { Component, OnInit, ViewEncapsulation } from '@angular/core';
import { Router } from '@angular/router';
import { CarrierService } from 'src/app/carrier/service/carrier.service';
import { DropdownItem } from 'src/app/statelist.service';
import { DashboardService } from '../dashboard.service';
import { DashboardTileViewModel } from '../Model/DashboardModel';
import { Declarations } from 'src/app/declarations.module';
declare var currentUserCompanyId: any;
@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.scss'],
  encapsulation: ViewEncapsulation.None,
})
export class HomeComponent implements OnInit {
  public isShowCountryDDL: boolean = true;
  CountryList: DropdownItem[] = [];
  public SelectedCountryId: number;
  public tiles: Tiles = {};
  IsLoading = false;
  public DashboardTileViewModelList: DashboardTileViewModel[] = [];
  pageID: string;
  constructor(private carrierService: CarrierService, private router: Router, private _dashboardSer: DashboardService) { }

  ngOnInit() {
    this.getCountries();
    this.getTiles();
  }

  getTiles() {
    this.IsLoading = true;
    this._dashboardSer.GetNewBuyerDashboardTileSettings().subscribe(data => {
      if (data) {
        this.pageID = data.PageId;
        this.DashboardTileViewModelList = data.TileDetails;
        this.initializeTiles();
        this.IsLoading = false;
      } else
        this.IsLoading = false;
    });
  }

  private getCountries() {
    this.carrierService.GetCountries(currentUserCompanyId).subscribe(data => {
      if (data != null) {
        if (data.ServingCountries != null && data.ServingCountries.length > 1) {
          //this.ServingCountries = data.ServingCountries;
          this.CountryList = data.CountryList as DropdownItem[];

          if (isNaN(this.SelectedCountryId) || this.SelectedCountryId == 0) {
            let countryId = localStorage.getItem('countryIdForDashboard');
            if (countryId)
              this.SelectedCountryId = +countryId;
            else {
              this.SelectedCountryId = Number(data.DefaultCountryId);
              localStorage.setItem('countryIdForDashboard', data.DefaultCountryId);
              localStorage.setItem('currencyTypeForDashboard', data.DefaultCountryId);
            }
          }
        }
        else {
          this.SelectedCountryId = Number(data.DefaultCountryId);
          this.isShowCountryDDL = false;
        }
      }
    });
  }

  public onCountryChange(event) {
    this.SelectedCountryId = (event.target.value == "null" || event.target.value == null) ? 1 : Number(event.target.value);
    // localStorage.setItem('countryFilterType', <string>this.CountryFilter);
    localStorage.setItem('countryIdForDashboard', this.SelectedCountryId.toString());
    localStorage.setItem('currencyTypeForDashboard', this.SelectedCountryId.toString());
  }

  navigate() {
    this.router.navigate([]).then(result => { window.open('/Buyer/Dashboard/Index', '_blank'); });
  }

  public ApplySettings(): void {

    this._dashboardSer.SaveDBTileSettings(this.pageID, this.DashboardTileViewModelList).subscribe(data => {
      if (data.StatusCode == 0) {
        Declarations.msgsuccess(data.StatusMessage, undefined, undefined);
        this.initializeTiles();
      }
      else {
        Declarations.msgerror(data == null ? 'Failed' : data.StatusMessage, undefined, undefined);
      }
    });
  }

  initializeTiles() {
    this.tiles.IsDelivery = this.DashboardTileViewModelList.find(f => f.TileDisplayName == "Deliveries").IsClosed;
    this.tiles.IsInventory = this.DashboardTileViewModelList.find(f => f.TileDisplayName == "Location Inventory").IsClosed;
    this.tiles.IsInvoice = this.DashboardTileViewModelList.find(f => f.TileDisplayName == "Invoices").IsClosed;
    this.tiles.IsMessage = this.DashboardTileViewModelList.find(f => f.TileDisplayName == "Messages").IsClosed;
  }


}


export class Tiles {
  IsWallyBoard?: boolean
  IsInvoice?: boolean
  IsDelivery?: boolean
  IsInventory?: boolean
IsMessage?:boolean
}

