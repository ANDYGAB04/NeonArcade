import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Order, CheckoutRequest, OrderStatus } from '../../../models/order.model';

/**
 * Service for managing order operations
 */
@Injectable({
  providedIn: 'root'
})
export class OrderService {
  private readonly API_URL = '/api/orders';

  constructor(private http: HttpClient) {}

  /**
   * Checkout and create an order from the current cart
   */
  // Server expects POST /api/orders/checkout with no request body
  checkout(): Observable<Order> {
    return this.http.post<Order>(`${this.API_URL}/checkout`, null as any);
  }

  /**
   * Get the current user's order history
   */
  getUserOrders(): Observable<Order[]> {
    return this.http.get<Order[]>(`${this.API_URL}`);
  }

  /**
   * Get the current user's order history (alias)
   */
  getOrderHistory(): Observable<Order[]> {
    return this.getUserOrders();
  }

  /**
   * Get a specific order by ID
   */
  getOrderById(orderId: number): Observable<Order> {
    return this.http.get<Order>(`${this.API_URL}/${orderId}`);
  }

  // ==================== ADMIN METHODS ====================

  /**
   * Get all orders (Admin only)
   */
  getAllOrders(): Observable<Order[]> {
    return this.http.get<Order[]>(this.API_URL);
  }

  /**
   * Update order status (Admin only)
   */
  updateOrderStatus(orderId: number, status: OrderStatus): Observable<void> {
    return this.http.patch<void>(`${this.API_URL}/${orderId}/status`, { status });
  }

  /**
   * Cancel an order (Admin only)
   */
  cancelOrder(orderId: number): Observable<void> {
    return this.updateOrderStatus(orderId, OrderStatus.Cancelled);
  }

  /**
   * Complete an order (Admin only)
   */
  completeOrder(orderId: number): Observable<void> {
    return this.updateOrderStatus(orderId, OrderStatus.Completed);
  }
}
