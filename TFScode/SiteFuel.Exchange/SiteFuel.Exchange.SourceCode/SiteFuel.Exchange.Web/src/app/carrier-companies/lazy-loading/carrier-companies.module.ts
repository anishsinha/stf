import { NgModule } from '@angular/core';
import { CarrierCompaniesRoutingModule } from './carrier-companies-routing.module';
import { CarrierCompaniesComponent } from '../carrier-companies.component';
import { SharedModule } from 'src/app/modules/shared.module';
import { DirectiveModule } from 'src/app/modules/directive.module';
import { DataTablesModule } from 'angular-datatables';


@NgModule({

    declarations: [
        CarrierCompaniesComponent,
    ],
    imports: [
        CarrierCompaniesRoutingModule,
        SharedModule,
        DataTablesModule,
        DirectiveModule
    ]
})
export class CarrierCompaniesModule { }
