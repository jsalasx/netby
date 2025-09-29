import { CommonModule } from '@angular/common';
import { Component, inject, input, output, signal } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { FilterProduct } from '@app/components/products/filter-product/filter-product';
import {
  TransactionTypeEnum,
  TransactionTypeOption,
  validStockByTransactionType,
} from '@app/enums/transaction-types.enum';
import { PriceFormatPipe } from '@app/pipes/price/price-format-pipe';
import {
  FilterProductsRequestDto,
  ProductDto,
  ProductService,
} from '@app/services/products/product-service';
import { ObjectUtils } from '@app/utils/clean-object-utils';
import { MessageService } from 'primeng/api';
import { ButtonModule } from 'primeng/button';
import { DialogModule } from 'primeng/dialog';
import { InputNumberModule } from 'primeng/inputnumber';
import { TableModule } from 'primeng/table';
import { ToastModule } from 'primeng/toast';

@Component({
  selector: 'app-choose-products',
  imports: [
    FilterProduct,
    ButtonModule,
    DialogModule,
    TableModule,
    PriceFormatPipe,
    ToastModule,
    InputNumberModule,
    CommonModule,
    FormsModule,
  ],
  templateUrl: './choose-products.html',
  styleUrl: './choose-products.css',
  providers: [MessageService],
})
export class ChooseProducts {
  isTypeSelected = input(false);
  typeSelected = input<TransactionTypeOption | undefined>(undefined);

  productService = inject(ProductService);
  productsToShowTable = signal<ProductDto[]>([]);
  visible = signal(false);

  selectedProducts = signal<ProductDto[]>([]);
  messageService = inject(MessageService);

  productsToEmit = output<ProductDto[]>();

  page = signal(1);
  size = signal(10);
  totalCount = signal(0);
  first = signal(0);
  last = signal(2);
  _filter = signal<FilterProductsRequestDto>({});

  loadProductsToTransaction() {
    console.log('typeSelected', this.typeSelected());
    console.log('Loading products for transaction...');
    console.log(this.selectedProducts());
    console.log(
      'Valid stock by transaction type',
      validStockByTransactionType(this.typeSelected())
    );
    if (validStockByTransactionType(this.typeSelected())) {
      let hasInsufficientStock = false;
      this.selectedProducts().forEach((p) => {
        console.log('Producto en selectedProducts', p);
        if (p.cantidad && p.cantidad > 0) {
          if (p.cantidad * 100 > p.stock) {
            hasInsufficientStock = true;
            this.messageService.add({
              severity: 'error',
              summary: 'Cantidad Insuficiente',
              detail: `La cantidad solicitada para ${p.name} excede el stock disponible.`,
            });
          }
        }
      });
      if (hasInsufficientStock) {
        return;
      } else {
        this.productsToEmit.emit(this.selectedProducts());
        this.closeDialog();
      }
    } else {
      this.productsToEmit.emit(this.selectedProducts());
      this.closeDialog();
    }
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
    this.onLoadProducts(this._filter());
  }

  onLoadProducts(filterAux?: any) {
    const filter: FilterProductsRequestDto = {
      ...ObjectUtils.cleanObject(filterAux ?? {}),
    };
    filter.page = this.page();
    filter.pageSize = this.size();

    this.productService.getFilteredProducts(filter).subscribe((products) => {
      console.log(products);
      this.productsToShowTable.set(products.products);
      console.log(this.productsToShowTable);
      this.totalCount.set(products.totalCount);
    });
  }

  selectProductToTransaction(product: ProductDto) {
    if (!this.selectedProducts().some((p) => p.id === product.id)) {
      this.selectedProducts.set([...this.selectedProducts(), product]);
      this.messageService.add({
        severity: 'info',
        summary: 'Product Deselected',
        detail: `Product ${product.name} deselected from transaction.`,
      });
      return;
    } else {
      this.messageService.add({
        severity: 'warn',
        summary: 'Product Already Selected',
        detail: `Product ${product.name} is already selected for transaction.`,
      });
    }
    console.log(this.selectedProducts());
  }

  deleteProductToTransaction(product: ProductDto) {
    this.selectedProducts.set(this.selectedProducts().filter((p) => p.id !== product.id));
    this.messageService.add({
      severity: 'error',
      summary: 'Product Deleted',
      detail: `Product ${product.name} deleted from transaction.`,
    });
  }

  onFilterProducts(filter: any) {
    console.log('Filter received in choose-products:', filter);
    this.onLoadProducts(filter);
    this._filter.set(filter);
    filter.page = 1;
    filter.pageSize = this.size();
    this.first.set(0);
    this.onLoadProducts(filter);
  }

  showDialog() {
    if (this.isTypeSelected() === false) {
      this.messageService.add({
        severity: 'error',
        summary: 'Error',
        detail: 'Por favor, seleccione un tipo de transacción primero.',
      });
      return;
    } else {
      this.visible.set(true);
    }
  }

  closeDialog() {
    this.visible.set(false);
  }
}
