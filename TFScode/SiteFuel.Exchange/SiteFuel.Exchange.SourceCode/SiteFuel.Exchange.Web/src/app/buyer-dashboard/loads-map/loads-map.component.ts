import { Component, Input, OnInit, QueryList, SimpleChanges, ViewChild, ViewChildren } from '@angular/core';
import { DataTableDirective } from 'angular-datatables';
import * as moment from 'moment';
import { Subject } from 'rxjs';
import { DriverAdditionalDetails, routesColor, WhereIsMyDriverModel } from 'src/app/buyer-wally-board/Models/BuyerWallyBoard';
import { BuyerwallyboardService } from 'src/app/buyer-wally-board/services/buyerwallyboard.service';
import { CarrierService } from 'src/app/carrier/service/carrier.service';
import { Declarations } from 'src/app/declarations.module';
import { MemberInfo } from 'src/app/shared-components/sendbird/sendbirdCommon';
declare function IsUserActive(): boolean;

export declare var google: any;
@Component({
  selector: 'app-loads-map',
  templateUrl: './loads-map.component.html',
  styleUrls: ['./loads-map.component.scss']
})
export class LoadsMapComponent implements OnInit {
  @Input() SelectedCountryId: any;
  public toogleMap: boolean = true;
  public selectedMaplable: any;
  public previousInfowindow: any = null;
  public previousInfowindowIndex: number = null
  public FuelUnit: string;
  public googleMap: any;
  public zoomLevel = 5;
  public centerLoactionLat = 39.1175;
  public centerLoactionLng = -103.8784;
  public MaxInputDate: Date = moment().add(1, 'year').toDate();
  public TodaysDate: string = moment().format('MM/DD/YYYY');


  private AUTO_REFRESH_TIME: number = 300; // seconds
  public autoRefreshTicks: number = this.AUTO_REFRESH_TIME;

  public driverModal = { modalDetails: { display: 'none', data: new WhereIsMyDriverModel() } };

  private UserCountry = "";
  private CountryCentre = {
    USA: { lat: 39.11757961, lng: -103.8784 },
    CAN: { lat: 57.88251631, lng: -98.54842922 }
  };
  public screenOptions = {
    position: 6
  };

  private searchLoadInterval: any;
  private autoRefreshInterval: any;
  private autoRefreshTimerInterval: any;
  private setCountryCenterInterval: any;

  public Drivers: WhereIsMyDriverModel[] = [];
  public OfflineDrivers: WhereIsMyDriverModel[] = [];
  public allLoads: WhereIsMyDriverModel[] = [];
  public OnGoingLoads: WhereIsMyDriverModel[] = [];
  public selectedDriverLoads: WhereIsMyDriverModel[] = [];
  public selectedDriverDetails: DriverAdditionalDetails = new DriverAdditionalDetails();

  public SearchedKeyword: string;

  public toogleFilter: Boolean = false;
  public toogleDriver: Boolean = false;
  public toogleGrid: Boolean = false;
  public toogleExpandMap: Boolean = false;

  public selectedDriverLoadsdtOptions: any = {};
  public selectedDriverLoadsdtTrigger: Subject<any> = new Subject();

  @ViewChildren(DataTableDirective) dtElements: QueryList<DataTableDirective>;
  @ViewChild('SelectedDriverLoad', { read: DataTableDirective, static: false }) selectedDriverLoad: DataTableDirective;
  public loadingData: boolean = true;
  public modalData: boolean = true;
  public IsShouldGoLoading: boolean;
  public IsCouldGoLoading: boolean;
  public IsMustGoLoading: boolean;
  public backgroudchatDefault = [];
  public memberInfo: MemberInfo[] = [];
  public disableControl: boolean = false;
  changeFilterValueIntervalForMultiWindow: any;

  constructor(private dispatcherService: BuyerwallyboardService, private carrierService: CarrierService) {
  }

  ngOnInit() {
    // this.filterDriverData();
    // this.dispatcherService.getDispatcherCountry().subscribe(data => {
    //   this.UserCountry = data;
    //   this.FuelUnit = (this.UserCountry === 'USA') ? 'Gallons' : 'Litres';
    //   this.setMapCenter();
    // });
  }

  clickOutsideDropdown() {
    if (this.toogleFilter) {
      this.toogleFilter = false;
    }
  }

  ngOnChanges(changes: SimpleChanges): void {
    if (changes.SelectedCountryId && changes.SelectedCountryId.currentValue) {
      this.filterDriverData();
      this.dispatcherService.getDispatcherCountry().subscribe(data => {
        this.UserCountry = data;
        this.FuelUnit = (this.UserCountry === 'USA') ? 'Gallons' : 'Litres';
        this.setMapCenter();
      });
    }
  }
  ngAfterViewInit(): void {
    this.getDispatcherLoads();
    this.autoRefreshLoads();
  }

