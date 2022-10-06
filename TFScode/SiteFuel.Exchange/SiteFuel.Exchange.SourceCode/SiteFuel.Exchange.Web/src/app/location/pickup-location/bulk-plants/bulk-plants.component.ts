import { Component, OnInit, ViewChildren, QueryList, ElementRef, ViewChild } from '@angular/core';
import { LocationService } from '../../services/location.service';
import { Subject } from 'rxjs';
import { DataTableDirective } from 'angular-datatables';
import { LocationDetailsModel, Country } from '../../models/location';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { DropdownItem, StateDropdownExtendedItem, StatelistService } from '../../../statelist.service';
import { AddressService } from '../../../address.service';
import { Declarations } from '../../../declarations.module';

export declare var google: any;

@Component({
  selector: 'app-bulk-plants',
  templateUrl: './bulk-plants.component.html',
  styleUrls: ['./bulk-plants.component.css']
})

export class BulkPlantsComponent implements OnInit {
    public dtOptions: any = {};
    public dtTrigger: Subject<any> = new Subject();
    public locations: LocationDetailsModel[] = [];
    public IsLoading: boolean;
    @ViewChildren(DataTableDirective) dtElements: QueryList<DataTableDirective>;
    public PickupForm: FormGroup;
    public StateList: StateDropdownExtendedItem[] = [];
    public CountryList: DropdownItem[] = [];
    public CountryGroupList: DropdownItem[] = [];
    public _loadingAddress: boolean = false;
    //public GridName: typeof GridName = GridName; 

    @ViewChild('closePickUpModal') closePickUpModal: ElementRef;

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
    constructor(private locationSercice: LocationService, private fb: FormBuilder, private stateService: StatelistService, private addresService: AddressService) { }

    ngOnInit() {
        this.PickupForm = this.initPickupForm(new LocationDetailsModel());
        let countryId = this.getCountryFilter();
        this.setAddressValidator(countryId);
        this.stateService.getStates().subscribe(x => this.StateList = x);
        this.stateService.getCountries().subscribe(x => this.CountryList = x);
        this.stateService.getCountryGroup(Country.CAR).subscribe(x => this.CountryGroupList = x);
        let exportColumns = { columns: ':visible' };
        this.dtOptions = {
            pagingType: 'simple_numbers',
            pageLength: 10,
            lengthMenu: [[10, 25, 50, 100, -1], [10, 25, 50, 100, "All"]],
            searching: true,
            destroy: true,
            dom: '<"html5buttons"B>lTfgitp',
            buttons: [
                { extend: 'copy', exportOptions: exportColumns },
                { extend: 'csv', title: 'Terminals Detail', exportOptions: exportColumns },
                { extend: 'pdf', title: 'Terminals Detail', orientation: 'landscape', exportOptions: exportColumns },
                { extend: 'print', exportOptions: exportColumns }
            ],
        };

        this.loadDataTable();    
    }

    loadDataTable(): void {
        this.IsLoading = true;
        this.locationSercice.GetBulkPlants(this.getCountryFilter()).subscribe((data) => {
            if (data != null) {
                this.locations = data;
                this.refreshDatatable();
            }
            this.IsLoading = false;
        });
    }
    refreshDatatable(): void {
        this.dtElements.forEach((dtElement: DataTableDirective) => {
            if (dtElement.dtInstance) {
                dtElement.dtInstance.then((dtInstance: DataTables.Api) => { dtInstance.destroy(); });
            }
        });
        this.dtTrigger.next();
    }

    getCountryFilter(): any{
        return (localStorage.getItem('countryFilterType')) ? (localStorage.getItem('countryFilterType')) : (localStorage.getItem('countryIdForDashboard') ? localStorage.getItem('countryIdForDashboard') : 1);
    }
    
    get StatesListByCountry(): any[] {
        let countryCode = this.PickupForm.get('CountryCode').value;
        if (countryCode && this.CountryList && this.CountryList.length > 0) {

            countryCode = countryCode == "US" ? "USA" : countryCode;
            let countryId = 0;
            let county = this.CountryList.find(c => c.Code == countryCode);
            if(county && county.Id)
                countryId = county.Id;
                
            if (countryId == Country.CAR) {
                let countryGroupId = this.PickupForm.get("CountryGroupId").value;
                return this.StateList.filter(t => t.CountryId == countryId && (countryGroupId ==0 || t.CountryGroupId == countryGroupId));
            }
            else {
                return this.StateList.filter(t => t.CountryId == countryId);
            }
        }
    }
    
    initPickupForm(loc: LocationDetailsModel): FormGroup {
        let _pForm = this.fb.group({
            Address: this.fb.control(loc.Address),
            City: this.fb.control(loc.City),
            StateId: this.fb.control(loc.StateId, [Validators.required]),
            StateCode: this.fb.control(loc.StateCode, [Validators.required]),
            CountryCode: this.fb.control(loc.CountryCode, [Validators.required]),
            CountryId: this.fb.control(loc.CountryId, [Validators.required]),
            ZipCode: this.fb.control(loc.ZipCode),
            County: this.fb.control(loc.County),
            Latitude: this.fb.control(loc.Latitude),
            Longitude: this.fb.control(loc.Longitude),
            Name: this.fb.control(loc.Name, [Validators.required]),
            CountryGroupId: this.fb.control(loc.CountryGroupId)
        });
        return _pForm;
    }

