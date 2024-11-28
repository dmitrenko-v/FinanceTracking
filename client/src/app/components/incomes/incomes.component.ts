import { Component } from "@angular/core";
import { MatButtonModule } from "@angular/material/button";
import { MatDialog } from "@angular/material/dialog";
import { IncomeFormDialogComponent } from "../income-form-dialog/income-form-dialog.component";
import { IncomeDto } from "../../dtos/income/incomeDto";
import { IncomeService } from "../../services/income.service";
import { MatCardModule } from "@angular/material/card";
import { DatePipe } from "@angular/common";
import { MatIconModule } from "@angular/material/icon";

@Component({
  selector: "app-incomes",
  standalone: true,
  imports: [MatButtonModule, MatCardModule, DatePipe, MatIconModule],
  templateUrl: "./incomes.component.html",
})
export class IncomesComponent {
  incomes: IncomeDto[] = [];

  constructor(
    private dialog: MatDialog,
    private incomeService: IncomeService,
  ) {
    this.loadIncomes();
  }

  loadIncomes() {
    this.incomeService
      .getUserIncomes()
      .subscribe((incomes) => (this.incomes = incomes));
  }

  openAddDialog() {
    const dialogRef = this.dialog.open(IncomeFormDialogComponent);
    dialogRef.afterClosed().subscribe(() => {
      this.loadIncomes();
    });
  }

  onDeleteClick(id: number) {
    this.incomeService.deleteIncome(id).subscribe(() => this.loadIncomes());
  }

  openEditDialog(income: IncomeDto) {
    const dialogRef = this.dialog.open(IncomeFormDialogComponent, {
      data: income,
    });
    dialogRef.afterClosed().subscribe(() => {
      this.loadIncomes();
    });
  }
}
