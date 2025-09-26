import { CommonModule } from '@angular/common';
import { Component, inject, signal } from '@angular/core';
import { FormArray, FormControl, FormGroup, ReactiveFormsModule } from '@angular/forms';
import {
  ProductDto,
} from '@app/services/products/product-service';
import { ButtonModule } from 'primeng/button';
import { DialogModule } from 'primeng/dialog';
import { InputNumberModule } from 'primeng/inputnumber';
import { InputTextModule } from 'primeng/inputtext';
import { TextareaModule } from 'primeng/textarea';
import { SelectModule } from 'primeng/select';
import { TransactionTypeOption, TransactionTypeOptions } from '@app/enums/transaction-types.enum';
import { ChooseProducts } from '../choose-products/choose-products';
import { Transaction, TransactionDetail } from '../transaction.model';
import { MessageService } from 'primeng/api';
import { TransactionServices } from '@app/services/transactions/transaction-services';

@Component({
  selector: 'app-save-transaction',
  imports: [
    DialogModule,
    ButtonModule,
    InputTextModule,
    ReactiveFormsModule,
    InputNumberModule,
    TextareaModule,
    SelectModule,
    CommonModule,
    ChooseProducts,
  ],
  templateUrl: './save-transaction.html',
  styleUrl: './save-transaction.css',
  providers: [MessageService]
})
export class SaveTransaction {
  visible = signal(false);
  transactionTypes = TransactionTypeOptions;
  selectedTransactionType: TransactionTypeOption | undefined = undefined;
  messageService = inject(MessageService);
  transactionFinal = signal<Transaction | undefined>(undefined);
  transactionService = inject(TransactionServices);

  updateTotalPrice(index: number) {
    const detailGroup = this.details.at(index) as FormGroup;
    const quantity = detailGroup.get('quantity')?.value || 0;
    const unitPrice = detailGroup.get('unitPrice')?.value || 0;
    detailGroup.patchValue({ totalPrice: quantity * unitPrice });
    this.transformToTransaction();
  }

  onProductsSelected(products: ProductDto[]) {
    this.details.clear();
    products.forEach((product) => {
      if (product.cantidad) {
        const detail = this.createDetail();
        detail.patchValue({
          quantity: product.cantidad,
          productId: product.id,
          productName : product.name,
          productCode : product.code,
          unitPrice: product.price /100 ,
          totalPrice: (product.price /100 ) * product.cantidad,
        });
        this.details.push(detail);
      }
    });
    this.transformToTransaction();
  }

  transformToTransaction() {
    const formValue = this.transactionForm.value;

    if (!formValue.type) {
      this.messageService.add({severity:'error', summary: 'Error', detail: 'Transaction type is required.'});
    }
    if (!formValue.details || formValue.details.length === 0) {
      this.messageService.add({severity:'error', summary: 'Error', detail: 'At least one transaction detail is required.'});
      return;
    }
    const transaction = new Transaction(
      formValue.type as any || undefined,
      formValue.details.map((detail: any) => new TransactionDetail(
        detail.productId,
        detail.productName,
        detail.productCode,
        detail.quantity,
        detail.unitPrice
      )),
      formValue.comment || ""
    );
    transaction.calculateTotal();
    this.transactionFinal.set(transaction);
    this.transactionForm.patchValue({ totalAmount: transaction.totalAmount });
  }

  transactionForm = new FormGroup({
    type: new FormControl(''),
    details: new FormArray<FormGroup>([]), // 👈 aquí usamos FormArray
    totalAmount: new FormControl(0),
    comment: new FormControl(''),
  });

  // Helper para acceder fácilmente al FormArray
  get details(): FormArray {
    return this.transactionForm.get('details') as FormArray;
  }

  // Método para crear un detalle
  createDetail(): FormGroup {
    return new FormGroup({
      productId: new FormControl(''),
      productName: new FormControl(''),
      productCode: new FormControl(''),
      quantity: new FormControl(0),
      unitPrice: new FormControl(0),
      totalPrice: new FormControl(0),
    });
  }

  // Agregar un detalle
  addDetail() {
    console.log('Adding detail');
    console.log(this.details);
    this.details.push(this.createDetail());
  }

  // Eliminar un detalle
  removeDetail(index: number) {
    this.details.removeAt(index);
  }

  showDialog() {
    this.visible.set(true);
  }

  closeDialog() {
    this.visible.set(false);
  }

  onSaveTransaction() {
    console.log(this.transactionForm.value);

    if (!this.transactionForm.value.type) {
      this.messageService.add({severity:'error', summary: 'Error', detail: 'Transaction type is required.'});
      throw new Error('Transaction type is required.');
    }

    if (!this.transactionForm.value.details || this.transactionForm.value.details.length === 0) {
      this.messageService.add({severity:'error', summary: 'Error', detail: 'At least one transaction detail is required.'});
      throw new Error('At least one transaction detail is required.');
    }

    if (!this.transactionForm.value.comment) {
      this.messageService.add({severity:'error', summary: 'Error', detail: 'Comment is required.'});
      throw new Error('Comment is required.');
    }

    this.transactionForm.value.details.forEach((detail: any, index: number) => {
      if (detail.quantity <= 0) {
        this.messageService.add({severity:'error', summary: 'Error', detail: `Quantity must be greater than 0 in detail ${index + 1}.`});
        throw new Error(`Quantity must be greater than 0 in detail ${index + 1}.`);
      }
      if (detail.unitPrice < 0) {
        this.messageService.add({severity:'error', summary: 'Error', detail: `Unit Price cannot be negative in detail ${index + 1}.`});
        throw new Error(`Unit Price cannot be negative in detail ${index + 1}.`);
      }
      if (detail.totalPrice < 0) {
        this.messageService.add({severity:'error', summary: 'Error', detail: `Total Price cannot be negative in detail ${index + 1}.`});
        throw new Error(`Total Price cannot be negative in detail ${index + 1}.`);
      }

      if (detail.totalPrice !== detail.quantity * detail.unitPrice) {
        this.messageService.add({severity:'error', summary: 'Error', detail: `Total Price must equal Quantity * Unit Price in detail ${index + 1}.`});
        throw new Error(`Total Price must equal Quantity * Unit Price in detail ${index + 1}.`);
      }

    });

    const transactionToSave = new Transaction(
      this.transactionForm.value.type as any,
      this.transactionForm.value.details!.map((detail: any) => new TransactionDetail(
        detail.productId,
        detail.productName,
        detail.productCode,
        detail.quantity,
        detail.unitPrice
      )),
      this.transactionForm.value.comment || ""
    );

    transactionToSave.multiply100();
    transactionToSave.calculateTotal();
    console.log(transactionToSave);
    this.transactionService.saveTransaction({
      type: transactionToSave.type!,
      details: transactionToSave.details,
      totalAmount: transactionToSave.totalAmount,
      comment: transactionToSave.comment || ""
    }).subscribe({
      next: (response) => {
        this.messageService.add({severity:'success', summary: 'Success', detail: 'Transaction saved successfully.'});
        console.log('Transaction saved successfully:', response);
      },
      error: (error) => {
        console.error('Error saving transaction:', error);
      }
    });
  }


}
