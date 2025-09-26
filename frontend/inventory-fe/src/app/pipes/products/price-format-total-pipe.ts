import { Pipe, PipeTransform } from '@angular/core';

@Pipe({
  name: 'priceFormatTotal'
})
export class PriceFormatTotalPipe implements PipeTransform {

  transform(value: number | null | undefined): string {
    if (value == null) return '0.00'; // caso null/undefined
    return (value / 10000).toFixed(2);
  }

}
