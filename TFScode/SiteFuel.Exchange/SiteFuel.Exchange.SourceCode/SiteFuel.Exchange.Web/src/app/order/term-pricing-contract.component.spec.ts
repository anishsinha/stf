import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { TermPricingContractComponent } from './term-pricing-contract.component';

describe('TermPricingContractComponent', () => {
  let component: TermPricingContractComponent;
  let fixture: ComponentFixture<TermPricingContractComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ TermPricingContractComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(TermPricingContractComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
