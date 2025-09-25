import { ComponentFixture, TestBed } from '@angular/core/testing';

import { SaveProduct } from './save-product';

describe('SaveProduct', () => {
  let component: SaveProduct;
  let fixture: ComponentFixture<SaveProduct>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [SaveProduct]
    })
    .compileComponents();

    fixture = TestBed.createComponent(SaveProduct);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
