export interface CartItem {
  id: number;
  gameId: number;
  gameTitle: string;
  gamePrice: number;
  gameImageUrl?: string;
  quantity: number;
  subtotal: number;
}

export interface Cart {
  id: number;
  userId: string;
  items: CartItem[];
  totalItems: number;
  totalPrice: number;
}

export interface AddToCartRequest {
  gameId: number;
  quantity: number;
}

export interface UpdateCartItemRequest {
  quantity: number;
}
