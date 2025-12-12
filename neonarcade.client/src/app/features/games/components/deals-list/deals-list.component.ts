import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { Game } from '../../../../models/game.model';
import { GameService } from '../../services/game.service';
import { CartService } from '../../../cart/services/cart.service';
import { AuthService } from '../../../../core/services/auth.service';

@Component({
  selector: 'app-deals-list',
  templateUrl: './deals-list.component.html',
  styleUrls: ['./deals-list.component.css'],
  standalone: false
})
export class DealsListComponent implements OnInit {
  // Properties
  games: Game[] = [];
  loading = false;

  constructor(
    private gameService: GameService,
    private cartService: CartService,
    private authService: AuthService,
    private router: Router
  ) {}

  ngOnInit(): void {
    this.loadDeals();
  }

  // Load games on sale
  loadDeals(): void {
    this.loading = true;
    
    this.gameService.getDeals().subscribe({
      next: (games) => {
        this.games = games;
        this.loading = false;
      },
      error: (error) => {
        console.error('Error loading deals:', error);
        this.loading = false;
      }
    });
  }

  // Navigate to game details
  viewGameDetails(gameId: number): void {
    this.router.navigate(['/games', gameId]);
  }

  // Add to cart
  addToCart(game: Game, event: Event): void {
    event.stopPropagation(); // Prevent navigation to details
    
    if (!this.authService.isAuthenticated()) {
      this.router.navigate(['/login'], { queryParams: { returnUrl: '/games/deals' } });
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

  // Calculate savings
  getSavings(game: Game): number {
    if (!game.discountPrice) return 0;
    return game.price - game.discountPrice;
  }

  // Check if game is on sale
  isOnSale(game: Game): boolean {
    return !!game.discountPrice && game.discountPrice < game.price;
  }
}
