import { Routes } from '@angular/router';
import { authGuard } from './core/auth/auth.guard';

export const routes: Routes = [
  { path: 'login', loadComponent: () => import('./features/auth/login.component').then(m => m.LoginComponent) },
  {
    path: '',
    canActivate: [authGuard],
    loadComponent: () => import('./layout/shell.component').then(m => m.ShellComponent),
    children: [
      { path: 'dashboard', loadComponent: () => import('./features/dashboard/dashboard.component').then(m => m.DashboardComponent) },
      { path: 'trips', loadComponent: () => import('./features/trips/trip-list.component').then(m => m.TripListComponent) },
      { path: 'trips/:id', loadComponent: () => import('./features/trips/trip-detail.component').then(m => m.TripDetailComponent) },
      { path: 'explore', loadComponent: () => import('./features/explore/explore.component').then(m => m.ExploreComponent) },
      { path: 'assistant', loadComponent: () => import('./features/assistant/assistant.component').then(m => m.AssistantComponent) },
      { path: 'settings', loadComponent: () => import('./features/settings/settings.component').then(m => m.SettingsComponent) },
      { path: '', redirectTo: 'dashboard', pathMatch: 'full' }
    ]
  }
];