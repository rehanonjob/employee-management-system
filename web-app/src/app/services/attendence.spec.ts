import { TestBed } from '@angular/core/testing';

import { Attendence } from './attendence';

describe('Attendence', () => {
  let service: Attendence;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(Attendence);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
