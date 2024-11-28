import { AddExpenseDto } from "./addExpenseDto";

export interface ExpenseDto extends AddExpenseDto {
  id: number;
}
