import { Component, inject } from "@angular/core";
import { MAT_DIALOG_DATA, MatDialogRef } from "@angular/material/dialog";
import {
  FormBuilder,
  FormGroup,
  FormsModule,
  ReactiveFormsModule,
  Validators,
} from "@angular/forms";
import { handleError } from "../../../utils";
import { BudgetDto } from "../../dtos/budget/budgetDto";
import { BudgetService } from "../../services/budget.service";
import { MatButtonModule } from "@angular/material/button";
import { MatFormFieldModule } from "@angular/material/form-field";
import { MatInputModule } from "@angular/material/input";
import { NgIf } from "@angular/common";
import { MatSelectModule } from "@angular/material/select";
import { CategoryService } from "../../services/category.service";

@Component({
  selector: "app-budget-form-dialog",
  standalone: true,
  imports: [
    FormsModule,
    MatButtonModule,
    MatFormFieldModule,
    MatInputModule,
    NgIf,
    ReactiveFormsModule,
    MatSelectModule,
  ],
  templateUrl: "./budget-form-dialog.component.html",
})
export class BudgetFormDialogComponent {
  private readonly data: BudgetDto | null = inject<BudgetDto | null>(
    MAT_DIALOG_DATA,
  );
  isEditing: boolean = false;

  fb = inject(FormBuilder);

  form!: FormGroup;

  errorsFromBackend: string[] = [];

  categories: string[] = [];

  constructor(
    private budgetService: BudgetService,
    private categoryService: CategoryService,
    private dialogRef: MatDialogRef<BudgetFormDialogComponent>,
  ) {
    if (this.data) {
      this.isEditing = true;
      this.form = this.fb.nonNullable.group({
        ceilingAmount: [
          this.data.ceilingAmount,
          [Validators.required, Validators.min(0)],
        ],
      });
    } else {
      this.form = this.fb.nonNullable.group({
        categoryName: ["", [Validators.required, Validators.maxLength(100)]],
        ceilingAmount: [0, [Validators.required, Validators.min(0)]],
      });
      this.categoryService
        .getCategories()
        .subscribe((categories) => (this.categories = categories));
    }
  }

  get ceilingAmount() {
    return this.form.controls["ceilingAmount"];
  }

  get categoryName() {
    return this.form.controls["categoryName"];
  }

  onSubmit() {
    if (this.isEditing) {
      this.budgetService
        .editBudget(this.data!.id, this.ceilingAmount.value)
        .subscribe({
          next: () => {
            this.dialogRef.close();
          },
          error: (error) => handleError(error, this.errorsFromBackend),
        });
    } else {
      this.budgetService.addBudget(this.form.getRawValue()).subscribe({
        next: () => {
          this.dialogRef.close();
        },
        error: (error) => handleError(error, this.errorsFromBackend),
      });
    }
  }
}
