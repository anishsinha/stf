import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { SharedModule } from '../modules/shared.module';
import { DirectiveModule } from '../modules/directive.module';
import { DataTablesModule } from 'angular-datatables';
import { SelfServiceAliasComponent } from './self-service-alias.component';
import { ProductMappingComponent } from './product-mapping/product-mapping.component';
import { CustomerMappingComponent } from './customer-mapping/customer-mapping.component';
import { EditCustomerComponent } from './edit-customer/edit-customer.component';
import { CompanyCarrierMappingComponent } from './company-carrier-mapping/company-carrier-mapping.component';
import { EditCarrierMappingComponent } from './edit-carrier-mapping/edit-carrier-mapping.component';
import { TerminalMappingComponent } from './terminal-mapping/terminal-mapping.component';
import { TerminalItemCodeMappingComponent } from './terminal-item-code-mapping/terminal-item-code-mapping.component';
import { CreateTerminalItemCodeComponent } from './create-terminal-item-code/create-terminal-item-code.component';
import { ExternalCustomerMappingsComponent } from './external-entity-mappings/external-customer-mappings/external-customer-mappings.component';
import { ExternalCustomerlocationMappingsComponent } from './external-entity-mappings/external-customerlocation-mappings/external-customerlocation-mappings.component';
import { ExternalProductMappingsComponent } from './external-entity-mappings/external-product-mappings/external-product-mappings.component';
import { ExternalSupplierMappingsComponent } from './external-entity-mappings/external-supplier-mappings/external-supplier-mappings.component';
import { ExternalTerminalMappingsComponent } from './external-entity-mappings/external-terminal-mappings/external-terminal-mappings.component';
import { ExternalDriverMappingsComponent } from './external-entity-mappings/external-driver-mappings/external-driver-mappings.component';
import { ExternalCarrierMappingsComponent } from './external-entity-mappings/external-carrier-mappings/external-carrier-mappings.component';
import { ExternalBulkPlantMappingsComponent } from './external-entity-mappings/external-bulk-plant-mappings/external-bulk-plant-mappings.component';
import { ExternalVehicleMappingsComponent } from './external-entity-mappings/external-vehicle-mappings/external-vehicle-mappings.component';

const routeSelfService: Routes = [
    {
        path: '',
        component: SelfServiceAliasComponent
    },

]

@NgModule({
    declarations: [
        SelfServiceAliasComponent,
        ProductMappingComponent,
        CustomerMappingComponent,
        EditCustomerComponent,
        CompanyCarrierMappingComponent,
        EditCarrierMappingComponent,
        TerminalMappingComponent,
        TerminalItemCodeMappingComponent,
        CreateTerminalItemCodeComponent,
        ExternalCustomerMappingsComponent,
        ExternalCustomerlocationMappingsComponent,
        ExternalProductMappingsComponent,
        ExternalSupplierMappingsComponent,
        ExternalTerminalMappingsComponent,
        ExternalDriverMappingsComponent,
        ExternalCarrierMappingsComponent,
        ExternalBulkPlantMappingsComponent,
        ExternalVehicleMappingsComponent,
    ],
    imports: [
        SharedModule,
        DirectiveModule,
        DataTablesModule,
        RouterModule.forChild(routeSelfService)
    ]
})
export class SelfServiceAliasModule { }
