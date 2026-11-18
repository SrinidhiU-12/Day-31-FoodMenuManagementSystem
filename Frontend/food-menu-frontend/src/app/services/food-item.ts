import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

import {
  FoodItem,
  CreateFoodItem,
  UpdateFoodItem
} from '../models/food-item';

@Injectable({
  providedIn: 'root'
})
export class FoodItemService {

  private apiUrl = 'https://localhost:7124/api/FoodItems';

  constructor(private http: HttpClient) {}

  getAll(): Observable<FoodItem[]> {
    return this.http.get<FoodItem[]>(this.apiUrl);
  }

  getById(id: number): Observable<FoodItem> {
    return this.http.get<FoodItem>(`${this.apiUrl}/${id}`);
  }

  create(data: CreateFoodItem): Observable<FoodItem> {
    return this.http.post<FoodItem>(this.apiUrl, data);
  }

  update(id: number, data: UpdateFoodItem): Observable<FoodItem> {
    return this.http.put<FoodItem>(`${this.apiUrl}/${id}`, data);
  }

  delete(id: number): Observable<any> {
    return this.http.delete(`${this.apiUrl}/${id}`);
  }
}