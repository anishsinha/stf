import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { SelfServiceAliasComponent } from './self-service-alias.component';

describe('SelfServiceAliasComponent', () => {
  let component: SelfServiceAliasComponent;
  let fixture: ComponentFixture<SelfServiceAliasComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ SelfServiceAliasComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(SelfServiceAliasComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
