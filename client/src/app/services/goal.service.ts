import { Injectable } from "@angular/core";
import { HttpClient } from "@angular/common/http";
import { Observable } from "rxjs";
import { GoalDto } from "../dtos/goal/goalDto";
import { AddGoalDto } from "../dtos/goal/addGoalDto";

@Injectable({
  providedIn: "root",
})
export class GoalService {
  private apiUrl = "http://localhost:5013/api/goal";
  constructor(private http: HttpClient) {}

  addGoal(goalData: AddGoalDto) {
    return this.http.post(this.apiUrl, goalData);
  }

  getUserGoals(): Observable<GoalDto[]> {
    return this.http.get<GoalDto[]>(this.apiUrl);
  }

  deleteGoal(id: number) {
    return this.http.delete(`${this.apiUrl}/${id}`);
  }

  editGoal(id: number, goalData: AddGoalDto) {
    return this.http.put(`${this.apiUrl}/${id}`, goalData);
  }
}
