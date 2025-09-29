import { SaveTransaction } from '../save-transaction/save-transaction';
import { Component, inject, signal } from '@angular/core';
import { ButtonModule } from 'primeng/button';
import { TableModule } from 'primeng/table';
import { ContextMenuModule } from 'primeng/contextmenu';
import { ConfirmationService, MessageService, MenuItem } from 'primeng/api';
import { ConfirmDialog } from 'primeng/confirmdialog';
import { ToastModule } from 'primeng/toast';
import { ObjectUtils } from '@app/utils/clean-object-utils';
import {
  FilterTransactionsRequestDto,
  TransactionResponseDto,
  TransactionServices,
} from '@app/services/transactions/transaction-services';
import { TypeTransactionTypePipe } from '@app/pipes/transaction/type-transaction-type-pipe';
import { GetTransaction } from '../get-transaction/get-transaction';
import { PriceFormatTotalPipe } from '@app/pipes/products/price-format-total-pipe';
import { FilterTransactions } from '../filter-transactions/filter-transactions';
import { DatePipe } from '@angular/common';
@Component({
  selector: 'app-list-transactions',
  imports: [
    SaveTransaction,
    ButtonModule,
    TableModule,
    ContextMenuModule,
    ToastModule,
    ConfirmDialog,
    TypeTransactionTypePipe,
    GetTransaction,
    PriceFormatTotalPipe,
    FilterTransactions,
    DatePipe,
  ],
  templateUrl: './list-transactions.html',
  styleUrl: './list-transactions.css',
  providers: [MessageService, ConfirmationService],
})
export class ListTransactions {
  _onClickView = signal(false);
  transactionService = inject(TransactionServices);
  transactions = signal<TransactionResponseDto[]>([]);
  page = signal(1);
  size = signal(
    10);
  totalCount = signal(0);
  first = signal(0);
  last = signal(2);
  _filter = signal<FilterTransactionsRequestDto>({});

  selectedTransaction = signal<TransactionResponseDto | undefined>(undefined);
  messageService = inject(MessageService);
  confirmationService = inject(ConfirmationService);

  next() {
    this.first.set(this.first() + this.size());
    this.pageChange({ first: this.first(), rows: this.size() });
  }

  prev() {
    this.first.set(this.first() - this.size());
    this.pageChange({ first: this.first(), rows: this.size() });
  }

  reset() {
    this.first.set(0);
    this.pageChange({ first: this.first(), rows: this.size() });
  }

  isLastPage(): boolean {
    return this.first() + this.size() >= this.totalCount();
  }

  isFirstPage(): boolean {
    return this.first() === 0;
  }

  menuItems: MenuItem[] = [
    {
      label: 'View',
      icon: 'pi pi-fw pi-eye',
      command: () => this.onGetTransaction(this.selectedTransaction()!),
    },
    {
      label: 'Delete',
      icon: 'pi pip-fw pi-times',
      command: () => this.onDeleteTransaction(this.selectedTransaction()!),
    },
  ];

  constructor() {
    this.onLoadTransactions();
  }

  pageChange(event: any) {
    console.log(event);
    this.first.set(event.first);
    if (event.first === 0) {

      this.page.set(1);
    } else {
      this.page.set(event.first / event.rows + 1);
    }
    this.size.set(event.rows);
    this.onLoadTransactions(this._filter());
  }

  getUserTimezone(): string {
    return Intl.DateTimeFormat().resolvedOptions().timeZone;
  }

  onCloseTransactionDialog() {
    this.selectedTransaction.set(undefined);
    this._onClickView.set(false);
  }

  onFilterTransactions(filter: FilterTransactionsRequestDto) {
    this._filter.set(filter);
    filter.page = 1;
    filter.pageSize = this.size();
    this.first.set(0);
    this.onLoadTransactions(filter);
  }

  onLoadTransactions(filterAux?: FilterTransactionsRequestDto) {
    const filter: FilterTransactionsRequestDto = {
      page: this.page(),
      pageSize: this.size(),
      ...ObjectUtils.cleanObject(filterAux ?? {}),
    };
    this.transactionService.getFilteredTransactions(filter).subscribe((data) => {
      console.log(data);
      this.transactions.set(data.transactions);
      this.totalCount.set(data.totalCount);
      console.log(this.transactions);
    });
  }

  onGetTransaction(transaction: TransactionResponseDto) {
    console.log('View Transaction');
    console.log(transaction);
    const promise = new Promise<void>((resolve, reject) => {
      if (transaction) {
        this.selectedTransaction.set(transaction);
        resolve();
      } else {
        reject();
      }
    });
    promise
      .then(() => {
        this._onClickView.set(true);
      })
      .catch(() => {
        this.messageService.add({
          severity: 'error',
          summary: 'Error',
          detail: 'Transaction is undefined.',
        });
      });
  }

  onDeleteTransaction(transaction: TransactionResponseDto) {
    this.confirmDelete(transaction);
  }

  confirmDelete(transaction: TransactionResponseDto) {
    this.confirmationService.confirm({
      message: 'Are you sure that you want to proceed?',
      header: 'Confirmation',
      closable: true,
      closeOnEscape: true,
      icon: 'pi pi-exclamation-triangle',
      rejectButtonProps: {
        label: 'Cancel',
        severity: 'secondary',
        outlined: true,
      },
      acceptButtonProps: {
        label: 'Save',
      },
      accept: () => {
        this.transactionService.deleteTransaction(transaction.id).subscribe({
          next: (res) => {
            this.transactions.set(this.transactions().filter((t) => t.id !== transaction.id));
            this.messageService.add({
              severity: 'info',
              summary: 'Confirmed',
            detail: 'Transaction Eliminada',
          });
        },          error: (err) => {
            this.messageService.add({
              severity: 'error',
              summary: 'Error',
              detail: 'Error no se elimino la transacción',
              life: 3000,
            });
          },
        });
      },
      reject: () => {
        this.messageService.add({
          severity: 'error',
          summary: 'Rejected',
          detail: 'No se acepto la eliminación',
          life: 3000,
        });
      },
    });
  }
}
