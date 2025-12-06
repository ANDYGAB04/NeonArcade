import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { Observable } from 'rxjs';
import { AuthService } from '../../../core/services/auth.service';
import { CartService } from '../../../features/cart/services/cart.service';
import { User } from '../../../models/user.model';

@Component({
  selector: 'app-header',
  templateUrl: './header.component.html',
  standalone: false,
  styleUrls: ['./header.component.css']
})
export class HeaderComponent implements OnInit {
  currentUser$: Observable<User | null>;
  cartItemCount$: Observable<number> = new Observable();
  isMenuCollapsed = true;

  constructor(
    public authService: AuthService,
    private cartService: CartService,
    private router: Router
  ) {
    this.currentUser$ = this.authService.currentUser$;
  }

  ngOnInit(): void {
    // Subscribe to cart changes
    this.cartService.cart$.subscribe(cart => {
      // Update cart count when cart changes
    });
  }

  getCartItemCount(): number {
    return this.cartService.getCartItemCount();
  }

  logout(): void {
    this.authService.logout();
  }

  navigateToCart(): void {
    this.router.navigate(['/cart']);
  }

  navigateToProfile(): void {
    this.router.navigate(['/profile']);
  }

  navigateToOrders(): void {
    this.router.navigate(['/orders/history']);
  }

  navigateToAdmin(): void {
    this.router.navigate(['/admin']);
  }
}
