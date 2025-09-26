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
    PriceFormatTotalPipe
  ],
  templateUrl: './list-transactions.html',
  styleUrl: './list-transactions.css',
  providers: [MessageService, ConfirmationService],
})
export class ListTransactions {
  _onClickView = signal(false);
  transactionService = inject(TransactionServices);
  transactions = signal<TransactionResponseDto[]>([]);

  selectedTransaction = signal<TransactionResponseDto | undefined>(undefined);
  messageService = inject(MessageService);
  confirmationService = inject(ConfirmationService);

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

  onCloseTransactionDialog() {
    this.selectedTransaction.set(undefined);
    this._onClickView.set(false);
  }

  onLoadTransactions(filterAux?: any) {
    const filter: FilterTransactionsRequestDto = {
      page: 1,
      size: 10,
      ...ObjectUtils.cleanObject(filterAux ?? {}),
    };
    this.transactionService.getFilteredTransactions(filter).subscribe((data) => {
      console.log(data);
      this.transactions.set(data.transactions);
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
    promise.then(() => {
      this._onClickView.set(true);
    }).catch(() => {
      this.messageService.add({severity:'error', summary: 'Error', detail: 'Transaction is undefined.'});
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
        this.transactionService.deleteTransaction(transaction.id).subscribe(() => {
          this.transactions.set(this.transactions().filter((t) => t.id !== transaction.id));
          this.messageService.add({
            severity: 'info',
            summary: 'Confirmed',
            detail: 'You have accepted',
          });
        });
      },
      reject: () => {
        this.messageService.add({
          severity: 'error',
          summary: 'Rejected',
          detail: 'You have rejected',
          life: 3000,
        });
      },
    });
  }
}
