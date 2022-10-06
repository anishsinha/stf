import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { CreateinvoiceComponent } from '../createinvoice.component';
import { InvoiceRoutingModule } from './invoice-routing.module';
import { BolDetailComponent } from '../bol-detail/bol-detail.component';
import { LiftTicketsComponent } from '../lift-tickets/lift-tickets.component';
import { LiftDetailComponent } from '../lift-tickets/lift-detail.component';
import { PaymentTermsComponent } from '../payment-terms/payment-terms.component';
import { ProducDetailComponent } from '../produc-detail/produc-detail.component';
import { BolListComponent } from '../bol-detail/bol-list.component';
import { VariousDropLocationComponent } from '../various-drop-location/various-drop-location.component';
//import { FeeListComponent } from '../fee-list/fee-list.component';
import { OtherProductTaxesComponent } from '../taxes/other-product-taxes.component';
import { CustomerDetailComponent } from '../customer-detail/customer-detail.component';
import { ImageuploadComponent } from 'src/app/imageupload/imageupload.component';
import { FuelSurchargeComponent } from '../produc-detail/fuel-surcharge.component';
import { AssetListComponent } from '../asset-list/asset-list.component';
//import { FeeTypeComponent } from '../fee-list/fee-type.component';
//import { FilterPipe } from '../fee-list/filter.pipe';
import { OnCreateDirective } from 'src/app/directives/on-create.directive';
import { SharedModule } from 'src/app/modules/shared.module';
import { DirectiveModule } from 'src/app/modules/directive.module';
import { GlobalModule } from 'src/app/modules/global.module';
import { FeesModule } from 'src/app/fees/fees.module';
import { BdrDetailComponent } from '../bdr-detail/bdr-detail.component';
import { ShowBdrDetailComponent } from '../bdr-detail/show-bdr-detail.component';



@NgModule({

    declarations: [
        CreateinvoiceComponent,
        BolDetailComponent,
        LiftTicketsComponent,
        LiftDetailComponent,
        PaymentTermsComponent,
        ProducDetailComponent,
        BolListComponent,
        VariousDropLocationComponent,
        OtherProductTaxesComponent,
        CustomerDetailComponent,
        ImageuploadComponent,
        FuelSurchargeComponent,
        AssetListComponent,
        OnCreateDirective,
        BdrDetailComponent,
        ShowBdrDetailComponent,      
    ],
    imports: [
        InvoiceRoutingModule,
        SharedModule,
        DirectiveModule,
        FeesModule
    ]
})
export class InvoiceModule { }
