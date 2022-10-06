import { AfterViewInit, Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { FuelSurchargeService } from '../services/fuelsurcharge.service';

@Component({
  selector: 'app-master',
  templateUrl: './master.component.html',
  styleUrls: ['./master.component.css']
})
export class MasterComponent implements OnInit, AfterViewInit {
 viewType = 0
    constructor(private router: Router, private fuelsurchargeService: FuelSurchargeService ) { }



    ngOnInit() {

        let _viewType = localStorage.getItem("fuelSurchargeTabId");
        if (_viewType && +_viewType>0) {
            this.viewType = +_viewType;
        }

        this.fuelsurchargeService.onSelectedTabChanged.subscribe(s => {
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
        this.fuelsurchargeService.onSelectedFuelSurchargeId.next(null);
        this.fuelsurchargeService.onSelectedTabChanged.next(value);
            

        //if(this.viewType==1)
        //this.router.navigate(['/Supplier/FuelSurcharge/CreateNew']);
    }

   
}
