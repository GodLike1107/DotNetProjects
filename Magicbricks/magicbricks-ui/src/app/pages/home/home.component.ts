import { Component } from '@angular/core';
import { HeroSearchComponent } from '../../components/hero-search/hero-search.component';
import { FeaturedCitiesComponent } from '../../components/featured-cities/featured-cities.component';

@Component({
  selector: 'app-home',
  standalone: true,
  imports: [HeroSearchComponent, FeaturedCitiesComponent],
  template: `
    <app-hero-search />
    <app-featured-cities />
  `
})
export class HomeComponent {}
