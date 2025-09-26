import { TransactionTypeEnum } from '@app/enums/transaction-types.enum';

export class Transaction {
  type: TransactionTypeEnum | undefined;
  details: TransactionDetail[];
  totalAmount: number;
  comment?: string;

  constructor(type?: TransactionTypeEnum, details: TransactionDetail[] = [], comment?: string) {
    this.type = type;
    this.details = details;
    this.comment = comment;
    this.totalAmount = 0;
    this.calculateTotal();
  }

  addDetail(detail: TransactionDetail) {
    // calcular el total del detalle
    detail.total = detail.quantity * detail.unitPrice;
    this.details.push(detail);
    this.calculateTotal();
  }

  removeDetail(index: number) {
    this.details.splice(index, 1);
    this.calculateTotal();
  }

  calculateTotal() {
    this.totalAmount = this.details.reduce((sum, d) => sum + d.quantity * d.unitPrice, 0);
    // actualizar cada detalle también
    this.details.forEach((d) => {
      d.total = d.quantity * d.unitPrice;
    });
  }

  multiply100() {
    this.totalAmount = Math.round(this.totalAmount * 100);
    this.details.forEach((d) => {
      d.quantity = Math.round(d.quantity * 100);
      d.unitPrice = Math.round(d.unitPrice * 100);
      d.total = d.quantity * d.unitPrice;
    });
  }
}

export class TransactionDetail {
  productId: string;
  productName: string;
  productCode: string;
  quantity: number;
  unitPrice: number;
  total: number;

  constructor(productId: string, productName: string, productCode: string, quantity: number, unitPrice: number) {
    this.productId = productId;
    this.productName = productName;
    this.productCode = productCode;
    this.quantity = quantity;
    this.unitPrice = unitPrice;
    this.total = this.quantity * this.unitPrice;
  }

  calculateTotalDetail() {
    this.total = this.quantity * this.unitPrice;
  }
}
