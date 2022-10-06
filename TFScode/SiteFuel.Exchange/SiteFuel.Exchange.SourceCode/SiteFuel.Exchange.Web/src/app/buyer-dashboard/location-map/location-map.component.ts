import { Component, Input, OnChanges, OnDestroy, OnInit, SimpleChanges, ViewChild } from '@angular/core';
import { DataTableDirective } from 'angular-datatables';
import { JobLocationDetailsModal } from 'src/app/buyer-wally-board/Models/BuyerWallyBoard';
import { DashboardService } from '../dashboard.service';
import { JobBuyerDashboardViewModel } from '../Model/DashboardModel';
export declare var google: any;
declare var currentCompanyId: any;

@Component({
    selector: 'app-location-map',
    templateUrl: './location-map.component.html',
    styleUrls: ['./location-map.component.scss']
})
export class LocationMapComponent implements OnDestroy, OnChanges {
    //@ViewChild(DataTableDirective)
    @ViewChild(DataTableDirective) datatableElement: DataTableDirective;
    @Input() SelectedCountryId: any
    public Map: any;
    public isLoading = false;
    public zoomLevel = 5;
    public dtOptions: any = {};
    public jobLocationData: JobBuyerDashboardViewModel[] = [];
    public jobLocationDataForMap: JobBuyerDashboardViewModel[] = [];
    public opendedJobDetails: JobBuyerDashboardViewModel;
    private setCountryCenterInterval: any;
    public UoM: string = '';
    public clickViewActive: Boolean = false;
    public toogleMap: Boolean = true;
    public centerLocationLat = 47.1853106;
    public centerLocationLog = -125.36955;
    private UserCountry = "USA";
    private CountryCentre = {
        USA: { lat: 39.11757961, lng: -103.8784 },
        CAN: { lat: 57.88251631, lng: -98.54842922 }
    };
    public screenOptions = {
        position: 3
    };
    public mustGoUrl = "src/assets/marker-mustgo.svg";
    public shouldGoUrl = "src/assets/marker-shouldgo.svg";
    public couldGoUrl = "src/assets/marker-couldgo.svg";
    public noDlrUrl = "src/assets/marker-nodr.svg";
    public noImageUrl = "Content/images/no-image.png";

    public CurrentCompanyId: any;
    constructor(private _dashboard: DashboardService) {
    }

    ngOnChanges(changes: SimpleChanges): void {
        if (changes.SelectedCountryId && changes.SelectedCountryId.currentValue) {
            this.fetchJobLocationData();
        }
    }
    ngOnDestroy(): void {

    }
    private fetchJobLocationData(): void {
        this.isLoading = true;
        this._dashboard.getJobDetailsForBuyerDashboard(this.SelectedCountryId).subscribe(res => {
            if (res) {
                this.jobLocationData = this.checkMostPriorityJob(res);
                this.jobLocationDataForMap = this.jobLocationData;
            }
            this.setCountryCentre();
            this.isLoading = false;
        });
    }

    private checkMostPriorityJob(jobLocationData: JobLocationDetailsModal[]): JobLocationDetailsModal[] {
        const jobLocationLength = jobLocationData.length;
        for (let i = 0; i < jobLocationLength; i++) {
            let deliveryRequests = jobLocationData[i].jobDeliveryRequests;
            if (deliveryRequests.length) {
                let filteredMustGoDRs = deliveryRequests.filter((data) => data.Priority === 1);
                let filteredShoudGoDRs = deliveryRequests.filter((data) => data.Priority === 2);
                let filteredCouldGoDRs = deliveryRequests.filter((data) => data.Priority === 3);
                if (filteredMustGoDRs.length > 0) {
                    jobLocationData[i].highestPriority = 1;
                    jobLocationData[i].iconUrl = this.mustGoUrl;
                }
                else if (filteredShoudGoDRs.length > 0) {
                    jobLocationData[i].highestPriority = 2;
                    jobLocationData[i].iconUrl = this.shouldGoUrl;
                }
                else {
                    jobLocationData[i].highestPriority = 3;
                    jobLocationData[i].iconUrl = this.couldGoUrl;
                }
            } else {
                jobLocationData[i].highestPriority = 4;
                jobLocationData[i].iconUrl = this.noDlrUrl;
            }
        }
        return jobLocationData;
    }

    private setCountryCentre(): void {
        if (this.UserCountry != "") {
            this.setCountryCenterInterval = window.setTimeout(() => {
                this.centerLocationLat = this.CountryCentre[this.UserCountry].lat;
                this.centerLocationLog = this.CountryCentre[this.UserCountry].lng;
                if (this.Map && this.jobLocationData.length == 0) {
                    this.Map.setCenter(new google.maps.LatLng(this.centerLocationLat, this.centerLocationLog));
                    this.Map.setZoom(5);
                } else {
                    const bounds = new google.maps.LatLngBounds();
                    this.jobLocationData.forEach((x: JobLocationDetailsModal) => {
                        bounds.extend(new google.maps.LatLng(x.Latitude, x.Longitude));
                    });
                    if (this.Map && bounds) {
                        this.Map.fitBounds(bounds);
                        this.Map.setCenter(bounds.getCenter()); 
                        this.Map.setZoom(5);        
                    }
            
                }
            }, 500);
        }
    }

    private setZoomLevel(): void {
        if (this.jobLocationData.length == 0) {
            this.setCountryCentre();
        } else {
            //this.zoomLevel = 10;
        }
    }

    public mouseHoverMarker(infoWindow, event: MouseEvent): void {
        infoWindow.open();
    }
    public mouseHoveOutMarker(infoWindow, event: MouseEvent): void {
        infoWindow.close();
    }

    public closeViewClicked(): void {
        this.clickViewActive = false;
    }

    public toggleMapView(): void {
        this.toogleMap = !this.toogleMap;
    }

    public onInfoViewClick(jobLocation: JobLocationDetailsModal): void {
        window.scrollTo(0, 0);
        this.opendedJobDetails = jobLocation;
        if ((this.opendedJobDetails.CountryCode === 'USA') || ((this.opendedJobDetails.CountryCode === 'US'))) {
            this.UoM = 'Gallons';
        }
        else {
            this.UoM = 'Litres';
        }
        this.clickViewActive = true;
        this.toogleMap = true;
    }

    public mapReady(map: any): void {
        this.Map = map;
        this.setCountryCentre();
    }

    public setCenterMap($event): void {
        if (this.UserCountry && !this.jobLocationData.length) {
            this.centerLocationLat = this.CountryCentre[this.UserCountry].lat;
            this.centerLocationLog = this.CountryCentre[this.UserCountry].lng;
            if (this.Map) {
                this.Map.setCenter({ lat: this.centerLocationLat, lng: this.centerLocationLog });
                this.Map.setZoom(5);
            }
        }
    }
}