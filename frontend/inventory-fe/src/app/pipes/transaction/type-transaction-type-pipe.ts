import { Pipe, PipeTransform } from '@angular/core';
import { TransactionTypeEnum } from '@app/enums/transaction-types.enum';

@Pipe({
  name: 'typeTransactionType'
})
export class TypeTransactionTypePipe implements PipeTransform {

  transform(value: number | null | undefined): string {
    if (value == null) return 'Unknown';
    return TransactionTypeEnum[value] ?? 'Unknown';
  }

}
