import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { SharedModule } from '../modules/shared.module';
import { DirectiveModule } from '../modules/directive.module';
import { DataTablesModule } from 'angular-datatables';
import { ViewTractorComponent } from './view-tractor/view-tractor.component';
import { CreateTractorComponent } from './create-tractor/create-tractor.component';


const routeTractor: Routes = [
    {
        path: '',
        component: ViewTractorComponent
    },

]

@NgModule({
    declarations: [
        ViewTractorComponent,
        CreateTractorComponent
    ],
    imports: [
        SharedModule,
        DirectiveModule,
        DataTablesModule,
        RouterModule.forChild(routeTractor)
    ]
})
export class TractorModule { }
