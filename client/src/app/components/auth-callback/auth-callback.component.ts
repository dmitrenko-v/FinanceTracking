import { Component } from "@angular/core";
import { HttpClient } from "@angular/common/http";
import { ActivatedRoute, Router } from "@angular/router";
import { AuthService } from "../../services/auth.service";
import { tokenGetter } from "../../app.config";
import { MatSnackBar } from "@angular/material/snack-bar";
import { MatProgressSpinnerModule } from "@angular/material/progress-spinner";

@Component({
  selector: "app-auth-callback",
  standalone: true,
  imports: [MatProgressSpinnerModule],
  templateUrl: "./auth-callback.component.html",
})
export class AuthCallbackComponent {
  authCode: string | null | undefined;
  constructor(
    private http: HttpClient,
    private route: ActivatedRoute,
    private router: Router,
    private authService: AuthService,
    private snackBar: MatSnackBar,
  ) {
    this.route.queryParams.subscribe((x) => {
      this.authCode = x["code"];
    });
    if (this.authCode) {
      this.authService.googleLogin(this.authCode).subscribe({
        next: () => {
          this.authService.setUser(tokenGetter());
          this.router.navigateByUrl("/expenses");
        },
        error: (err) => {
          const message = err.error.detail;
          this.router.navigateByUrl("/login");
          this.snackBar.open(message);
        },
      });
    }
  }
}
