export interface Payment {
  id: number;
  orderId: number;
  amount: number;
  paymentMethod: string;
  paymentStatus: string;
  transactionId?: string;
  paymentDate?: string;
  createdAt: string;
}