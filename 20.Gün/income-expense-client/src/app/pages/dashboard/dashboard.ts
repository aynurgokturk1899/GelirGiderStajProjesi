import { CurrencyPipe, DatePipe } from '@angular/common';
import { Component, inject, OnInit, signal } from '@angular/core';
import { RouterLink } from '@angular/router';
import { DashboardSummary, Transaction } from '../../core/models/transaction.models';
import { DashboardService } from '../../core/services/dashboard.service';
import { TransactionService } from '../../core/services/transaction.service';

@Component({
  selector: 'app-dashboard',
  imports: [CurrencyPipe, DatePipe, RouterLink],
  templateUrl: './dashboard.html',
  styleUrl: './dashboard.scss',
})
export class Dashboard implements OnInit {
  private readonly dashboardService = inject(DashboardService);
  private readonly transactionService = inject(TransactionService);
  protected readonly summary = signal<DashboardSummary>({ totalIncome: 0, totalExpense: 0, balance: 0 });
  protected readonly recentTransactions = signal<Transaction[]>([]);
  protected readonly hasError = signal(false);

  ngOnInit(): void {
    this.dashboardService.getSummary().subscribe({ next: (summary) => this.summary.set(summary), error: () => this.hasError.set(true) });
    this.transactionService.getAll().subscribe({ next: (transactions) => this.recentTransactions.set(transactions.slice(0, 5)), error: () => this.hasError.set(true) });
  }
}
