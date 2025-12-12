import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { CartService } from '../../../cart/services/cart.service';
import { OrderService } from '../../services/order.service';

@Component({
  selector: 'app-checkout',
  templateUrl: './checkout.component.html',
  styleUrls: ['./checkout.component.css'],
  standalone: false
})
export class CheckoutComponent implements OnInit {
  cart: any = null;
  loading = false;
  processing = false;
  errorMessage: string | null = null;

  constructor(
    private cartService: CartService,
    private orderService: OrderService,
    private router: Router
  ) { }

  ngOnInit(): void {
    this.loadCart();
  }

  loadCart(): void {
    this.loading = true;
    this.cartService.getCart().subscribe({
      next: cart => { this.cart = cart; this.loading = false; },
      error: err => { console.error('Failed to load cart', err); this.loading = false; }
    });
  }

  placeOrder(): void {
    if (!this.cart || this.cart.totalItems === 0) return;
    this.processing = true;
    this.errorMessage = null;

    // backend expects POST /api/order/checkout without body
    this.orderService.checkout().subscribe({
      next: (order) => {
        // Navigate to order details
        this.processing = false;
        this.router.navigate(['/orders', order.id]);
      },
      error: (error) => {
        console.error('Checkout failed', error);
        this.processing = false;
        this.errorMessage = error?.error?.message || 'Checkout failed. Please try again.';
      }
    });
  }

  continueShopping(): void { this.router.navigate(['/games']); }
}
