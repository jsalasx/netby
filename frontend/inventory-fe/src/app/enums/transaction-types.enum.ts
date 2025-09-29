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

export const TransactionTypeOptionsToSave = [
  { key: TransactionTypeEnum.Purchase, label: 'Compra' },
  { key: TransactionTypeEnum.Sale, label: 'Venta' },
  { key: TransactionTypeEnum.PurchaseAdjustment, label: 'Ajuste de compra' },
  { key: TransactionTypeEnum.SaleAdjustment, label: 'Ajuste de venta' }
];

export function validStockByTransactionType(type: TransactionTypeOption | undefined): boolean {
  if (!type) return false;
  return [TransactionTypeEnum.Sale, TransactionTypeEnum.SaleAdjustment].includes(type.key);
}


export interface TransactionTypeOption {
  key: TransactionTypeEnum;
  label: string;
}


const TransactionTypeMap: Record<number, string> = {
  [TransactionTypeEnum.Purchase]: 'Compra',
  [TransactionTypeEnum.Sale]: 'Venta',
  [TransactionTypeEnum.Return]: 'Devolución de ventas',
  [TransactionTypeEnum.PurchaseReturn]: 'Devolución de compra',
  [TransactionTypeEnum.PurchaseAdjustment]: 'Ajuste de compra',
  [TransactionTypeEnum.SaleAdjustment]: 'Ajuste de venta',
};

export function getTransactionTypeLabel(value: number): string {
  return TransactionTypeMap[value] ?? 'Unknown';
}
