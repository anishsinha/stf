import { AfterViewInit, Component, OnInit } from '@angular/core';
import { AccessorialFeesService } from '../service/accessorialfees.service';
@Component({
    selector: 'app-master',
    templateUrl: './master.component.html',
    styleUrls: ['./master.component.css']
})
export class MasterComponent implements OnInit, AfterViewInit {
    viewType = 0
    constructor(private accessorialFeeService: AccessorialFeesService) { }

    ngOnInit() {

        let _viewType = localStorage.getItem("fuelSurchargeTabId");
        if (_viewType && +_viewType > 0) {
            this.viewType = +_viewType;
        }

        this.accessorialFeeService.onSelectedTabChanged.subscribe(s => {
            if (s == 2) {
                this.viewType = 2;

            }
            else {
                this.viewType = 1;
            }
        })
        this.viewType = +_viewType;


    }
    ngAfterViewInit() {
        this.changeViewType(this.viewType)
    }

    public changeViewType(value: number) {

        localStorage.setItem("fuelSurchargeTabId", value.toString());

        this.viewType = value;
        this.accessorialFeeService.onSelectedAccessorialFeeId.next(null);
        this.accessorialFeeService.onSelectedTabChanged.next(value);   
    }
    public onCreateFees(viewType) {
        this.changeViewType(viewType);
    }
}
