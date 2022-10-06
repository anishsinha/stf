import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { CompanyCarrierMappingComponent } from './company-carrier-mapping.component';

describe('CompanyCarrierMappingComponent', () => {
  let component: CompanyCarrierMappingComponent;
  let fixture: ComponentFixture<CompanyCarrierMappingComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ CompanyCarrierMappingComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(CompanyCarrierMappingComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
