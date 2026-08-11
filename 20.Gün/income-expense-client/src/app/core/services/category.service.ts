import { HttpClient } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { API_BASE_URL } from '../config/api.config';
import { Category, CategoryRequest } from '../models/category.models';

@Injectable({ providedIn: 'root' })
export class CategoryService {
  private readonly http = inject(HttpClient);

  getAll(isActive?: boolean) {
    const options = isActive === undefined ? {} : { params: { isActive } };
    return this.http.get<Category[]>(`${API_BASE_URL}/categories`, options);
  }

  create(request: CategoryRequest) {
    return this.http.post<Category>(`${API_BASE_URL}/categories`, request);
  }

  update(id: number, request: CategoryRequest) {
    return this.http.put<Category>(`${API_BASE_URL}/categories/${id}`, request);
  }

  delete(id: number) {
    return this.http.delete<void>(`${API_BASE_URL}/categories/${id}`);
  }
}
