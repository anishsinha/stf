import { NgModule, ModuleWithProviders } from '@angular/core';
import { CommonModule } from '@angular/common';
import { TankChartComponent } from './tank-chart.component';
import { ForcastingTankChartComponent} from './forcasting/tank-chart.component'
import { FormsModule } from '@angular/forms';
import { ChartsModule } from 'ng2-charts';


@NgModule({
    declarations: [TankChartComponent, ForcastingTankChartComponent],
    imports: [
        CommonModule,
        FormsModule,
        ChartsModule,
        // TankChartRoutingModule
    ],
    exports: [TankChartComponent, ForcastingTankChartComponent]
})
export class TankChartModule {
    static forRoot(): ModuleWithProviders<TankChartModule> {
        return {
            ngModule: TankChartModule,
            providers: [

            ]
        };
    }
}
