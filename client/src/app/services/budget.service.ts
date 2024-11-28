import { Injectable } from "@angular/core";
import { HttpClient } from "@angular/common/http";
import { Observable } from "rxjs";
import { AddBudgetDto } from "../dtos/budget/addBudgetDto";
import { BudgetDto } from "../dtos/budget/budgetDto";

@Injectable({
  providedIn: "root",
})
export class BudgetService {
  private apiUrl = "http://localhost:5013/api/budget";
  constructor(private http: HttpClient) {}

  addBudget(budgetData: AddBudgetDto) {
    return this.http.post(this.apiUrl, budgetData);
  }

  getUserBudgets(): Observable<BudgetDto[]> {
    return this.http.get<BudgetDto[]>(this.apiUrl);
  }

  deleteBudget(id: number) {
    return this.http.delete(`${this.apiUrl}/${id}`);
  }

  editBudget(id: number, ceilingAmount: string) {
    return this.http.put(`${this.apiUrl}/${id}`, null, {
      params: { ceilingAmount },
    });
  }
}
