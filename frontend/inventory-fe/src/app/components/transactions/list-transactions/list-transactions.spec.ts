import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ListTransactions } from './list-transactions';

describe('ListTransactions', () => {
  let component: ListTransactions;
  let fixture: ComponentFixture<ListTransactions>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [ListTransactions]
    })
    .compileComponents();

    fixture = TestBed.createComponent(ListTransactions);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
