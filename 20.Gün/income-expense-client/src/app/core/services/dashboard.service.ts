import { HttpClient } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { API_BASE_URL } from '../config/api.config';
import { DashboardSummary } from '../models/transaction.models';

@Injectable({ providedIn: 'root' })
export class DashboardService {
  private readonly http = inject(HttpClient);
  getSummary() { return this.http.get<DashboardSummary>(`${API_BASE_URL}/dashboard/summary`); }
}
