import { Injectable } from "@angular/core";
import { HttpClient } from "@angular/common/http";
import { AddIncomeDto } from "../dtos/income/addIncomeDto";
import { Observable } from "rxjs";
import { AddExpenseDto } from "../dtos/expense/addExpenseDto";
import { ExpenseDto } from "../dtos/expense/expenseDto";

@Injectable({
  providedIn: "root",
})
export class ExpenseService {
  private apiUrl = "http://localhost:5013/api/expense";
  constructor(private http: HttpClient) {}

  addExpense(expenseData: AddExpenseDto) {
    return this.http.post(this.apiUrl, expenseData);
  }

  getUserExpenses(): Observable<ExpenseDto[]> {
    return this.http.get<ExpenseDto[]>(this.apiUrl);
  }

  deleteExpense(id: number) {
    return this.http.delete(`${this.apiUrl}/${id}`);
  }

  editExpense(id: number, expenseData: AddIncomeDto) {
    return this.http.put(`${this.apiUrl}/${id}`, expenseData);
  }
}
