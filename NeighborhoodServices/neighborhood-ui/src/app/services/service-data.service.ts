import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable, map } from 'rxjs';
import { Service } from '../models/service.model';

@Injectable({
  providedIn: 'root'
})
export class ServiceDataService {

  private readonly baseUrl = 'https://localhost:7121/api';

  constructor(private http: HttpClient) {}

  // ✅ Fetch all services
  getServices(): Observable<Service[]> {
    return this.http.get<any>(`${this.baseUrl}/services`).pipe(
      map(res => res?.$values ?? res)
    );
  }

  // ✅ Fetch a single service by ID
  getServiceById(id: number): Observable<Service> {
    return this.http.get<Service>(`${this.baseUrl}/services/${id}`);
  }
}
