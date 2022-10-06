import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ProducDetailComponent } from './produc-detail.component';

describe('ProducDetailComponent', () => {
  let component: ProducDetailComponent;
  let fixture: ComponentFixture<ProducDetailComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ProducDetailComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ProducDetailComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
