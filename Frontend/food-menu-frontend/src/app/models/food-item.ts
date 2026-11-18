export interface FoodItem {
  id: number;
  name: string;
  description: string;
  price: number;
  imageUrl: string;
  isAvailable: boolean;
  createdAt: string;
  categoryId: number;
  categoryName: string;
}

export interface CreateFoodItem {
  name: string;
  description: string;
  price: number;
  imageUrl: string;
  categoryId: number;
}

export interface UpdateFoodItem {
  name: string;
  description: string;
  price: number;
  imageUrl: string;
  isAvailable: boolean;
  categoryId: number;
}