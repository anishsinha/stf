import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { LiftTicketsComponent } from './lift-tickets.component';

describe('LiftTicketsComponent', () => {
  let component: LiftTicketsComponent;
  let fixture: ComponentFixture<LiftTicketsComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ LiftTicketsComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(LiftTicketsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