  ngOnDestroy(): void {
    this.clearAllIntervals();
    if (this.changeFilterValueIntervalForMultiWindow)
      clearInterval(this.changeFilterValueIntervalForMultiWindow);
  }

  setMapCenter(): void {
    if (this.UserCountry != "") {
      this.setCountryCenterInterval = window.setTimeout(() => {
        this.centerLoactionLat = this.CountryCentre[this.UserCountry].lat;
        this.centerLoactionLng = this.CountryCentre[this.UserCountry].lng;
        if (this.googleMap && this.OnGoingLoads.length == 0) {
          const bounds = new google.maps.LatLngBounds();
          bounds.extend(new google.maps.LatLng(this.centerLoactionLat, this.centerLoactionLng));
          this.googleMap.fitBounds(bounds);
          this.googleMap.setZoom(5);
        } else {
          const bounds = new google.maps.LatLngBounds();
          this.OnGoingLoads.filter(t => t.Lat != null && t.Lng != null).forEach(x => {
            x.statusColor = routesColor[x.SttsId];
            bounds.extend(new google.maps.LatLng(x.Lat, x.Lng));
          });
          this.googleMap.fitBounds(bounds);

          const locationbounds = new google.maps.LatLngBounds();
          this.OnGoingLoads.forEach(x => {
              locationbounds.extend(new google.maps.LatLng(x.dLat, x.dLng));
          });
          if (this.googleMap && locationbounds) {
              this.googleMap.setCenter(locationbounds.getCenter());         
          }
          this.googleMap.setZoom(5);
        }
      }, 500);
    }
  }

  searchDrivers(event: any): void {
    this.SearchedKeyword = event.target.value;
    this.filterDriverData();
  }


  refreshDatatable(): void {
    if (this.driverModal.modalDetails.display === "block") {
      this.showDriverDetails(this.driverModal.modalDetails.data);
    }
  }

  filterDriverData(): void {
    this.clearAllIntervals();
    this.searchLoadInterval = window.setTimeout(() => {
      this.getDispatcherLoads();
      this.autoRefreshLoads();
    }, 2000);
  }

  clearAllIntervals(): void {
    if (this.searchLoadInterval) {
      clearInterval(this.searchLoadInterval);
    }
    if (this.autoRefreshInterval) {
      clearInterval(this.autoRefreshInterval);
    }
    if (this.setCountryCenterInterval) {
      clearInterval(this.setCountryCenterInterval);
    }
    if (this.autoRefreshTimerInterval) {
      clearInterval(this.autoRefreshTimerInterval);
    }
  }

  getDispatcherLoads(statusId?): void {

    let inputs = {
      // FromDate: moment().format('MM/DD/YYYY'),
      // ToDate: moment().format('MM/DD/YYYY'),
      DriverSearch: this.SearchedKeyword,
      IsRequestFromDashboard: true,
      CountryId:this.SelectedCountryId,
    };
    this.loadingData = true;
    this.dispatcherService.getOnGoingLoadsForMap(inputs).subscribe((data) => {
      this.initailizeOnGoingLoad(data);
    });

  }

  private initailizeOnGoingLoad(data) {
    this.OnGoingLoads = data;// data.filter(x => x.Lat != null && x.Lng != null);

    this.Drivers = this.OnGoingLoads.filter((thing, i, arr) => {
      return arr.indexOf(arr.find(t => t.Id === thing.Id)) === i;
    });
    this.Drivers = this.Drivers.filter(x => x.Name != null && x.Name != undefined && x.Name.trim() != '');
    //last location not available
    this.OfflineDrivers = [];
    var driverFilter = [];
    data && data.map(m => {
      if (!driverFilter.find(f => f && f.Name == m.Name)) {
        driverFilter.push(m);
        if (m.Lat == null && m.Lng == null && m.Name != null && m.Name != undefined && m.Name.trim() != '')
          (this.Drivers && this.Drivers.filter(f => f.Name == m.Name).length > 0) ? '' : this.OfflineDrivers.push(m);
      }
    })
    this.setMapCenter();
    this.startAutoRefreshTimer();
    this.loadingData = false;
  }



  autoRefreshLoads(): void {
    this.autoRefreshInterval = window.setInterval(() => {
      if (IsUserActive()) {
        this.getDispatcherLoads();
      }
    }, this.AUTO_REFRESH_TIME * 1000);
  }

