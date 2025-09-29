import { Component, effect, inject, input, output, signal } from '@angular/core';
import { FormControl, FormGroup, ReactiveFormsModule } from '@angular/forms';
import { FilterProductsRequestDto } from '@app/services/products/product-service';
import { ButtonModule } from 'primeng/button';
import { DialogModule } from 'primeng/dialog';
import { InputNumberModule } from 'primeng/inputnumber';
import { InputTextModule } from 'primeng/inputtext';
import { TextareaModule } from 'primeng/textarea';
@Component({
  selector: 'app-filter-product',
  imports: [DialogModule, ButtonModule, InputTextModule, ReactiveFormsModule,
     InputNumberModule, TextareaModule],
  templateUrl: './filter-product.html',
  styleUrl: './filter-product.css'
})
export class FilterProduct {

  filterToFetch = output<FilterProductsRequestDto>();


  productFilterForm = new FormGroup({
    id: new FormControl(''),
    name: new FormControl(''),
    code: new FormControl(''),
    category: new FormControl(''),
    priceGreaterThanEqual: new FormControl(""),
    priceLessThan: new FormControl(""),
    priceEqual: new FormControl(""),
    stockGreaterThanEqual: new FormControl(""),
    stockLessThan: new FormControl(""),
    stockEqual: new FormControl(""),
  })


  onFilter() {
    console.log(this.productFilterForm.value);


    const filter: FilterProductsRequestDto = {
      id: this.productFilterForm.value.id || undefined,
      name: this.productFilterForm.value.name || undefined,
      code: this.productFilterForm.value.code || undefined,
      category: this.productFilterForm.value.category || undefined,
      priceGreaterThanEqual: this.productFilterForm.value.priceGreaterThanEqual ? Number(this.productFilterForm.value.priceGreaterThanEqual)*100 : undefined,
      priceLessThanEqual: this.productFilterForm.value.priceLessThan ? Number(this.productFilterForm.value.priceLessThan)*100 : undefined,
      priceEqual: this.productFilterForm.value.priceEqual ? Number(this.productFilterForm.value.priceEqual)*100 : undefined,
      stockGreaterThanEqual: this.productFilterForm.value.stockGreaterThanEqual ? Number(this.productFilterForm.value.stockGreaterThanEqual)*100 : undefined,
      stockLessThanEqual: this.productFilterForm.value.stockLessThan ? Number(this.productFilterForm.value.stockLessThan)*100 : undefined,
      stockEqual: this.productFilterForm.value.stockEqual ? Number(this.productFilterForm.value.stockEqual)*100 : undefined,

    }

    this.filterToFetch.emit(filter);
  }

   onClear() {
    this.productFilterForm.reset();
  }
}
