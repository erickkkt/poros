import { TestBed } from '@angular/core/testing';

import { DataGlobalService } from './data-global.service';

describe('DataGlobalService', () => {
  beforeEach(() => TestBed.configureTestingModule({}));

  it('should be created', () => {
    const service: DataGlobalService = TestBed.get(DataGlobalService);
    expect(service).toBeTruthy();
  });
});
