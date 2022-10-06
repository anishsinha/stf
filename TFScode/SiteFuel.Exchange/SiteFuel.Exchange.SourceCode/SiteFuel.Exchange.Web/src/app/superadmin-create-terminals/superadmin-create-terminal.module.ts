import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule, Routes } from '@angular/router';
import { SharedModule } from '../modules/shared.module';
import { DataTablesModule } from 'angular-datatables';
import { MasterComponent } from './master.component';
import { CreateTerminalComponent } from './create-terminals/create-terminal.component';
import { TerminalProductAssignmentComponent } from './terminal-product assignment/terminal-product-assignment.component';

const route: Routes = [
    { path: '', component: MasterComponent }
]

@NgModule({
  declarations: [MasterComponent, CreateTerminalComponent, TerminalProductAssignmentComponent],
  imports: [
      CommonModule,
      SharedModule,
      DataTablesModule,
      RouterModule.forChild(route)
  ]
})
export class SuperadminCreateTerminalModule { }
