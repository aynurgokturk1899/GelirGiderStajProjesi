import { HttpClient, HttpParams } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { API_BASE_URL } from '../config/api.config';
import { Transaction, TransactionFilter, TransactionRequest } from '../models/transaction.models';

@Injectable({ providedIn: 'root' })
export class TransactionService {
  private readonly http = inject(HttpClient);

  getAll(filter: TransactionFilter = {}) {
    let params = new HttpParams();
    if (filter.type) params = params.set('type', filter.type);
    if (filter.categoryId) params = params.set('categoryId', filter.categoryId);
    if (filter.startDate) params = params.set('startDate', filter.startDate);
    if (filter.endDate) params = params.set('endDate', filter.endDate);
    return this.http.get<Transaction[]>(`${API_BASE_URL}/transactions`, { params });
  }

  create(request: TransactionRequest) {
    return this.http.post<Transaction>(`${API_BASE_URL}/transactions`, request);
  }

  update(id: number, request: TransactionRequest) {
    return this.http.put<Transaction>(`${API_BASE_URL}/transactions/${id}`, request);
  }

  delete(id: number) {
    return this.http.delete<void>(`${API_BASE_URL}/transactions/${id}`);
  }
}
