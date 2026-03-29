import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Property, PaginatedResponse, PropertySearch, ContactInquiry } from '../models/property.model';

@Injectable({
  providedIn: 'root'
})
export class PropertyService {
  private apiUrl = 'https://localhost:5001/api/properties';

  constructor(private http: HttpClient) {}

  private getSessionId(): string {
    let sessionId = localStorage.getItem('mb_session_id');
    if (!sessionId) {
      sessionId = crypto.randomUUID();
      localStorage.setItem('mb_session_id', sessionId);
    }
    return sessionId;
  }

  private getHeaders() {
    return { 'X-Session-Id': this.getSessionId() };
  }

  searchProperties(search: PropertySearch): Observable<PaginatedResponse<Property>> {
    let params = new HttpParams();
    if (search.city) params = params.set('city', search.city);
    if (search.listingType) params = params.set('listingType', search.listingType);
    if (search.minBudget) params = params.set('minBudget', search.minBudget.toString());
    if (search.maxBudget) params = params.set('maxBudget', search.maxBudget.toString());
    if (search.bedrooms) params = params.set('bedrooms', search.bedrooms.toString());
    if (search.propertyType) params = params.set('propertyType', search.propertyType);
    if (search.searchText) params = params.set('searchText', search.searchText);
    if (search.furnishing) params = params.set('furnishing', search.furnishing);
    if (search.sortBy) params = params.set('sortBy', search.sortBy);
    if (search.page) params = params.set('page', search.page.toString());
    if (search.pageSize) params = params.set('pageSize', search.pageSize.toString());

    return this.http.get<PaginatedResponse<Property>>(`${this.apiUrl}/search`, {
      params,
      headers: this.getHeaders()
    });
  }

  getPropertyById(id: number): Observable<Property> {
    return this.http.get<Property>(`${this.apiUrl}/${id}`, {
      headers: this.getHeaders()
    });
  }

  getCities(): Observable<string[]> {
    return this.http.get<string[]>(`${this.apiUrl}/cities`);
  }

  getLocalities(city: string): Observable<string[]> {
    return this.http.get<string[]>(`${this.apiUrl}/localities/${city}`);
  }

  toggleFavorite(propertyId: number): Observable<{ isFavorited: boolean; message: string }> {
    return this.http.post<{ isFavorited: boolean; message: string }>(
      `${this.apiUrl}/favorites/${propertyId}`,
      {},
      { headers: this.getHeaders() }
    );
  }

  getFavorites(): Observable<Property[]> {
    return this.http.get<Property[]>(`${this.apiUrl}/favorites`, {
      headers: this.getHeaders()
    });
  }

  submitContact(inquiry: ContactInquiry): Observable<{ message: string }> {
    return this.http.post<{ message: string }>(`${this.apiUrl}/contact`, inquiry);
  }
}
