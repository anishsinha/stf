import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { TankChartComponent } from './tank-chart.component';


const routes: Routes = [
  {path:'',component:TankChartComponent}
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class TankChartRoutingModule { }