  startAutoRefreshTimer(): void {
    this.stopAutoRefreshTimer();
    this.autoRefreshTicks = this.AUTO_REFRESH_TIME;
    this.autoRefreshTimerInterval = window.setInterval(() => {
      if (IsUserActive()) {
        if (this.autoRefreshTicks == 0) {
          this.autoRefreshTicks = this.AUTO_REFRESH_TIME;
          this.stopAutoRefreshTimer();
        } else {
          this.autoRefreshTicks--;
        }
      }
    }, 1000);
  }

  stopAutoRefreshTimer(): void {
    if (this.autoRefreshTimerInterval) {
      clearInterval(this.autoRefreshTimerInterval);
    }
  }

  mapReady(map: any): void {
    this.googleMap = map;
    this.setMapCenter();
  }

  setZoomLevel(): void {
    if (this.OnGoingLoads.length == 0) {
      this.setMapCenter();
    } else {
      this.zoomLevel = 8; // default zoom level
    }
  }

  public toggleExpandMapView() {
    this.toogleExpandMap = !this.toogleExpandMap;
  }

  public toggleMapView() {
    this.toogleMap = !this.toogleMap;
  }

  public toggleGrids() {
    this.toogleGrid = !this.toogleGrid;
  }

  public toggleFilterView() {
    this.toogleFilter = !this.toogleFilter;
  }
  public toggleDriverView() {
    this.toogleDriver = !this.toogleDriver;
  }


  public mouseHoverMarker(infoWindow, event: MouseEvent): void {
    if (this.previousInfowindow && this.previousInfowindow.isOpen) {
      this.previousInfowindow.close();
    }
    if (infoWindow) {
      this.previousInfowindow = infoWindow;
      this.previousInfowindow.isOpen = true;
      infoWindow.open();
    }
  }

  public mouseHoveOutMarker(infoWindow, event: MouseEvent, index: number = null): void {
    if (this.previousInfowindow && this.previousInfowindow.isOpen && infoWindow) {
      this.previousInfowindow.close();
      this.previousInfowindow.isOpen = false;
    }
    if (infoWindow) {
      infoWindow.close();
    }
  }

  public showDriverDetails(driver: WhereIsMyDriverModel, infoWindow: any = null): void {
    window.scrollTo(0, 0);
    this.driverModal = { modalDetails: { display: 'block', data: driver } };
    if (infoWindow && infoWindow.isOpen) {
      infoWindow.close();
    }
    this.selectedDriverDetails = new DriverAdditionalDetails();
    this.modalData = true;
    this.dispatcherService.getDriverAdditionalDetails(driver.Id).subscribe(data => {
      if (data) {
        this.selectedDriverDetails = new DriverAdditionalDetails(data);
        this.modalData = false;
      }
      else {
        this.selectedDriverDetails = new DriverAdditionalDetails();
        Declarations.msgwarning('Please try again later.', 'Something Went Wrong', 3000);
        this.modalData = false;
      }
    });
  }

  public modalClose(): void {
    this.driverModal = { modalDetails: { display: 'none', data: new WhereIsMyDriverModel() } };
  }


  private closePreviousWindow(index: number): void {
    if (this.previousInfowindowIndex != null && this.previousInfowindow != null) {
      this.OnGoingLoads[this.previousInfowindowIndex].routeShow = false;
      if (this.previousInfowindow && this.previousInfowindow.isOpen)
        this.previousInfowindow.close();
      this.setMapCenter();
    }
  }
  public showHideRoutes(index: number): void {
    if (index == this.previousInfowindowIndex || this.previousInfowindowIndex == null) {
      this.OnGoingLoads[index].routeShow = !this.OnGoingLoads[index].routeShow;
      if (!this.OnGoingLoads[index].routeShow)
        this.setMapCenter();
    } else {
      this.closePreviousWindow(index);
    }
    this.previousInfowindowIndex = index;
  }

  filterMapByStatus(statusId) {
    this.selectedMaplable = statusId;
    this.getDispatcherLoads(statusId)

  }

  isArrayEqual(value, other) {
    var type = Object.prototype.toString.call(value);
    if (type !== Object.prototype.toString.call(other)) return false;
    if (['[object Array]', '[object Object]'].indexOf(type) < 0) return false;
    var valueLen = type === '[object Array]' ? value.length : Object.keys(value).length;
    var otherLen = type === '[object Array]' ? other.length : Object.keys(other).length;
    if (valueLen !== otherLen) return false;
    var compare = function (item1, item2) {
    };
    var match;
    if (type === '[object Array]') {
      for (var i = 0; i < valueLen; i++) {
        compare(value[i], other[i]);
      }
    } else {
      for (var key in value) {
        if (value.hasOwnProperty(key)) {
          compare(value[key], other[key]);
        }
      }
    }
    return true;

  }

}
