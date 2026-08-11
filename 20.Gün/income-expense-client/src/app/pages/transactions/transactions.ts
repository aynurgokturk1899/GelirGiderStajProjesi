import { CurrencyPipe, DatePipe } from '@angular/common';
import { HttpErrorResponse } from '@angular/common/http';
import { Component, computed, inject, OnInit, signal } from '@angular/core';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { ActivatedRoute } from '@angular/router';
import { finalize } from 'rxjs';
import { ApiProblemDetails } from '../../core/models/auth.models';
import { Category, TransactionType } from '../../core/models/category.models';
import { Transaction } from '../../core/models/transaction.models';
import { CategoryService } from '../../core/services/category.service';
import { TransactionService } from '../../core/services/transaction.service';

@Component({
  selector: 'app-transactions',
  imports: [CurrencyPipe, DatePipe, ReactiveFormsModule],
  templateUrl: './transactions.html',
  styleUrl: './transactions.scss',
})
export class Transactions implements OnInit {
  private readonly transactionService = inject(TransactionService);
  private readonly categoryService = inject(CategoryService);
  private readonly formBuilder = inject(FormBuilder);
  private readonly route = inject(ActivatedRoute);
  protected readonly transactions = signal<Transaction[]>([]);
  protected readonly categories = signal<Category[]>([]);
  protected readonly isLoading = signal(true);
  protected readonly errorMessage = signal('');
  protected readonly formError = signal('');
  protected readonly successMessage = signal('');
  protected readonly isFormOpen = signal(false);
  protected readonly isSaving = signal(false);
  protected readonly editingTransaction = signal<Transaction | null>(null);
  protected readonly selectedFormType = signal<TransactionType>(TransactionType.Expense);
  protected readonly currentPage = signal(1);
  protected readonly searchQuery = signal('');
  protected readonly pageSize = 10;
  protected readonly filterForm = this.formBuilder.nonNullable.group({ search: [''], type: [''], categoryId: [''], startDate: [''], endDate: [''] });
  protected readonly transactionForm = this.formBuilder.nonNullable.group({
    type: [TransactionType.Expense, [Validators.required]],
    categoryId: [0, [Validators.required, Validators.min(1)]],
    amount: [0, [Validators.required, Validators.min(0.01)]],
    transactionDate: [this.today(), [Validators.required]],
    description: ['', [Validators.maxLength(500)]],
  });
  protected readonly availableCategories = computed(() => this.categories().filter((category) =>
    category.isActive && category.type === this.selectedFormType()));
  protected readonly filteredTransactions = computed(() => {
    const search = this.searchQuery().trim().toLocaleLowerCase('tr-TR');
    if (!search) return this.transactions();
    return this.transactions().filter((transaction) =>
      transaction.categoryName.toLocaleLowerCase('tr-TR').includes(search)
      || transaction.description?.toLocaleLowerCase('tr-TR').includes(search));
  });
  protected readonly pagedTransactions = computed(() => {
    const start = (this.currentPage() - 1) * this.pageSize;
    return this.filteredTransactions().slice(start, start + this.pageSize);
  });
  protected readonly totalPages = computed(() => Math.max(1, Math.ceil(this.filteredTransactions().length / this.pageSize)));
  protected readonly totalIncome = computed(() => this.filteredTransactions().filter((item) => this.isIncome(item.type)).reduce((sum, item) => sum + item.amount, 0));
  protected readonly totalExpense = computed(() => this.filteredTransactions().filter((item) => !this.isIncome(item.type)).reduce((sum, item) => sum + item.amount, 0));

  ngOnInit(): void {
    this.loadTransactions();
    this.categoryService.getAll().subscribe({ next: (categories) => this.categories.set(categories) });
    if (this.route.snapshot.queryParamMap.get('new') === 'true') this.openCreateForm();
  }

