import { SaveTransaction } from '../save-transaction/save-transaction';
import { Component, inject, signal } from '@angular/core';
import {
  FilterProductsRequestDto,
  ProductDto,
  ProductService,
} from '@app/services/products/product-service';
import { ButtonModule } from 'primeng/button';
import { TableModule } from 'primeng/table';
import { PriceFormatPipe } from '@app/pipes/price/price-format-pipe';
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
@Component({
  selector: 'app-list-transactions',
  imports: [
    SaveTransaction,
    ButtonModule,
    TableModule,
    PriceFormatPipe,
    ContextMenuModule,
    ToastModule,
    ConfirmDialog,
    TypeTransactionTypePipe,
  ],
  templateUrl: './list-transactions.html',
  styleUrl: './list-transactions.css',
  providers: [MessageService, ConfirmationService],
})
export class ListTransactions {
  transactionService = inject(TransactionServices);
  transactions = signal<TransactionResponseDto[]>([]);

  selectedTransaction = signal<TransactionResponseDto | undefined>(undefined);
  messageService = inject(MessageService);
  confirmationService = inject(ConfirmationService);

  menuItems: MenuItem[] = [
    {
      label: 'Delete',
      icon: 'pi pip-fw pi-times',
      command: () => this.onDeleteTransaction(this.selectedTransaction()!),
    },
  ];

  onLoadTransactions(filterAux?: any) {
    const filter: FilterTransactionsRequestDto = {
      page: 1,
      size: 10,
      ...ObjectUtils.cleanObject(filterAux ?? {})
    };
    this.transactionService.getFilteredTransactions(filter).subscribe((data) => {
      console.log(data);
      this.transactions.set(data.transactions);
      console.log(this.transactions);
    });
  }


  onGetTransaction(transaction: TransactionResponseDto) {
    console.log(transaction);
    this.selectedTransaction.set(transaction);
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
