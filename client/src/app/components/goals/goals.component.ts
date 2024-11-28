import { Component } from "@angular/core";
import { DatePipe, NgIf } from "@angular/common";
import { MatButtonModule } from "@angular/material/button";
import { MatCardModule } from "@angular/material/card";
import { MatDialog } from "@angular/material/dialog";
import { GoalDto } from "../../dtos/goal/goalDto";
import { GoalService } from "../../services/goal.service";
import { GoalFormDialogComponent } from "../goal-form-dialog/goal-form-dialog.component";
import { MatProgressBarModule } from "@angular/material/progress-bar";

@Component({
  selector: "app-goals",
  standalone: true,
  imports: [
    DatePipe,
    MatButtonModule,
    MatCardModule,
    MatProgressBarModule,
    NgIf,
  ],
  templateUrl: "./goals.component.html",
})
export class GoalsComponent {
  goals: GoalDto[] = [];

  constructor(
    private dialog: MatDialog,
    private goalsService: GoalService,
  ) {
    this.loadGoals();
  }

  loadGoals() {
    this.goalsService.getUserGoals().subscribe((goals) => (this.goals = goals));
  }

  openAddDialog() {
    const dialogRef = this.dialog.open(GoalFormDialogComponent);
    dialogRef.afterClosed().subscribe(() => {
      this.loadGoals();
    });
  }

  onDeleteClick(id: number) {
    this.goalsService.deleteGoal(id).subscribe(() => this.loadGoals());
  }

  openEditDialog(goal: GoalDto) {
    const dialogRef = this.dialog.open(GoalFormDialogComponent, {
      data: goal,
    });
    dialogRef.afterClosed().subscribe(() => {
      this.loadGoals();
    });
  }
}
