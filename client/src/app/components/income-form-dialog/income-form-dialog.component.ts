import { Component, inject } from "@angular/core";
import { FormBuilder, ReactiveFormsModule, Validators } from "@angular/forms";
import { MatFormFieldModule } from "@angular/material/form-field";
import { MatInputModule } from "@angular/material/input";
import { MatButtonModule } from "@angular/material/button";
import { NgIf } from "@angular/common";
import { IncomeService } from "../../services/income.service";
import { dateValidator } from "../../customValidators";
import { IncomeDto } from "../../dtos/income/incomeDto";
import { MAT_DIALOG_DATA, MatDialogRef } from "@angular/material/dialog";
import { handleError } from "../../../utils";

@Component({
  selector: "app-income-form-dialog",
  standalone: true,
  imports: [
    MatFormFieldModule,
    MatInputModule,
    ReactiveFormsModule,
    MatButtonModule,
    NgIf,
  ],
  templateUrl: "./income-form-dialog.component.html",
})
export class IncomeFormDialogComponent {
  private readonly data: IncomeDto | null = inject<IncomeDto | null>(
    MAT_DIALOG_DATA,
  );
  isEditing: boolean = false;
  fb = inject(FormBuilder);
  form = this.fb.nonNullable.group({
    title: ["", [Validators.required, Validators.maxLength(100)]],
    description: ["", [Validators.required, Validators.maxLength(200)]],
    date: ["", [Validators.required, dateValidator()]],
    amount: [0, [Validators.required, Validators.min(0)]],
  });
  errorsFromBackend: string[] = [];

  constructor(
    private incomeService: IncomeService,
    private dialogRef: MatDialogRef<IncomeFormDialogComponent>,
  ) {
    if (this.data) {
      this.isEditing = true;
      this.form.patchValue({
        ...this.data,
        date: new Date(this.data.date).toLocaleString("sv").replace(" ", "T"),
      });
    }
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

  onSubmit() {
    const dto = { ...this.form.getRawValue(), date: new Date(this.date.value) };
    if (this.isEditing) {
      this.incomeService.editIncome(this.data!.id, dto).subscribe({
        next: () => {
          this.dialogRef.close();
        },
        error: (error) => handleError(error, this.errorsFromBackend),
      });
    } else {
      this.incomeService.addIncome(dto).subscribe({
        next: () => {
          this.dialogRef.close();
        },
        error: (error) => handleError(error, this.errorsFromBackend),
      });
    }
  }
}
