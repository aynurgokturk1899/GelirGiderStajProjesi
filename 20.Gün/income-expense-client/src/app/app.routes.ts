import { Routes } from '@angular/router';
import { authGuard, guestGuard } from './core/guards/auth.guard';

export const routes: Routes = [
  { path: 'login', title: 'Giriş Yap | Bütçem', canActivate: [guestGuard], loadComponent: () => import('./pages/auth/login/login').then((c) => c.Login) },
  { path: 'register', title: 'Hesap Oluştur | Bütçem', canActivate: [guestGuard], loadComponent: () => import('./pages/auth/register/register').then((c) => c.Register) },
  {
    path: '',
    canActivate: [authGuard],
    loadComponent: () =>
      import('./layout/app-shell/app-shell').then((component) => component.AppShell),
    children: [
      { path: '', pathMatch: 'full', redirectTo: 'dashboard' },
      {
        path: 'dashboard',
        title: 'Genel Bakış | Bütçem',
        loadComponent: () =>
          import('./pages/dashboard/dashboard').then((component) => component.Dashboard),
      },
      {
        path: 'transactions',
        title: 'İşlemler | Bütçem',
        loadComponent: () =>
          import('./pages/transactions/transactions').then((component) => component.Transactions),
      },
      {
        path: 'categories',
        title: 'Kategoriler | Bütçem',
        loadComponent: () =>
          import('./pages/categories/categories').then((component) => component.Categories),
      },
    ],
  },
  {
    path: '**',
    title: 'Sayfa Bulunamadı | Bütçem',
    loadComponent: () =>
      import('./pages/not-found/not-found').then((component) => component.NotFound),
  },
];
