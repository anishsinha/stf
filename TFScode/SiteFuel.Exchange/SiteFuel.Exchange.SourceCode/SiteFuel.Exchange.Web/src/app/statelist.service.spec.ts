import { TestBed } from '@angular/core/testing';

import { StatelistService } from './statelist.service';

describe('StatelistService', () => {
  beforeEach(() => TestBed.configureTestingModule({}));

  it('should be created', () => {
    const service: StatelistService = TestBed.get(StatelistService);
    expect(service).toBeTruthy();
  });
});
