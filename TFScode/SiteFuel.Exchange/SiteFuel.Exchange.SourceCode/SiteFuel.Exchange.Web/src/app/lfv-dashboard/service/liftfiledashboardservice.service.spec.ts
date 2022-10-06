import { TestBed } from '@angular/core/testing';

import { LiftfiledashboardserviceService } from './liftfiledashboardservice.service';

describe('LiftfiledashboardserviceService', () => {
  beforeEach(() => TestBed.configureTestingModule({}));

  it('should be created', () => {
    const service: LiftfiledashboardserviceService = TestBed.get(LiftfiledashboardserviceService);
    expect(service).toBeTruthy();
  });
});
