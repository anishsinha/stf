import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { NgDragDropModule } from 'ng-drag-drop';
import { DeliveryRequestDisplayComponent } from './delivery-request-display.component';
import { SharedModule } from 'src/app/modules/shared.module';
import { DirectiveModule } from 'src/app/modules/directive.module';
import { Routes, RouterModule } from '@angular/router';

const routesDrDisplay: Routes = [
    {
        path: "",
        component: DeliveryRequestDisplayComponent
    },
];

@NgModule({
    declarations: [
        DeliveryRequestDisplayComponent,
    ],
    imports: [
        CommonModule,
        SharedModule,
        DirectiveModule,
        NgDragDropModule.forRoot(),
        RouterModule.forChild(routesDrDisplay)
    ]
})

export class DeliveryRequestDisplayModule { }
