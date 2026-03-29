import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { FormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { BudgetRange } from '../../models/property.model';

@Component({
  selector: 'app-hero-search',
  standalone: true,
  imports: [FormsModule, CommonModule],
  templateUrl: './hero-search.component.html',
  styleUrl: './hero-search.component.css'
})
export class HeroSearchComponent {
  listingType: string = 'Buy';
  city: string = '';
  minBudget: string = '';
  maxBudget: string = '';
  searchText: string = '';

  buyBudgets: BudgetRange[] = [
    { label: 'No Min', min: undefined },
    { label: '₹20 Lac', min: 2000000 },
    { label: '₹50 Lac', min: 5000000 },
    { label: '₹80 Lac', min: 8000000 },
    { label: '₹1 Cr', min: 10000000 },
    { label: '₹2 Cr', min: 20000000 },
    { label: '₹5 Cr', min: 50000000 },
  ];

  buyMaxBudgets: BudgetRange[] = [
    { label: 'No Max', max: undefined },
    { label: '₹50 Lac', max: 5000000 },
    { label: '₹80 Lac', max: 8000000 },
    { label: '₹1 Cr', max: 10000000 },
    { label: '₹1.5 Cr', max: 15000000 },
    { label: '₹2 Cr', max: 20000000 },
    { label: '₹5 Cr', max: 50000000 },
    { label: '₹10 Cr', max: 100000000 },
  ];

  rentBudgets: BudgetRange[] = [
    { label: 'No Min', min: undefined },
    { label: '₹5,000', min: 5000 },
    { label: '₹10,000', min: 10000 },
    { label: '₹15,000', min: 15000 },
    { label: '₹20,000', min: 20000 },
    { label: '₹30,000', min: 30000 },
    { label: '₹50,000', min: 50000 },
  ];

  rentMaxBudgets: BudgetRange[] = [
    { label: 'No Max', max: undefined },
    { label: '₹10,000', max: 10000 },
    { label: '₹15,000', max: 15000 },
    { label: '₹20,000', max: 20000 },
    { label: '₹25,000', max: 25000 },
    { label: '₹30,000', max: 30000 },
    { label: '₹50,000', max: 50000 },
    { label: '₹1,00,000', max: 100000 },
  ];

  constructor(private router: Router) {}

  get currentMinBudgets(): BudgetRange[] {
    return this.listingType === 'Rent' ? this.rentBudgets : this.buyBudgets;
  }

  get currentMaxBudgets(): BudgetRange[] {
    return this.listingType === 'Rent' ? this.rentMaxBudgets : this.buyMaxBudgets;
  }

  setListingType(type: string) {
    this.listingType = type;
    this.minBudget = '';
    this.maxBudget = '';
  }

  onSearch() {
    const queryParams: any = {};
    if (this.listingType) queryParams.listingType = this.listingType;
    if (this.city) queryParams.city = this.city;
    if (this.minBudget) queryParams.minBudget = this.minBudget;
    if (this.maxBudget) queryParams.maxBudget = this.maxBudget;
    if (this.searchText) queryParams.searchText = this.searchText;

    this.router.navigate(['/search'], { queryParams });
  }
}
