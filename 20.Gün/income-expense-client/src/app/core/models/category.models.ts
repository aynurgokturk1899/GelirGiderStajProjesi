export enum TransactionType {
  Income = 1,
  Expense = 2,
}

export interface Category {
  id: number;
  name: string;
  type: TransactionType;
  isActive: boolean;
}

export interface CategoryRequest {
  name: string;
  type: TransactionType;
}
