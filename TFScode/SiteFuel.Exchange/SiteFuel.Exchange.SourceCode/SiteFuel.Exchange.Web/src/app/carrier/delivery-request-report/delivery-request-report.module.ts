import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { SharedModule } from 'src/app/modules/shared.module';
import { DirectiveModule } from 'src/app/modules/directive.module';
import { Routes, RouterModule } from '@angular/router';
import { DataTablesModule } from 'angular-datatables';
import { DeliveryRequestReportComponent } from './delivery-request-report.component';
const routesDrReport: Routes = [
    {
        path: "",
        component: DeliveryRequestReportComponent
    },
];

@NgModule({
  declarations: [DeliveryRequestReportComponent],
  imports: [
      CommonModule,
      SharedModule,
      DirectiveModule,
      DataTablesModule,
      RouterModule.forChild(routesDrReport)
  ]
})
export class DeliveryRequestReportModule { }
