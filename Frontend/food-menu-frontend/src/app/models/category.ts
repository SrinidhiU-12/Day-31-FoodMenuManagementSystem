export interface Category {
  id: number;
  name: string;
  description: string;
  isActive: boolean;
  createdAt: string;
}

export interface CreateCategory {
  name: string;
  description: string;
}

export interface UpdateCategory {
  name: string;
  description: string;
  isActive: boolean;
}