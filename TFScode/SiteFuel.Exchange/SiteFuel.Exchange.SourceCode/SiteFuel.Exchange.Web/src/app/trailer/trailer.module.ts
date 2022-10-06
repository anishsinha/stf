import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { SharedModule } from '../modules/shared.module';
import { DirectiveModule } from '../modules/directive.module';
import { DataTablesModule } from 'angular-datatables';
import { ViewTrailerComponent } from './view-trailer/view-trailer.component';

const routeTrailer: Routes = [
    {
        path: '',
        component: ViewTrailerComponent
    },

]

@NgModule({
    declarations: [
        ViewTrailerComponent,
    ],
    imports: [
        SharedModule,
        DirectiveModule,
        DataTablesModule,
        RouterModule.forChild(routeTrailer)
    ]
})
export class TrailerModule { }
