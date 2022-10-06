import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { HomeComponent } from './home/home.component';
import { MapViewComponent } from './map-view/map-view.component';
import { LocationMapComponent } from './location-map/location-map.component';
import { LoadsMapComponent } from './loads-map/loads-map.component';
import { DeliveryComponent } from './delivery/delivery.component';
import { InventoryComponent } from './inventory/inventory.component';
import { InvoiceComponent } from './invoice/invoice.component';
import { MessageComponent } from './message/message.component';
import { RouterModule, Routes } from '@angular/router';
import { SharedModule } from '../modules/shared.module';
import { DirectiveModule } from '../modules/directive.module';
import { DataTablesModule } from 'angular-datatables';
import { AgmDirectionModule } from 'agm-direction';
import { FormsModule } from '@angular/forms';


const route:Routes=[{path:'',component:HomeComponent}]


@NgModule({
  declarations: [HomeComponent, MapViewComponent, LocationMapComponent, LoadsMapComponent, DeliveryComponent, InventoryComponent, InvoiceComponent, MessageComponent],
  imports: [
    CommonModule,
    SharedModule,
    DirectiveModule,
    DataTablesModule,
    AgmDirectionModule,
    FormsModule,
    RouterModule.forChild(route)
  ]
})
export class BuyerDashboardModule { }
