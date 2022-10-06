import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { ViewOrderGroupComponent } from '../view-order-group/view-order-group.component';

const routeOrder: Routes = [
    {
        path: '',
        component: ViewOrderGroupComponent
    }
];
@NgModule({
    imports: [
        RouterModule.forChild(routeOrder)
    ],
    exports: [
        RouterModule
    ]
})
export class OrderRoutingModule { }
