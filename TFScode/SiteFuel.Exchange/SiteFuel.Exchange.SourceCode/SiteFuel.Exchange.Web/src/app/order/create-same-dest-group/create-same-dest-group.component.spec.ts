import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { CreateSameDestGroupComponent } from './create-same-dest-group.component';

describe('CreateSameDestGroupComponent', () => {
  let component: CreateSameDestGroupComponent;
  let fixture: ComponentFixture<CreateSameDestGroupComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ CreateSameDestGroupComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(CreateSameDestGroupComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
