import { Component, effect, inject, input, output, signal } from '@angular/core';
import { FormControl, FormGroup, ReactiveFormsModule } from '@angular/forms';
import { ProductDto, ProductService, UpdateProductRequestDto } from '@app/services/products/product-service';
import { MessageService } from 'primeng/api';
import { ButtonModule } from 'primeng/button';
import { DialogModule } from 'primeng/dialog';
import { InputNumberModule } from 'primeng/inputnumber';
import { InputTextModule } from 'primeng/inputtext';
import { TextareaModule } from 'primeng/textarea';
import { ToastModule } from 'primeng/toast';
@Component({
  selector: 'app-edit-product',
  imports: [DialogModule, ButtonModule, InputTextModule, ReactiveFormsModule,
     InputNumberModule, TextareaModule, ToastModule],
  templateUrl: './edit-product.html',
  styleUrl: './edit-product.css',
  providers: [MessageService]
})
export class EditProduct {

  visible = signal(false);
  productService = inject(ProductService)
  saved = output<void>();
  onCloseDialog = output<void>();
  product = input<ProductDto | undefined>();
  onClickEdit = input<boolean>(false);
  messageService = inject(MessageService);

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
          price: this.product()?.price ? (this.product()?.price! / 100) : 0,
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
      price: this.productForm.value.price ? this.productForm.value.price * 100 : 0,
    };
    console.log(req);
    this.productService.UpdateProduct(req).subscribe({
      next: (res) => {
        console.log('Product updated successfully', res);
        this.saved.emit();
        this.closeDialog();
        this.messageService.add({severity:'success', summary: 'Success', detail: 'Producto actualizado correctamente'});
      },
      error: (err) => {
        this.messageService.add({severity:'error', summary: 'Error', detail: 'Error al actualizar el producto'});
        console.error('Error updating product', err);
      }
    });
  }

}
