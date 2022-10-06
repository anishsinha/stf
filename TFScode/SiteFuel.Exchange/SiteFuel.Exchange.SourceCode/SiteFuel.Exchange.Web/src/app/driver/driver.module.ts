import { NgModule } from '@angular/core';
import { DriverScheduleRoutingModule } from './driver-routing.module';
import { DriverScheduleCalenderComponent } from './driver-schedule-calender/driver-schedule-calender.component';
import { CalendarModule, DateAdapter } from 'angular-calendar';
import { adapterFactory } from 'angular-calendar/date-adapters/date-fns';
import { NgbModalModule } from '@ng-bootstrap/ng-bootstrap';
import { SharedModule } from 'src/app/modules/shared.module';
import { FlatpickrModule } from 'angularx-flatpickr';
import { DriverManagementComponent } from './driver-management/driver-management.component';
import { CreateDriverScheduleComponent } from './create-driver-schedule/create-driver-schedule.component';
import { DriverComponent } from './driver/driver.component';
import { ViewDriverComponent } from './driver-management/view-driver/view-driver.component';
import { CreateDriverComponent } from './driver-management/create-driver/create-driver.component';
import { DirectiveModule } from '../modules/directive.module';
import { DataTablesModule } from 'angular-datatables';
import { CreateTrailerScheduleComponent } from './create-trailer-schedule/create-trailer-schedule.component';
import { CreateRegionComponent } from './create-region-schedule/create-region.component';


@NgModule({
    declarations: [DriverScheduleCalenderComponent, DriverManagementComponent, CreateDriverScheduleComponent, DriverComponent, ViewDriverComponent, CreateDriverComponent, CreateTrailerScheduleComponent, CreateRegionComponent],
    
  imports: [
      DriverScheduleRoutingModule,
      SharedModule,
      DirectiveModule,
      NgbModalModule,
      DataTablesModule,
      FlatpickrModule.forRoot(),
      CalendarModule.forRoot({
          provide: DateAdapter,
          useFactory: adapterFactory
      }),
     
  ]
})
export class DriverModule { }
