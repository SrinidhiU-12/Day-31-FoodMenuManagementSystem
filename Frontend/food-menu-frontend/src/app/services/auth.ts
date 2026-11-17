import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable, tap } from 'rxjs';
import {
  RegisterRequest,
  LoginRequest,
  AuthResponse
} from '../models/auth';

@Injectable({
  providedIn: 'root'
})
export class AuthService {

  private apiUrl = 'https://localhost:7124/api/Auth';

  constructor(private http: HttpClient) {}

  register(data: RegisterRequest): Observable<AuthResponse> {
    return this.http.post<AuthResponse>(
      `${this.apiUrl}/register`, data
    ).pipe(
      tap(response =>
        localStorage.setItem('food_menu_token', response.token)
      )
    );
  }

  login(data: LoginRequest): Observable<AuthResponse> {
    return this.http.post<AuthResponse>(
      `${this.apiUrl}/login`, data
    ).pipe(
      tap(response =>
        localStorage.setItem('food_menu_token', response.token)
      )
    );
  }

  logout(): void {
    localStorage.removeItem('food_menu_token');
  }

  isLoggedIn(): boolean {
    return !!localStorage.getItem('food_menu_token');
  }
}