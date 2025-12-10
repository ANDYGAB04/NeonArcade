import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { Game } from '../../../../models/game.model';
import { GameService } from '../../services/game.service';
import { CartService } from '../../../cart/services/cart.service';
import { AuthService } from '../../../../core/services/auth.service';

@Component({
  selector: 'app-game-details',
  templateUrl: './game-details.component.html',
  styleUrls: ['./game-details.component.css'],
  standalone: false
})
export class GameDetailsComponent implements OnInit {
  game: Game | null = null;
  loading = false;
  quantity = 1;
  addingToCart = false;

  constructor(
    private route: ActivatedRoute,
    private router: Router,
    private gameService: GameService,
    private cartService: CartService,
    private authService: AuthService
  ) {}

  ngOnInit(): void {
    this.route.params.subscribe(params => {
      const id = +params['id'];
      if (id) {
        this.loadGameDetails(id);
      }
    });
  }

  loadGameDetails(id: number): void {
    this.loading = true;
    this.gameService.getGameById(id).subscribe({
      next: (game) => {
        this.game = game;
        this.loading = false;
      },
      error: (error) => {
        console.error('Error loading game details:', error);
        this.loading = false;
        this.router.navigate(['/games']);
      }
    });
  }

  getFinalPrice(): number {
    if (!this.game) return 0;
    if (this.game.discountPercentage && this.game.discountPercentage > 0) {
      return this.game.price - (this.game.price * this.game.discountPercentage / 100);
    }
    return this.game.price;
  }

  isOnSale(): boolean {
    return !!this.game?.discountPercentage && this.game.discountPercentage > 0;
  }

  increaseQuantity(): void {
    if (this.game && this.quantity < this.game.stockQuantity) {
      this.quantity++;
    }
  }

  decreaseQuantity(): void {
    if (this.quantity > 1) {
      this.quantity--;
    }
  }

  addToCart(): void {
    if (!this.game) return;

    if (!this.authService.isAuthenticated()) {
      this.router.navigate(['/login'], { 
        queryParams: { returnUrl: `/games/${this.game.id}` } 
      });
      return;
    }

    this.addingToCart = true;
    this.cartService.addToCart({
      gameId: this.game.id,
      quantity: this.quantity
    }).subscribe({
      next: () => {
        alert(`${this.game!.title} added to cart!`);
        this.addingToCart = false;
      },
      error: (error) => {
        console.error('Error adding to cart:', error);
        alert('Failed to add to cart. Please try again.');
        this.addingToCart = false;
      }
    });
  }

  goBack(): void {
    this.router.navigate(['/games']);
  }
}
