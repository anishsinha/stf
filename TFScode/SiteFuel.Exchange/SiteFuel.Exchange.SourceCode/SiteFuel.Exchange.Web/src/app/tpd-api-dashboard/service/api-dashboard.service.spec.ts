import { TestBed } from '@angular/core/testing';

import { ApiDashboardService } from './api-dashboard.service';

describe('ApiDashboardService', () => {
  beforeEach(() => TestBed.configureTestingModule({}));

  it('should be created', () => {
    const service: ApiDashboardService = TestBed.get(ApiDashboardService);
    expect(service).toBeTruthy();
  });
});
