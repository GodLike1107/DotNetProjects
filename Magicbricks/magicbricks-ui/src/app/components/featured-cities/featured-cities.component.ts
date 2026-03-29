import { Component } from '@angular/core';
import { RouterLink } from '@angular/router';

@Component({
  selector: 'app-featured-cities',
  standalone: true,
  imports: [RouterLink],
  templateUrl: './featured-cities.component.html',
  styleUrl: './featured-cities.component.css'
})
export class FeaturedCitiesComponent {}
