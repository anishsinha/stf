import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { DriverScheduleCalenderComponent } from './driver-schedule-calender/driver-schedule-calender.component';
import { DriverManagementComponent } from './driver-management/driver-management.component';
import { DriverComponent } from './driver/driver.component';

const routeDriver: Routes = [
  {
    path: '',
    component: DriverComponent
  },
  {
    path: 'View',
    component: DriverManagementComponent
  },
  {
    path: 'schedule',
    component: DriverScheduleCalenderComponent
  }
];

@NgModule({
  imports: [
    RouterModule.forChild(routeDriver)
  ],
  exports: [
    RouterModule
  ]
})

export class DriverScheduleRoutingModule { }
