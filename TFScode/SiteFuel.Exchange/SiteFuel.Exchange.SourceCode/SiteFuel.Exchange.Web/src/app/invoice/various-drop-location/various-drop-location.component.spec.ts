import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { VariousDropLocationComponent } from './various-drop-location.component';

describe('VariousDropLocationComponent', () => {
  let component: VariousDropLocationComponent;
  let fixture: ComponentFixture<VariousDropLocationComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ VariousDropLocationComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(VariousDropLocationComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
