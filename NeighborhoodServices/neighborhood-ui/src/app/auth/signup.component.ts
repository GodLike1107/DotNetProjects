import { Component, OnInit } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { Router, RouterModule } from '@angular/router';
import { HttpClient, HttpClientModule } from '@angular/common/http';

declare const google: any;

@Component({
  selector: 'app-signup',
  standalone: true,
  imports: [CommonModule, FormsModule, RouterModule, HttpClientModule],
  templateUrl: './signup.component.html',
  styleUrls: ['./signup.component.css']
})
export class SignupComponent implements OnInit {
  name = '';
  email = '';
  password = '';
  confirmPassword = '';
  error: string | null = null;
  success: string | null = null;
  isLoading = false;

  constructor(
    private router: Router,
    private http: HttpClient
  ) {}

  ngOnInit(): void {
    // Initialize Google Sign-In
    google.accounts.id.initialize({
      client_id: '610496913959-f750248v5n8n1pkg1vc986trlho8t892.apps.googleusercontent.com',
      callback: this.handleCredentialResponse.bind(this)
    });

    // Render the Google Sign-In button
    google.accounts.id.renderButton(
      document.getElementById('g_id_signin'),
      {
        theme: 'outline',
        size: 'large',
        shape: 'pill',
        width: 280
      }
    );
  }

  handleCredentialResponse(response: any) {
    const idToken = response.credential;
    this.error = null;
    this.success = null;
    this.isLoading = true;

    this.http.post('https://localhost:7121/api/auth/google', {
      idToken: idToken
    }, {
      responseType: 'text' as 'json'
    }).subscribe({
      next: () => {
        this.isLoading = false;
        this.router.navigate(['/services']);
      },
      error: (err) => {
        this.error = this.extractError(err);
        this.isLoading = false;
      }
    });
  }

  signup() {
    this.error = null;
    this.success = null;

    if (!this.name || !this.email || !this.password || !this.confirmPassword) {
      this.error = 'All fields are required.';
      return;
    }

    if (this.password !== this.confirmPassword) {
      this.error = 'Passwords do not match.';
      return;
    }

    const payload = {
      name: this.name,
      email: this.email,
      password: this.password,
      role: 'Customer'
    };

    this.isLoading = true;

    this.http.post('https://localhost:7121/api/auth/signup', payload, {
      responseType: 'text' as 'json'
    }).subscribe({
      next: (res: any) => {
        this.success = typeof res === 'string' ? res : 'Signup successful!';
        this.isLoading = false;
        setTimeout(() => this.router.navigate(['/login']), 2000);
      },
      error: (err) => {
        this.error = this.extractError(err);
        this.isLoading = false;
      }
    });
  }

  private extractError(err: any): string {
    if (err?.error) {
      if (typeof err.error === 'string') return err.error;
      if (err.error.message) return err.error.message;
      if (err.error.error) return err.error.error;
    }
    return 'Signup failed. Please try again.';
  }
}
