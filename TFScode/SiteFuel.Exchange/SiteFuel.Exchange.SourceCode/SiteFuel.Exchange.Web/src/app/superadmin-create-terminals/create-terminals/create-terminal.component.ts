import { Component, Input, OnInit, SimpleChanges } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Subject } from 'rxjs';
import { MarinePortsandvesselsService } from '../../marine-ports-vessels/marine-portsandvessels.service';
import { SalesUserService } from '../../sales-user/sales-user.service';
import { ConfirmationDialogService } from '../../shared-components/confirmation-dialog/confirmation-dialog.service';
import { TerminalDetailsModel, Geocode } from './../models';
import { CreateterminalsService} from './../createterminals.service';
import { Declarations } from '../../declarations.module';

@Component({
  selector: 'app-create-terminal',
  templateUrl: './create-terminal.component.html',
  styleUrls: ['./create-terminal.component.css']
})
export class CreateTerminalComponent implements OnInit {
    @Input() SelectedCountryId: any;

    public ModalText: string;

    public TerminalDetailsData: TerminalDetailsModel[] = [];
    public TerminalDetail: TerminalDetailsModel = new TerminalDetailsModel();
    dtOptions: any = {};
    dtTrigger: Subject<any> = new Subject();

    public popoverTitle: string = 'Are you sure?';
    public confirmButtonText: string = 'Yes';
    public cancelButtonText: string = 'No';

    terminalCreateForm: FormGroup;

    mapConstants: MapConstants = new MapConstants();

    public countryList = [];
    public currucyList = [];
    public statesList = [];
    public filteredStatesList = [];
    public countryGroupList = [];
    public currentCountryId: any;

    public IsLoading: boolean = false;

    constructor(private marineService: MarinePortsandvesselsService, private fb: FormBuilder,
        private salesService: SalesUserService, private confirmationdialogueservice: ConfirmationDialogService,
        private terminalService: CreateterminalsService) { }
    
    ngOnInit(): void {
        this.ModalText = 'Create Terminal';
        var exportColumns = { columns: [0, 1, 2, 3, 4, 5, 6, 7] };
        this.dtOptions = {
            dom: '<"html5buttons"B>lTfgitp',
            buttons: [
                { extend: 'colvis' },
                { extend: 'copy', exportOptions: exportColumns },
                { extend: 'csv', title: 'Terminal Details', exportOptions: exportColumns },
                { extend: 'pdf', title: 'Terminal Details', orientation: 'landscape', exportOptions: exportColumns },
                { extend: 'print', exportOptions: exportColumns }
            ],
            pagingType: 'first_last_numbers',
            pageLength: 10,
            ordering: true,
            order: [],
            lengthMenu: [[10, 25, 50, 100, -1], [10, 25, 50, 100, "All"]]
        };
        this.getCountryList();
        this.getcountryGroupList();
        this.getStatesOfAllCountries();
        this.TerminalDetail.CountryId = this.SelectedCountryId;
        if (this.SelectedCountryId == 2) { //canada
            this.mapConstants.CenterLat = 56.14;
            this.mapConstants.CenterLon = -106.34;
        }
        this.initializeTerminalCreationForm(this.TerminalDetail);
        jQuery("#AddressDetails_Country_Id").val("0").change();
    }
    ngOnChanges(changes: SimpleChanges): void {
        if (changes.SelectedCountryId && changes.SelectedCountryId.currentValue) {
            // get call for grid data
            this.getTerminalDetailsData();
            this.TerminalDetail.CountryId = this.SelectedCountryId;
            this.setMapCenter(this.SelectedCountryId);
            this.setAddressValidators(this.SelectedCountryId);
            jQuery("#AddressDetails_Country_Id").val("0").change();
        }
    }

    createTerminal(header: string) {
        this.ModalText = header;
        this.filteredStatesList = this.statesList.filter(s => s.CountryId == this.SelectedCountryId) || [];
        this.terminalCreateForm.get('CountryId').setValue(this.SelectedCountryId);
        this.terminalCreateForm.get('IsGeocodeUsed').setValue(false);
        this.setLatLongValidator(false);
        this.setMapCenter(this.SelectedCountryId);
        this.terminalCreateForm.get('StateId').setValue(null);
        jQuery("#AddressDetails_Country_Id").val("0").change();
    }

