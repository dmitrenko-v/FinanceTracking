import { Component, inject } from "@angular/core";
import { MAT_DIALOG_DATA, MatDialogRef } from "@angular/material/dialog";
import {
  FormBuilder,
  FormsModule,
  ReactiveFormsModule,
  Validators,
} from "@angular/forms";
import { GoalDto } from "../../dtos/goal/goalDto";
import { GoalService } from "../../services/goal.service";
import { MatButtonModule } from "@angular/material/button";
import { MatFormFieldModule } from "@angular/material/form-field";
import { MatInputModule } from "@angular/material/input";
import { NgIf } from "@angular/common";

@Component({
  selector: "app-goal-form-dialog",
  standalone: true,
  imports: [
    FormsModule,
    MatButtonModule,
    MatFormFieldModule,
    MatInputModule,
    NgIf,
    ReactiveFormsModule,
  ],
  templateUrl: "./goal-form-dialog.component.html",
})
export class GoalFormDialogComponent {
  private readonly data: GoalDto | null = inject<GoalDto | null>(
    MAT_DIALOG_DATA,
  );
  
  isEditing: boolean = false;
  
  fb = inject(FormBuilder);
  
  form = this.fb.nonNullable.group({
    title: ["", [Validators.required, Validators.maxLength(100)]],
    description: ["", [Validators.required, Validators.maxLength(100)]],
    deadline: ["", [Validators.required]],
    goalAmount: [0, [Validators.required, Validators.min(0)]],
    currentAmount: [0, [Validators.required, Validators.min(0)]],
    storedIn: ["", [Validators.required, Validators.maxLength(50)]],
  });
  
  errorsFromBackend: string[] = [];

  constructor(
    private goalService: GoalService,
    private dialogRef: MatDialogRef<GoalFormDialogComponent>,
  ) {
    if (this.data) {
      this.isEditing = true;
      this.form.patchValue({
        ...this.data,
        deadline: new Date(this.data.deadline)
          .toLocaleString("sv")
          .replace(" ", "T"),
      });
    }
  }

  get title() {
    return this.form.controls["title"];
  }

  get description() {
    return this.form.controls["description"];
  }

  get deadline() {
    return this.form.controls["deadline"];
  }

  get currentAmount() {
    return this.form.controls["currentAmount"];
  }

  get goalAmount() {
    return this.form.controls["goalAmount"];
  }

  get storedIn() {
    return this.form.controls["storedIn"];
  }

  onSubmit() {
    const dto = {
      ...this.form.getRawValue(),
      currentAmount:
        this.currentAmount.value >= this.goalAmount.value
          ? this.goalAmount.value
          : this.currentAmount.value,
      deadline: new Date(this.deadline.value),
    };
    if (this.isEditing) {
      this.goalService.editGoal(this.data!.id, dto).subscribe({
        next: () => {
          this.dialogRef.close();
        },
      });
    } else {
      this.goalService.addGoal(dto).subscribe({
        next: () => {
          this.dialogRef.close();
        },
      });
    }
  }
}
