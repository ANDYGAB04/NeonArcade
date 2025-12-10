export interface Game {
  id: number;
  title: string;
  description: string;
  shortDescription?: string;
  price: number;
  discountPrice?: number;
  coverImageUrl?: string;
  trailerUrl?: string;
  screenshotUrls?: string[];
  developer: string;
  publisher: string;
  releaseDate: Date;
  ageRating?: string;
  genres?: string[];
  platforms?: string[];
  tags?: string[];
  minimumRequirements?: string;
  recommendedRequirements?: string;
  stockQuantity: number;
  isAvailable: boolean;
  isFeatured: boolean;
  createdAt?: Date;
  updatedAt?: Date;
  
  // Computed properties
  imageUrl?: string;  // Alias for coverImageUrl
  category?: string;  // First genre
  discountPercentage?: number;
  finalPrice?: number;
}

export interface GameFilterParams {
  searchTerm?: string;
  category?: string;
  minPrice?: number;
  maxPrice?: number;
  sortBy?: string;
  pageNumber?: number;
  pageSize?: number;
}

export interface PaginatedResponse<T> {
  items: T[];
  totalCount: number;
  pageNumber: number;
  pageSize: number;
  totalPages: number;
}
