import { AfterViewInit, Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { DatatableCustomSortingService } from 'src/app/directives/sorting.pipe';
import { MyLocalStorage } from 'src/app/my.localstorage';
import { BuyerwallyboardService } from '../services/buyerwallyboard.service';

@Component({
    selector: 'app-wally-dashboard',
    templateUrl: './wally-dashboard.component.html',
    styleUrls: ['./wally-dashboard.component.css']
})

export class WallyDashboardComponent implements OnInit, AfterViewInit {
    public viewType: any;
    public singleMulti: any;
    public disableControl: boolean = false;
    private dispatcherDashboard: Window;

    constructor(private wallyBoardService: BuyerwallyboardService, private _activateRoute: ActivatedRoute, private customSortingService: DatatableCustomSortingService) { }

    ngOnInit() {
        this.checkWindowSelection();
        //this.singleMulti = (localStorage.getItem('singleMulti')) ? +(localStorage.getItem('singleMulti')) : 1;
        let params: any = this._activateRoute.snapshot.queryParams;
        if (params && params.viewTypeFromDashboard)
            this.changeViewType(params.viewTypeFromDashboard);
    }

    ngAfterViewInit(){
        this.customSortingService.configColumnDefsNullToBottom();
    }

    public changeViewType(type: any): void {
        localStorage.setItem('viewType', <string>type);
        if (this.singleMulti === 2) {
            this.dispatcherDashboard = window.open("/Buyer/Job/BuyerWallyBoard", "_blank");
        } else {
            this.viewType = type;
        }
    }


    public changeWindowType(type: number): void {
        this.singleMulti = type;
        this.wallyBoardService.SingleMultiWindowSubject.next(type);
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



