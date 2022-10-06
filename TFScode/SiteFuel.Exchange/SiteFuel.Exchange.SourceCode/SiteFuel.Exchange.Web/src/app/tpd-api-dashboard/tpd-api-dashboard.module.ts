import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { DashboardComponent } from './dashboard/dashboard.component';
import { Routes, RouterModule } from '@angular/router';
import { SharedModule } from '../modules/shared.module';
import { FormsModule } from '@angular/forms';
import { NgxJsonViewerModule } from 'ngx-json-viewer';
import { DirectiveModule } from '../modules/directive.module';
import { DataTablesModule } from 'angular-datatables';

const routeTpd: Routes = [
  {
    path: '',
    component: DashboardComponent
  },
];

@NgModule({
  declarations: [DashboardComponent],
  imports: [
    SharedModule,
    DirectiveModule,
    NgxJsonViewerModule,
    DataTablesModule,
    RouterModule.forChild(routeTpd)
  ]
})
export class TpdApiDashboardModule { }
