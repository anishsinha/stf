import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { DsbCalendarComponent } from './dsb-calendar/dsb-calendar.component';
import { RouterModule, Routes } from '@angular/router';
/*import { FullCalendarModule } from '@fullcalendar/angular'; // must go before plugins*/
import { FlatpickrModule } from 'angularx-flatpickr';
import { CalendarModule, DateAdapter } from 'angular-calendar';
import { adapterFactory } from 'angular-calendar/date-adapters/date-fns';
import { DirectiveModule } from '../modules/directive.module';
import { SharedModule } from '../modules/shared.module';
import { NgDragDropModule } from 'ng-drag-drop';

//FullCalendarModule.registerPlugins([
//    dayGridPlugin,
//    interactionPlugin
//]);

const cal_route: Routes = [
    {
        path: '',
        component: DsbCalendarComponent
    }, {
        path: 'Index',
        component: DsbCalendarComponent
    }];


@NgModule({
    declarations: [DsbCalendarComponent],
  imports: [
      CommonModule,
      SharedModule,
      DirectiveModule,
      NgDragDropModule.forRoot(),
      RouterModule.forChild(cal_route),
      FlatpickrModule.forRoot(),
      CalendarModule.forRoot({
          provide: DateAdapter,
          useFactory: adapterFactory,
      }),
  ]
})
export class TfcalendarModule { }
