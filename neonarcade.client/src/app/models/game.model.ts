export interface Game {
  id: number;
  title: string;
  description: string;
  price: number;
  imageUrl?: string;
  category: string;
  releaseDate: Date;
  developer: string;
  publisher: string;
  tags?: string[];
  rating?: number;
  stockQuantity: number;
  isFeatured: boolean;
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
