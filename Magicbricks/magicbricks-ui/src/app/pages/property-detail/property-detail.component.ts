import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { ActivatedRoute, Router, RouterLink } from '@angular/router';
import { PropertyService } from '../../services/property.service';
import { Property, ContactInquiry } from '../../models/property.model';

@Component({
  selector: 'app-property-detail',
  standalone: true,
  imports: [CommonModule, FormsModule, RouterLink],
  templateUrl: './property-detail.component.html',
  styleUrl: './property-detail.component.css'
})
export class PropertyDetailComponent implements OnInit {
  property: Property | null = null;
  isLoading = true;
  toastMessage = '';
  showToast = false;
  toastType = 'success';

  // Contact Form
  contactForm: ContactInquiry = {
    propertyId: 0,
    name: '',
    email: '',
    phone: '',
    message: ''
  };
  isSubmitting = false;
  formSubmitted = false;

  constructor(
    private propertyService: PropertyService,
    private route: ActivatedRoute,
    private router: Router
  ) {}

  ngOnInit() {
    this.route.params.subscribe(params => {
      const id = +params['id'];
      this.loadProperty(id);
    });
  }

  loadProperty(id: number) {
    this.isLoading = true;
    this.propertyService.getPropertyById(id).subscribe({
      next: (property) => {
        this.property = property;
        this.contactForm.propertyId = property.id;
        this.contactForm.message = `Hi, I am interested in "${property.title}". Please share more details.`;
        this.isLoading = false;
      },
      error: () => {
        this.isLoading = false;
        this.router.navigate(['/search']);
      }
    });
  }

  toggleFavorite() {
    if (!this.property) return;
    this.propertyService.toggleFavorite(this.property.id).subscribe({
      next: (res) => {
        if (this.property) {
          this.property.isFavorited = res.isFavorited;
        }
        this.showToastMessage(res.message, 'success');
      }
    });
  }

  submitContact() {
    if (!this.contactForm.name || !this.contactForm.email) {
      this.showToastMessage('Please fill in required fields', 'error');
      return;
    }

    this.isSubmitting = true;
    this.propertyService.submitContact(this.contactForm).subscribe({
      next: () => {
        this.formSubmitted = true;
        this.isSubmitting = false;
        this.showToastMessage('Inquiry submitted successfully!', 'success');
      },
      error: () => {
        this.isSubmitting = false;
        this.showToastMessage('Failed to submit. Please try again.', 'error');
      }
    });
  }

  getPropertyImage(): string {
    if (!this.property) return '';
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

  private showToastMessage(message: string, type: string) {
    this.toastMessage = message;
    this.toastType = type;
    this.showToast = true;
    setTimeout(() => { this.showToast = false; }, 3000);
  }
}
