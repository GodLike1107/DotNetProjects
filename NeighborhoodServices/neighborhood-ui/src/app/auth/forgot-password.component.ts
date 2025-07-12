import { Component } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { FormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';

@Component({
  selector: 'app-forgot-password',
  standalone: true,
  imports: [CommonModule, FormsModule, RouterModule],
  templateUrl: './forgot-password.component.html',
  styleUrls: ['./forgot-password.component.css']
})
export class ForgotPasswordComponent {
  email = '';
  message: string | null = null;
  error: string | null = null;
  isLoading = false;

  constructor(private http: HttpClient) {}

  sendResetLink() {
    if (!this.email) {
      this.error = 'Email is required.';
      return;
    }

    this.isLoading = true;

    this.http.post('https://localhost:7121/api/auth/forgot-password', {
      email: this.email,
      clientAppUrl: 'http://localhost:4200'
    }).subscribe({
      next: () => {
        this.message = 'Reset link sent to your email.';
        this.error = null;
        this.isLoading = false;
      },
      error: (err) => {
        this.error = 'Failed to send reset link.';
        this.message = null;
        this.isLoading = false;
        console.error('Forgot password error:', err);
      }
    });
  }
}
