import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ViewOrderGroupComponent } from './view-order-group.component';

describe('ViewOrderGroupComponent', () => {
  let component: ViewOrderGroupComponent;
  let fixture: ComponentFixture<ViewOrderGroupComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ViewOrderGroupComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ViewOrderGroupComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
