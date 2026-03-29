export interface Property {
  id: number;
  title: string;
  description: string;
  propertyType: string;
  listingType: string;
  price: number;
  formattedPrice: string;
  area: number;
  bedrooms: number;
  bathrooms: number;
  city: string;
  locality: string;
  address: string;
  furnishing: string;
  imageUrl: string;
  isAvailable: boolean;
  postedDate: string;
  isFavorited: boolean;
}

export interface PaginatedResponse<T> {
  items: T[];
  totalCount: number;
  page: number;
  pageSize: number;
  totalPages: number;
}

export interface PropertySearch {
  city?: string;
  listingType?: string;
  minBudget?: number;
  maxBudget?: number;
  bedrooms?: number;
  propertyType?: string;
  searchText?: string;
  furnishing?: string;
  sortBy?: string;
  page?: number;
  pageSize?: number;
}

export interface ContactInquiry {
  propertyId: number;
  name: string;
  email: string;
  phone?: string;
  message?: string;
}

export interface BudgetRange {
  label: string;
  min?: number;
  max?: number;
}
