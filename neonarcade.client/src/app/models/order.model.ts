export interface Order {
  id: number;
  userId: string;
  orderDate: Date;
  totalAmount: number;
  status: OrderStatus;
  items: OrderItem[];
}

export interface OrderItem {
  id: number;
  orderId: number;
  gameId: number;
  gameTitle: string;
  gameImageUrl?: string;
  price: number;
  quantity: number;
  gameKey?: string;
  subtotal: number;
}

export enum OrderStatus {
  Pending = 'Pending',
  Processing = 'Processing',
  Completed = 'Completed',
  Cancelled = 'Cancelled',
  Refunded = 'Refunded'
}

export interface CheckoutRequest {
  // Add payment details when implementing payment integration
  paymentMethod?: string;
  shippingAddress?: string;
}