    setLatLongValidator(isGeoCoded: any) {
        if (isGeoCoded) {
            let val = [Validators.required];
            this.terminalCreateForm.get('Latitude').setValidators(val);
            this.terminalCreateForm.get('Longitude').setValidators(val);
            this.terminalCreateForm.get('Latitude').updateValueAndValidity();
            this.terminalCreateForm.get('Longitude').updateValueAndValidity();
        }
        else {
            this.terminalCreateForm.get('Latitude').clearValidators();
            this.terminalCreateForm.get('Latitude').updateValueAndValidity();
            this.terminalCreateForm.get('Longitude').clearValidators();
            this.terminalCreateForm.get('Longitude').updateValueAndValidity();
        }
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

    clearPanelData() {
        this.terminalCreateForm.reset();
        this.terminalCreateForm.get('CountryId').setValue(this.SelectedCountryId);
        this.terminalCreateForm.get('StateId').setValue(null);
        this.filteredStatesList = this.statesList.filter(s => s.CountryId == this.SelectedCountryId) || [];
        this.setMapCenter(this.SelectedCountryId);
        jQuery("#AddressDetails_Country_Id").val("0").change();
        this.setLatLongValidator(false);

    }
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
    public getStatesOfAllCountries(countryId?: number) {
        this.marineService.GetStatesOfAllCountries(countryId).subscribe(data => {
            if (data && data.length > 0) {
                this.statesList = data;
                this.filteredStatesList = this.statesList;
            }
        });
    }

    initializeTerminalCreationForm(terminal: TerminalDetailsModel): FormGroup {
        this.terminalCreateForm = this.fb.group({
            Id: this.fb.control(terminal.Id),
            Name: this.fb.control(terminal.Name, [Validators.required]),
            Abbreviation: this.fb.control(terminal.Abbreviation),
            TerminalOwner: this.fb.control(terminal.TerminalOwner),
            ControlNumber: this.fb.control(terminal.ControlNumber),
            Address: this.fb.control(terminal.Address),
            City: this.fb.control(terminal.City),
            County: this.fb.control(terminal.County),
            StateId: this.fb.control(terminal.StateId, [Validators.required]),
            CountryId: this.fb.control(terminal.CountryId, [Validators.required]),
            StateName: this.fb.control(terminal.StateCode),
            ZipCode: this.fb.control(terminal.ZipCode),
            IsGeocodeUsed: this.fb.control(terminal.IsGeoCoded),
            Latitude: this.fb.control(terminal.Latitude, Validators.pattern('^[0-9.-]*$')),
            Longitude: this.fb.control(terminal.Longitude, Validators.pattern('^[0-9.-]*$')),

        })
        this.setAddressValidators(terminal.CountryId);
        return this.terminalCreateForm;

    }

    setAddressValidators(countryId: number) {
        if (countryId && this.terminalCreateForm) {
            let validator: any;
            if (countryId && (countryId == 1 || countryId == 2)) {
                validator = [Validators.required];
            }
            else {
                validator = [];
            }
            this.terminalCreateForm.get('Address').setValidators(validator);
            this.terminalCreateForm.get('Address').updateValueAndValidity();
            this.terminalCreateForm.get('City').setValidators(validator);
            this.terminalCreateForm.get('City').updateValueAndValidity();
            this.terminalCreateForm.get('County').setValidators(validator);
            this.terminalCreateForm.get('County').updateValueAndValidity();
            this.terminalCreateForm.get('ZipCode').setValidators(validator);
            this.terminalCreateForm.get('ZipCode').updateValueAndValidity();
        }
    }

    getTerminalDetailsData() {
        let countryId = this.SelectedCountryId;
        this.IsLoading = true;
        this.terminalService.getTerminalsForGrid(countryId).subscribe((data) => {
            if (data) {
                jQuery("#terminal-datatable").DataTable().clear().destroy();
                this.TerminalDetailsData = data;
                this.dtTrigger.next();
                this.IsLoading = false;
            }

        });      
    }

    editTerminal(terminal) {
        if (terminal) {
            this.terminalCreateForm.get('Id').setValue(terminal.Id);
            this.terminalCreateForm.get('Name').setValue(terminal.Name);
            this.terminalCreateForm.get('Abbreviation').setValue(terminal.Abbreviation);
            this.terminalCreateForm.get('ControlNumber').setValue(terminal.ControlNumber);
            this.terminalCreateForm.get('TerminalOwner').setValue(terminal.TerminalOwner);
            this.terminalCreateForm.get('Address').setValue(terminal.Address);
            this.terminalCreateForm.get('City').setValue(terminal.City);
            this.terminalCreateForm.get('County').setValue(terminal.County);
            this.terminalCreateForm.get('StateId').setValue(terminal.StateId);
            this.terminalCreateForm.get('CountryId').setValue(terminal.CountryId);
            //this.terminalCreateForm.get('StateName').setValue(terminal.State);
            this.terminalCreateForm.get('ZipCode').setValue(terminal.ZipCode);
            this.terminalCreateForm.get('IsGeocodeUsed').setValue(false); // we dont save IsGeoCode flag at terminal level so it to false always 
            this.terminalCreateForm.get('Latitude').setValue(terminal.Latitude);
            this.terminalCreateForm.get('Longitude').setValue(terminal.Longitude);
            if (terminal.Latitude) {
                this.mapConstants.CenterLat = parseFloat(terminal.Latitude);
                this.mapConstants.CenterLon = parseFloat(terminal.Longitude);
            }
            let countryId = this.terminalCreateForm.get('CountryId').value;
            this.setAddressValidators(countryId);
            jQuery("#AddressDetails_Country_Id").val("0").change();
        }
    }

    onSubmit() {
        this.terminalCreateForm.markAllAsTouched();
        this.terminalCreateForm.value;
        if (this.terminalCreateForm.valid) {
            this.IsLoading = true;
            // serverside api to save terminal
            let terminal = this.terminalCreateForm.value;
            this.terminalService.saveTerminalDetails(terminal).subscribe(data => {
                if (data != null && data.StatusCode == 0) {
                    Declarations.msgsuccess(data.StatusMessage, undefined, undefined);
                    this.IsLoading = false;
                    this.terminalCreateForm.reset();
                    this.clearPanelData();
                    let dismissSlider = document.getElementById('btnCancel') as HTMLElement;
                    dismissSlider.click();
                    this.getTerminalDetailsData();
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

    isInvalid(name: string): boolean {
        var result = this.terminalCreateForm.get(name).invalid
            &&
            (
            this.terminalCreateForm.get(name).dirty
                ||
            this.terminalCreateForm.get(name).touched
            )
        return result;
    }

    isRequired(name: string): boolean {
        return this.terminalCreateForm.get(name).errors.required;
    }

    public getAddressByZip() {
        var zipCode = this.terminalCreateForm.get('ZipCode').value;
        if (zipCode) {
            this.marineService.GetAddressByZip(zipCode).subscribe(data => {
                if (data) {
                    var country = this.countryList.find(t => t.Code.includes(data.CountryCode));
                    if (country) {
                        var countryId = country.Id;
                        this.terminalCreateForm.get('CountryId').patchValue(countryId);
                        /*this.terminalCreateForm.get('Address').patchValue(data.Address);*/
                        this.terminalCreateForm.get('County').patchValue(data.CountyName);
                        this.terminalCreateForm.get('City').patchValue(data.City);
                        var stateId = this.statesList.find(x => x.StateCode == data.StateCode).StateId;
                        this.terminalCreateForm.get('StateId').patchValue(stateId);
                        this.terminalCreateForm.get('Latitude').patchValue(data.Latitude);
                        this.terminalCreateForm.get('Longitude').patchValue(data.Longitude);
                        this.mapConstants.CenterLat = data.Latitude;
                        this.mapConstants.CenterLon = data.Longitude;
                        this.filteredStatesList = this.statesList.filter(s => s.CountryId == countryId) || [];

                        if (!this.terminalCreateForm.get('Address').value) {
                            this.terminalCreateForm.get('Address').patchValue(data.Address);
                        }
                    }
                }
            });
        }

    }
    getAddressByLatLong(lat: number, long: number) {
        let isGeoCoded = this.terminalCreateForm.get('IsGeocodeUsed').value;
        if (isGeoCoded && lat && long) {
            this.updateGeoCode(lat, long);
        }
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
    updateAddressData(address: Geocode) {
        let countryId = (address.CountryCode == 'US' || address.CountryCode == 'USA') ? 1 : (address.CountryCode == 'CA' || address.CountryCode == 'CAN') ? 2 : this.terminalCreateForm.get('CountryId').value;
        let stateName = (address.StateName != null && address.StateName != '' && address.StateName != undefined) ? address.StateName : address.CountryName;

        if (stateName) {
            let state = this.statesList.find(st => st.StateName.toLowerCase() == stateName.toLowerCase());
            let stateId = (state && state.StateId) ? state.StateId : this.terminalCreateForm.get('StateId').value;
            this.terminalCreateForm.get('StateId').patchValue(stateId);
        }
        else //set first state after filtering by countryID
        {
            let states = this.statesList.filter(s => s.CountryId == countryId) || [];
            if (states && states.length > 0) {
                let stateId = states[0].StateId;
                if (stateId) {
                    this.terminalCreateForm.get('StateId').patchValue(stateId);
                }
            }
        }

        this.terminalCreateForm.get('Address').patchValue(address.Address);
        this.terminalCreateForm.get('City').patchValue(address.City);
        this.terminalCreateForm.get('ZipCode').patchValue(address.ZipCode);

        this.terminalCreateForm.get('CountryId').patchValue(countryId);

        this.terminalCreateForm.get('County').patchValue(address.CountyName);



        if (address.Latitude) {
            this.terminalCreateForm.get('Latitude').patchValue(address.Latitude);
            this.terminalCreateForm.get('Longitude').patchValue(address.Longitude);
            this.mapConstants.CenterLat = address.Latitude;
            this.mapConstants.CenterLon = address.Longitude;
        }

        this.filteredStatesList = this.statesList.filter(s => s.CountryId == countryId) || [];

    }

    markerDragEnd(event) {
        this.confirmationdialogueservice.confirm('Map update', 'Geo Codes shifted to a new location!')
            .then((confirmed) => (confirmed == true) ? this.updateGeoCode(event.coords.lat, event.coords.lng) : this.previousLatLon())
            .catch(() => this.previousLatLon());
    }
    public previousLatLon() {
        this.mapConstants.CenterLat = this.terminalCreateForm.get('Latitude').value;
        this.mapConstants.CenterLon = this.terminalCreateForm.get('Longitude').value;
    }

    ngOnDestroy(): void {
        this.dtTrigger.unsubscribe();
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
