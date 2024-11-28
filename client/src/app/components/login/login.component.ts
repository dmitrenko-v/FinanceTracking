import { Component, inject } from "@angular/core";
import { GoogleButtonComponent } from "../google-button/google-button.component";
import { FormBuilder, ReactiveFormsModule, Validators } from "@angular/forms";
import { AuthService } from "../../services/auth.service";
import { Router } from "@angular/router";
import { MatInputModule } from "@angular/material/input";
import { NgIf } from "@angular/common";
import { tokenGetter } from "../../app.config";
import { MatButtonModule } from "@angular/material/button";
import { handleError } from "../../../utils";

@Component({
  selector: "app-login",
  standalone: true,
  imports: [
    GoogleButtonComponent,
    ReactiveFormsModule,
    MatInputModule,
    NgIf,
    MatButtonModule,
  ],
  templateUrl: "./login.component.html",
})
export class LoginComponent {
  fb = inject(FormBuilder);
  form = this.fb.nonNullable.group({
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

  onSubmit() {
    this.form.markAllAsTouched();
    if (this.form.valid) {
      this.authService.credentialsLogin(this.form.getRawValue()).subscribe({
        next: () => {
          this.authService.setUser(tokenGetter());
          this.router.navigateByUrl("/expenses");
        },
        error: (error) => handleError(error, this.errorsFromBackend),
      });
    }
  }
}
