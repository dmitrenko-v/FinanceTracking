import { Injectable } from "@angular/core";
import { HttpClient } from "@angular/common/http";
import { AddIncomeDto } from "../dtos/income/addIncomeDto";
import { IncomeDto } from "../dtos/income/incomeDto";
import { Observable } from "rxjs";

@Injectable({
  providedIn: "root",
})
export class IncomeService {
  private apiUrl = "http://localhost:5013/api/income";
  constructor(private http: HttpClient) {}

  addIncome(incomeData: AddIncomeDto) {
    return this.http.post(this.apiUrl, incomeData);
  }

  getUserIncomes(): Observable<IncomeDto[]> {
    return this.http.get<IncomeDto[]>(this.apiUrl);
  }

  deleteIncome(id: number) {
    return this.http.delete(`${this.apiUrl}/${id}`);
  }

  editIncome(id: number, incomeData: AddIncomeDto) {
    return this.http.put(`${this.apiUrl}/${id}`, incomeData);
  }
}
