import { Component, signal } from '@angular/core';
import { ButtonModule } from 'primeng/button';
import { DialogModule } from 'primeng/dialog';
import { InputTextModule } from 'primeng/inputtext';
@Component({
  selector: 'app-save-product',
  imports: [DialogModule, ButtonModule, InputTextModule],
  templateUrl: './save-product.html',
  styleUrl: './save-product.css'
})
export class SaveProduct {
  visible = signal(false);

  showDialog() {
    this.visible.set(true);
  }
}
