import { ComponentFixture, TestBed } from '@angular/core/testing';

import { FilterProduct } from './filter-product';

describe('FilterProduct', () => {
  let component: FilterProduct;
  let fixture: ComponentFixture<FilterProduct>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [FilterProduct]
    })
    .compileComponents();

    fixture = TestBed.createComponent(FilterProduct);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
