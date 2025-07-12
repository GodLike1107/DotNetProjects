import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { CommonModule } from '@angular/common';
import { ServiceDataService } from './services/service-data.service';
import { Service } from './models/service.model';

@Component({
  selector: 'app-service-details',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './service-details.component.html',
  styleUrls: ['./service-details.component.css']
})
export class ServiceDetailsComponent implements OnInit {
  serviceId!: number;
  service: Service | null = null;
  isLoading = true;
  error: string | null = null;

  constructor(
    private route: ActivatedRoute,
    private router: Router,
    private serviceData: ServiceDataService
  ) {}

  ngOnInit(): void {
    this.route.paramMap.subscribe(params => {
      const id = params.get('id');
      if (id) {
        this.serviceId = +id;
        this.fetchServiceDetails(this.serviceId);
      } else {
        this.error = 'Invalid service ID.';
        this.isLoading = false;
      }
    });
  }

  fetchServiceDetails(id: number): void {
    this.serviceData.getServiceById(id).subscribe({
      next: (data: any) => {
        this.service = data;
        this.isLoading = false;
      },
      error: () => {
        this.error = 'Service not found.';
        this.isLoading = false;
      }
    });
  }

  backToServices(): void {
    this.router.navigate(['/services']);
  }
}