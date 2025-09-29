import { CommonModule } from '@angular/common';
import { Component, inject, output, signal } from '@angular/core';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { ProductService, SaveProductRequestDto } from '@app/services/products/product-service';
import { MessageService } from 'primeng/api';
import { ButtonModule } from 'primeng/button';
import { DialogModule } from 'primeng/dialog';
import { InputNumberModule } from 'primeng/inputnumber';
import { InputTextModule } from 'primeng/inputtext';
import { TextareaModule } from 'primeng/textarea';
import { ToastModule } from 'primeng/toast';
@Component({
  selector: 'app-save-product',
  imports: [
    DialogModule,
    ButtonModule,
    InputTextModule,
    ReactiveFormsModule,
    InputNumberModule,
    TextareaModule,
    ToastModule,
    CommonModule,
  ],
  templateUrl: './save-product.html',
  styleUrl: './save-product.css',
  providers: [MessageService],
})
export class SaveProduct {
  visible = signal(false);
  productService = inject(ProductService);
  saved = output<void>();
  fb = inject(FormBuilder);
  messageService = inject(MessageService);

  showDialog() {
    this.visible.set(true);
  }
  closeDialog() {
    this.visible.set(false);
  }

  productForm: FormGroup = this.fb.group({
    name: ['', [Validators.required, Validators.minLength(3)]],
    code: ['', [Validators.required, Validators.minLength(2)]],
    category: ['', [Validators.required]],
    description: [''],
    imageUri: ['', [Validators.pattern(/^(https?:\/\/.*\.(?:png|jpg|jpeg|gif|svg|webp))$/i)]],
    price: [null, [Validators.required, Validators.min(0.01)]],
    stock: [null, [Validators.required, Validators.min(0)]],
  });

  async onSave() {
    if (this.productForm.invalid) {
      this.messageService.add({
        severity: 'error',
        summary: 'Formulario inválido',
        detail: 'Por favor completa todos los campos obligatorios.',
      });
      this.productForm.markAllAsTouched();
      return;
    }
    console.log(this.productForm.value);
    const req: SaveProductRequestDto = {
      name: this.productForm.value.name || '',
      code: this.productForm.value.code || '',
      category: this.productForm.value.category || '',
      description: this.productForm.value.description || '',
      imageUri: this.productForm.value.imageUri || '',
      price: this.productForm.value.price ? this.productForm.value.price * 100 : 0,
      stock: this.productForm.value.stock ? this.productForm.value.stock * 100 : 0,
    };
    console.log(req);
    this.productService.SaveProduct(req).subscribe({
      next: (res) => {
        console.log('Product saved successfully', res);
        this.saved.emit();
        this.closeDialog();
        this.messageService.add({
          severity: 'success',
          summary: 'Success',
          detail: 'Producto guardado correctamente',
        });
      },
      error: (err) => {
        this.messageService.add({
          severity: 'error',
          summary: 'Error',
          detail: 'Error al guardar el producto',
        });
        console.error('Error saving product', err);
      },
    });
  }
}
