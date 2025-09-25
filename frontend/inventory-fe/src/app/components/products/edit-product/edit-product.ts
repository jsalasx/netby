import { Component, effect, inject, input, output, signal } from '@angular/core';
import { FormControl, FormGroup, ReactiveFormsModule } from '@angular/forms';
import { ProductDto, ProductService, UpdateProductRequestDto } from '@app/services/products/product-service';
import { ButtonModule } from 'primeng/button';
import { DialogModule } from 'primeng/dialog';
import { InputNumberModule } from 'primeng/inputnumber';
import { InputTextModule } from 'primeng/inputtext';
import { TextareaModule } from 'primeng/textarea';
@Component({
  selector: 'app-edit-product',
  imports: [DialogModule, ButtonModule, InputTextModule, ReactiveFormsModule,
     InputNumberModule, TextareaModule],
  templateUrl: './edit-product.html',
  styleUrl: './edit-product.css'
})
export class EditProduct {

  visible = signal(false);
  productService = inject(ProductService)
  saved = output<void>();
  onCloseDialog = output<void>();
  product = input<ProductDto | undefined>();
  onClickEdit = input<boolean>(false);

  productForm = new FormGroup({
    name: new FormControl(''),
    description: new FormControl(''),
    category: new FormControl(''),
    imageUri: new FormControl(''),
    price: new FormControl(0),
  })

  constructor() {
    effect(() => {
      if (this.onClickEdit()) {
        console.log("EDIT PRODUCT")
        this.visible.set(true);
      }

      if (this.product()) {
        this.productForm.setValue({
          name: this.product()?.name || '',
          description: this.product()?.description || '',
          category: this.product()?.category || '',
          imageUri: this.product()?.imageUri || '',
          price: this.product()?.price || 0,
        });
      }
    });
  }


  showDialog() {
    this.visible.set(true);
  }

  closeDialog() {
    this.onCloseDialog.emit();
    this.visible.set(false);
  }

  async onSave() {
    console.log(this.productForm.value);
    const req: UpdateProductRequestDto = {
      id : this.product()?.id || '',
      name: this.productForm.value.name || '',
      category: this.productForm.value.category || '',
      description: this.productForm.value.description || '',
      imageUri: this.productForm.value.imageUri || '',
      price: this.productForm.value.price || 0,
    };
    console.log(req);
    this.productService.UpdateProduct(req).subscribe({
      next: (res) => {
        console.log('Product updated successfully', res);
        this.saved.emit();
        this.closeDialog();
      },
      error: (err) => {
        console.error('Error updating product', err);
      }
    });
  }

}
