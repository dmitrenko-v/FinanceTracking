import { Component, inject } from "@angular/core";
import {
  FormBuilder,
  FormsModule,
  ReactiveFormsModule,
  Validators,
} from "@angular/forms";
import { AuthService } from "../../services/auth.service";
import { Router } from "@angular/router";
import { tokenGetter } from "../../app.config";
import { GoogleButtonComponent } from "../google-button/google-button.component";
import { MatButtonModule } from "@angular/material/button";
import { MatFormFieldModule } from "@angular/material/form-field";
import { MatInputModule } from "@angular/material/input";
import { NgForOf, NgIf } from "@angular/common";

@Component({
  selector: "app-register",
  standalone: true,
  imports: [
    FormsModule,
    GoogleButtonComponent,
    MatButtonModule,
    MatFormFieldModule,
    MatInputModule,
    NgIf,
    ReactiveFormsModule,
  ],
  templateUrl: "./register.component.html",
})
export class RegisterComponent {
  fb = inject(FormBuilder);
  form = this.fb.nonNullable.group({
    firstName: ["", [Validators.required, Validators.maxLength(50)]],
    lastName: ["", [Validators.required, Validators.maxLength(50)]],
    email: ["", [Validators.required, Validators.email]],
    password: ["", Validators.required],
  });
  errorsFromBackend: string[] = [];
  constructor(
    private authService: AuthService,
    private router: Router,
  ) {}

  get email() {
    return this.form.controls["email"];
  }

  get password() {
    return this.form.controls["password"];
  }

  get firstName() {
    return this.form.controls["firstName"];
  }

  get lastName() {
    return this.form.controls["lastName"];
  }

  onSubmit() {
    this.errorsFromBackend = [];
    this.form.markAllAsTouched();
    if (this.form.valid) {
      this.authService.credentialsRegister(this.form.getRawValue()).subscribe({
        next: () => {
          this.authService.setUser(tokenGetter());
          this.router.navigateByUrl("/expenses");
        },
        error: (error) => {
          try {
            this.errorsFromBackend = JSON.parse(error.error.detail).map(
              (e: { Code: string; Description: string }) => e.Description,
            );
          } catch {
            this.errorsFromBackend.push(error.error.detail);
          }
        },
      });
    }
  }
}
