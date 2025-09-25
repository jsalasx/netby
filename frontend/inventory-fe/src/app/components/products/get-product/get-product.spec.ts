import { ComponentFixture, TestBed } from '@angular/core/testing';

import { GetProduct } from './get-product';

describe('GetProduct', () => {
  let component: GetProduct;
  let fixture: ComponentFixture<GetProduct>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [GetProduct]
    })
    .compileComponents();

    fixture = TestBed.createComponent(GetProduct);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
