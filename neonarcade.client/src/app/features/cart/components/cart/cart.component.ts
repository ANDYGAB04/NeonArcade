import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { Cart } from '../../../../models/cart.model';
import { CartService } from '../../services/cart.service';

@Component({
  selector: 'app-cart',
  templateUrl: './cart.component.html',
  styleUrls: ['./cart.component.css'],
  standalone: false
})
export class CartComponent implements OnInit {
  cart: Cart | null = null;
  loading = false;
  updatingItemId: number | null = null;

  constructor(
    private cartService: CartService,
    private router: Router
  ) {}

  ngOnInit(): void {
    this.loadCart();
  }

  loadCart(): void {
    this.loading = true;
    this.cartService.getCart().subscribe({
      next: (cart) => {
        this.cart = cart;
        this.loading = false;
      },
      error: (error) => {
        console.error('Error loading cart:', error);
        this.loading = false;
      }
    });
  }

  updateQuantity(gameId: number, newQuantity: number): void {
    if (newQuantity < 1) return;

    this.updatingItemId = gameId;
    this.cartService.updateCartItem(gameId, { quantity: newQuantity }).subscribe({
      next: () => {
        this.loadCart();
        this.updatingItemId = null;
      },
      error: (error) => {
        console.error('Error updating quantity:', error);
        alert('Failed to update quantity');
        this.updatingItemId = null;
      }
    });
  }

  removeItem(gameId: number): void {
    if (!confirm('Remove this item from cart?')) return;

    this.cartService.removeFromCart(gameId).subscribe({
      next: () => {
        this.loadCart();
      },
      error: (error) => {
        console.error('Error removing item:', error);
        alert('Failed to remove item');
      }
    });
  }

  clearCart(): void {
    if (!confirm('Clear all items from cart?')) return;

    this.cartService.clearCart().subscribe({
      next: () => {
        this.cart = null;
      },
      error: (error) => {
        console.error('Error clearing cart:', error);
        alert('Failed to clear cart');
      }
    });
  }

  checkout(): void {
    this.router.navigate(['/orders/checkout']);
  }

  continueShopping(): void {
    this.router.navigate(['/games']);
  }
}
