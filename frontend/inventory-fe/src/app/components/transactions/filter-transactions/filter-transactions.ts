import { CommonModule } from '@angular/common';
import { Component, effect, inject, input, output, signal } from '@angular/core';
import { FormArray, FormControl, FormGroup, ReactiveFormsModule } from '@angular/forms';
import { FilterTransactionsRequestDto } from '@app/services/transactions/transaction-services';
import { ButtonModule } from 'primeng/button';
import { DialogModule } from 'primeng/dialog';
import { InputNumberModule } from 'primeng/inputnumber';
import { InputTextModule } from 'primeng/inputtext';
import { TextareaModule } from 'primeng/textarea';
import { DatePickerModule } from 'primeng/datepicker';
import { TransactionTypeOptions } from '@app/enums/transaction-types.enum';
import { SelectModule } from 'primeng/select';
@Component({
  selector: 'app-filter-transactions',
  imports: [
    DialogModule,
    ButtonModule,
    InputTextModule,
    ReactiveFormsModule,
    InputNumberModule,
    TextareaModule,
    CommonModule,
    DatePickerModule,
    SelectModule
  ],
  templateUrl: './filter-transactions.html',
  styleUrl: './filter-transactions.css',
})
export class FilterTransactions {
  filterToFetch = output<any>();
  filterDtoToFetch: FilterTransactionsRequestDto | undefined
  transactionTypes = TransactionTypeOptions;

  transactionFilterForm = new FormGroup({
    id: new FormControl(''),
    type: new FormControl(0),
    productIds: new FormArray<FormControl<string>>([]),
    totalAmountGreaterThanEqual: new FormControl(''),
    totalAmountLessThan: new FormControl(''),
    createdAfterEqual: new FormControl(''),
    createdBefore: new FormControl(''),
  });

  formToFilterDto() {
    const formValue = this.transactionFilterForm.value;

    const productIdsValidated = formValue.productIds?.filter((id: string) => id && id.trim() !== '');
    formValue.productIds = productIdsValidated;

    this.filterDtoToFetch = {
      id: formValue.id ? (formValue.id) : undefined,
      type: formValue.type ? (formValue.type) : undefined,
      productIds: formValue.productIds ? formValue.productIds.map((id: string) => (id)) : undefined,
      totalAmountGreaterThanEqual: formValue.totalAmountGreaterThanEqual ? Number(formValue.totalAmountGreaterThanEqual)*10000 : undefined,
      totalAmountLessThan: formValue.totalAmountLessThan ? Number(formValue.totalAmountLessThan)*10000 : undefined,
      createdAfterEqual: formValue.createdAfterEqual ? new Date(formValue.createdAfterEqual) : undefined,
      createdBefore: formValue.createdBefore ? new Date(formValue.createdBefore) : undefined,
    };
    return this.filterDtoToFetch;
  }

  onFilter() {
    console.log(this.transactionFilterForm.value);
    this.filterToFetch.emit(this.formToFilterDto());
  }

  onClear() {
    this.transactionFilterForm.reset();
  }

  productIds = this.transactionFilterForm.get('productIds') as FormArray;

  addProductId() {
    this.productIds.push(new FormControl(''));
  }

  removeProductId(index: number) {
    this.productIds.removeAt(index);
  }
}
