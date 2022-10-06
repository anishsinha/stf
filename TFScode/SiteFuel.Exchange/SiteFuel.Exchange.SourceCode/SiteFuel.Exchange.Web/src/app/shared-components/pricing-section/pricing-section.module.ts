import { NgModule } from '@angular/core';
import { AgmDirectionModule } from 'agm-direction';
import { DirectiveModule } from 'src/app/modules/directive.module';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { PricingSectionComponent } from './pricing-section.component';
import { AutocompleteLibModule } from 'angular-ng-autocomplete';
import { GlobalModule } from 'src/app/modules/global.module';

@NgModule({
    declarations: [
        PricingSectionComponent
    ],
    imports: [
        GlobalModule,
        AutocompleteLibModule,
        DirectiveModule,
        AgmDirectionModule,
        FormsModule
    ],
    exports: [
        PricingSectionComponent
    ]
})
export class PricingSectionModule { }
