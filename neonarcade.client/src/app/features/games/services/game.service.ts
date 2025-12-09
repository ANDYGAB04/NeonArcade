import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Game, GameFilterParams, PaginatedResponse } from '../../../models/game.model';

/**
 * Service for managing game-related API calls
 */
@Injectable({
  providedIn: 'root'
})
export class GameService {
  private readonly API_URL = '/api/games';

  constructor(private http: HttpClient) {}

  /**
   * Get all games with optional filters and pagination
   */
  getAllGames(filters?: GameFilterParams): Observable<PaginatedResponse<Game>> {
    let params = new HttpParams();
    
    if (filters) {
      if (filters.searchTerm) params = params.set('searchTerm', filters.searchTerm);
      if (filters.category) params = params.set('genre', filters.category); // Map category to genre
      if (filters.minPrice !== undefined) params = params.set('minPrice', filters.minPrice.toString());
      if (filters.maxPrice !== undefined) params = params.set('maxPrice', filters.maxPrice.toString());
      if (filters.sortBy) params = params.set('sortBy', filters.sortBy);
      if (filters.pageNumber) params = params.set('pageNumber', filters.pageNumber.toString());
      if (filters.pageSize) params = params.set('pageSize', filters.pageSize.toString());
    }

    return this.http.get<PaginatedResponse<Game>>(this.API_URL, { params });
  }

  /**
   * Get a single game by ID
   */
  getGameById(id: number): Observable<Game> {
    return this.http.get<Game>(`${this.API_URL}/${id}`);
  }

  /**
   * Get featured games
   */
  getFeaturedGames(): Observable<Game[]> {
    return this.http.get<Game[]>(`${this.API_URL}/featured`);
  }

  /**
   * Get games on sale (with discounts)
   */
  getDeals(): Observable<Game[]> {
    return this.http.get<Game[]>(`${this.API_URL}/deals`);
  }

  /**
   * Search games by term
   */
  searchGames(searchTerm: string): Observable<Game[]> {
    return this.http.get<Game[]>(`${this.API_URL}/search`, {
      params: { searchTerm }
    });
  }

  // ==================== ADMIN METHODS ====================

  /**
   * Create a new game (Admin only)
   */
  createGame(game: Partial<Game>): Observable<Game> {
    return this.http.post<Game>(this.API_URL, game);
  }

  /**
   * Update an existing game (Admin only)
   */
  updateGame(id: number, game: Partial<Game>): Observable<void> {
    return this.http.put<void>(`${this.API_URL}/${id}`, game);
  }

  /**
   * Delete a game (Admin only)
   */
  deleteGame(id: number): Observable<void> {
    return this.http.delete<void>(`${this.API_URL}/${id}`);
  }
}
