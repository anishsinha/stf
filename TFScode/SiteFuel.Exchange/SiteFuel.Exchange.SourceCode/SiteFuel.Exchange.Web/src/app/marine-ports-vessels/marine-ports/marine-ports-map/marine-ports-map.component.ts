import { Component, Input, OnChanges, OnInit, SimpleChanges, AfterViewInit } from '@angular/core';
import { MarinePortsandvesselsService } from '../../marine-portsandvessels.service'
import { MarinePortModel } from '../../models'
@Component({
  selector: 'app-marine-ports-map',
  templateUrl: './marine-ports-map.component.html',
  styleUrls: ['./marine-ports-map.component.css']
})
export class MarinePortsMapComponent implements OnInit, OnChanges, AfterViewInit{
    @Input() SelectedCountryId: any;
    public IsLoading: boolean = false;
    public marinePortsData: MarinePortModel[] = [];

    mapConstants: MapConstants = new MapConstants();
    public screenOptions = {
        position: 3
    };
    public Map: any;
    public zoomLevel = 5;

    constructor(private marineService: MarinePortsandvesselsService) { }

  ngOnInit(): void {
  }
  ngOnChanges(changes: SimpleChanges): void {
    if(changes.SelectedCountryId && changes.SelectedCountryId.currentValue){
        //get call for location data based on selected country id
        this.getMarinePortsData(changes.SelectedCountryId.currentValue);
    }
    }
    getMarinePortsData(countryId: number) {
        //let countryId = this.SelectedCountryId;
        this.IsLoading = true;
        this.marineService.getMarinePorts(countryId).subscribe((data) => {
            if (data) {
                this.IsLoading = false;
                this.marinePortsData = data;
                if (this.marinePortsData && this.marinePortsData.length == 0) {
                    if (this.SelectedCountryId == 2) { //canada
                        this.mapConstants.CenterLat = 56.14;
                        this.mapConstants.CenterLon = -106.34;
                    }        
                }
                
            }
        });
    }
    setMapCenter(countryId: number) {
        if (countryId == 2) {
            this.mapConstants.CenterLat = 56.14;
            this.mapConstants.CenterLon = -106.34;
        }
        else if (countryId == 4) {
            this.mapConstants.CenterLat = 13.193887;
            this.mapConstants.CenterLon = -59.543198;
        }
        else {
            this.mapConstants.CenterLat = 38;
            this.mapConstants.CenterLon = -98.35;
        }
    }
    ngAfterViewInit() {
        this.subscribeReloadMapSubject();
    }

    public mouseHoverMarker(infoWindow, event: MouseEvent): void {
        infoWindow.open();
    }
    public mouseHoveOutMarker(infoWindow, event: MouseEvent): void {
        infoWindow.close();
    }
    public mapReady(map: any): void {
        this.Map = map;
        if (this.SelectedCountryId == 2) {
            this.mapConstants.CenterLat = 57.88251631;
            this.mapConstants.CenterLon = -98.54842922;
        }
    }
    private subscribeReloadMapSubject(): void {
        let subs = this.marineService.reloadPortMapsSubject.subscribe(countryId=> {
            if (countryId) {
                this.getMarinePortsData(countryId);
            }
        });       
    }

}
export class MapConstants {
    CenterLat: number;
    CenterLon: number;
    ZoomArea: number;
    IconUrl: MapIconUrl;

    constructor() {
        this.CenterLat = 38;
        this.CenterLon = -98.35;
        this.ZoomArea = 15;
        this.IconUrl = { url: 'https://maps.google.com/mapfiles/ms/icons/blue-dot.png', scaledSize: { width: 40, height: 40 } }
    }
}

export class MapIconUrl {
    url: string;
    scaledSize: MapIconSize;
}
export class MapIconSize {
    width: number;
    height: number
}