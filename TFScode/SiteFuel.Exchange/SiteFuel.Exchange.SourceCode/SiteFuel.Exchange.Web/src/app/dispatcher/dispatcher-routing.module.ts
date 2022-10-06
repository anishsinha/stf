import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { DispatcherDashboardComponent } from './dispatcher-dashboard/dispatcher-dashboard.component';


const routeDispatcher: Routes = [
    {
        path: "",
        component: DispatcherDashboardComponent
    },
    {
        path: "Dashboard",
        component: DispatcherDashboardComponent
    },
    {
        path: "Dashboard/Index",
        component: DispatcherDashboardComponent
    }
];

@NgModule({
    imports: [
        RouterModule.forChild(routeDispatcher)
    ],
    exports: [
        RouterModule
    ]
})
export class DispatcherRoutingModule { }
