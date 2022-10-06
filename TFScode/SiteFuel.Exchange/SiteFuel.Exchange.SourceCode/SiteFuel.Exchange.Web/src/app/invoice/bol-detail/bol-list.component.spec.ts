import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { BolListComponent } from './bol-list.component';

describe('BolListComponent', () => {
  let component: BolListComponent;
  let fixture: ComponentFixture<BolListComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ BolListComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(BolListComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
