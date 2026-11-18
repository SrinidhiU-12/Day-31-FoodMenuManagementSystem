
import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Order } from '../models/order';

@Injectable({
  providedIn: 'root'
})
export class OrderService {

  private apiUrl = 'https://localhost:7124/api/Orders';

  constructor(private http: HttpClient) {}

private getHeaders(): HttpHeaders {
  const token = localStorage.getItem('food_menu_token');

  if (!token) {
    console.error('JWT token not found in localStorage');
    throw new Error('Please log in first.');
  }

  return new HttpHeaders({
    Authorization: `Bearer ${token}`
  });
}

  getAll(): Observable<Order[]> {
    return this.http.get<Order[]>(this.apiUrl, {
      headers: this.getHeaders()
    });
  }

  getById(id: number): Observable<Order> {
    return this.http.get<Order>(`${this.apiUrl}/${id}`, {
      headers: this.getHeaders()
    });
  }

  create(): Observable<Order> {
    return this.http.post<Order>(this.apiUrl, {}, {
      headers: this.getHeaders()
    });
  }

  cancel(id: number): Observable<any> {
    return this.http.delete(`${this.apiUrl}/${id}`, {
      headers: this.getHeaders()
    });
  }
}
