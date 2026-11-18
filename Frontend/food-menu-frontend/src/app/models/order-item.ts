export interface OrderItem {
  id: number;
  orderId: number;
  foodItemId: number;
  foodItemName: string;
  quantity: number;
  unitPrice: number;
  subTotal: number;
}

export interface FoodItemOption {
  id: number;
  name: string;
  price: number;
  isAvailable: boolean;
}