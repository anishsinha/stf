import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ViewOrderGroupComponent } from '../view-order-group/view-order-group.component';
import { OrderRoutingModule } from './order-routing.module';
import { CreateBlendGroupComponent } from '../create-blend-group/create-blend-group.component';
import { CreateSameDestGroupComponent } from '../create-same-dest-group/create-same-dest-group.component';
import { FilterGroupComponent } from '../filter-group/filter-group.component';
import { TermPricingContractComponent } from '../term-pricing-contract.component';
import { SharedModule } from 'src/app/modules/shared.module';
import { DirectiveModule } from 'src/app/modules/directive.module';
import { NgDragDropModule } from 'ng-drag-drop';


@NgModule({

    declarations: [
        ViewOrderGroupComponent,
        CreateBlendGroupComponent,
        CreateSameDestGroupComponent,
        FilterGroupComponent,
        TermPricingContractComponent, 

    ],
    imports: [
        OrderRoutingModule,
        SharedModule,
        DirectiveModule,
        NgDragDropModule.forRoot(),
    ]
})
export class OrderModule { }
