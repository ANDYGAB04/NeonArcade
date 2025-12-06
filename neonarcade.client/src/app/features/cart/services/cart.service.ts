import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable, BehaviorSubject, tap } from 'rxjs';
import { Cart, AddToCartRequest, UpdateCartItemRequest } from '../../../models/cart.model';

/**
 * Service for managing shopping cart operations
 */
@Injectable({
  providedIn: 'root'
})
export class CartService {
  private readonly API_URL = '/api/cart';
  private cartSubject = new BehaviorSubject<Cart | null>(null);
  
  /**
   * Observable cart state for components to subscribe to
   */
  public cart$ = this.cartSubject.asObservable();

  constructor(private http: HttpClient) {}

  /**
   * Get the current user's cart
   */
  getCart(): Observable<Cart> {
    return this.http.get<Cart>(this.API_URL).pipe(
      tap(cart => this.cartSubject.next(cart))
    );
  }

  /**
   * Add a game to the cart
   */
  addToCart(request: AddToCartRequest): Observable<Cart> {
    return this.http.post<Cart>(`${this.API_URL}/items`, request).pipe(
      tap(cart => this.cartSubject.next(cart))
    );
  }

  /**
   * Update the quantity of a cart item
   */
  updateCartItem(itemId: number, request: UpdateCartItemRequest): Observable<Cart> {
    return this.http.put<Cart>(`${this.API_URL}/items/${itemId}`, request).pipe(
      tap(cart => this.cartSubject.next(cart))
    );
  }

  /**
   * Remove an item from the cart
   */
  removeFromCart(itemId: number): Observable<void> {
    return this.http.delete<void>(`${this.API_URL}/items/${itemId}`).pipe(
      tap(() => this.refreshCart())
    );
  }

  /**
   * Clear all items from the cart
   */
  clearCart(): Observable<void> {
    return this.http.delete<void>(this.API_URL).pipe(
      tap(() => this.cartSubject.next(null))
    );
  }

  /**
   * Refresh cart data from the server
   */
  private refreshCart(): void {
    this.getCart().subscribe();
  }

  /**
   * Get the current cart item count
   */
  getCartItemCount(): number {
    return this.cartSubject.value?.totalItems ?? 0;
  }

  /**
   * Get the current cart total price
   */
  getCartTotalPrice(): number {
    return this.cartSubject.value?.totalPrice ?? 0;
  }

  /**
   * Get the current cart value (synchronous)
   */
  getCurrentCart(): Cart | null {
    return this.cartSubject.value;
  }
}
