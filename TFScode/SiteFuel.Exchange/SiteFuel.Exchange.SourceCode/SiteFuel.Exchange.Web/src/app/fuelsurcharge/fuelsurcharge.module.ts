import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { SharedModule } from '../modules/shared.module';
import { FormsModule } from '@angular/forms';
import { RouterModule, Routes } from '@angular/router';
import { MasterComponent } from './master/master.component';
import { CreateFuelSurchargeComponent } from './Create/create-fuel-surcharge.component';
import { ViewFuelSurchargeComponent } from './View/view-fuel-surcharge.component';
import { DataTablesModule } from 'angular-datatables';
import { DirectiveModule } from '../modules/directive.module';
import { ViewFuelSurchargePricingdetailsComponent } from './View/view-fuel-surcharge-pricingdetails/view-fuel-surcharge-pricingdetails.component';
import { ViewHistoricalPriceComponent } from './View/view-historical-price/view-historical-price.component';
import { AngularMultiSelectModule } from 'angular2-multiselect-dropdown';

const route: Routes = [
    { path: '', component: MasterComponent },
    { path: 'CreateNew', component: MasterComponent },
    { path: 'CreateNew/:fuelsurchargeId', component: MasterComponent }
]

@NgModule({
  declarations: [
    MasterComponent, 
    CreateFuelSurchargeComponent, 
    ViewFuelSurchargeComponent, 
    ViewFuelSurchargePricingdetailsComponent, 
    ViewHistoricalPriceComponent,
  ],
  imports: [
      CommonModule, 
      SharedModule, 
      FormsModule, 
      DataTablesModule, 
      DirectiveModule, 
      RouterModule.forChild(route),
      AngularMultiSelectModule
  ]
})
export class FuelsurchargeModule { }
