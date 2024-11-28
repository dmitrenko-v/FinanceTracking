import { AddBudgetDto } from "./addBudgetDto";

export interface BudgetDto extends AddBudgetDto {
  id: number;
  currentAmount: number;
}
