import { Component, inject, signal } from '@angular/core';
import {
  FilterProductsRequestDto,
  ProductDto,
  ProductService,
} from '@app/services/products/product-service';
import { ButtonModule } from 'primeng/button';
import { TableModule } from 'primeng/table';
import { SaveProduct } from '../save-product/save-product';
import { EditProduct } from '../edit-product/edit-product';
import { PriceFormatPipe } from '@app/pipes/price/price-format-pipe';
import { ContextMenuModule } from 'primeng/contextmenu';
import { ConfirmationService, MessageService, MenuItem } from 'primeng/api';
import { ConfirmDialog } from 'primeng/confirmdialog';
import { ToastModule } from 'primeng/toast';
import { FilterProduct } from '../filter-product/filter-product';
import { ObjectUtils } from '@app/utils/clean-object-utils';
@Component({
  selector: 'app-list-products',
  imports: [
    ButtonModule,
    TableModule,
    SaveProduct,
    EditProduct,
    PriceFormatPipe,
    ContextMenuModule,
    ToastModule,
    ConfirmDialog,
    FilterProduct,
  ],
  templateUrl: './list-products.html',
  styleUrl: './list-products.css',
  providers: [MessageService, ConfirmationService],
})
export class ListProducts {
  productService = inject(ProductService);
  products = signal<ProductDto[]>([]);
  selectedProduct = signal<ProductDto | undefined>(undefined);
  onClickEdit = signal(false);
  messageService = inject(MessageService);
  confirmationService = inject(ConfirmationService);
  page = signal(1);
  size = signal(2);
  totalCount = signal(0);
  first = signal(0);
  last = signal(2);
  _filter = signal<FilterProductsRequestDto>({});

  menuItems: MenuItem[] = [
    {
      label: 'Edit',
      icon: 'pi pi-pencil',
      command: () => this.onEditProduct(this.selectedProduct()!),
    },
    {
      label: 'Delete',
      icon: 'pi pip-fw pi-times',
      command: () => this.onDeleteProduct(this.selectedProduct()!),
    },
  ];

  constructor() {
    this.onLoadProducts();
    this.onClickEdit.set(false);
  }

  pageChange(event: any) {
    console.log(event);
    this.first.set(event.first);
    if (event.first === 0) {

      this.page.set(1);
    } else {
      this.page.set((event.first / event.rows) + 1);
    }
    this.size.set(event.rows);
    this.onLoadProducts(this._filter());
  }



  onLoadProducts(filterAux?: FilterProductsRequestDto) {
    const filter: FilterProductsRequestDto = {
      ...ObjectUtils.cleanObject(filterAux ?? {}),
      page: this.page(),
      pageSize: this.size(),
    };
    console.log('Loading products with filter', filter);
    this.productService.getFilteredProducts(filter).subscribe((products) => {
      console.log(products);
      this.products.set(products.products);
      console.log(this.products);
      this.totalCount.set(products.totalCount);
    });
  }

  onProductSaved() {
    this.onLoadProducts();
    this.onClickEdit.set(false);
  }

  onCloseEdit() {
    this.onClickEdit.set(false);
  }

  onEditProduct(product: ProductDto) {
    this.messageService.add({
      severity: 'info',
      summary: 'Product Selected',
      detail: product.name,
    });
    this.selectedProduct.set(product);
    this.onClickEdit.set(true);
  }

  onDeleteProduct(product: ProductDto) {
    console.log('Delete product', product);
    this.confirmDelete(product);
  }

  onFilterProducts(filter: FilterProductsRequestDto) {
    console.log('Filter products', filter);
    this._filter.set(filter);
    filter.page = 1;
    filter.pageSize = this.size();
    this.first.set(0);
    this.onLoadProducts(filter);
  }

  confirmDelete(product: ProductDto) {
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
        this.productService.DeleteProduct(product.id).subscribe({
          next: (res) => {
            this.products.set(this.products().filter((p) => p.id !== product.id));
            this.messageService.add({
              severity: 'info',
              summary: 'Info',
              detail: 'Producto eliminado correctamente',
              life: 3000,
            });
          },
          error: (err) => {
            this.messageService.add({
              severity: 'error',
              summary: 'Error',
              detail: 'Error no se elimino el producto',
              life: 3000,
            });
            console.error('Error deleting product', err);
          }
        });
      },
      reject: () => {
        this.messageService.add({
          severity: 'error',
          summary: 'Rejected',
          detail: 'Error no se elimino el producto',
          life: 3000,
        });
      },
    });
  }
}
