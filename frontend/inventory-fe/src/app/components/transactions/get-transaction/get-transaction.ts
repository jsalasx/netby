import { CommonModule } from '@angular/common';
import { Component, effect, input, output, signal } from '@angular/core';
import { ReactiveFormsModule } from '@angular/forms';
import { ButtonModule } from 'primeng/button';
import { DialogModule } from 'primeng/dialog';
import { InputNumberModule } from 'primeng/inputnumber';
import { InputTextModule } from 'primeng/inputtext';
import { TextareaModule } from 'primeng/textarea';
import { SelectModule } from 'primeng/select';
import { TransactionResponseDto } from '@app/services/transactions/transaction-services';
import { TransactionTypeEnum } from '@app/enums/transaction-types.enum';
import { PriceFormatPipe } from '@app/pipes/price/price-format-pipe';
import { PriceFormatTotalPipe } from '@app/pipes/products/price-format-total-pipe';
@Component({
  selector: 'app-get-transaction',
  imports: [DialogModule,
    ButtonModule,
    InputTextModule,
    ReactiveFormsModule,
    InputNumberModule,
    TextareaModule,
    SelectModule,
    CommonModule,
    PriceFormatPipe,
    PriceFormatTotalPipe
    ,],
  templateUrl: './get-transaction.html',
  styleUrl: './get-transaction.css'
})
export class GetTransaction {

  visible = signal(false);
  transaction = input<TransactionResponseDto | undefined>(undefined);
  onCloseDialog = output<void>();
  onClickView = input<boolean>(false);
  TransactionTypeEnum = TransactionTypeEnum;

  closeDialog() {
    this.visible.set(false);
    this.onCloseDialog.emit();
  }

  constructor() {
    effect(() => {
      if (this.onClickView()) {
        console.log("VIEW TRANSACTION")
        console.log(this.transaction())
        this.visible.set(true);
      }else {
        console.log("HIDE TRANSACTION")
        this.visible.set(false);
      }
    });
  }

}
