import { __decorate } from "tslib";
import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { CreateNominationComponent } from './create-nomination/create-nomination.component';
import { RouterModule } from '@angular/router';
import { SharedModule } from '../modules/shared.module';
import { DirectiveModule } from '../modules/directive.module';
import { DataTablesModule } from 'angular-datatables';
import { FormsModule } from '@angular/forms';
import { AutocompleteLibModule } from 'angular-ng-autocomplete';
import { NgbModule } from '@ng-bootstrap/ng-bootstrap';
import { FeesModule } from '../fees/fees.module';
import { ConfirmationPopoverModule } from 'angular-confirmation-popover';
const route = [
    {
        path: 'CreateTPONomination',
        component: CreateNominationComponent
    },
    // {
    //   path: 'SourcingRequest/Details/:Id',
    //   component: CreateNominationComponent,
    // }
];
let NominationTPOModule = class NominationTPOModule {
};
NominationTPOModule = __decorate([
    NgModule({
        declarations: [CreateNominationComponent],
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
        ],
    })
], NominationTPOModule);
export { NominationTPOModule };
//# sourceMappingURL=nomination-tpo.module.js.map