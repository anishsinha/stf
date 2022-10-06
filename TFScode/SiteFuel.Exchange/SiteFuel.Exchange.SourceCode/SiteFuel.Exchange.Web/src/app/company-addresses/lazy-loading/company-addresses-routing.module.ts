import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { CompanyAddressesComponent } from '../company-addresses.component';
import { RegionComponent } from '../region/region.component';


const routeComAddress: Routes = [
    {
        path: 'CompanyAddresses', component: CompanyAddressesComponent,
        data: { title: 'Company Addresses' }
    },
    {
        path: 'View', component: RegionComponent,
        data: { title: 'Region' }
    },
    {
        path: 'CompanyDetails', component: CompanyAddressesComponent,
        data: { title: 'Company Addresses' }
    }
];
@NgModule({
    imports: [
        RouterModule.forChild(routeComAddress)
    ],
    exports: [
        RouterModule
    ]
})
export class CompanyAddressesRoutingModule { }
