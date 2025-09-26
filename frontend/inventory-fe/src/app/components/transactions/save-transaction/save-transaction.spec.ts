import { ComponentFixture, TestBed } from '@angular/core/testing';

import { SaveTransaction } from './save-transaction';

describe('SaveTransaction', () => {
  let component: SaveTransaction;
  let fixture: ComponentFixture<SaveTransaction>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [SaveTransaction]
    })
    .compileComponents();

    fixture = TestBed.createComponent(SaveTransaction);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
