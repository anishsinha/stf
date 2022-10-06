import { NgModule } from '@angular/core';
import { WallyDashboardComponent } from './wally-dashboard/wally-dashboard.component';
import { Routes, RouterModule } from '@angular/router';
import { SharedModule } from '../modules/shared.module';
import { BuyerLocationsComponent } from './buyer-locations.component';
import { WhereIsMyDriverComponent } from './where-is-my-driver.component';
import { DirectiveModule } from '../modules/directive.module';
import { WhereIsMyDriverMapViewComponent } from './wally-dashboard/map-view.component';
import { WhereIsMyDriverGridViewComponent } from './wally-dashboard/grid-view.component';
import { ChartsModule } from 'ng2-charts';
import { AgmDirectionModule } from 'agm-direction';
import { DataTablesModule } from 'angular-datatables';
import { SalesComponent } from './sales.component';
import { PriorityViewComponent } from './sales-data/priority-view.component';
import { TankViewComponent } from './sales-data/tank-view.component';
import { LocationViewComponent } from './sales-data/location-view.component';
import { TankChartModule } from '../tank-chart/tank-chart.module';
import { FormsModule } from '@angular/forms';
import { TankViewMasterComponent } from './sales-data/tank-view-master.component';

const routeWallyBoard: Routes = [
    {
        path: '',
        component: WallyDashboardComponent
    },
]

@NgModule({
    declarations: [
        WallyDashboardComponent,
        BuyerLocationsComponent,
        WhereIsMyDriverComponent,
        WhereIsMyDriverMapViewComponent,
        WhereIsMyDriverGridViewComponent,
        SalesComponent,
        PriorityViewComponent,
        TankViewComponent,
        LocationViewComponent,
        TankViewMasterComponent
    ],
    imports: [
        SharedModule,
        DirectiveModule,
        ChartsModule,
        DataTablesModule,
        AgmDirectionModule,
        TankChartModule,
        FormsModule,
        RouterModule.forChild(routeWallyBoard)
    ]
})
export class BuyerWallyBoardModule { }
