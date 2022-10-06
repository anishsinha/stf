import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { PickupLocationComponent } from '../pickup-location/pickup-location.component';
import { LocationRoutingModule } from './location-routing.module';
import { SharedModule } from 'src/app/modules/shared.module';
import { BulkPlantsComponent } from '../pickup-location/bulk-plants/bulk-plants.component';
import { TerminalsComponent } from '../pickup-location/terminals/terminals.component';
import { DirectiveModule } from 'src/app/modules/directive.module';
import { DataTablesModule } from 'angular-datatables';



@NgModule({

    declarations: [        
        PickupLocationComponent,
        BulkPlantsComponent,
        TerminalsComponent
    ],
    imports: [
        LocationRoutingModule,
        SharedModule,
        DirectiveModule,
        DataTablesModule,
    ]
})
export class LocationModule { }
