
import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable } from 'rxjs';
import { OrderItem } from '../models/order-item';

@Injectable({
  providedIn: 'root'
})
export class OrderItemService {

  private apiUrl = 'https://localhost:7124/api/OrderItems';

  constructor(private http: HttpClient) {}

  private getHeaders(): HttpHeaders {
    const token = localStorage.getItem('food_menu_token');

    if (!token) {
      throw new Error('Please log in first.');
    }

    return new HttpHeaders({
      Authorization: `Bearer ${token}`
    });
  }

  getByOrderId(orderId: number): Observable<OrderItem[]> {
    return this.http.get<OrderItem[]>(
      `${this.apiUrl}/order/${orderId}`,
      { headers: this.getHeaders() }
    );
  }

  create(data: {
    orderId: number;
    foodItemId: number;
    quantity: number;
  }): Observable<OrderItem> {
    return this.http.post<OrderItem>(
      this.apiUrl,
      data,
      { headers: this.getHeaders() }
    );
  }

  update(id: number, quantity: number): Observable<OrderItem> {
    return this.http.put<OrderItem>(
      `${this.apiUrl}/${id}`,
      { quantity },
      { headers: this.getHeaders() }
    );
  }

  delete(id: number): Observable<any> {
    return this.http.delete(
      `${this.apiUrl}/${id}`,
      { headers: this.getHeaders() }
    );
  }
}
