import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { HttpClient } from '@angular/common/http';
import { Router, ActivatedRoute } from '@angular/router';

interface PasswordStrength {
  score: number;
  label: string;
  color: string;
}

@Component({
  selector: 'app-reset-password',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './reset-password.component.html',
  styleUrls: ['./reset-password.component.css']
})
export class ResetPasswordComponent implements OnInit {
  email: string = '';
  newPassword: string = '';
  confirmPassword: string = '';
  message: string | null = null;
  error: string | null = null;
  isLoading = false;
  showPassword = false;
  showConfirmPassword = false;
  passwordStrength: PasswordStrength = { score: 0, label: '', color: '' };
  passwordErrors: string[] = [];
  isFormValid = false;
  showSuccessPopup = false;

  constructor(
    private http: HttpClient,
    private router: Router,
    private route: ActivatedRoute
  ) {}

  ngOnInit() {
    this.route.queryParams.subscribe(params => {
      this.email = params['email'] || '';
    });
  }

  closeSuccessPopup() {
    this.showSuccessPopup = false;
    this.router.navigate(['/login'], { queryParams: { email: this.email } });
  }

  togglePasswordVisibility() {
    this.showPassword = !this.showPassword;
  }

  toggleConfirmPasswordVisibility() {
    this.showConfirmPassword = !this.showConfirmPassword;
  }

  onPasswordChange() {
    this.calculatePasswordStrength();
    this.validateForm();
  }

  onConfirmPasswordChange() {
    this.validateForm();
  }

  calculatePasswordStrength() {
    this.passwordErrors = [];
    let score = 0;

    if (!this.newPassword) {
      this.passwordStrength = { score: 0, label: '', color: '' };
      return;
    }

    if (this.newPassword.length >= 8) {
      score += 1;
    } else {
      this.passwordErrors.push('At least 8 characters');
    }

    if (/[A-Z]/.test(this.newPassword)) {
      score += 1;
    } else {
      this.passwordErrors.push('One uppercase letter');
    }

    if (/[a-z]/.test(this.newPassword)) {
      score += 1;
    } else {
      this.passwordErrors.push('One lowercase letter');
    }

    if (/\d/.test(this.newPassword)) {
      score += 1;
    } else {
      this.passwordErrors.push('One number');
    }

    if (/[!@#$%^&*(),.?":{}|<>]/.test(this.newPassword)) {
      score += 1;
    } else {
      this.passwordErrors.push('One special character');
    }

    switch (score) {
      case 0:
      case 1:
        this.passwordStrength = { score, label: 'Weak', color: '#ef4444' };
        break;
      case 2:
      case 3:
        this.passwordStrength = { score, label: 'Fair', color: '#f59e0b' };
        break;
      case 4:
        this.passwordStrength = { score, label: 'Good', color: '#3b82f6' };
        break;
      case 5:
        this.passwordStrength = { score, label: 'Strong', color: '#10b981' };
        break;
      default:
        this.passwordStrength = { score: 0, label: '', color: '' };
    }
  }

  validateForm() {
    this.isFormValid =
      this.newPassword.length >= 8 &&
      this.newPassword === this.confirmPassword &&
      this.passwordStrength.score >= 3;
  }

  getPasswordMatchError(): string | null {
    if (!this.confirmPassword) return null;
    return this.newPassword !== this.confirmPassword ? 'Passwords do not match' : null;
  }

  resetPassword() {
    this.error = null;
    this.message = null;

    if (!this.newPassword || !this.confirmPassword) {
      this.error = 'Both password fields are required.';
      return;
    }

    if (this.newPassword !== this.confirmPassword) {
      this.error = 'Passwords do not match.';
      return;
    }

    if (this.passwordStrength.score < 3) {
      this.error = 'Password does not meet minimum requirements.';
      return;
    }

    this.isLoading = true;

    const payload = {
      email: this.email,
      newPassword: this.newPassword
    };

    this.http.post('https://localhost:7121/api/auth/reset-password', payload, {
      responseType: 'text' as 'json'
    }).subscribe({
      next: (res: any) => {
        let successMessage = 'Password reset successful!';
        try {
          const parsedRes = typeof res === 'string' ? JSON.parse(res) : res;
          successMessage = parsedRes?.message || successMessage;
        } catch {
          successMessage = typeof res === 'string' ? res : successMessage;
        }

        this.message = successMessage;
        this.isLoading = false;
        this.showSuccessPopup = true;

        setTimeout(() => {
          this.router.navigate(['/login'], { queryParams: { email: this.email } });
        }, 3000);
      },
      error: (err) => {
        console.error('Reset password error:', err);

        let errorMessage = 'Failed to reset password. Please try again.';
        if (err?.error) {
          if (typeof err.error === 'string') {
            errorMessage = err.error;
          } else if (err.error?.message) {
            errorMessage = err.error.message;
          } else if (err.error?.error) {
            errorMessage = err.error.error;
          } else if (err.error?.title) {
            errorMessage = err.error.title;
          }
        } else if (err?.message && !err.message.includes('JSON')) {
          errorMessage = err.message;
        } else if (typeof err === 'string') {
          errorMessage = err;
        }

        this.error = errorMessage;
        this.isLoading = false;
      }
    });
  }
}
