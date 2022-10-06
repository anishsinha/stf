import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { CreateDriverScheduleComponent } from './create-driver-schedule.component';

describe('CreateDriverScheduleComponent', () => {
  let component: CreateDriverScheduleComponent;
  let fixture: ComponentFixture<CreateDriverScheduleComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ CreateDriverScheduleComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(CreateDriverScheduleComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
