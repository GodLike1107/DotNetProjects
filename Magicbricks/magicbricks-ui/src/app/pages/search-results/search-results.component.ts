import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { PropertyCardComponent } from '../../components/property-card/property-card.component';
import { PropertyService } from '../../services/property.service';
import { Property, PropertySearch, PaginatedResponse } from '../../models/property.model';

@Component({
  selector: 'app-search-results',
  standalone: true,
  imports: [CommonModule, FormsModule, PropertyCardComponent],
  templateUrl: './search-results.component.html',
  styleUrl: './search-results.component.css'
})
export class SearchResultsComponent implements OnInit {
  properties: Property[] = [];
  totalCount = 0;
  totalPages = 0;
  currentPage = 1;
  isLoading = true;
  toastMessage = '';
  showToast = false;

  // Filters
  filters: PropertySearch = {
    listingType: '',
    city: '',
    minBudget: undefined,
    maxBudget: undefined,
    bedrooms: undefined,
    propertyType: '',
    furnishing: '',
    sortBy: 'date_desc',
    searchText: '',
    page: 1,
    pageSize: 12
  };

  constructor(
    private propertyService: PropertyService,
    private route: ActivatedRoute,
    private router: Router
  ) {}

  ngOnInit() {
    this.route.queryParams.subscribe(params => {
      this.filters.listingType = params['listingType'] || '';
      this.filters.city = params['city'] || '';
      this.filters.minBudget = params['minBudget'] ? +params['minBudget'] : undefined;
      this.filters.maxBudget = params['maxBudget'] ? +params['maxBudget'] : undefined;
      this.filters.bedrooms = params['bedrooms'] ? +params['bedrooms'] : undefined;
      this.filters.propertyType = params['propertyType'] || '';
      this.filters.searchText = params['searchText'] || '';
      this.filters.sortBy = params['sortBy'] || 'date_desc';
      this.filters.page = params['page'] ? +params['page'] : 1;
      this.currentPage = this.filters.page!;
      this.searchProperties();
    });
  }

  searchProperties() {
    this.isLoading = true;
    const search: PropertySearch = { ...this.filters };

    // Clean undefined/empty values
    Object.keys(search).forEach(key => {
      const k = key as keyof PropertySearch;
      if (search[k] === '' || search[k] === undefined || search[k] === null) {
        delete search[k];
      }
    });

    this.propertyService.searchProperties(search).subscribe({
      next: (response: PaginatedResponse<Property>) => {
        this.properties = response.items;
        this.totalCount = response.totalCount;
        this.totalPages = response.totalPages;
        this.isLoading = false;
      },
      error: () => {
        this.isLoading = false;
        this.properties = [];
      }
    });
  }

  applyFilters() {
    this.filters.page = 1;
    this.currentPage = 1;
    this.updateUrl();
  }

  onSortChange() {
    this.applyFilters();
  }

  onPageChange(page: number) {
    if (page < 1 || page > this.totalPages) return;
    this.filters.page = page;
    this.currentPage = page;
    this.updateUrl();
    window.scrollTo({ top: 0, behavior: 'smooth' });
  }

  toggleFavorite(propertyId: number) {
    this.propertyService.toggleFavorite(propertyId).subscribe({
      next: (res) => {
        const property = this.properties.find(p => p.id === propertyId);
        if (property) {
          property.isFavorited = res.isFavorited;
        }
        this.showToastMessage(res.message);
      }
    });
  }

  clearFilters() {
    this.filters = {
      listingType: '',
      city: '',
      minBudget: undefined,
      maxBudget: undefined,
      bedrooms: undefined,
      propertyType: '',
      furnishing: '',
      sortBy: 'date_desc',
      searchText: '',
      page: 1,
      pageSize: 12
    };
    this.updateUrl();
  }

  private updateUrl() {
    const queryParams: any = {};
    if (this.filters.listingType) queryParams.listingType = this.filters.listingType;
    if (this.filters.city) queryParams.city = this.filters.city;
    if (this.filters.minBudget) queryParams.minBudget = this.filters.minBudget;
    if (this.filters.maxBudget) queryParams.maxBudget = this.filters.maxBudget;
    if (this.filters.bedrooms) queryParams.bedrooms = this.filters.bedrooms;
    if (this.filters.propertyType) queryParams.propertyType = this.filters.propertyType;
    if (this.filters.searchText) queryParams.searchText = this.filters.searchText;
    if (this.filters.sortBy && this.filters.sortBy !== 'date_desc') queryParams.sortBy = this.filters.sortBy;
    if (this.filters.page && this.filters.page > 1) queryParams.page = this.filters.page;

    this.router.navigate(['/search'], { queryParams });
  }

  private showToastMessage(message: string) {
    this.toastMessage = message;
    this.showToast = true;
    setTimeout(() => { this.showToast = false; }, 3000);
  }

  get pagesArray(): number[] {
    return Array.from({ length: this.totalPages }, (_, i) => i + 1);
  }
}
