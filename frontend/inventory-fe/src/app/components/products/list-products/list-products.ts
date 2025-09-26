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



  onLoadProducts(filterAux?: any) {
    const filter: FilterProductsRequestDto = {
      page: 1,
      size: 10,
      ...ObjectUtils.cleanObject(filterAux ?? {})
    };
    this.productService.getFilteredProducts(filter).subscribe((products) => {
      console.log(products);
      this.products.set(products.products);
      console.log(this.products);
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

  onFilterProducts(filter: any) {
    console.log('Filter products', filter);
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
        this.productService.DeleteProduct(product.id).subscribe(() => {
          this.products.set(this.products().filter((p) => p.id !== product.id));
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
