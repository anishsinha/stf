import { Component, OnInit, Output,Input, EventEmitter } from '@angular/core';
import { FormBuilder, FormGroup, Validators,AbstractControl } from '@angular/forms';
import { CustomerDetailsViewModel } from 'src/app/carrier/models/CustomerDetailsViewModel';
import { CarrierService } from 'src/app/carrier/service/carrier.service';
import { Declarations } from 'src/app/declarations.module';
declare function closeSlidePanel(): any;


@Component({
  selector: 'app-edit-customer',
  templateUrl: './edit-customer.component.html',
  styleUrls: ['./edit-customer.component.css']
})
export class EditCustomerComponent implements OnInit {
 
    public CustomerForm: FormGroup;
    public isShowLoader: boolean = false;
    @Input() CustomerDataToBeSend: CustomerDetailsViewModel;
    @Output() getAllCustomerData: EventEmitter<any> = new EventEmitter();
    public IdTaken: number = 5;

    constructor(public fb: FormBuilder, private carrierService: CarrierService) {
        this.CustomerForm = this.fb.group({

            Id: this.fb.control(0),
            TotalDDTCount: this.fb.control(''),
            TotalInvoiceCount: this.fb.control(''),
            BuyerCompanyId: this.fb.control(''),
            BuyerName: this.fb.control(''),
            TotalOrders: this.fb.control(''),
            CarrierAssignedCustomerId: this.fb.control(''),

        });
    }

    ngOnInit() {
    }

    checkMyCustomerIdDuplicate(customerDetail) {
        this.carrierService.CustomerIdNotTaken(customerDetail).subscribe(data => {
            if (data.StatusCode == 0) {
                this.IdTaken = 0;
                this.submitForm(customerDetail);
            }

             if (data.StatusCode == 2) {
                 Declarations.msgerror(data.StatusMessage, undefined, undefined);
                 this.IdTaken = 2;
            }
        });
    }

    onSubmit() {
        var CustomerDetail =
        {
            BuyerCompanyId: this.CustomerForm.get("BuyerCompanyId").value,
            BuyerName: this.CustomerForm.get("BuyerName").value,
            CarrierAssignedCustomerId: this.CustomerForm.get("CarrierAssignedCustomerId").value,
            Id: this.CustomerForm.get("Id").value
        }
        if(this.CustomerForm.get("CarrierAssignedCustomerId").value){
            this.checkMyCustomerIdDuplicate(CustomerDetail);
        } else {
            this.IdTaken = 0;
            this.submitForm(CustomerDetail);
        }
    }

    submitForm(CustomerDetail) {
        this.isShowLoader = true;
        this.carrierService.saveAndUpdateCustomerMapping(CustomerDetail).subscribe(data => {
            this.isShowLoader = false;
            if (data.StatusCode == 0) {
                Declarations.msgsuccess(data.StatusMessage, undefined, undefined);
                this.IdTaken = 5;
                closeSlidePanel();
                this.clearForm();
            }
            else if (data.StatusCode == 2) {
                Declarations.msgwarning(data.StatusMessage, undefined, undefined);
            }
            else {
                Declarations.msgerror(data == null ? 'Failed' : data.StatusMessage, undefined, undefined);
            }
            this.getAllCustomerData.emit();
        });
    }

    clearForm() {
        this.CustomerForm.reset();
    }
}
