// src/app/app.routes.ts

import { Routes } from '@angular/router';
import { HomeComponent } from './home.component';
import { ServicesComponent } from './services.component';
import { MyBookingsComponent } from './my-bookings.component';
import { LoginComponent } from './auth/login.component';
import { SignupComponent } from './auth/signup.component';
import { ForgotPasswordComponent } from './auth/forgot-password.component';
import { ServiceDetailsComponent } from './service-details.component';
import { AuthGuard } from './auth/auth.guard';

export const routes: Routes = [
  { path: '', redirectTo: '/login', pathMatch: 'full' }, // ✅ Login is landing page
  { path: 'login', component: LoginComponent },
  { path: 'signup', component: SignupComponent },
  { path: 'forgot-password', component: ForgotPasswordComponent },
  { path: 'reset-password', loadComponent: () => import('./auth/reset-password.component').then(m => m.ResetPasswordComponent) },
  { path: 'home', component: HomeComponent }, // ✅ Keep this for /home
  { path: 'services', component: ServicesComponent },
  { path: 'service/:id', component: ServiceDetailsComponent },
  { path: 'my-bookings', component: MyBookingsComponent, canActivate: [AuthGuard] },
  { path: '**', redirectTo: '/login' } // fallback
];