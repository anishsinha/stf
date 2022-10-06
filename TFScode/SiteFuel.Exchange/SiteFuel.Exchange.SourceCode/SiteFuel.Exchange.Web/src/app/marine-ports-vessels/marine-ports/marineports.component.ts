import { Component, Input, OnChanges, OnDestroy, OnInit, SimpleChange, SimpleChanges } from '@angular/core';
import { MarinePortModel, Geocode } from '../models';
import { SalesUserService } from '../../sales-user/sales-user.service'
import { ConfirmationDialogService } from '../../shared-components/confirmation-dialog/confirmation-dialog.service';

import { Subject } from 'rxjs';
import { MarinePortsandvesselsService } from '../marine-portsandvessels.service';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { Country } from 'src/app/self-service-alias/models/location';
import { Declarations } from 'src/app/declarations.module';
import {  DropdownItem } from 'src/app/statelist.service';

@Component({
  selector: 'app-marineports',
  templateUrl: './marineports.component.html',
  styleUrls: ['./marineports.component.css']
})
export class MarineportsComponent implements OnInit ,OnChanges,OnDestroy{
    @Input() SelectedCountryId: any;

  public ModalText: string;

   public MarinePortsData: MarinePortModel[] = [];
    public MarinePort: MarinePortModel = new MarinePortModel();
  dtOptions: any = {};
  dtTrigger: Subject<any> = new Subject();

  public popoverTitle: string = 'Are you sure?';
  public confirmButtonText: string = 'Yes';
  public cancelButtonText: string = 'No';

    portCreateForm: FormGroup;

    mapConstants: MapConstants = new MapConstants();

  public countryList = [];
  public currucyList = [];
  public statesList = [];
  public filteredStatesList = [];
  public countryGroupList = [];
    public currentCountryId: any;

    public IsLoading: boolean = false;

   // public defaultCountryGroupDDLValue: DropdownItem = new DropdownItem;

