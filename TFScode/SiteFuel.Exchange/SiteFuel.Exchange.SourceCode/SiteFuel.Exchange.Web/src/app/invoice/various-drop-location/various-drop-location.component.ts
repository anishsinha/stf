import { Component, OnInit, forwardRef, Input, SimpleChanges } from '@angular/core';
import { NG_VALUE_ACCESSOR, FormGroup, FormBuilder } from '@angular/forms';
import { AddressModel } from '../models/DropDetail';
import { DropdownItem, StatelistService } from 'src/app/statelist.service';
import { AddressService } from 'src/app/address.service';
import { RegExConstants } from 'src/app/app.constants';

@Component({
    selector: 'app-various-drop-location',
    templateUrl: './various-drop-location.component.html',
    styleUrls: ['./various-drop-location.component.css'],
    providers: [
        {
            provide: NG_VALUE_ACCESSOR,
            useExisting: forwardRef(() => VariousDropLocationComponent),
            multi: true
        }
    ]
})
export class VariousDropLocationComponent implements OnInit {

    public FuelDropLocation: FormGroup;

    //public DropLocationData: AddressModel;

    @Input() public IsVariousOrigin: boolean;
    public IsAddressAvailable: boolean = false;
    public StateList: DropdownItem[];
    public CountryList: DropdownItem[];
    public _loadingAddress: boolean;

    constructor(private fb: FormBuilder, private stateService: StatelistService, private addresService: AddressService) {
        this.StateList = [];
        this.CountryList = [];
        this.stateService.getStates().subscribe(data => this.StateList = data);
        this.stateService.getCountries().subscribe(data => this.CountryList = data);
    }

    ngOnInit() {
        this.FuelDropLocation = this.buildAddress(new AddressModel());
        //this.registerOnChange((data: AddressModel) => {
        //    this.DropLocationData = data;
        //    if (this.IsVariousOrigin) {
        //        this.DropLocationData.Address = null;
        //        this.DropLocationData.City = null;
        //        this.DropLocationData.ZipCode = null;
        //    }
        //});
    }

    ngOnChanges(change: SimpleChanges) {
        if (change.IsVariousOrigin) {
            this.IsVariousOrigin = change.IsVariousOrigin.currentValue || false;
        }
    }

    buildAddress(model: AddressModel): FormGroup {
        return this.fb.group({
            IsAddressAvailable: this.fb.control(false),
            Address: this.fb.control(model.Address),
            Latitude: this.fb.control(model.Latitude),
            Longitude: this.fb.control(model.Longitude),
            City: this.fb.control(model.City),
            CountyName: this.fb.control(model.CountyName),
            State: this.fb.group({
                Id: this.fb.control(model.State.Id),
                Code: this.fb.control(model.State.Code)
            }),
            Country: this.fb.group({
                Id: this.fb.control(model.Country.Id),
                Code: this.fb.control(model.Country.Code)
            }),
            ZipCode: this.fb.control(model.ZipCode),
        });
    }

    updateAddressFlag(address: FormGroup): void {
        this.IsAddressAvailable = !this.IsAddressAvailable;
        if (!this.IsAddressAvailable) {
            var empty = {
                Address: '',
                City: '',
                CountyName: '',
                ZipCode: '',
                Latitude: '',
                Longitude: ''
            }
            this.FuelDropLocation.patchValue(empty);
        }
        address.get('IsAddressAvailable').setValue(this.IsAddressAvailable);
    }

    getAddressByZip(address: FormGroup, event: any): void {
        var zipCode = event.target.value;
        if (RegExConstants.UsaZip.test(zipCode) || RegExConstants.CanZip.test(zipCode)) {
            this._loadingAddress = true;
            this.addresService.getAddress(zipCode)
                .subscribe(data => {
                    this._loadingAddress = false;
                    if (data != null && data != undefined && data.StateCode != null) {
                        data.Address = address.controls['Address'].value;
                        data.CountryCode == 'US' || data.CountryCode == 'USA' ? data.CountryCode = 'USA' : data.CountryCode = 'CAN';
                        var addressModel = this.addressMapper(data);
                        address.patchValue(addressModel);
                        this.setStateAndCountry(address, data.StateCode, data.CountryCode);
                    }
                });
        }
    }

    addressMapper(data: any): AddressModel {
        var _address = new AddressModel();
        _address.Address = data.Address;
        _address.City = data.City;
        _address.CountyName = data.CountyName
        _address.Latitude = data.Latitude;
        _address.Longitude = data.Longitude;
        _address.ZipCode = data.ZipCode;
        _address.State.Code = data.StateCode;
        _address.Country.Code = data.CountryCode;
        return _address;
    }

    setStateName(address: FormGroup, event: any) {
        address.get('State.Code').setValue(event.target.selectedOptions[0].text);
    }

    setCountryName(address: FormGroup, event: any) {
        address.get('Country.Code').setValue(event.target.selectedOptions[0].text);
    }

    setStateAndCountry(address: FormGroup, stateCode: string, countryCode: string) {
        var state = this.StateList.find(t => t.Code == stateCode);
        var country = this.CountryList.find(t => t.Code == countryCode);

        if (state != null) {
            address.get('State.Id').setValue(state.Id);
        }
        if (country != null) {
            address.get('Country.Id').setValue(country.Id);
        }
    }

    //----------------- DO NOT MODIFY: Control Value accessor ----------------------

    public onTouched: () => void = () => { };

    writeValue(val: any): void {
        val && this.FuelDropLocation.setValue(val, { emitEvent: true });
    }
    registerOnChange(fn: any): void {
        this.FuelDropLocation.valueChanges.subscribe(fn);
    }
    registerOnTouched(fn: any): void {
        this.onTouched = fn;
    }
    setDisabledState?(isDisabled: boolean): void {
        isDisabled ? this.FuelDropLocation.disable() : this.FuelDropLocation.enable();
    }

    //----------------- DO NOT MODIFY: Control Value accessor ----------------------

}
