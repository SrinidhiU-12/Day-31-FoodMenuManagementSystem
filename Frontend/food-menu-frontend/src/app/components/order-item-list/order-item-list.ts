
import { Component, OnInit, ChangeDetectorRef } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { ActivatedRoute, RouterLink } from '@angular/router';
import { HttpClient, HttpHeaders } from '@angular/common/http';

import {
  OrderItem,
  FoodItemOption
} from '../../models/order-item';

import { OrderItemService } from '../../services/order-item';

@Component({
  selector: 'app-order-item-list',
  standalone: true,
  imports: [CommonModule, FormsModule, RouterLink],
  templateUrl: './order-item-list.html',
  styleUrl: './order-item-list.css'
})
export class OrderItemList implements OnInit {

  orderId = 0;
  items: OrderItem[] = [];
  foodItems: FoodItemOption[] = [];

  selectedFoodItemId = 0;
  quantity = 1;

  errorMessage = '';
  successMessage = '';
  isLoading = false;

  private foodApiUrl = 'https://localhost:7124/api/FoodItems';

  constructor(
    private route: ActivatedRoute,
    private http: HttpClient,
    private orderItemService: OrderItemService,
    private cdr: ChangeDetectorRef
  ) {}

  ngOnInit(): void {
    this.orderId = Number(
      this.route.snapshot.paramMap.get('orderId')
    );

    if (!this.orderId || this.orderId <= 0) {
      this.errorMessage = 'Invalid order ID.';
      return;
    }

    this.loadFoodItems();
    this.loadItems();
  }

  private getHeaders(): HttpHeaders {
    const token = localStorage.getItem('food_menu_token');

    return new HttpHeaders({
      Authorization: `Bearer ${token}`
    });
  }

  loadFoodItems(): void {
    this.http.get<FoodItemOption[]>(
      this.foodApiUrl,
      { headers: this.getHeaders() }
    ).subscribe({
      next: data => {
        this.foodItems = data.filter(item => item.isAvailable);
        this.cdr.detectChanges();
      },
      error: () => {
        this.errorMessage = 'Unable to load food items.';
        this.cdr.detectChanges();
      }
    });
  }

  loadItems(): void {
    this.isLoading = true;

    this.orderItemService.getByOrderId(this.orderId).subscribe({
      next: data => {
        this.items = data;
        this.isLoading = false;
        this.cdr.detectChanges();
      },
      error: error => {
        this.errorMessage =
          error.error?.message || 'Unable to load order items.';
        this.isLoading = false;
        this.cdr.detectChanges();
      }
    });
  }

  addItem(): void {
    this.errorMessage = '';
    this.successMessage = '';

    if (this.selectedFoodItemId <= 0 || this.quantity <= 0) {
      this.errorMessage = 'Select a food item and valid quantity.';
      return;
    }

    this.orderItemService.create({
      orderId: this.orderId,
      foodItemId: this.selectedFoodItemId,
      quantity: this.quantity
    }).subscribe({
      next: () => {
        this.successMessage = 'Food item added successfully!';
        this.quantity = 1;
        this.loadItems();
      },
      error: error => {
        this.errorMessage =
          error.error?.message || 'Unable to add food item.';
        this.cdr.detectChanges();
      }
    });
  }

  updateQuantity(item: OrderItem): void {
    const newQuantity = Number(
      prompt('Enter the new quantity:', String(item.quantity))
    );

    if (!Number.isInteger(newQuantity) || newQuantity <= 0) {
      return;
    }

    this.orderItemService.update(item.id, newQuantity).subscribe({
      next: () => {
        this.successMessage = 'Quantity updated successfully!';
        this.loadItems();
      },
      error: error => {
        this.errorMessage =
          error.error?.message || 'Unable to update quantity.';
        this.cdr.detectChanges();
      }
    });
  }

  removeItem(id: number): void {
    if (!confirm('Are you sure you want to remove this item?')) {
      return;
    }

    this.orderItemService.delete(id).subscribe({
      next: () => {
        this.successMessage = 'Item removed successfully!';
        this.loadItems();
      },
      error: error => {
        this.errorMessage =
          error.error?.message || 'Unable to remove item.';
        this.cdr.detectChanges();
      }
    });
  }

  get totalAmount(): number {
    return this.items.reduce(
      (total, item) => total + item.subTotal, 0
    );
  }
}
