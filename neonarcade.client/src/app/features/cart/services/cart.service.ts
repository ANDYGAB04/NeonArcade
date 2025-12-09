import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable, BehaviorSubject, tap, map } from 'rxjs';
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
    return this.http.get<any>(this.API_URL).pipe(
      map(items => this.mapCartItemsToCart(items)),
      tap(cart => this.cartSubject.next(cart))
    );
  }

  /**
   * Add a game to the cart
   */
  addToCart(request: AddToCartRequest): Observable<any> {
    return this.http.post<any>(this.API_URL, request).pipe(
      tap(() => this.refreshCart())
    );
  }

  /**
   * Update the quantity of a cart item
   */
  updateCartItem(gameId: number, request: UpdateCartItemRequest): Observable<any> {
    return this.http.put<any>(`${this.API_URL}/${gameId}`, request).pipe(
      tap(() => this.refreshCart())
    );
  }

  /**
   * Remove an item from the cart
   */
  removeFromCart(gameId: number): Observable<void> {
    return this.http.delete<void>(`${this.API_URL}/${gameId}`).pipe(
      tap(() => this.refreshCart())
    );
  }

  /**
   * Clear all items from the cart
   */
  clearCart(): Observable<void> {
    return this.http.delete<void>(`${this.API_URL}/clear`).pipe(
      tap(() => this.cartSubject.next(null))
    );
  }

  /**
   * Map cart items array to Cart object
   * Transforms backend CartItemResponse[] to frontend Cart model
   */
  private mapCartItemsToCart(items: any[]): Cart {
    // Map each item from backend format to frontend format
    const cartItems = items.map(item => ({
      id: item.id,
      gameId: item.gameId,
      gameTitle: item.game?.title || 'Unknown Game',
      gamePrice: item.price,
      gameImageUrl: item.game?.coverImageUrl || '',
      quantity: item.quantity,
      subtotal: item.subTotal
    }));

    const totalItems = cartItems.reduce((sum, item) => sum + item.quantity, 0);
    const totalPrice = cartItems.reduce((sum, item) => sum + item.subtotal, 0);
    
    return {
      id: 0,
      userId: '',
      items: cartItems,
      totalItems: totalItems,
      totalPrice: totalPrice
    };
  }

  /**
   * Refresh cart data from the server
   */
  private refreshCart(): void {
    this.getCart().subscribe();
  }

  /**
   * Get the current cart item count as observable
   */
  getCartItemCount$(): Observable<number> {
    return this.cart$.pipe(
      map(cart => cart?.totalItems ?? 0)
    );
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
