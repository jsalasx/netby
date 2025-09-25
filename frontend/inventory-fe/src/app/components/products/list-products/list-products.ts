import { Component, inject, signal } from '@angular/core';
import { FilterProductsRequestDto, ProductDto, ProductService } from '@app/services/products/product-service';
import {  ButtonModule } from 'primeng/button';
import { TableModule } from 'primeng/table';
@Component({
  selector: 'app-list-products',
  imports: [ButtonModule, TableModule],
  templateUrl: './list-products.html',
  styleUrl: './list-products.scss'
})
export class ListProducts {

  private productService = inject(ProductService);
  products = signal<ProductDto[]>([]);

  onLoadProducts() {
    const filter: FilterProductsRequestDto = {
        page: 1,
        size: 10
    }
    this.productService.getFilteredProducts(filter).subscribe(products => {
      console.log(products);
      this.products.set(products.products);
      console.log(this.products);
    });
  }

}
