import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { CreateinvoiceComponent } from '../createinvoice.component';


const routeInvoice: Routes = [
    {
        path: 'CreateNew', component: CreateinvoiceComponent,
        data: { title: 'Create invoice' }
    },
    {
        path: 'CreditAndRebill/:number', component: CreateinvoiceComponent,
        data: { title: 'Rebill invoice' }
    },
    {
        path: 'ConvertToInvoice/:number', component: CreateinvoiceComponent,
        data: { title: 'Convert to invoice' }
    }
];
@NgModule({
    imports: [
        RouterModule.forChild(routeInvoice)
    ],
    exports: [
        RouterModule
    ]
})
export class InvoiceRoutingModule { }
