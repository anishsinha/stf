import { Component, OnInit, Input, SimpleChanges } from '@angular/core';
import { DispatcherService } from 'src/app/carrier/service/dispatcher.service';
import { FormGroup } from '@angular/forms';

@Component({
    selector: 'app-tank-view-master',
    templateUrl: './tank-view-master.component.html',
    styleUrls: ['./tank-view-master.component.css']
})
export class TankViewMasterComponent implements OnInit {
    @Input() salesTabFilterForm: FormGroup;

    constructor(private dispatcherService: DispatcherService) { }
    ngOnInit() {
        this.verifyForcastingAccountLevelSetting();
    }
    public verifyForcastingAccountLevelSetting() {
        this.dispatcherService.getForcastingSetting().subscribe((resp: any) => {
            if (resp && resp.IsForecatingAccountLevel == 1) {
                this.salesTabFilterForm.get('RateOfConsumption').setValue(true);
            }
            else {
                this.salesTabFilterForm.get('RateOfConsumption').setValue(false);
            }
        });
    }
}
