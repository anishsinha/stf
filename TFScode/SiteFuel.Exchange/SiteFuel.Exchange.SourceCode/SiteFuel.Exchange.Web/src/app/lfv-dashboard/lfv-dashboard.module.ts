import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MasterComponent } from './master/master.component';
import { ValidationComponent } from './validation/validation.component';
import { CarrierComponent } from './carrier/carrier.component';
import { SharedModule } from '../modules/shared.module';
import { FormsModule } from '@angular/forms';
import { RouterModule, Routes } from '@angular/router';
import { LeftSideFilterComponent } from './left-side-filter/left-side-filter.component';
import { LfvScratchReportComponent } from './lfv-scratch-report/lfv-scratch-report.component';
import { DataTablesModule } from 'angular-datatables';
import { DirectiveModule } from '../modules/directive.module';
import { CarrierBolReportComponent } from './carrier-bol-report/carrier-bol-report.component';
import { SupplierBolReportComponent } from './supplier-bol-report/supplier-bol-report.component';
import { LfvAccrualReportComponent } from './lfv-accrual-report/lfv-accrual-report.component';
const route: Routes = [
  { path: '', component: MasterComponent },
    { path: 'Dashboard', component: MasterComponent },
    { path: 'LFVScratchReport', component: LfvScratchReportComponent },
    { path: 'CarrierBolReport', component: CarrierBolReportComponent },
    { path: 'SupplierBolReport', component: SupplierBolReportComponent },
    { path: 'LFVAccrualReport', component: LfvAccrualReportComponent }
]

@NgModule({
    declarations: [MasterComponent, ValidationComponent, CarrierComponent,
        LeftSideFilterComponent, LfvScratchReportComponent, CarrierBolReportComponent,
        SupplierBolReportComponent,
        LfvAccrualReportComponent],
  imports: [
    CommonModule,
    SharedModule,
    DirectiveModule,
    FormsModule,
    DataTablesModule,
    RouterModule.forChild(route),
    DirectiveModule
  ]
})
export class LfvDashboardModule { }
