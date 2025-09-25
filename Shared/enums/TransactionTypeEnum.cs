namespace Shared.Enums;

public enum TransactionTypeEnum
{
    Purchase = 1,    // Compra
    Sale = 2,        // Venta
    Return = 3,      // Devolución de ventas
    PurchaseReturn = 4, // Devolución de compra
    Purchase_Adjustment = 5, // Ajuste de compra
    Sale_Adjustment = 6      // Ajuste de venta

}