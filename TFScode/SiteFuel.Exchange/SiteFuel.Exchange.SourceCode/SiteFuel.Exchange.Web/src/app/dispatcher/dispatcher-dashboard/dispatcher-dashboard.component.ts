import { AfterViewInit, Component, OnInit } from '@angular/core';
import { MyLocalStorage } from 'src/app/my.localstorage';
import { DispatcherService } from 'src/app/carrier/service/dispatcher.service';
import { CarrierService } from 'src/app/carrier/service/carrier.service';
import { FormBuilder } from '@angular/forms';
import { DatatableCustomSortingService } from 'src/app/directives/sorting.pipe';
declare var currentUserCompanyId;

@Component({
    selector: 'app-dispatcher-dashboard',
    templateUrl: './dispatcher-dashboard.component.html',
    styleUrls: ['./dispatcher-dashboard.component.css']
})
export class DispatcherDashboardComponent implements OnInit, AfterViewInit {

    public viewType: any;
    public singleMulti: any;
    public disableControl: boolean = false;
    private dispatcherDashboard: Window;


    constructor(private fb: FormBuilder, private dispatcherService: DispatcherService, private carrierService: CarrierService, private customSortingService: DatatableCustomSortingService) { }

    public ngOnInit(): void {
        this.checkWindowSelection();
        this.getCountries();
    }

    ngAfterViewInit(){
        this.customSortingService.configColumnDefsNullToBottom();
    }

    getCountries() {
        this.carrierService.GetCountries(currentUserCompanyId).subscribe(data => {
            if (data != null) {
                localStorage.setItem('countryIdForDashboard', data.DefaultCountryId);
                localStorage.setItem('currencyTypeForDashboard', data.DefaultCountryId);
            }
        });
    }
    public changeViewType(type: any): void {
        localStorage.setItem('viewType', <string>type);
        if (this.singleMulti === 2) {
            this.dispatcherDashboard = window.open("/Dispatcher/Dashboard", "_blank");
        } else {
            this.viewType = type;
        }
    }


    public changeWindowType(type: number): void {
        this.singleMulti = type;
        this.dispatcherService.SingleMultiWindowSubject.next(type);
        if (type === 1 && +(localStorage.getItem('singleMulti')) !== 1) {
            setTimeout(() => {
                this.dispatcherDashboard.close();
            }, 10000);
        }
        localStorage.setItem('singleMulti', <string>this.singleMulti);
    }

    private checkWindowSelection(): void {
        this.singleMulti = (localStorage.getItem('singleMulti')) ? +(localStorage.getItem('singleMulti')) : 1;
        this.viewType = (localStorage.getItem('viewType')) ? +(localStorage.getItem('viewType')) : 1;
        let readonlyKey = MyLocalStorage.getData(MyLocalStorage.DSB_READONLY_KEY);
        if (readonlyKey == '') {
            this.disableControl = false;
        }
        else {
            this.disableControl = readonlyKey;
        }
        if (this.disableControl == true) {
            this.viewType = 2;
        }
    }
}
