import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable } from 'rxjs';
import { ServiceBooking } from '../models/service-booking.model';
import { CreateBookingDto } from '../models/create-booking.dto';
import { BookingDto } from '../models/booking.dto';
import { AuthService } from './auth.service';

@Injectable({
  providedIn: 'root'
})
export class BookingService {
  private apiUrl = 'https://localhost:7121/api/bookings';

  constructor(
    private http: HttpClient,
    private authService: AuthService // <-- Inject here
  ) {}

  getMyBookings(): Observable<ServiceBooking[]> {
    return this.http.get<ServiceBooking[]>(this.apiUrl);
  }

  createBooking(booking: CreateBookingDto): Observable<ServiceBooking> {
    return this.http.post<ServiceBooking>(this.apiUrl, booking);
  }

  deleteBooking(id: number): Observable<void> {
    return this.http.delete<void>(`${this.apiUrl}/${id}`);
  }

  getProviderBookings(): Observable<BookingDto[]> {
    const token = this.authService.getToken();
    console.log('📤 Sending token:', token);
    const headers = new HttpHeaders({
      'Authorization': `Bearer ${token}`
    });

    return this.http.get<BookingDto[]>(`${this.apiUrl}/provider`, { headers });
  }
}
