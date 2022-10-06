import { Component, Input, OnChanges, OnInit, SimpleChanges } from '@angular/core';
import { FormArray, FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ContactPersonModel } from '../sales-user/sales-user.model';
import { SalesUserService } from '../sales-user/sales-user.service';

@Component({
    selector: 'app-contact-person',
    templateUrl: './contact-person.component.html',
    styleUrls: ['./contact-person.component.css']
})
export class ContactPersonComponent implements OnInit {

    @Input() public Parent: FormGroup;
    regexPhone: RegExp = /^\(?([0-9]{3})\)?[-. ]?([0-9]{3})[-. ]?([0-9]{4})$/;
    constructor(private fb: FormBuilder, private salesUserService: SalesUserService,) { }

    ngOnInit(): void {        
        
    }


    public onChangeMobileNumber(event: any, contactPerson: any) {
        if (contactPerson.get('PhoneNumber').value) {
            this.salesUserService.IsPhoneNumberValid(contactPerson.get('PhoneNumber').value).subscribe(data => {
                contactPerson.get('IsPhoneNumberConfirmed').setValue(data);
            })
        } else {
            contactPerson.get('IsPhoneNumberConfirmed').setValue(true);
        }
    }

    getNewContactPerson(): FormGroup {
        var _contactPersonForm = this.fb.group({
            Name: this.fb.control(null),
            PhoneNumber: this.fb.control(null, [Validators.required, Validators.pattern(this.regexPhone)]),
            Email: this.fb.control(null, [Validators.required, Validators.email]),
            IsPhoneNumberConfirmed: this.fb.control(true)
        });
        return _contactPersonForm;
    }


    removeContactPerson(idx: number) {
        (<FormArray>this.Parent.get('ContactPersons')).removeAt(idx);
    }

    addContactPerson(): void {
        var contactPerson = new ContactPersonModel();
        (<FormArray>this.Parent.get('ContactPersons')).push(this.getNewContactPerson());
    }

}
