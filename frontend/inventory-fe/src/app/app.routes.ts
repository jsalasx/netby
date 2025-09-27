import { Routes } from '@angular/router';
import { ProductsPage } from './pages/products-page/products-page';
import { TransactionsPage } from './pages/transactions-page/transactions-page';
import { LoginPage } from './pages/login-page/login-page';
import { RegisterPage } from './pages/register-page/register-page';
import { ErrorPage } from './pages/error-page/error-page';

export const routes: Routes = [
  {
    path: '',
    redirectTo: 'login',
    pathMatch: 'full'
  },
  {
    path: 'products',
    component: ProductsPage
  },
  {
    path: 'transactions',
    component: TransactionsPage
  },

  {
    path: 'login',
    component: LoginPage
  },

  {
    path: "register",
    component: RegisterPage
  },

  {
    path: 'error',
    component: ErrorPage
  },

  {
    path: '**',
    redirectTo: 'error',
    pathMatch: 'full'
  }

];
