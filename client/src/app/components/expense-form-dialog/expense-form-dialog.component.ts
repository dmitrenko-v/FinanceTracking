import { Component, inject } from "@angular/core";
import { MAT_DIALOG_DATA, MatDialogRef } from "@angular/material/dialog";
import {
  FormBuilder,
  FormsModule,
  ReactiveFormsModule,
  Validators,
} from "@angular/forms";
import { dateValidator } from "../../customValidators";
import { IncomeService } from "../../services/income.service";
import { handleError } from "../../../utils";
import { ExpenseDto } from "../../dtos/expense/expenseDto";
import { MatButtonModule } from "@angular/material/button";
import { MatFormFieldModule } from "@angular/material/form-field";
import { MatInputModule } from "@angular/material/input";
import { NgIf } from "@angular/common";
import { MatOptionModule } from "@angular/material/core";
import { MatSelectModule } from "@angular/material/select";
import { CategoryService } from "../../services/category.service";
import { ExpenseService } from "../../services/expense.service";

@Component({
  selector: "app-expense-form-dialog",
  standalone: true,
  imports: [
    FormsModule,
    MatButtonModule,
    MatFormFieldModule,
    MatInputModule,
    NgIf,
    ReactiveFormsModule,
    MatOptionModule,
    MatSelectModule,
  ],
  templateUrl: "./expense-form-dialog.component.html",
})
export class ExpenseFormDialogComponent {
  private readonly data: ExpenseDto | null = inject<ExpenseDto | null>(
    MAT_DIALOG_DATA,
  );

  isEditing: boolean = false;

  fb = inject(FormBuilder);

  form = this.fb.nonNullable.group({
    title: ["", [Validators.required, Validators.maxLength(100)]],
    description: ["", [Validators.required, Validators.maxLength(200)]],
    categoryName: ["", [Validators.required]],
    date: ["", [Validators.required, dateValidator()]],
    amount: [0, [Validators.required, Validators.min(0)]],
  });

  categories: string[] = [];

  errorsFromBackend: string[] = [];

  constructor(
    private expenseService: ExpenseService,
    private categoryService: CategoryService,
    private dialogRef: MatDialogRef<ExpenseFormDialogComponent>,
  ) {
    if (this.data) {
      this.isEditing = true;
      this.form.patchValue({
        ...this.data,
        date: new Date(this.data.date).toLocaleString("sv").replace(" ", "T"),
      });
    }
    this.categoryService
      .getCategories()
      .subscribe((categories) => (this.categories = categories));
  }

  get title() {
    return this.form.controls["title"];
  }

  get description() {
    return this.form.controls["description"];
  }

  get date() {
    return this.form.controls["date"];
  }

  get amount() {
    return this.form.controls["amount"];
  }

  get categoryName() {
    return this.form.controls["categoryName"];
  }

  onSubmit() {
    const dto = { ...this.form.getRawValue(), date: new Date(this.date.value) };
    if (this.isEditing) {
      this.expenseService.editExpense(this.data!.id, dto).subscribe({
        next: () => {
          this.dialogRef.close();
        },
        error: (error) => handleError(error, this.errorsFromBackend),
      });
    } else {
      this.expenseService.addExpense(dto).subscribe({
        next: () => {
          this.dialogRef.close();
        },
        error: (error) => handleError(error, this.errorsFromBackend),
      });
    }
  }
}
