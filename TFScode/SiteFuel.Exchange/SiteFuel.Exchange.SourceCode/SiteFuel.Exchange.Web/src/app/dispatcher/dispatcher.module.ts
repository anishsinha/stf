import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { AgmDirectionModule } from 'agm-direction';
import { SharedModule } from '../modules/shared.module';
import { DispatcherRoutingModule } from './dispatcher-routing.module';
import { DispatcherDashboardComponent } from './dispatcher-dashboard/dispatcher-dashboard.component';
import { LocationComponent } from './dispatcher-dashboard/location.component';
import { WhereIsMyDriverComponent } from './dispatcher-dashboard/where-is-my-driver.component';
import { WhereIsMyDriverMapViewComponent } from './dispatcher-dashboard/where-is-my-driver-map-view.component';
import { WhereIsMyDriverGridViewComponent } from './dispatcher-dashboard/where-is-my-driver-grid-view.component';
import { DirectiveModule } from '../modules/directive.module';
import { ChartsModule } from 'ng2-charts';
import { DataTablesModule } from 'angular-datatables';
import { SalesDataComponent } from './dispatcher-dashboard/sales-data/sales-data.component';
import { GridViewComponent } from './dispatcher-dashboard/sales-data/grid-view.component';
import { TankViewComponent } from './dispatcher-dashboard/sales-data/tank-view.component';
import { LocationViewComponent } from './dispatcher-dashboard/sales-data/location-view.component';
import { TankChartModule } from '../tank-chart/tank-chart.module';
import { FormsModule } from '@angular/forms';
import { JobTankHierarchyComponent } from './dispatcher-dashboard/job-tank-hierarchy/job-tank-hierarchy.component';
import { TankViewMasterComponent } from './dispatcher-dashboard/sales-data/tank-view-master.component';

@NgModule({
    declarations: [
        DispatcherDashboardComponent,
        LocationComponent,
        WhereIsMyDriverComponent,
        WhereIsMyDriverMapViewComponent,
        WhereIsMyDriverGridViewComponent,
        SalesDataComponent,
        GridViewComponent,
        TankViewComponent,
        LocationViewComponent,
        JobTankHierarchyComponent,
        TankViewMasterComponent,
    ],
    imports: [
        CommonModule,
        AgmDirectionModule,
        DispatcherRoutingModule,
        SharedModule,
        DirectiveModule,
        ChartsModule,
        DataTablesModule,
        TankChartModule,
        FormsModule
    ]
})
export class DispatcherModule { }
