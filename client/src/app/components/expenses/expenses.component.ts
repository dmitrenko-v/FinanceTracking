import { Component } from "@angular/core";
import { MatDialog } from "@angular/material/dialog";
import { ExpenseDto } from "../../dtos/expense/expenseDto";
import { ExpenseService } from "../../services/expense.service";
import { ExpenseFormDialogComponent } from "../expense-form-dialog/expense-form-dialog.component";
import { MatCardModule } from "@angular/material/card";
import { DatePipe } from "@angular/common";
import { MatButtonModule } from "@angular/material/button";

@Component({
  selector: "app-expenses",
  standalone: true,
  imports: [MatCardModule, DatePipe, MatButtonModule],
  templateUrl: "./expenses.component.html",
})
export class ExpensesComponent {
  expenses: ExpenseDto[] = [];

  constructor(
    private dialog: MatDialog,
    private expenseService: ExpenseService,
  ) {
    this.loadIncomes();
  }

  loadIncomes() {
    this.expenseService
      .getUserExpenses()
      .subscribe((expenses) => (this.expenses = expenses));
  }

  openAddDialog() {
    const dialogRef = this.dialog.open(ExpenseFormDialogComponent);
    dialogRef.afterClosed().subscribe(() => {
      this.loadIncomes();
    });
  }

  onDeleteClick(id: number) {
    this.expenseService.deleteExpense(id).subscribe(() => this.loadIncomes());
  }

  openEditDialog(expense: ExpenseDto) {
    const dialogRef = this.dialog.open(ExpenseFormDialogComponent, {
      data: expense,
    });
    dialogRef.afterClosed().subscribe(() => {
      this.loadIncomes();
    });
  }
}
