import { ComponentFixture, TestBed } from '@angular/core/testing';

import { GetTransaction } from './get-transaction';

describe('GetTransaction', () => {
  let component: GetTransaction;
  let fixture: ComponentFixture<GetTransaction>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [GetTransaction]
    })
    .compileComponents();

    fixture = TestBed.createComponent(GetTransaction);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
