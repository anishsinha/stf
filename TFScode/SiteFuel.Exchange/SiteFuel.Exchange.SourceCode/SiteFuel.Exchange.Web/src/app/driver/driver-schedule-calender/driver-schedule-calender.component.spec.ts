import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { DriverScheduleCalenderComponent } from './driver-schedule-calender.component';

describe('DriverScheduleCalenderComponent', () => {
  let component: DriverScheduleCalenderComponent;
  let fixture: ComponentFixture<DriverScheduleCalenderComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ DriverScheduleCalenderComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(DriverScheduleCalenderComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
