import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { OtherProductTaxesComponent } from './other-product-taxes.component';

describe('OtherProductTaxesComponent', () => {
  let component: OtherProductTaxesComponent;
  let fixture: ComponentFixture<OtherProductTaxesComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ OtherProductTaxesComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(OtherProductTaxesComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
