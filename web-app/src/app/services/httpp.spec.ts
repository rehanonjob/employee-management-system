import { TestBed } from '@angular/core/testing';

import { Httpp } from './httpp';

describe('Httpp', () => {
  let service: Httpp;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(Httpp);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
