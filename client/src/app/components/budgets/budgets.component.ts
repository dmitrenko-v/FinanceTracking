import { Component } from "@angular/core";
import { MatDialog } from "@angular/material/dialog";
import { BudgetService } from "../../services/budget.service";
import { BudgetDto } from "../../dtos/budget/budgetDto";
import { BudgetFormDialogComponent } from "../budget-form-dialog/budget-form-dialog.component";
import { DatePipe, NgIf } from "@angular/common";
import { MatButtonModule } from "@angular/material/button";
import { MatCardModule } from "@angular/material/card";
import { MatProgressBarModule } from "@angular/material/progress-bar";

@Component({
  selector: "app-budgets",
  standalone: true,
  imports: [
    MatButtonModule,
    MatCardModule,
    MatProgressBarModule,
    NgIf,
  ],
  templateUrl: "./budgets.component.html",
})
export class BudgetsComponent {
  budgets: BudgetDto[] = [];

  constructor(
    private dialog: MatDialog,
    private budgetService: BudgetService,
  ) {
    this.loadBudgets();
  }

  loadBudgets() {
    this.budgetService
      .getUserBudgets()
      .subscribe((budgets) => (this.budgets = budgets));
  }

  openAddDialog() {
    const dialogRef = this.dialog.open(BudgetFormDialogComponent);
    dialogRef.afterClosed().subscribe(() => {
      this.loadBudgets();
    });
  }

  onDeleteClick(id: number) {
    this.budgetService.deleteBudget(id).subscribe(() => this.loadBudgets());
  }

  openEditDialog(budget: BudgetDto) {
    const dialogRef = this.dialog.open(BudgetFormDialogComponent, {
      data: budget,
    });
    dialogRef.afterClosed().subscribe(() => {
      this.loadBudgets();
    });
  }
}
