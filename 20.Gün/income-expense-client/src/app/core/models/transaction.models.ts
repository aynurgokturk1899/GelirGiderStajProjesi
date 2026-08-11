import { TransactionType } from './category.models';

export interface Transaction {
  id: number;
  categoryId: number;
  categoryName: string;
  type: TransactionType;
  amount: number;
  transactionDate: string;
  description: string | null;
  createdDate: string;
}

export interface TransactionFilter {
  type?: TransactionType | '';
  categoryId?: number | '';
  startDate?: string;
  endDate?: string;
}

export interface TransactionRequest {
  categoryId: number;
  type: TransactionType;
  amount: number;
  transactionDate: string;
  description: string | null;
}

export interface DashboardSummary {
  totalIncome: number;
  totalExpense: number;
  balance: number;
}
