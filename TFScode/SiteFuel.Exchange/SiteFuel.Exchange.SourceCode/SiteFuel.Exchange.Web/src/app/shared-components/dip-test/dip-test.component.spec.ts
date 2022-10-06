import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { DipTestComponent } from './dip-test.component';

describe('DipTestComponent', () => {
  let component: DipTestComponent;
  let fixture: ComponentFixture<DipTestComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ DipTestComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(DipTestComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
