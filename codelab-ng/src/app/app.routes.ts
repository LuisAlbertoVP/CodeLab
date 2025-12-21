import { Routes } from '@angular/router';
import { noAuthenticatedGuard } from './core/guards/no-authenticated-guard';

export const routes: Routes = [
    {
        path: '',
        loadComponent: () => import('./login-component/login-component').then(m => m.LoginComponent)
    },
    {
        path: 'dashboard',
        loadComponent: () => import('./dashboard/dashboard').then(m => m.Dashboard),
        canActivate: [noAuthenticatedGuard]
    }
];
