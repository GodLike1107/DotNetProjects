// src/app/home.component.ts

import { Component, OnInit } from '@angular/core';
import { AuthService } from './services/auth.service';
import { RouterModule, Router } from '@angular/router';
import { CommonModule } from '@angular/common';
import { RecommendationService } from './services/recommendation.service';

@Component({
  selector: 'app-home',
  standalone: true,
  imports: [CommonModule, RouterModule],
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.css'],
})
export class HomeComponent implements OnInit {
  recommendedServices: any[] = [];
  isLoading = true;
  error: string | null = null;

  constructor(
    public authService: AuthService,
    private recommendationService: RecommendationService,
    private router: Router
  ) {}

  ngOnInit(): void {
    const user = this.authService.getUserInfo();
    const userId = user?.['http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier'] || user?.id;

    console.log('🔍 Authenticated user:', user);
    console.log('👤 Extracted userId:', userId);
    const role = this.authService.getRole();
    console.log('👤 Role:', role);

    if (userId && role === 'Customer') {
      this.recommendationService.getRecommendations(userId).subscribe({
        next: (data) => {
          console.log('🧠 Recommended Services:', data);
          this.recommendedServices = data;
          this.isLoading = false;
        },
        error: (err) => {
          console.error('❌ Recommendation API error:', err);
          this.error = 'Failed to load recommendations.';
          this.isLoading = false;
        }
      });
    } else {
      console.warn('⚠️ Invalid user or not a customer. Skipping recommendation fetch.');
      this.isLoading = false;
    }
  }

  logout(): void {
    this.authService.logout();
  }

  bookService(serviceId: number): void {
    this.router.navigate(['/services'], { queryParams: { highlight: serviceId } });
  }
}
