import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { FreightRateRulesService } from '../Services/freight-rate-rules.service';

@Component({
    selector: 'app-master',
    templateUrl: './master.component.html',
    styleUrls: ['./master.component.css']
})
export class MasterComponent implements OnInit {
    viewType = 0
    constructor(private router: Router, private freightRateRulesService: FreightRateRulesService) { }



    ngOnInit() {
        this.freightRateRulesService.onSelectedTabChanged.subscribe(s => {
            if (s == 2) {
                this.viewType = 2;

            }
            else {
                this.viewType = 1;
            }
        })

    }

    public changeViewType(value) {
        this.viewType = value;
        this.freightRateRulesService.onSelectedFreightRateRuleId.next(null);
        this.freightRateRulesService.onSelectedTabChanged.next(value);

        //if(this.viewType==1)
        //this.router.navigate(['/Supplier/FuelSurcharge/CreateNew']);
    }


}
