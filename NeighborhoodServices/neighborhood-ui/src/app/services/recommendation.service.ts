import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class RecommendationService {
  private baseUrl = 'https://localhost:7121/api/recommendations';

  constructor(private http: HttpClient) {}

  getRecommendations(customerId: number): Observable<any[]> {
    return this.http.get<any[]>(`${this.baseUrl}/${customerId}`);
  }
}
