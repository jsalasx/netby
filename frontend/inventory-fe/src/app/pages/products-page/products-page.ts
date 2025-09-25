import { Component } from '@angular/core';
import { ListProducts } from "../../components/products/list-products/list-products";

@Component({
  selector: 'app-products-page',
  imports: [ListProducts],
  templateUrl: './products-page.html',
  styleUrl: './products-page.css'
})
export class ProductsPage {

}