    constructor(private marineService: MarinePortsandvesselsService, private fb: FormBuilder,
        private salesService: SalesUserService, private confirmationdialogueservice: ConfirmationDialogService) { }
  ngOnInit(): void {
    this.ModalText = 'Create Port';
    var exportColumns = { columns: [0, 1, 2, 3, 4, 5, 6, 7] };
    this.dtOptions = {
      dom: '<"html5buttons"B>lTfgitp',
      buttons: [
          { extend: 'colvis' },
          { extend: 'copy', exportOptions: exportColumns },
          { extend: 'csv', title: 'Port Details', exportOptions: exportColumns },
          { extend: 'pdf', title: 'Port Details', orientation: 'landscape', exportOptions: exportColumns },
          { extend: 'print', exportOptions: exportColumns }
      ],
      pagingType: 'first_last_numbers',
      pageLength: 10,
      ordering: true,
      lengthMenu: [[10, 25, 50, 100, -1], [10, 25, 50, 100, "All"]]
    };
      //this.getUoMList();
      //this.getCurrecyList();
      this.getCountryList();
      this.getcountryGroupList();
      this.getStatesOfAllCountries();
      this.MarinePort.CountryId = this.SelectedCountryId;
      if (this.SelectedCountryId == 2) { //canada
          this.mapConstants.CenterLat = 56.14;
          this.mapConstants.CenterLon = -106.34;
      }
      this.initializePortCreationForm(this.MarinePort);
      //this.filterStateList(this.MarinePort.CountryId);
     // this.defaultCountryGroupDDLValue.Id = 0;
      jQuery("#AddressDetails_Country_Id").val("0").change();
  }
  ngOnChanges(changes: SimpleChanges): void {
    if(changes.SelectedCountryId && changes.SelectedCountryId.currentValue){
       // get call for grid data
        this.getMarinePortsData();
        this.MarinePort.CountryId = this.SelectedCountryId;       
        this.setMapCenter(this.SelectedCountryId);
        this.setAddressValidators(this.SelectedCountryId);
        jQuery("#AddressDetails_Country_Id").val("0").change();
    }
  }
    createPort(header: string) {     
        this.ModalText = header;
        this.filteredStatesList = this.statesList.filter(s => s.CountryId == this.SelectedCountryId) || [];
        this.portCreateForm.get('CountryId').setValue(this.SelectedCountryId);
        this.portCreateForm.get('IsGeocodeUsed').setValue(false);
        this.setLatLongValidator(false);
        this.setMapCenter(this.SelectedCountryId);
        jQuery("#AddressDetails_Country_Id").val("0").change();
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

  getMarinePortsData(){
      let countryId = this.SelectedCountryId;
      this.IsLoading = true;
    this.marineService.getMarinePorts(countryId).subscribe((data)=>{
        if (data) {
            jQuery("#port-datatable").DataTable().clear().destroy();
            this.MarinePortsData = data;
            this.dtTrigger.next();
            this.IsLoading = false;
        }
        
    });
  }
    initializePortCreationForm(port: MarinePortModel): FormGroup {        
         this.portCreateForm = this.fb.group({
            JobID: this.fb.control(port.JobID),
            JobName: this.fb.control(port.JobName, [Validators.required] ),       
            Address: this.fb.control(port.Address),
            City: this.fb.control(port.City),
            CountyName: this.fb.control(port.CountyName),
            StateId: this.fb.control(port.StateID, [Validators.required]),
            CountryId: this.fb.control(port.CountryId, [Validators.required]),                       
            StateName: this.fb.control(port.State),
            ZipCode: this.fb.control(port.ZipCode),
            IsGeocodeUsed: this.fb.control(port.IsGeoCoded),
            Latitude: this.fb.control(port.Latitude, Validators.pattern('^[0-9.-]*$')),
            Longitude: this.fb.control(port.Longitude, Validators.pattern('^[0-9.-]*$')), 
        })
        this.setAddressValidators(port.CountryId);
        return this.portCreateForm;
        
    }
    editPort(port) {
        if (port) {
            this.portCreateForm.get('JobID').setValue(port.JobID);
            this.portCreateForm.get('JobName').setValue(port.JobName);
            this.portCreateForm.get('Address').setValue(port.Address);
            this.portCreateForm.get('City').setValue(port.City);
            this.portCreateForm.get('CountyName').setValue(port.CountyName);
            this.portCreateForm.get('StateId').setValue(port.StateID);
            this.portCreateForm.get('CountryId').setValue(port.CountryId);
            this.portCreateForm.get('StateName').setValue(port.State);
            this.portCreateForm.get('ZipCode').setValue(port.ZipCode);
            this.portCreateForm.get('IsGeocodeUsed').setValue(port.IsGeocodeUsed);
            this.portCreateForm.get('Latitude').setValue(port.Latitude);
            this.portCreateForm.get('Longitude').setValue(port.Longitude);
            if (port.Latitude) {
                this.mapConstants.CenterLat = parseFloat(port.Latitude);
                this.mapConstants.CenterLon = parseFloat(port.Longitude);
            }
            let countryId = this.portCreateForm.get('CountryId').value;
            this.setAddressValidators(countryId);
            jQuery("#AddressDetails_Country_Id").val("0").change();
        }
  }
    deletePort(port) {
        if (port && port.JobID) {
            this.marineService.deletePort(port.JobID).subscribe(data => {
                if (data != null && data.StatusCode == 0) {
                    Declarations.msgsuccess(data.StatusMessage, undefined, undefined);                   
                    this.getMarinePortsData();
                    this.marineService.setReloadMapSubject(port.CountryId);
                }
                else {
                    Declarations.msgerror(data == null ? 'Failed' : data.StatusMessage, undefined, undefined);
                }
         });
        }
    }
    filterStateList(countryId: number) {
        this.filteredStatesList = this.statesList.filter(s => s.CountryId == this.SelectedCountryId) || [];
    }

    clearPanelData() {
        this.portCreateForm.reset();
        this.portCreateForm.get('CountryId').setValue(this.SelectedCountryId);
        this.portCreateForm.get('StateId').setValue(null);
        this.filteredStatesList = this.statesList.filter(s => s.CountryId == this.SelectedCountryId) || [];
        this.setMapCenter(this.SelectedCountryId);
        jQuery("#AddressDetails_Country_Id").val("0").change();
        this.setLatLongValidator(false);

  }
    //public getUoMList() {
    //  this.salesUserService.GetUoMList().subscribe(data => {
    //        if (data && data.length > 0) {
    //            this.UomList = data;
    //        }
    //    });
    //}
    //public getCurrecyList() {
    //    this.salesUserService.GetCurrenyList().subscribe(data => {
    //        if (data && data.length > 0) {
    //            this.currucyList = data;
    //        }
    //    });
    //}
    public getCountryList() {
        this.marineService.getCountryList().subscribe(data => {
            if (data && data.length > 0) {
                this.countryList = data;
            }
        });
    }
    public getcountryGroupList() {
        this.marineService.getCountryGroupList(4).subscribe(data => {
            if (data && data.length > 0) {
                this.countryGroupList = data;
            }
        });
    }
    countryChanged() {
        this.portCreateForm.get('StateId').setValue(null)
        this.filteredStatesList = this.statesList.filter(s => s.CountryId == this.portCreateForm.get('CountryId').value) || [];
        let countryId = this.portCreateForm.get('CountryId').value;
        this.setAddressValidators(countryId);
        this.setMapCenter(countryId);
    }

    countryGroupChanged(selectedCountryGroupId: any) {
        if (selectedCountryGroupId) {
            var countryGroup = selectedCountryGroupId.target.value;
            if (countryGroup === '0') {
                this.filteredStatesList = this.statesList.filter(s => s.CountryId == this.portCreateForm.get('CountryId').value) || [];
                this.portCreateForm.get('StateId').setValue(null)
            }
            else {
                this.filteredStatesList = this.statesList.filter(s => s.CountryGroupId == countryGroup) || [];
                this.portCreateForm.get('StateId').setValue(null)
            }
        }
    }
    public getStatesOfAllCountries(countryId?: number) {
        this.marineService.GetStatesOfAllCountries(countryId).subscribe(data => {
            if (data && data.length > 0) {
                this.statesList = data;
                this.filteredStatesList = this.statesList;
            }
        });
    }
    public getAddressByZip() {
        var zipCode = this.portCreateForm.get('ZipCode').value;
        if (zipCode) {
            this.marineService.GetAddressByZip(zipCode).subscribe(data => {
                if (data) {
                    var country = this.countryList.find(t => t.Code.includes(data.CountryCode));
                    if (country) {
                        var countryId = country.Id;
                        this.portCreateForm.get('CountryId').patchValue(countryId);
                        //Commented below line for Impediment 30459-P2-Parkland US/MFN - Ports Address Misbehaves
                        //this.portCreateForm.get('Address').patchValue(data.Address);
                        this.portCreateForm.get('CountyName').patchValue(data.CountyName);
                        this.portCreateForm.get('City').patchValue(data.City);
                        var stateId = this.statesList.find(x => x.StateCode == data.StateCode).StateId;
                        this.portCreateForm.get('StateId').patchValue(stateId);                       
                        this.portCreateForm.get('Latitude').patchValue(data.Latitude);
                        this.portCreateForm.get('Longitude').patchValue(data.Longitude);
                        this.mapConstants.CenterLat = data.Latitude;
                        this.mapConstants.CenterLon = data.Longitude;
                        this.filteredStatesList = this.statesList.filter(s => s.CountryId == countryId) || [];
                    }
                }
            });
        }

    }
    markerDragEnd(event) {
        this.confirmationdialogueservice.confirm('Map update', 'Geo Codes shifted to a new location!')
            .then((confirmed) => (confirmed == true) ? this.updateGeoCode(event.coords.lat, event.coords.lng) : this.previousLatLon())
            .catch(() => this.previousLatLon());
    }
    updateGeoCode(lat, lng) {
        this.salesService.GetAddressByLongLat(lat, lng).subscribe(data => {
            if (data) {
                data.Latitude = parseFloat(lat);
                data.Longitude = parseFloat(lng);
                this.updateAddressData(data);
            }
            else { // if no address fetched for lat-long then set only map marker on UI
                this.mapConstants.CenterLat = lat;
                this.mapConstants.CenterLon = lng;
            }
           
        })
    }
    public previousLatLon() {
        this.mapConstants.CenterLat = this.portCreateForm.get('Latitude').value;
        this.mapConstants.CenterLon = this.portCreateForm.get('Longitude').value;
    }

    updateAddressData(address: Geocode) {
        let countryId = (address.CountryCode == 'US' || address.CountryCode == 'USA') ? 1 : (address.CountryCode == 'CA' || address.CountryCode == 'CAN') ? 2 : this.portCreateForm.get('CountryId').value;
        let stateName = (address.StateName != null && address.StateName != '' && address.StateName != undefined) ? address.StateName : address.CountryName;

        if (stateName) {
            let state = this.statesList.find(st => st.StateName.toLowerCase() == stateName.toLowerCase());
            let stateId = (state && state.StateId) ? state.StateId : this.portCreateForm.get('StateId').value;
            this.portCreateForm.get('StateId').patchValue(stateId);
        }
        else //set first state after filtering by countryID
        {
            let states = this.statesList.filter(s => s.CountryId == countryId) || [];
            if (states && states.length > 0) {
                let stateId = states[0].StateId;
                if (stateId) {
                    this.portCreateForm.get('StateId').patchValue(stateId);
                }
            }
        }
        
        this.portCreateForm.get('Address').patchValue(address.Address);
        this.portCreateForm.get('City').patchValue(address.City);
        this.portCreateForm.get('ZipCode').patchValue(address.ZipCode);
       
        this.portCreateForm.get('CountryId').patchValue(countryId);

        this.portCreateForm.get('CountyName').patchValue(address.CountyName);
        //this.portCreateForm.get('CountryCode').patchValue(address.CountryCode);     
        //this.portCreateForm.get('StateName').patchValue(address.StateName);
        

        if (address.Latitude) {
            this.portCreateForm.get('Latitude').patchValue(address.Latitude);
            this.portCreateForm.get('Longitude').patchValue(address.Longitude);
            this.mapConstants.CenterLat = address.Latitude;
            this.mapConstants.CenterLon = address.Longitude;
        }

        this.filteredStatesList = this.statesList.filter(s => s.CountryId == countryId) || [];

    }
    setLatLongValidator(isGeoCoded: any) {
        if (isGeoCoded) {
            let val = [Validators.required];
            this.portCreateForm.get('Latitude').setValidators(val);
            this.portCreateForm.get('Longitude').setValidators(val);
            this.portCreateForm.get('Latitude').updateValueAndValidity();
            this.portCreateForm.get('Longitude').updateValueAndValidity();
        }
        else {
            this.portCreateForm.get('Latitude').clearValidators();
            this.portCreateForm.get('Latitude').updateValueAndValidity();
            this.portCreateForm.get('Longitude').clearValidators();
            this.portCreateForm.get('Longitude').updateValueAndValidity();
        }
    }

    getAddressByLatLong(lat: number, long: number) {
        let isGeoCoded = this.portCreateForm.get('IsGeocodeUsed').value;
        if (isGeoCoded && lat && long) {
            this.updateGeoCode(lat, long);
        }
    }
  ngOnDestroy(): void {
    this.dtTrigger.unsubscribe();
    }

    isInvalid(name: string): boolean {
        var result = this.portCreateForm.get(name).invalid
            &&
            (
            this.portCreateForm.get(name).dirty
                ||
            this.portCreateForm.get(name).touched
            )
        return result;
    }

    isRequired(name: string): boolean {
        return this.portCreateForm.get(name).errors.required;
    }
    setAddressValidators(countryId: number) {
        if (countryId && this.portCreateForm)
        {
            let validator: any;
            if (countryId && (countryId == 1 || countryId == 2)) {
                validator = [Validators.required];
            }
            else {
                validator = [];
            }
            this.portCreateForm.get('Address').setValidators(validator);
            this.portCreateForm.get('Address').updateValueAndValidity();
            this.portCreateForm.get('City').setValidators(validator);
            this.portCreateForm.get('City').updateValueAndValidity();
            //this.portCreateForm.get('CountyName').setValidators(validator);
            //this.portCreateForm.get('CountyName').updateValueAndValidity();
            this.portCreateForm.get('ZipCode').setValidators(validator);
            this.portCreateForm.get('ZipCode').updateValueAndValidity();
        }        
    }

    onSubmit() {      
        this.portCreateForm.markAllAsTouched();
        if (this.portCreateForm.valid) {
            this.IsLoading = true;
            // serverside api to save port
            let port = this.portCreateForm.value;
            this.marineService.saveMarinePort(port).subscribe(data => {
                if (data != null && data.StatusCode == 0) {
                    Declarations.msgsuccess(data.StatusMessage, undefined, undefined);
                    this.IsLoading = false;
                    this.portCreateForm.reset();
                    this.clearPanelData();
                    let dismissSlider = document.getElementById('btnCancel') as HTMLElement;
                    dismissSlider.click();
                    this.getMarinePortsData();
                    this.marineService.setReloadMapSubject(port.CountryId);
                } else {
                    this.IsLoading = false;
                    Declarations.msgerror(data == null ? 'Failed' : data.StatusMessage, undefined, undefined);
                }
            });
        }
        else {
            return;
        }
    }
    //updateGeoCode(lat: any, lng: any) {
    //    this.addLocationService.GetAddressByLongLat(lat, lng).subscribe(response => {
    //        if (response) {
    //            this.updateAddress(response, true);
    //            this.f.AddressDetails.get('Latitude').patchValue(lat.toFixed(8));
    //            this.f.AddressDetails.get('Longitude').patchValue(lng.toFixed(8));
    //            this.getTimeZoneUsingLatLng(lat, lng);
    //        }
    //    });
    //}

    
  
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
