import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ChooseProducts } from './choose-products';

describe('ChooseProducts', () => {
  let component: ChooseProducts;
  let fixture: ComponentFixture<ChooseProducts>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [ChooseProducts]
    })
    .compileComponents();

    fixture = TestBed.createComponent(ChooseProducts);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
