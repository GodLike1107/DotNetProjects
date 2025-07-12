import { Component, CUSTOM_ELEMENTS_SCHEMA } from '@angular/core';
import { Router } from '@angular/router';
import { FormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { AuthService } from '../services/auth.service';

@Component({
  selector: 'app-login',
  standalone: true,
  imports: [CommonModule, FormsModule, RouterModule],
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css'],
  schemas: [CUSTOM_ELEMENTS_SCHEMA] // Needed for <google-signin-button>
})
export class LoginComponent {
  email = '';
  password = '';
  error: string | null = null;
  isLoading = false;

  constructor(
    private authService: AuthService,
    private router: Router
  ) {}

  login() {
    if (!this.email || !this.password) {
      this.error = 'Email and password are required.';
      return;
    }

    this.isLoading = true;
    this.error = null;

    this.authService.login(this.email, this.password).subscribe({
      next: () => {
        this.isLoading = false;
        this.router.navigate(['/services']);
      },
      error: (err: any) => {
        console.error('Login failed:', err);
        this.error = 'Invalid login credentials';
        this.isLoading = false;
      }
    });
  }

  handleGoogleSignIn(user: any) {
    if (!user || !user.idToken) {
      this.error = 'Google sign-in failed or cancelled.';
      return;
    }

    console.log('✅ Google user:', user);

    this.authService.loginWithGoogle(user.idToken).subscribe({
      next: () => {
        this.router.navigate(['/services']);
      },
      error: (err) => {
        console.error('❌ Google Login failed:', err);
        this.error = 'Google login failed.';
      }
    });
  }
}
