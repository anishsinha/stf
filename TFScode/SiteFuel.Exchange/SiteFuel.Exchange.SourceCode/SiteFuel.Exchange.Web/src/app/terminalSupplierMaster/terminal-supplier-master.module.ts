import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { TerminalSupplierComponent } from './terminal-supplier.component';

import { SharedModule } from '../modules/shared.module';
import { TerminalCodeComponent } from './terminal-code/terminal-code.component';
import { TerminalDescriptionComponent } from './terminal-description/terminal-description.component';
import { RouterModule, Routes } from '@angular/router';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { DataTablesModule } from 'angular-datatables';

const route:Routes=[{path:'',component:TerminalSupplierComponent}]


@NgModule({
  declarations: [TerminalSupplierComponent, TerminalCodeComponent, TerminalDescriptionComponent],
  imports: [
    CommonModule,
    SharedModule,
    FormsModule,
    ReactiveFormsModule,
    DataTablesModule,
    RouterModule.forChild(route)
  ]
})
export class TerminalSupplierMasterModule { }
