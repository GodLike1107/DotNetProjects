// src/app/my-bookings.component.ts

import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { BookingService } from './services/booking.service';
import { AuthService } from './services/auth.service';
import { RazorpayService } from './services/razorpay.service';
import { ServiceBooking } from './models/service-booking.model';
import { BookingDto } from './models/booking.dto';
import jsPDF from 'jspdf';
import autoTable from 'jspdf-autotable';

@Component({
  selector: 'app-my-bookings',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './my-bookings.component.html',
  styleUrls: ['./my-bookings.component.css']
})
export class MyBookingsComponent implements OnInit {
  userRole: string | null = null;
  isLoading = true;
  error: string | null = null;

  customerBookings: ServiceBooking[] = [];
  providerBookings: BookingDto[] = [];

  constructor(
    private bookingService: BookingService,
    private authService: AuthService,
    private razorpayService: RazorpayService
  ) {}

  ngOnInit(): void {
    this.userRole = this.authService.getRole();
    if (this.userRole === 'Customer') {
      this.fetchCustomerBookings();
    } else if (this.userRole === 'Provider') {
      this.fetchProviderBookings();
    }
  }

  fetchCustomerBookings() {
    this.bookingService.getMyBookings().subscribe({
      next: (data: any) => {
        this.customerBookings = data?.$values ?? data;
        this.isLoading = false;
      },
      error: (err) => {
        console.error('❌ Customer booking fetch failed:', err);
        this.error = 'Failed to load customer bookings';
        this.isLoading = false;
      }
    });
  }

  fetchProviderBookings() {
    this.bookingService.getProviderBookings().subscribe({
      next: (data: any) => {
        this.providerBookings = data?.$values ?? [];
        this.isLoading = false;
      },
      error: (err) => {
        console.error('❌ Provider booking fetch failed:', err);
        this.error = 'Failed to load provider bookings';
        this.isLoading = false;
      }
    });
  }

  cancelBooking(bookingId: number) {
    if (confirm('Are you sure you want to cancel this booking?')) {
      this.bookingService.deleteBooking(bookingId).subscribe({
        next: () => {
          this.customerBookings = this.customerBookings.filter(b => b.id !== bookingId);
        },
        error: () => {
          alert('Failed to cancel booking.');
        }
      });
    }
  }

  payNow(booking: ServiceBooking) {
    const customer = this.authService.getUser();
    const name = customer?.name ?? 'Customer';
    const email = customer?.email ?? 'customer@example.com';

    const amount = 499; // 💰 fixed or dynamic amount per service (₹499 here)
    this.razorpayService.pay(amount, booking.id!, name, email);
  }

  exportToPDF() {
    const doc = new jsPDF();
    doc.text('Service Bookings Report', 14, 10);
    autoTable(doc, {
      head: [['#', 'Service Title', 'Customer', 'Scheduled Time', 'Status']],
      body: this.providerBookings.map((b, i) => [
        i + 1,
        b.serviceTitle,
        b.customerName,
        new Date(b.scheduledTime).toLocaleString(),
        b.status
      ])
    });
    doc.save('provider-bookings.pdf');
  }

  exportToCSV() {
    const rows = this.providerBookings.map((b, i) => [
      i + 1,
      b.serviceTitle,
      b.customerName,
      new Date(b.scheduledTime).toLocaleString(),
      b.status
    ]);

    const csvContent =
      'Index,Service Title,Customer,Scheduled Time,Status\n' +
      rows.map(e => e.join(',')).join('\n');

    const blob = new Blob([csvContent], { type: 'text/csv' });
    const url = window.URL.createObjectURL(blob);

    const link = document.createElement('a');
    link.href = url;
    link.download = 'provider-bookings.csv';
    link.click();
    window.URL.revokeObjectURL(url);
  }
}
