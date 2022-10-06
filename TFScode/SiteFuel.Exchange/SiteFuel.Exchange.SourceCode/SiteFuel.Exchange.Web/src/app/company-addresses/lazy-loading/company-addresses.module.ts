import { NgModule } from '@angular/core';
import { CompanyAddressesRoutingModule } from './company-addresses-routing.module';
import { CompanyAddressesComponent } from '../company-addresses.component';
import { AddressComponent } from '../address/address.component';
import { SharedModule } from 'src/app/modules/shared.module';
import { RegionComponent } from '../region/region.component';
import { RegionCreateComponent } from '../region/create/region-create.component';
import { DirectiveModule } from 'src/app/modules/directive.module';
import { SourceRegionComponent } from '../region/source-region/source-region.component';
import { DispatchRegionComponent } from '../region/dispatch-region/dispatch-region.component';
import { NgSelectModule } from '@ng-select/ng-select'
import { ReactiveFormsModule } from '@angular/forms';

@NgModule({

    declarations: [
        CompanyAddressesComponent,
        AddressComponent,
        RegionComponent,
        RegionCreateComponent,
        SourceRegionComponent,
        DispatchRegionComponent
        
    ],
    imports: [
        CompanyAddressesRoutingModule,
        SharedModule,
        NgSelectModule,
        ReactiveFormsModule,
        DirectiveModule
    ]
})
export class CompanyAddressesModule { }
