import { TestBed } from '@angular/core/testing';

import { BuyerwallyboardService } from './buyerwallyboard.service';

describe('BuyerwallyboardService', () => {
  beforeEach(() => TestBed.configureTestingModule({}));

  it('should be created', () => {
    const service: BuyerwallyboardService = TestBed.get(BuyerwallyboardService);
    expect(service).toBeTruthy();
  });
});
