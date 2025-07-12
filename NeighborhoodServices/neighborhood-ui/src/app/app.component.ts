import { Component } from '@angular/core';
import { Router, RouterModule, RouterOutlet } from '@angular/router';
import { HomeComponent } from './home.component';
import { ServicesComponent } from './services.component';
import { AuthService } from './services/auth.service';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [RouterModule, RouterOutlet, HomeComponent, ServicesComponent, CommonModule],
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent {
  constructor(public auth: AuthService, private router: Router) {}

  getUserName(): string | null {
    const decoded = this.auth.getUserInfo();
    return decoded?.['http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name'] ?? null;
  }

  getUserRole(): string | null {
    return this.auth.getRole();
  }

  logout() {
    this.auth.logout();
    window.location.href = '/login';
  }

  // ✅ Hides navbar on specific pages
  hideNavbar(): boolean {
    const currentUrl = this.router.url.split('?')[0]; // Strip query params
    const hideRoutes = [
      '/login',
      '/signup',
      '/forgot-password',
      '/reset-password'
    ];
    return hideRoutes.includes(currentUrl);
  }
}