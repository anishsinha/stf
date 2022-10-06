import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { SharedModule } from '../modules/shared.module';
import { FormsModule } from '@angular/forms';
import { RouterModule, Routes } from '@angular/router';
import { MasterComponent } from './Master/master.component';
import { CreateFreightRateRules } from './Create/create-freight-rate-rules-component';
import { ViewFreightRateRules } from './View/view-freight-rate-rules.component';
import { DataTablesModule } from 'angular-datatables';
import { DirectiveModule } from '../modules/directive.module';
import { FreightComponent } from '../shared-components/Freight/freight.component';
import { AngularMultiSelectModule } from 'angular2-multiselect-dropdown';

const route: Routes = [
    { path: '', component: MasterComponent },
    { path: 'Create', component: MasterComponent },
    { path: 'Create/:freightrateruleId', component: MasterComponent }
]

@NgModule({
    declarations: [
        MasterComponent,
        CreateFreightRateRules,
        ViewFreightRateRules,
        FreightComponent
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
export class FreightRateRulesModule { }
