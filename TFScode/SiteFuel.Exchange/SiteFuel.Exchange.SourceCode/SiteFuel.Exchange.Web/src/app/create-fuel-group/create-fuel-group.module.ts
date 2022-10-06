import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { CreateFuelGroupComponent } from './create-fuel-group.component';
import { FuelGroupMappingComponent } from './fuel-group-mapping/fuel-group-mapping.component';
import { ReactiveFormsModule } from '@angular/forms';
import { NgMultiSelectDropDownModule } from 'ng-multiselect-dropdown';
import { RouterModule, Routes } from '@angular/router';
import { DataTablesModule } from 'angular-datatables';
import { DirectiveModule } from '../modules/directive.module';
import { SharedModule } from '../modules/shared.module';


const routes: Routes = [
  {
      path: "",
      component: FuelGroupMappingComponent
  },
];
@NgModule({
  declarations: [
    CreateFuelGroupComponent,
    FuelGroupMappingComponent   
  ],
  imports: [
    CommonModule,
    ReactiveFormsModule,
    NgMultiSelectDropDownModule,
    SharedModule,
    DirectiveModule,
    DataTablesModule,
    RouterModule.forChild(routes),
    
  ]
})
export class CreateFuelGroupModule { }
