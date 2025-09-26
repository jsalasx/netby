import { Component } from '@angular/core';
import { ListTransactions } from "@app/components/transactions/list-transactions/list-transactions";

@Component({
  selector: 'app-transactions-page',
  imports: [ListTransactions],
  templateUrl: './transactions-page.html',
  styleUrl: './transactions-page.css'
})
export class TransactionsPage {

}
