
import { NgModule } from '@angular/core';
import { ConfirmationPopoverModule } from 'angular-confirmation-popover';
import { AgmCoreModule } from '@agm/core';
import { NgMultiSelectDropDownModule } from 'ng-multiselect-dropdown';
import { environment } from '../../environments/environment'
import { GlobalModule } from './global.module';
import { DirectiveModule } from './directive.module';
import { CreateTrailerComponent } from '../shared-components/create-trailer/create-trailer.component';
import { DemandCaptureChartComponent } from '../shared-components/demand-capture-chart/demand-capture-chart.component';
import { DipTestComponent } from '../shared-components/dip-test/dip-test.component';
import { SendbirdComponent } from '../shared-components/sendbird/sendbird.component';
import { BuyerSendbirdComponent } from '../shared-components/sendbird/buyer-sendbird/buyer-sendbird.component';
import { ForcastingTankViewComponent } from '../shared-components/forcasting/tank-view-component';
import { ConfirmationDialogComponent } from '../shared-components/confirmation-dialog/confirmation-dialog.component';
import { DataTablesModule } from 'angular-datatables';
import { TankChartModule } from '../tank-chart/tank-chart.module';
import { NgbModule } from '@ng-bootstrap/ng-bootstrap';
import { AutocompleteLibModule } from 'angular-ng-autocomplete';
import { ConfirmationDialogService } from '../shared-components/confirmation-dialog/confirmation-dialog.service';

const googleMapAPIKey = environment.googleMapApiKey;

@NgModule({

    declarations: [
        DemandCaptureChartComponent,
        SendbirdComponent,
        CreateTrailerComponent,
        DipTestComponent,
        BuyerSendbirdComponent,
        ForcastingTankViewComponent,
        ConfirmationDialogComponent
    ],

    imports: [
        GlobalModule,
        NgbModule,
        AgmCoreModule.forRoot({
            apiKey: googleMapAPIKey
        }),
        NgMultiSelectDropDownModule.forRoot(),
        ConfirmationPopoverModule.forRoot({
            confirmButtonType: 'danger' // set defaults here
        }),
        DirectiveModule,
        DataTablesModule,
        TankChartModule,
        AutocompleteLibModule,

    ],

    exports: [
        GlobalModule,
        NgbModule,
        DemandCaptureChartComponent,
        SendbirdComponent,
        CreateTrailerComponent,
        BuyerSendbirdComponent,
        ForcastingTankViewComponent,
        AgmCoreModule,
        NgMultiSelectDropDownModule,
        ConfirmationPopoverModule,
        DipTestComponent,
        AutocompleteLibModule,
    ],
    providers: [ ConfirmationDialogService ],
    entryComponents: [ConfirmationDialogComponent],
})

export class SharedModule { }
