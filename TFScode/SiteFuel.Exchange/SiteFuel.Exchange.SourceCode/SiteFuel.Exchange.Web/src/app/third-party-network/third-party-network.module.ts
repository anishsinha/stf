import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule, Routes } from '@angular/router';
import { SharedModule } from '../modules/shared.module';
import { DirectiveModule } from '../modules/directive.module';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { DataTablesModule } from 'angular-datatables';
import { DashboardComponent } from './dashboard/dashboard.component';

const routeTPN: Routes = [
  {
    path: "Dashboard",
    component: DashboardComponent
  }
];

@NgModule({
  declarations: [
    DashboardComponent,
  ],

  imports: [
    CommonModule,
    DirectiveModule,
    SharedModule,
    FormsModule,
    ReactiveFormsModule,
    DataTablesModule,
    RouterModule.forChild(routeTPN)
  ]
})
export class ThirdPartyNetworkModule { }