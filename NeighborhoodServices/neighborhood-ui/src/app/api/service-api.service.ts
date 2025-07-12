// src/app/api/service-api.service.ts
import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Service } from '../models/service.model';

@Injectable({ providedIn: 'root' })
export class ServiceApiService {
  private apiUrl = 'https://localhost:7121/api/services'; // Adjust if needed

  constructor(private http: HttpClient) {}

  getServices(): Observable<Service[]> {
    return this.http.get<Service[]>(this.apiUrl);
  }

  addService(service: Service): Observable<Service> {
    return this.http.post<Service>(this.apiUrl, service);
  }
}
