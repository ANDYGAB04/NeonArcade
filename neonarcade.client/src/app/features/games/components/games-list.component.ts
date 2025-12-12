import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { Game } from '../../../models/game.model';
import { GameService } from '../services/game.service';
import { CartService } from '../../cart/services/cart.service';
import { AuthService } from '../../../core/services/auth.service';

@Component({
  selector: 'app-games-list',
  templateUrl: './games-list.component.html',
  styleUrls: ['./games-list.component.css'],
  standalone: false
})
export class GamesListComponent implements OnInit {
  // Properties
  games: Game[] = [];
  loading = false;
  searchTerm = '';
  selectedCategory = '';
  sortBy = 'title';
  
  // Pagination
  currentPage = 1;
  pageSize = 12;
  totalPages = 0;
  
  // Categories (you can get these from backend or hardcode)
  categories = ['RPG', 'Action', 'Adventure', 'Strategy', 'Shooter'];

  constructor(
    private gameService: GameService,
    private cartService: CartService,
    private authService: AuthService,
    private router: Router
  ) {}

  ngOnInit(): void {
    this.loadGames();
  }

  // Load games with filters
  loadGames(): void {
    this.loading = true;
    
    this.gameService.getAllGames({
      searchTerm: this.searchTerm,
      category: this.selectedCategory,
      sortBy: this.sortBy,
      pageNumber: this.currentPage,
      pageSize: this.pageSize
    }).subscribe({
      next: (response) => {
        this.games = response.items;
        this.totalPages = response.totalPages;
        this.loading = false;
      },
      error: (error) => {
        console.error('Error loading games:', error);
        this.loading = false;
      }
    });
  }

  // Search functionality
  onSearch(): void {
    this.currentPage = 1;
    this.loadGames();
  }

  // Filter by category
  onCategoryChange(): void {
    this.currentPage = 1;
    this.loadGames();
  }

  // Sort functionality
  onSortChange(): void {
    this.currentPage = 1;
    this.loadGames();
  }

  // Pagination
  goToPage(page: number): void {
    this.currentPage = page;
    this.loadGames();
  }

  nextPage(): void {
    if (this.currentPage < this.totalPages) {
      this.currentPage++;
      this.loadGames();
    }
  }

  previousPage(): void {
    if (this.currentPage > 1) {
      this.currentPage--;
      this.loadGames();
    }
  }

  // Navigate to game details
  viewGameDetails(gameId: number): void {
    this.router.navigate(['/games', gameId]);
  }

  // Add to cart
  addToCart(game: Game, event: Event): void {
    event.stopPropagation(); // Prevent navigation to details
    
    if (!this.authService.isAuthenticated()) {
      this.router.navigate(['/login'], { queryParams: { returnUrl: '/games' } });
      return;
    }

    this.cartService.addToCart({
      gameId: game.id,
      quantity: 1
    }).subscribe({
      next: () => {
        alert(`${game.title} added to cart!`);
      },
      error: (error) => {
        console.error('Error adding to cart:', error);
        alert('Failed to add to cart. Please try again.');
      }
    });
  }

  // Calculate final price with discount
  getFinalPrice(game: Game): number {
    return game.discountPrice || game.price;
  }

  // Calculate discount percentage
  getDiscountPercentage(game: Game): number {
    if (!game.discountPrice) return 0;
    return Math.round(((game.price - game.discountPrice) / game.price) * 100);
  }

  // Check if game is on sale
  isOnSale(game: Game): boolean {
    return !!game.discountPrice && game.discountPrice < game.price;
  }
}
