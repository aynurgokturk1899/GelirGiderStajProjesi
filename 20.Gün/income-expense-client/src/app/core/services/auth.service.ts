import { HttpClient } from '@angular/common/http';
import { computed, inject, Injectable, signal } from '@angular/core';
import { tap } from 'rxjs';
import { API_BASE_URL } from '../config/api.config';
import { AuthResponse, LoginRequest, RegisterRequest } from '../models/auth.models';

const SESSION_KEY = 'income-expense-session';

@Injectable({ providedIn: 'root' })
export class AuthService {
  private readonly http = inject(HttpClient);
  private readonly session = signal<AuthResponse | null>(this.readSession());
  readonly currentUser = computed(() => this.session()?.user ?? null);
  readonly isAuthenticated = computed(() => this.session() !== null);

  login(request: LoginRequest) {
    return this.http.post<AuthResponse>(`${API_BASE_URL}/auth/login`, request)
      .pipe(tap((response) => this.saveSession(response)));
  }

  register(request: RegisterRequest) {
    return this.http.post<AuthResponse>(`${API_BASE_URL}/auth/register`, request)
      .pipe(tap((response) => this.saveSession(response)));
  }

  getAccessToken(): string | null { return this.session()?.accessToken ?? null; }

  logout(): void {
    localStorage.removeItem(SESSION_KEY);
    this.session.set(null);
  }

  private saveSession(response: AuthResponse): void {
    localStorage.setItem(SESSION_KEY, JSON.stringify(response));
    this.session.set(response);
  }

  private readSession(): AuthResponse | null {
    const storedSession = localStorage.getItem(SESSION_KEY);
    if (!storedSession) return null;
    try {
      const session = JSON.parse(storedSession) as AuthResponse;
      if (!session.accessToken || new Date(session.expiresAtUtc).getTime() <= Date.now()) {
        localStorage.removeItem(SESSION_KEY);
        return null;
      }
      return session;
    } catch {
      localStorage.removeItem(SESSION_KEY);
      return null;
    }
  }
}
