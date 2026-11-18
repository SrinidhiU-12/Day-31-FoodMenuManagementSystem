
import { Component, OnInit, ChangeDetectorRef } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Order } from '../../models/order';
import { OrderService } from '../../services/order';

@Component({
  selector: 'app-order-list',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './order-list.html',
  styleUrl: './order-list.css'
})
export class OrderList implements OnInit {

  orders: Order[] = [];
  selectedOrder: Order | null = null;

  errorMessage = '';
  successMessage = '';
  isLoading = false;

  constructor(
    private orderService: OrderService,
    private cdr: ChangeDetectorRef
  ) {}

  ngOnInit(): void {
    this.loadOrders();
  }

  loadOrders(): void {
    this.isLoading = true;
    this.errorMessage = '';

    this.orderService.getAll().subscribe({
      next: (data) => {
        this.orders = data;
        this.isLoading = false;
        this.cdr.detectChanges();
      },
      error: (error) => {
        this.errorMessage = error.status === 401
          ? 'Please log in to view your orders.'
          : 'Unable to load orders.';

        this.isLoading = false;
        this.cdr.detectChanges();
      }
    });
  }

  createOrder(): void {
    this.errorMessage = '';
    this.successMessage = '';

    this.orderService.create().subscribe({
      next: () => {
        this.successMessage = 'Order created successfully!';
        this.loadOrders();
        this.cdr.detectChanges();
      },
      error: (error) => {
        this.errorMessage = error.error?.message
          || 'Unable to create order.';

        this.cdr.detectChanges();
      }
    });
  }

  viewOrder(id: number): void {
    this.errorMessage = '';

    this.orderService.getById(id).subscribe({
      next: (data) => {
        this.selectedOrder = data;
        this.cdr.detectChanges();
      },
      error: (error) => {
        this.errorMessage = error.error?.message
          || 'Unable to load order details.';

        this.cdr.detectChanges();
      }
    });
  }

  closeDetails(): void {
    this.selectedOrder = null;
  }

  cancelOrder(id: number): void {
    if (!confirm('Are you sure you want to cancel this order?')) {
      return;
    }

    this.orderService.cancel(id).subscribe({
      next: () => {
        this.selectedOrder = null;
        this.successMessage = 'Order cancelled successfully!';
        this.loadOrders();
        this.cdr.detectChanges();
      },
      error: (error) => {
        this.errorMessage = error.error?.message
          || 'Unable to cancel order.';

        this.cdr.detectChanges();
      }
    });
  }
}
