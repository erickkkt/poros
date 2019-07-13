import { TestBed } from '@angular/core/testing';

import { EventGlobalService } from './event-global.service';

describe('EventGlobalService', () => {
  beforeEach(() => TestBed.configureTestingModule({}));

  it('should be created', () => {
    const service: EventGlobalService = TestBed.get(EventGlobalService);
    expect(service).toBeTruthy();
  });
});
