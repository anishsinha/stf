import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { PickupLocationComponent } from 'src/app/location/pickup-location/pickup-location.component';

const routelocation: Routes = [
    {
        path: "", component: PickupLocationComponent,
        data: { title: 'Supplier Location' }
    }
];
@NgModule({
    imports: [RouterModule.forChild(routelocation)],
    exports: [RouterModule]
})
export class LocationRoutingModule { }
