import { Component, inject, output, signal } from '@angular/core';
import { FormControl, FormGroup, ReactiveFormsModule } from '@angular/forms';
import { ProductService, SaveProductRequestDto } from '@app/services/products/product-service';
import { ButtonModule } from 'primeng/button';
import { DialogModule } from 'primeng/dialog';
import { InputNumberModule } from 'primeng/inputnumber';
import { InputTextModule } from 'primeng/inputtext';
import { TextareaModule } from 'primeng/textarea';
@Component({
  selector: 'app-save-product',
  imports: [DialogModule, ButtonModule, InputTextModule, ReactiveFormsModule,
     InputNumberModule, TextareaModule],
  templateUrl: './save-product.html',
  styleUrl: './save-product.css'
})
export class SaveProduct {
  visible = signal(false);
  productService = inject(ProductService)
  saved = output<void>();

  showDialog() {
    this.visible.set(true);
  }
  closeDialog() {
    this.visible.set(false);
  }

  productForm = new FormGroup({
    name: new FormControl(''),
    description: new FormControl(''),
    code: new FormControl(''),
    category: new FormControl(''),
    imageUri: new FormControl(''),
    price: new FormControl(0),
    stock: new FormControl(0)
  })

  async onSave() {
    console.log(this.productForm.value);
    const req: SaveProductRequestDto = {
      name: this.productForm.value.name || '',
      code: this.productForm.value.code || '',
      category: this.productForm.value.category || '',
      description: this.productForm.value.description || '',
      imageUri: this.productForm.value.imageUri || '',
      price: this.productForm.value.price || 0,
      stock: this.productForm.value.stock || 0
    };
    console.log(req);
    this.productService.SaveProduct(req).subscribe({
      next: (res) => {
        console.log('Product saved successfully', res);
        this.saved.emit();
        this.closeDialog();
      },
      error: (err) => {
        console.error('Error saving product', err);
      }
    });
  }


}
