import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MasterComponent } from './master/master.component';
import { ViewAccessorialFeesComponent } from './view/view-accessorial-fees.component';
import { Routes, RouterModule } from '@angular/router';
import { CreateAccessorialFeesComponent } from './create/create-accessorial-fees.component';
import { SharedModule } from '../modules/shared.module';
import { FormsModule } from '@angular/forms';
import { DataTablesModule } from 'angular-datatables';
import { DirectiveModule } from '../modules/directive.module';
import { FeeListComponent } from './create/child-components/fee-list.component';
import { FeeTypeComponent } from './create/child-components/fee-type.component';
import { ViewFeesDetailsComponent } from './view/view-fees-details/view-fees-details.component';
import { AngularMultiSelectModule } from 'angular2-multiselect-dropdown';

const route: Routes = [
    { path: '', component: MasterComponent },
    { path: 'Create', component: MasterComponent }
]

@NgModule({
    declarations: [
        MasterComponent,
        ViewAccessorialFeesComponent,
        CreateAccessorialFeesComponent,
        FeeListComponent,
        FeeTypeComponent,
        ViewFeesDetailsComponent
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
export class AccessorialFeesModule { }
