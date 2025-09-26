import { Component } from '@angular/core';
import { SaveTransaction } from "../save-transaction/save-transaction";

@Component({
  selector: 'app-list-transactions',
  imports: [SaveTransaction],
  templateUrl: './list-transactions.html',
  styleUrl: './list-transactions.css'
})
export class ListTransactions {

}
