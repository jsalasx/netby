import { Component, effect, inject, input, output, signal } from '@angular/core';
import { FormControl, FormGroup, ReactiveFormsModule } from '@angular/forms';
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

  filterToFetch = output<any>();


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
    this.filterToFetch.emit(this.productFilterForm.value);
  }

   onClear() {
    this.productFilterForm.reset();
  }
}
