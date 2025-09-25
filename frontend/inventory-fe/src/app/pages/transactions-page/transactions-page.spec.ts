import { ComponentFixture, TestBed } from '@angular/core/testing';

import { TransactionsPage } from './transactions-page';

describe('TransactionsPage', () => {
  let component: TransactionsPage;
  let fixture: ComponentFixture<TransactionsPage>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [TransactionsPage]
    })
    .compileComponents();

    fixture = TestBed.createComponent(TransactionsPage);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
