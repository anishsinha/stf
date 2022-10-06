import { Component, OnInit, ViewChildren, QueryList, ViewChild, AfterViewInit, OnDestroy } from '@angular/core';
import { LocationService } from '../../services/location.service';
import { Subject } from 'rxjs';
import { DataTableDirective } from 'angular-datatables';
import { LocationDetailsModel, Country } from '../../models/location';
import { DataTablesResponse } from '../../../shared/models/DataTable-models';
export declare var google: any;

@Component({
    selector: 'app-terminals',
    templateUrl: './terminals.component.html',
    styleUrls: ['./terminals.component.css']
})

export class TerminalsComponent implements OnInit, AfterViewInit, OnDestroy {
    public dtOptions: any = {};
    public dtTrigger: Subject<any> = new Subject();
    public terminals: LocationDetailsModel[] = [];
    public mapTerminals: LocationDetailsModel[] = [];
    public IsLoading: boolean;

    @ViewChild(DataTableDirective) dtElement: DataTableDirective;

    public zoomLevel = 4;
    public toogleMap: Boolean = true;
    public googleMap: any;
    public screenOptions = { position: 3 };
    public centerLocationLat = 47.1853106;
    public centerLocationLog = -125.36955;
    private CountryCentre = {
        USA: { lat: 39.11757961, lng: -103.8784 },
        CAN: { lat: 57.88251631, lng: -98.54842922 }
    };

    constructor(private locationService: LocationService) { }


    ngOnInit() {
        let exportColumns = { columns: ':visible' };
        this.dtOptions = {
            pagingType: 'simple_numbers',
            pageLength: 5,
            lengthMenu: [[5, 10, 25, 50], [5, 10, 25, 50]],
            serverSide: true,
            processing: true,
            ajax: (dataTablesParameters: any, callback) => {
                let requestModel = dataTablesParameters;
                requestModel['CountryId'] = this.getCountryFilter();
                this.locationService.getTerminals(requestModel).subscribe((resp: DataTablesResponse) => {
                    this.terminals = resp.data;
                    this.SetMapTerminals();
                        callback({
                            recordsTotal: resp.recordsTotal,
                            recordsFiltered: resp.recordsFiltered,
                            data: resp.data
                        });
                    });
            },
            dom: '<"html5buttons"B>lTfgitp',
            buttons: [
                { extend: 'copy', exportOptions: exportColumns },
                { extend: 'csv', title: 'Terminals Detail', exportOptions: exportColumns },
                { extend: 'pdf', title: 'Terminals Detail', orientation: 'landscape', exportOptions: exportColumns },
                { extend: 'print', exportOptions: exportColumns }
            ],
            columns: [
                { title: 'Name', data: 'Name', name: 'Name' },
                { title: 'Abbreviation', data: 'Abbreviation', name: 'Abbreviation'  }, 
                { title: 'Control Number', data: 'ControlNumber', name: 'ControlNumber' }, 
                { title: 'Address', data: 'Address', name: 'Address' }, 
                { title: 'City', data: 'City', name: 'City' }, 
                { title: 'State', data: 'StateCode', name: 'StateCode' }]
        };
    }
    ngAfterViewInit(): void {
        this.dtTrigger.next();
    }

    ngOnDestroy(): void {
        // Do not forget to unsubscribe the event
        this.dtTrigger.unsubscribe();
    }
    SetMapTerminals() {
        this.mapTerminals = this.terminals.filter(t => t.Latitude != 0 && t.Longitude != 0);
    }

    loadDataTable(): void {
        this.refreshDatatable();
    }

    refreshDatatable() {
        if (this.dtElement.dtInstance) {
            this.dtElement.dtInstance.then((dtInstance: DataTables.Api) => {
                dtInstance.draw();
            });
        }
    }

    getCountryFilter(): any {
        return (localStorage.getItem('countryFilterType')) ? (localStorage.getItem('countryFilterType')) : (localStorage.getItem('countryIdForDashboard') ? localStorage.getItem('countryIdForDashboard') : 1);
    }

    mapReady(map: any): void {
        this.googleMap = map;
        this.setMapCenter();
    }

    public setCenterMap($event): void {
        if (!this.mapTerminals.length) {
        let selectedCountryId = this.getCountryFilter();
        this.centerLocationLat = this.CountryCentre[Country[selectedCountryId]].lat;
        this.centerLocationLog = this.CountryCentre[Country[selectedCountryId]].lng;
        if (this.googleMap) {
            this.googleMap.setCenter({ lat: this.centerLocationLat, lng: this.centerLocationLog });
            this.googleMap.setZoom(4);
            }
        }
    }

    setMapCenter(): void {
        let selectedCountryId = this.getCountryFilter();
        this.centerLocationLat = this.CountryCentre[Country[selectedCountryId]].lat;
        this.centerLocationLog = this.CountryCentre[Country[selectedCountryId]].lng;
        if (this.googleMap && this.terminals.length == 0 && this.terminals.length == 0) {
            const bounds = new google.maps.LatLngBounds();
            bounds.extend(new google.maps.LatLng(this.centerLocationLat, this.centerLocationLog));
            this.googleMap.fitBounds(bounds);
            this.googleMap.setZoom(4);
        } else {
            const bounds = new google.maps.LatLngBounds();
            this.terminals.forEach(x => {
                bounds.extend(new google.maps.LatLng(x.Latitude, x.Longitude));
            });
            this.googleMap.fitBounds(bounds);
        }
    }
}
