import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { EditCarrierMappingComponent } from './edit-carrier-mapping.component';

describe('EditCarrierMappingComponent', () => {
  let component: EditCarrierMappingComponent;
  let fixture: ComponentFixture<EditCarrierMappingComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ EditCarrierMappingComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(EditCarrierMappingComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
