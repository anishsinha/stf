import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { LiftDetailComponent } from './lift-detail.component';

describe('LiftDetailComponent', () => {
  let component: LiftDetailComponent;
  let fixture: ComponentFixture<LiftDetailComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ LiftDetailComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(LiftDetailComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
