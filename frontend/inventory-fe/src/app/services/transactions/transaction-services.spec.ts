import { TestBed } from '@angular/core/testing';

import { TransactionServices } from './transaction-services';

describe('TransactionServices', () => {
  let service: TransactionServices;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(TransactionServices);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
