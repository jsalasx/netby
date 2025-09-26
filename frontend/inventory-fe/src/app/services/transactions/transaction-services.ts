import { Transaction } from './../../components/transactions/transaction.model';
import { FilterTransactions } from './../../components/transactions/filter-transactions/filter-transactions';
import { HttpClient } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';

export interface SaveTransactionRequestDto {
  type: number;
  details: SaveTransactionDetailRequestDto[];
  totalAmount: number;
  comment: string;
}

export interface SaveTransactionDetailRequestDto {
  productId: string;
  quantity: number;
  unitPrice: number;
  total: number;
}

export interface FilterTransactionsRequestDto {
  id?: string;
  type? : number;
  productsIds?: string[];
  totalAmountGreaterThanEqual?: number;
  totalAmontLessThan?: number;
  createdAfterEqual?: Date;
  createdBefore?: Date;
  page: number;
  size: number;

}

export interface TransactionResponseDto {

  id: string;
  type: number;
  details: TransactionDetailResponseDto[];
  totalAmount: number;
  comment: string;
  createdAt: Date;
  updatedAt: Date;
}

export interface TransactionDetailResponseDto {
  productId: string;
  quantity: number;
  unitPrice: number;
  total: number;
}

export interface TransactionFilterResponseDto {
  transactions: TransactionResponseDto[];
  totalCount: number;
  page: number;
  size: number;
}

@Injectable({
  providedIn: 'root'
})
export class TransactionServices {

  private http = inject(HttpClient);

  private baseUrl = 'http://netby.drkapp.com/api/transaction';

  saveTransaction(req: SaveTransactionRequestDto) {
    return this.http.post(`${this.baseUrl}`, req);
  }

  deleteTransaction(id: string) {
    return this.http.delete(`${this.baseUrl}/${id}`);
  }

  getTransactionById(id: string) {
    return this.http.get(`${this.baseUrl}/${id}`);
  }

  getFilteredTransactions(filter: FilterTransactionsRequestDto) {
    return this.http.post<TransactionFilterResponseDto>(`${this.baseUrl}/filter`, filter);
  }

}
