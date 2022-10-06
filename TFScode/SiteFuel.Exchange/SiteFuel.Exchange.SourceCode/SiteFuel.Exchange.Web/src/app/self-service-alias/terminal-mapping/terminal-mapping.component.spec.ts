import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { TerminalMappingComponent } from './terminal-mapping.component';

describe('TerminalMappingComponent', () => {
  let component: TerminalMappingComponent;
  let fixture: ComponentFixture<TerminalMappingComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ TerminalMappingComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(TerminalMappingComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
