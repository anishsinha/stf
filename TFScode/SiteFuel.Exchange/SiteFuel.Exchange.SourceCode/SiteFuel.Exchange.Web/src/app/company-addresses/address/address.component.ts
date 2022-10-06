import { Component, OnInit } from '@angular/core';
import { Address } from './model/address';
import { AddressService } from './service/address.service';
import { Declarations } from '../../declarations.module';
import { ActivatedRoute } from '@angular/router';
declare var currentUserCompanyId: number;
@Component({
    selector: 'address-component',
    templateUrl: './address.component.html',
    styleUrls: ['./address.component.css']
})
export class AddressComponent implements OnInit {

    constructor(private addressService: AddressService, private route: ActivatedRoute) { }
    public IsLoading: boolean = true;
    public IsEmpty: boolean = false;

    public CompanyAddressId: number;
    public IsActive: boolean;
    public IsDefault: boolean;
    public CompanyId: number;
    public status: any;
    ngOnInit() {
        this.CompanyId = currentUserCompanyId != undefined ? currentUserCompanyId : parseInt(this.route.snapshot.queryParamMap.get('id'));
            this.getAddresses();
        }
    
    Addresses: Address[];

    getAddresses(): void {       
        this.IsLoading = true;
        this.addressService.getAddresses(this.CompanyId)
        .subscribe((Addresses: Address[]) => {
            this.Addresses = Addresses;            
            this.IsLoading = false;          
        });
    };
    showSuccess() {
        Declarations.msgwarning("Cannot Make Default Address as InActive", undefined, undefined);
    }
    changeAddressStatus(CompanyAddressId, IsActive, IsDefault): void {
        if (IsDefault) {
            this.showSuccess();           
        }
        else {
            this.addressService.changeAddressStatus(CompanyAddressId, IsActive).
                subscribe((response: any) => {
                    this.getAddresses();
                });
        };
     }
   
}
