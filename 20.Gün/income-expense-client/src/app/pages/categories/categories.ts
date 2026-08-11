import { HttpErrorResponse } from '@angular/common/http';
import { Component, computed, inject, OnInit, signal } from '@angular/core';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { finalize } from 'rxjs';
import { ApiProblemDetails } from '../../core/models/auth.models';
import { Category, TransactionType } from '../../core/models/category.models';
import { CategoryService } from '../../core/services/category.service';

@Component({
  selector: 'app-categories',
  imports: [ReactiveFormsModule],
  templateUrl: './categories.html',
  styleUrl: './categories.scss',
})
export class Categories implements OnInit {
  private readonly categoryService = inject(CategoryService);
  private readonly formBuilder = inject(FormBuilder);
  protected readonly categories = signal<Category[]>([]);
  protected readonly isLoading = signal(true);
  protected readonly errorMessage = signal('');
  protected readonly formError = signal('');
  protected readonly successMessage = signal('');
  protected readonly isFormOpen = signal(false);
  protected readonly isSaving = signal(false);
  protected readonly editingCategory = signal<Category | null>(null);
  protected readonly selectedType = signal<TransactionType | 'all'>('all');
  protected readonly visibleCategories = computed(() => this.selectedType() === 'all' ? this.categories() : this.categories().filter((item) => item.type === this.selectedType()));
  protected readonly incomeCount = computed(() => this.categories().filter((item) => this.isIncome(item.type)).length);
  protected readonly expenseCount = computed(() => this.categories().filter((item) => !this.isIncome(item.type)).length);
  protected readonly categoryForm = this.formBuilder.nonNullable.group({
    name: ['', [Validators.required, Validators.maxLength(100), Validators.pattern(/\S/)]],
    type: [TransactionType.Expense, [Validators.required]],
  });

  ngOnInit(): void {
    this.loadCategories();
  }

  protected loadCategories(): void {
    this.isLoading.set(true);
    this.errorMessage.set('');
    this.categoryService.getAll(true).pipe(
      finalize(() => this.isLoading.set(false)),
    ).subscribe({
      next: (categories) => this.categories.set(categories),
      error: (error: HttpErrorResponse) => {
        const problem = error.error as ApiProblemDetails | null;
        this.errorMessage.set(problem?.detail ?? 'Kategoriler yüklenemedi. Backend bağlantısını kontrol ediniz.');
      },
    });
  }

  protected typeLabel(type: TransactionType): string {
    return type === TransactionType.Income ? 'Gelir' : 'Gider';
  }

  protected isIncome(type: TransactionType): boolean {
    return type === TransactionType.Income;
  }

  protected setType(type: TransactionType | 'all'): void { this.selectedType.set(type); }

  protected openCreateForm(): void {
    this.editingCategory.set(null); this.formError.set('');
    this.categoryForm.reset({ name: '', type: TransactionType.Expense });
    this.isFormOpen.set(true);
  }

  protected openEditForm(category: Category): void {
    this.editingCategory.set(category); this.formError.set('');
    this.categoryForm.reset({ name: category.name, type: category.type });
    this.isFormOpen.set(true);
  }

  protected closeForm(): void { if (!this.isSaving()) this.isFormOpen.set(false); }

  protected saveCategory(): void {
    if (this.categoryForm.invalid) { this.categoryForm.markAllAsTouched(); return; }
    this.isSaving.set(true); this.formError.set('');
    const value = this.categoryForm.getRawValue();
    const request = { name: value.name.trim(), type: Number(value.type) as TransactionType };
    const editing = this.editingCategory();
    const operation = editing ? this.categoryService.update(editing.id, request) : this.categoryService.create(request);
    operation.pipe(finalize(() => this.isSaving.set(false))).subscribe({
      next: () => { this.isFormOpen.set(false); this.successMessage.set(editing ? 'Kategori güncellendi.' : 'Kategori eklendi.'); this.loadCategories(); },
      error: (error: HttpErrorResponse) => this.formError.set(this.problemMessage(error, 'Kategori kaydedilemedi.')),
    });
  }

  protected deleteCategory(category: Category): void {
    if (!confirm(`“${category.name}” kategorisini silmek istediğinize emin misiniz?`)) return;
    this.categoryService.delete(category.id).subscribe({
      next: () => { this.successMessage.set('Kategori silindi.'); this.loadCategories(); },
      error: (error: HttpErrorResponse) => this.errorMessage.set(this.problemMessage(error, 'Kategori silinemedi.')),
    });
  }

  private problemMessage(error: HttpErrorResponse, fallback: string): string {
    const problem = error.error as ApiProblemDetails | null;
    if (problem?.detail) return problem.detail;
    const errors = (error.error as { errors?: Record<string, string[]> } | null)?.errors;
    return errors ? Object.values(errors).flat()[0] ?? fallback : fallback;
  }
}
