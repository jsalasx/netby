import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ListProducts } from './list-products';

describe('ListProducts', () => {
  let component: ListProducts;
  let fixture: ComponentFixture<ListProducts>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [ListProducts]
    })
    .compileComponents();

    fixture = TestBed.createComponent(ListProducts);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
