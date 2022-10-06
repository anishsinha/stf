import { Component, OnInit } from '@angular/core';
import { FormBuilder } from '@angular/forms';
import { AccessorialFeesService } from '../../service/accessorialfees.service';
//import { FeesDetailModel, AccessorialFuelFeeModel } from '../../model/accessorial-fees';

@Component({
    selector: 'app-view-fees-details',
    templateUrl: './view-fees-details.component.html',
    styleUrls: ['./view-fees-details.component.css']
})
export class ViewFeesDetailsComponent implements OnInit {

    constructor(private fb: FormBuilder, private accessorialFeeService: AccessorialFeesService) { }
    //public AccessorialFuelFee: AccessorialFuelFeeModel;
    //public FeesDetailList: FeesDetailModel[]
    ngOnInit() {
    }

    getAccessorialFeesDetails(accessorialFeeId: number) {
        //this.accessorialFeeService.getAccessorialFee(accessorialFeeId).subscribe(data => {
        //    this.AccessorialFuelFee = data as AccessorialFuelFeeModel;
        //    this.FeesDetailList = this.AccessorialFuelFee.FuelFees;
        //});
    }
}
