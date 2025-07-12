import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';

import { ServiceDataService } from './services/service-data.service';
import { BookingService } from './services/booking.service';
import { Service } from './models/service.model';
import { CreateBookingDto } from './models/create-booking.dto';
import { AuthService } from './services/auth.service';

@Component({
  selector: 'app-services',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './services.component.html',
  styleUrls: ['./services.component.css']
})
export class ServicesComponent implements OnInit {
  services: Service[] = [];
  filteredServices: Service[] = [];
  bookingTime: { [serviceId: number]: string } = {};
  categories: string[] = [];
  userRole: string | null = null;

  searchQuery = '';
  selectedCategory = '';

  isLoading = true;
  error: string | null = null;

  highlightedServiceId: number | null = null;

  // Toast Message
  message = '';
  messageType: 'success' | 'error' | 'warning' | '' = '';
  showMessage = false;

  constructor(
    private route: ActivatedRoute,
    private serviceData: ServiceDataService,
    private bookingService: BookingService,
    private authService: AuthService,
    private router: Router
  ) {}

  ngOnInit(): void {
    this.userRole = this.authService.getRole();
    console.log('User Role:', this.userRole);

    this.route.queryParams.subscribe(params => {
      const highlightId = params['highlight'];
      if (highlightId) {
        this.highlightedServiceId = +highlightId;
        this.loadHighlightedService(this.highlightedServiceId);
      } else {
        this.highlightedServiceId = null;
        this.loadServices();
      }
    });
  }

  canBook(): boolean {
    return this.userRole?.toLowerCase() === 'customer';
  }

  loadServices(): void {
    this.serviceData.getServices().subscribe({
      next: (data: any) => {
        this.services = data?.$values ?? data;
        this.filteredServices = [...this.services];
        this.extractCategories();
        this.isLoading = false;
      },
      error: () => {
        this.error = 'Failed to load services';
        this.isLoading = false;
      }
    });
  }

  loadHighlightedService(serviceId: number): void {
    this.serviceData.getServiceById(serviceId).subscribe({
      next: (service) => {
        this.services = [service];
        this.filteredServices = [service];
        this.extractCategories();
        this.isLoading = false;

        setTimeout(() => {
          const el = document.getElementById(`service-${service.id}`);
          if (el) el.scrollIntoView({ behavior: 'smooth', block: 'center' });
        }, 200);
      },
      error: () => {
        this.error = 'Highlighted service not found';
        this.isLoading = false;
      }
    });
  }

  extractCategories(): void {
    const categorySet = new Set<string>();
    this.services.forEach(s => {
      if (s.category) categorySet.add(s.category);
    });
    this.categories = Array.from(categorySet);
  }

  filterServices(): void {
    this.filteredServices = this.services.filter(service => {
      const matchesSearch =
        service.title?.toLowerCase().includes(this.searchQuery.toLowerCase()) ||
        service.description?.toLowerCase().includes(this.searchQuery.toLowerCase());

      const matchesCategory =
        !this.selectedCategory || service.category === this.selectedCategory;

      return matchesSearch && matchesCategory;
    });
  }

  showMessageWithType(message: string, type: 'success' | 'error' | 'warning'): void {
    this.message = message;
    this.messageType = type;
    this.showMessage = true;
    setTimeout(() => this.hideMessage(), 5000);
  }

  hideMessage(): void {
    this.showMessage = false;
    this.message = '';
    this.messageType = '';
  }

  bookService(serviceId: number): void {
    const selectedTime = this.bookingTime[serviceId];
    if (!selectedTime) {
      this.showMessageWithType('Please choose a date and time.', 'warning');
      return;
    }

    const scheduledTime = new Date(selectedTime);
    const now = new Date();
    if (scheduledTime < now) {
      this.showMessageWithType('Cannot book a service in the past.', 'error');
      return;
    }

    const booking: CreateBookingDto = {
      serviceId,
      scheduledTime: scheduledTime.toISOString(),
      status: 'Pending'
    };

    this.bookingService.createBooking(booking).subscribe({
      next: () => {
        this.showMessageWithType('Booking successful!', 'success');
        this.bookingTime[serviceId] = '';
      },
      error: (err) => {
        let errorMessage = 'Booking failed. Try again.';
        if (err.error?.message) {
          errorMessage = err.error.message;
        } else if (err.error?.errors) {
          const messages = Object.values(err.error.errors).flat();
          errorMessage = messages.join(', ');
        }
        this.showMessageWithType(errorMessage, 'error');
      }
    });
  }

  viewDetails(serviceId: number): void {
    this.router.navigate(['/service', serviceId]);
  }

  clearHighlight(): void {
    this.router.navigate(['/services']);
  }
}
