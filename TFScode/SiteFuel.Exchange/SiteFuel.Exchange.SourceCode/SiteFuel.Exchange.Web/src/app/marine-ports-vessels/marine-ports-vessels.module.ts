import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MasterComponent } from './master/master.component';
import { RouterModule, Routes } from '@angular/router';
import { MarineportsComponent } from './marine-ports/marineports.component';
import { MarinevesselsComponent } from './marine-vessels/marinevessels.component';
import { SharedModule } from '../modules/shared.module';
import { MarinePortsMapComponent } from './marine-ports/marine-ports-map/marine-ports-map.component';
import { DataTablesModule } from 'angular-datatables';
const route: Routes = [
    { path: '', component: MasterComponent }
]


@NgModule({
  declarations: [MasterComponent, MarineportsComponent, MarinevesselsComponent, MarinePortsMapComponent],
  imports: [
      CommonModule,
      SharedModule,
       DataTablesModule,
      RouterModule.forChild(route),
  ]
})
export class MarinePortsVesselsModule { }
