export enum TransactionTypeEnum {
  Purchase = 1,          // Compra
  Sale = 2,              // Venta
  Return = 3,            // Devolución de ventas
  PurchaseReturn = 4,    // Devolución de compra
  PurchaseAdjustment = 5, // Ajuste de compra
  SaleAdjustment = 6      // Ajuste de venta
}


export const TransactionTypeOptions = [
  { key: TransactionTypeEnum.Purchase, label: 'Compra' },
  { key: TransactionTypeEnum.Sale, label: 'Venta' },
  { key: TransactionTypeEnum.Return, label: 'Devolución de ventas' },
  { key: TransactionTypeEnum.PurchaseReturn, label: 'Devolución de compra' },
  { key: TransactionTypeEnum.PurchaseAdjustment, label: 'Ajuste de compra' },
  { key: TransactionTypeEnum.SaleAdjustment, label: 'Ajuste de venta' }
];

export interface TransactionTypeOption {
  key: TransactionTypeEnum;
  label: string;
}
