import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { SharedModule } from '../modules/shared.module';
import { DeliveryRequestComponent } from './delivery-request/delivery-request.component';
import { ScheduleBuilderComponent } from './schedule-builder/schedule-builder.component';
import { DeliveryGroupComponent } from './delivery-group/delivery-group.component';
import { ScheduleBuilderFilterComponent } from './schedule-builder-filter.component';
import { DirectiveModule } from '../modules/directive.module';
import { NgDragDropModule } from 'ng-drag-drop';
import { Routes, RouterModule } from '@angular/router';
import { DataTablesModule } from 'angular-datatables';
import { RouteInfoComponent } from './schedule-builder/route-info/route-info.component';
import { CompartmentQuantityValidatorDirective } from './schedule-builder/compartment-quantity-validator.directive';
import { OttoBuilderComponent } from './schedule-builder/otto-builder.component';
import { OttoNotificationComponent } from './schedule-builder/otto-notification/otto-notification.component';
import { SplitDeliveryRequestComponent } from './delivery-request/split-delivery-request/split-delivery-request.component';
import { DriverColumnViewComponent } from './schedule-builder/driver-column-view/driver-column-view.component';
import { DriverScheduleColumnViewComponent } from './schedule-builder/child-components/driver-schedule-column-view.component';
import { ScheduleBuilderGridFilterComponent } from './schedule-builder-grid-filter.component';
import { LoadQueueService } from './schedule-builder/dsb-load-queue/load-queue.service';
import { DsbLoadQueueComponent } from './schedule-builder/dsb-load-queue/dsb-load-queue.component';
import { AssignedToMeComponent } from './assigned-to-me/assigned-to-me.component';
import { AssignedByMeComponent } from './delivery-request/assigned-by-me/assigned-by-me.component';
import { AddLocationComponent } from './schedule-builder/add-location/add-location.component';
import { PricingSectionModule } from '../shared-components/pricing-section/pricing-section.module';

const routeCarrier: Routes = [
    {
        path: "",
        component: ScheduleBuilderComponent
    },
];

@NgModule({
    declarations: [
        DeliveryRequestComponent,
        ScheduleBuilderComponent,
        DeliveryGroupComponent,
        ScheduleBuilderFilterComponent,
        RouteInfoComponent,
        CompartmentQuantityValidatorDirective,
        OttoBuilderComponent,
        OttoNotificationComponent,
        SplitDeliveryRequestComponent,
        DriverColumnViewComponent,
        DriverScheduleColumnViewComponent,
        ScheduleBuilderGridFilterComponent,
        DsbLoadQueueComponent,
        AssignedToMeComponent,
        AssignedByMeComponent,
        AddLocationComponent,

    ],
    imports: [
        CommonModule,
        SharedModule,
        DirectiveModule,
        DataTablesModule,
        PricingSectionModule,
        NgDragDropModule.forRoot(),
        RouterModule.forChild(routeCarrier)
    ],
    providers:[
        LoadQueueService
    ]
})
export class CarrierModule { }
