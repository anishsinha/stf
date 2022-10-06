import { TestBed } from '@angular/core/testing';

import { CreateterminalsService } from './createterminals.service';

describe('CreateterminalsService', () => {
  let service: CreateterminalsService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(CreateterminalsService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
