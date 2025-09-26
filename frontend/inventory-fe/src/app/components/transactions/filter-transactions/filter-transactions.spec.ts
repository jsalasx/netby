import { ComponentFixture, TestBed } from '@angular/core/testing';

import { FilterTransactions } from './filter-transactions';

describe('FilterTransactions', () => {
  let component: FilterTransactions;
  let fixture: ComponentFixture<FilterTransactions>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [FilterTransactions]
    })
    .compileComponents();

    fixture = TestBed.createComponent(FilterTransactions);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
