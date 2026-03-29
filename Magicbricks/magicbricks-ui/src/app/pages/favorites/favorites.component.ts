import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterLink } from '@angular/router';
import { PropertyCardComponent } from '../../components/property-card/property-card.component';
import { PropertyService } from '../../services/property.service';
import { Property } from '../../models/property.model';

@Component({
  selector: 'app-favorites',
  standalone: true,
  imports: [CommonModule, RouterLink, PropertyCardComponent],
  templateUrl: './favorites.component.html',
  styleUrl: './favorites.component.css'
})
export class FavoritesComponent implements OnInit {
  favorites: Property[] = [];
  isLoading = true;
  toastMessage = '';
  showToast = false;

  constructor(private propertyService: PropertyService) {}

  ngOnInit() {
    this.loadFavorites();
  }

  loadFavorites() {
    this.isLoading = true;
    this.propertyService.getFavorites().subscribe({
      next: (properties) => {
        this.favorites = properties;
        this.isLoading = false;
      },
      error: () => {
        this.favorites = [];
        this.isLoading = false;
      }
    });
  }

  toggleFavorite(propertyId: number) {
    this.propertyService.toggleFavorite(propertyId).subscribe({
      next: (res) => {
        if (!res.isFavorited) {
          this.favorites = this.favorites.filter(p => p.id !== propertyId);
        }
        this.showToastMessage(res.message);
      }
    });
  }

  private showToastMessage(message: string) {
    this.toastMessage = message;
    this.showToast = true;
    setTimeout(() => { this.showToast = false; }, 3000);
  }
}
