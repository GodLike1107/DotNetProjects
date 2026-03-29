import { Component, Input, Output, EventEmitter } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterLink } from '@angular/router';
import { Property } from '../../models/property.model';

@Component({
  selector: 'app-property-card',
  standalone: true,
  imports: [CommonModule, RouterLink],
  templateUrl: './property-card.component.html',
  styleUrl: './property-card.component.css'
})
export class PropertyCardComponent {
  @Input() property!: Property;
  @Output() favoriteToggled = new EventEmitter<number>();

  onFavoriteClick(event: Event) {
    event.preventDefault();
    event.stopPropagation();
    this.favoriteToggled.emit(this.property.id);
  }

  getPropertyImage(): string {
    // Use generated placeholder with gradient based on property id
    const colors = [
      ['#E23744', '#FF6659'],
      ['#1A1A2E', '#16213E'],
      ['#F5A623', '#FFD166'],
      ['#00C853', '#69F0AE'],
      ['#2196F3', '#64B5F6'],
      ['#9C27B0', '#CE93D8'],
      ['#FF5722', '#FF8A65'],
      ['#607D8B', '#90A4AE'],
    ];
    const idx = (this.property.id - 1) % colors.length;
    return `linear-gradient(135deg, ${colors[idx][0]}, ${colors[idx][1]})`;
  }
}
