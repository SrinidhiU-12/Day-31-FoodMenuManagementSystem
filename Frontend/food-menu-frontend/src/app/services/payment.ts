import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Payment } from '../models/payment';

@Injectable({
  providedIn: 'root'
})
export class PaymentService {
  private apiUrl = 'https://localhost:7124/api/Payments';

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

  create(orderId: number, paymentMethod: string): Observable<Payment> {
    return this.http.post<Payment>(
      this.apiUrl,
      { orderId, paymentMethod },
      { headers: this.getHeaders() }
    );
  }

  getByOrderId(orderId: number): Observable<Payment> {
    return this.http.get<Payment>(
      `${this.apiUrl}/order/${orderId}`,
      { headers: this.getHeaders() }
    );
  }

  getMyPayments(): Observable<Payment[]> {
    return this.http.get<Payment[]>(
      `${this.apiUrl}/my-payments`,
      { headers: this.getHeaders() }
    );
  }
}