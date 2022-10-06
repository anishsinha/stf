import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { SalesUserDashboardComponent } from './sales-user-dashboard/sales-user-dashboard.component';
import { SharedModule } from '../modules/shared.module';
import { DirectiveModule } from '../modules/directive.module';
import { DataTablesModule } from 'angular-datatables';
import { FormsModule } from '@angular/forms';
import { RouterModule, Routes } from '@angular/router';
import { CreateSourcingRequestComponent } from './create-sourcing-request/create-sourcing-request.component';
import { SourcingRequestGridComponent } from './sourcing-request-grid/sourcing-request-grid/sourcing-request-grid.component';
import { NgbModule } from '@ng-bootstrap/ng-bootstrap';
import { AutocompleteLibModule } from 'angular-ng-autocomplete';
import { ConfirmationPopoverModule } from 'angular-confirmation-popover';
import { FeesModule } from 'src/app/fees/fees.module';
import { PricingSectionModule } from '../shared-components/pricing-section/pricing-section.module';
import { ContactPersonComponent } from 'src/app/contact-person/contact-person.component';


const route:Routes=[
  {
    path:'Dashboard',
    component:SalesUserDashboardComponent
  },
  {
    path:'Dashboard/Index',
    component:SalesUserDashboardComponent
  },
   {
     path:'SourcingRequest/Create',
     component:CreateSourcingRequestComponent
   },
  {
    path:'SourcingRequest',
    component:SourcingRequestGridComponent
  },
  {
    path:'SourcingRequest/Index',
    component:SourcingRequestGridComponent
  },
  {
    path:'SourcingRequest/Details/:Id',
    component:CreateSourcingRequestComponent,
  }
]
@NgModule({
    declarations: [SalesUserDashboardComponent, CreateSourcingRequestComponent, SourcingRequestGridComponent, ContactPersonComponent],
  imports: [
    CommonModule,
    SharedModule,
    DirectiveModule,
    DataTablesModule,
    FormsModule,
    AutocompleteLibModule,
      NgbModule,
      FeesModule,
    RouterModule.forChild(route),
    ConfirmationPopoverModule.forRoot({
      confirmButtonType: 'danger', 
    }),
    PricingSectionModule
  ],
})
export class SalesUserModule {

 }
