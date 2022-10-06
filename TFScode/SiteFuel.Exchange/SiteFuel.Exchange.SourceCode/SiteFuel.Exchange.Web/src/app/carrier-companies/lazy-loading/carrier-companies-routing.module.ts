import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { CarrierCompaniesComponent } from '../carrier-companies.component';



const routeCarrierCompanies: Routes = [
    {
        path: '', component: CarrierCompaniesComponent,
        children: [
            {
                path: '',
                component: CarrierCompaniesComponent,
                data: {
                    title: 'Carrier Companies'
                }
            }
        ]
    }
];

@NgModule({
    imports: [
        RouterModule.forChild(routeCarrierCompanies)
    ],
    exports: [
        RouterModule
    ]
})

export class CarrierCompaniesRoutingModule { }