  protected loadTransactions(): void {
    this.currentPage.set(1);
    this.isLoading.set(true);
    this.errorMessage.set('');
    const values = this.filterForm.getRawValue();
    this.transactionService.getAll({
      type: values.type ? Number(values.type) as TransactionType : '',
      categoryId: values.categoryId ? Number(values.categoryId) : '',
      startDate: values.startDate,
      endDate: values.endDate,
    }).pipe(finalize(() => this.isLoading.set(false))).subscribe({
      next: (transactions) => this.transactions.set(transactions),
      error: (error: HttpErrorResponse) => {
        const problem = error.error as ApiProblemDetails | null;
        this.errorMessage.set(problem?.detail ?? 'İşlemler yüklenemedi.');
      },
    });
  }

  protected clearFilters(): void { this.filterForm.reset(); this.searchQuery.set(''); this.loadTransactions(); }
  protected applySearch(): void { this.searchQuery.set(this.filterForm.controls.search.value); this.currentPage.set(1); }
  protected previousPage(): void { this.currentPage.update((page) => Math.max(1, page - 1)); }
  protected nextPage(): void { this.currentPage.update((page) => Math.min(this.totalPages(), page + 1)); }
  protected isIncome(type: TransactionType): boolean { return type === TransactionType.Income; }

  protected openCreateForm(): void {
    this.editingTransaction.set(null);
    this.selectedFormType.set(TransactionType.Expense);
    this.formError.set('');
    this.transactionForm.reset({ type: TransactionType.Expense, categoryId: 0, amount: 0, transactionDate: this.today(), description: '' });
    this.isFormOpen.set(true);
  }

  protected openEditForm(transaction: Transaction): void {
    this.editingTransaction.set(transaction);
    this.selectedFormType.set(transaction.type);
    this.formError.set('');
    this.transactionForm.reset({
      type: transaction.type, categoryId: transaction.categoryId, amount: transaction.amount,
      transactionDate: transaction.transactionDate.slice(0, 10), description: transaction.description ?? '',
    });
    this.isFormOpen.set(true);
  }

  protected closeForm(): void { if (!this.isSaving()) this.isFormOpen.set(false); }

  protected onTypeChange(): void {
    this.selectedFormType.set(Number(this.transactionForm.controls.type.value) as TransactionType);
    const selected = this.categories().find((category) => category.id === Number(this.transactionForm.controls.categoryId.value));
    if (!selected || selected.type !== Number(this.transactionForm.controls.type.value)) this.transactionForm.controls.categoryId.setValue(0);
  }

  protected saveTransaction(): void {
    if (this.transactionForm.invalid) { this.transactionForm.markAllAsTouched(); return; }
    this.isSaving.set(true); this.formError.set('');
    const value = this.transactionForm.getRawValue();
    const request = { ...value, type: Number(value.type) as TransactionType, categoryId: Number(value.categoryId), amount: Number(value.amount), description: value.description.trim() || null };
    const editing = this.editingTransaction();
    const operation = editing ? this.transactionService.update(editing.id, request) : this.transactionService.create(request);
    operation.pipe(finalize(() => this.isSaving.set(false))).subscribe({
      next: () => { this.isFormOpen.set(false); this.successMessage.set(editing ? 'İşlem güncellendi.' : 'Yeni işlem eklendi.'); this.loadTransactions(); },
      error: (error: HttpErrorResponse) => this.formError.set(this.problemMessage(error, 'İşlem kaydedilemedi.')),
    });
  }

  protected deleteTransaction(transaction: Transaction): void {
    if (!confirm(`“${transaction.categoryName}” işlemini silmek istediğinize emin misiniz?`)) return;
    this.transactionService.delete(transaction.id).subscribe({
      next: () => { this.successMessage.set('İşlem silindi.'); this.loadTransactions(); },
      error: (error: HttpErrorResponse) => this.errorMessage.set(this.problemMessage(error, 'İşlem silinemedi.')),
    });
  }

  private problemMessage(error: HttpErrorResponse, fallback: string): string {
    const problem = error.error as ApiProblemDetails | null;
    if (problem?.detail) return problem.detail;
    const errors = (error.error as { errors?: Record<string, string[]> } | null)?.errors;
    return errors ? Object.values(errors).flat()[0] ?? fallback : fallback;
  }

  private today(): string { return new Date().toISOString().slice(0, 10); }
}
