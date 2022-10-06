import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FeeListComponent } from './fee-list/fee-list.component';
import { FeeTypeComponent } from './fee-list/fee-type.component';
import { FilterPipe } from './fee-list/filter.pipe';
import { GlobalModule } from '../modules/global.module';
import { NgbModule } from '@ng-bootstrap/ng-bootstrap';
import { NgMultiSelectDropDownModule } from 'ng-multiselect-dropdown';
import { ConfirmationPopoverModule } from 'angular-confirmation-popover';
import { DirectiveModule } from '../modules/directive.module';
import { AngularMultiSelectModule } from 'angular2-multiselect-dropdown';

@NgModule({
    declarations: [
        FeeListComponent,
        FeeTypeComponent,
        FilterPipe],
    imports: [
        CommonModule,
        GlobalModule,
        NgbModule,
        NgMultiSelectDropDownModule.forRoot(),
        ConfirmationPopoverModule.forRoot({
            confirmButtonType: 'danger' // set defaults here
        }),
        DirectiveModule,
        AngularMultiSelectModule
    ],
    exports: [
        FeeListComponent, 
        FeeTypeComponent,
        FilterPipe]
})
export class FeesModule { }