    setAddressValidator(countryId: any) {
        if (countryId == Country.CAR) {
            this.PickupForm.get('Address').clearValidators();
            this.PickupForm.get('Address').updateValueAndValidity();

            this.PickupForm.get('City').clearValidators();
            this.PickupForm.get('City').updateValueAndValidity();

            this.PickupForm.get('ZipCode').clearValidators();
            this.PickupForm.get('ZipCode').updateValueAndValidity();

            //this.PickupForm.get('County').clearValidators();
            //this.PickupForm.get('County').updateValueAndValidity();

            this.PickupForm.get('Latitude').clearValidators();
            this.PickupForm.get('Latitude').updateValueAndValidity();

            this.PickupForm.get('Longitude').clearValidators();
            this.PickupForm.get('Longitude').updateValueAndValidity();
        }
        else {
            let validator = [Validators.required];
            this.PickupForm.get('Address').setValidators(validator);
            this.PickupForm.get('Address').updateValueAndValidity();

            this.PickupForm.get('City').setValidators(validator);
            this.PickupForm.get('City').updateValueAndValidity();

            this.PickupForm.get('ZipCode').setValidators(validator);
            this.PickupForm.get('ZipCode').updateValueAndValidity();

            //this.PickupForm.get('County').setValidators(validator);
            //this.PickupForm.get('County').updateValueAndValidity();

            this.PickupForm.get('Latitude').setValidators(validator);
            this.PickupForm.get('Latitude').updateValueAndValidity();

            this.PickupForm.get('Longitude').setValidators(validator);
            this.PickupForm.get('Longitude').updateValueAndValidity();


        }
    }
    
    getAddressByZip(event: any): void {
        const zipCode: string = event.target.value;
        //const regexUsa = new RegExp(this.regexUsaZip);
        //const regexCan = new RegExp(this.regexCanZip);
        if (zipCode.length > 2) {
            this._loadingAddress = true;
            this.addresService.getAddress(zipCode)
                .subscribe(data => {
                    this._loadingAddress = true;
                    const _address = this.PickupForm.get('Address');
                    if (data != null && data != undefined && data.CountryCode != null) {
                        this.setCountryCode(data);
                        data.Address = _address.value;
                        let countryGroupId = null;
                        const state = this.StateList.find(x => x.Code == data.StateCode);
                        let country = this.CountryList.find(x => x.Code == data.CountryCode);
                        let countrygroup = new DropdownItem();
                        if (country && country.Id > 0) {
                            countryGroupId = 1;
                            countrygroup.Id = 1;
                        } else {
                            countrygroup = this.CountryGroupList.find(x => x.Code == data.CountryCode);
                            country = new DropdownItem();
                            country.Id = 4;
                            country.Code = "CAR";
                        }
                        this.PickupForm.patchValue({
                            City: data.City,
                            StateId: state ? state.Id : null,
                            StateCode: data.StateCode,
                            CountryId: country.Id, 
                            CountryCode: country.Code,
                            CountryGroupId: countrygroup.Id,
                            ZipCode: data.ZipCode,
                            County: data.CountyName,
                            Latitude: data.Latitude,
                            Longitude: data.Longitude
                        });
                        this.PickupForm.markAllAsTouched();
                        this.PickupForm.markAsDirty();
                    }
                    this._loadingAddress = false;
                });
        }
    }
    setCountryCode(data) {
        if (data.CountryCode == 'US')
        {
            data.CountryCode = 'USA'
        }
        else if (data.CountryCode == 'CA')
        {
            data.CountryCode = 'CAN'
        }
    }
    
    setStateCode(event: any) {
        this.PickupForm.get('StateCode').setValue(event.target.selectedOptions[0].text);
    }

    mapReady(map: any): void {
        this.googleMap = map;
        this.setMapCenter();
    }

    public setCenterMap($event): void {
        if (!this.locations.length) {
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
        if (this.googleMap && this.locations.length == 0 && this.locations.length == 0) {
            const bounds = new google.maps.LatLngBounds();
            bounds.extend(new google.maps.LatLng(this.centerLocationLat, this.centerLocationLog));
            this.googleMap.fitBounds(bounds);
            this.googleMap.setZoom(4);
        } else {
            const bounds = new google.maps.LatLngBounds();
            this.locations.forEach(x => {
                bounds.extend(new google.maps.LatLng(x.Latitude, x.Longitude));
            });
            this.googleMap.fitBounds(bounds);
        }
    }

    savePickupLocation(): void {
        this.locationSercice.PostBulkPlantLocation(this.PickupForm.value).subscribe((response) => {
            if (response != null && response.StatusCode == 0) {
                Declarations.msgsuccess(response.StatusMessage, undefined, undefined);
                this.closePickUpModal.nativeElement.click();
                this.loadDataTable();
            } else {
                Declarations.msgerror(response.StatusMessage, undefined, undefined);
            }
        });
    }

    clearPickUpform() {
        this.PickupForm.reset();
    }

    countryChanged() {
        let countryId = 1;
        let countryCode = this.PickupForm.get('CountryCode').value;
        if (countryCode) {
            if (countryCode == "CAN") {
                countryId = 2;
            }
            else if (countryCode == "CAR") {
                countryId = 4;
            }
            this.setAddressValidator(countryId);
            this.PickupForm.get('CountryId').setValue(countryId);
        }
    }
}