import { Pipe, PipeTransform } from '@angular/core';

@Pipe({
  name: 'priceFormat'
})
export class PriceFormatPipe implements PipeTransform {

  transform(value: number | null | undefined): string {
    if (value == null) return '0.00'; // caso null/undefined
    return (value / 100).toFixed(2);
  }

}
