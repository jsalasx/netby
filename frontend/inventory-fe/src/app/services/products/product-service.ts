import { HttpClient } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';

export interface FilterProductsRequestDto {
  id?: string;
  name?: string;
  category?: string;
  priceGreaterThanEqual?: number;
  priceLessThanEqual?: number;
  stockGreaterThanEqual?: number;
  stockLessThanEqual?: number;
  page: number;
  size: number;
}

export interface ProductResponseDto {
  products: ProductDto[];
  totalCount: number;
  page: number;
  pageSize: number;

}
export interface ProductDto {
  id: string;
  name: string;
  category: string;
  description: string;
  imageUri: string;
  price: number;
  stock: number;
  createdAt: string;
  updatedAt: string;
}

@Injectable({
  providedIn: 'root'
})
export class ProductService {

  private http = inject(HttpClient);

  private baseUrl = 'http://netby.drkapp.com/api/products';



  getFilteredProducts(filter: FilterProductsRequestDto) {
    return this.http.post<ProductResponseDto>(`${this.baseUrl}/filter`, filter);
  }

}
