import { Component } from "@angular/core";
import { AuthService } from "../../services/auth.service";
import { UserData } from "../../UserData";
import { NgIf } from "@angular/common";
import { RouterLink } from "@angular/router";
import { MatToolbarModule } from "@angular/material/toolbar";

@Component({
  selector: "app-navbar",
  standalone: true,
  imports: [NgIf, RouterLink, MatToolbarModule],
  templateUrl: "./navbar.component.html",
})
export class NavbarComponent {
  user: UserData | undefined;

  constructor(private authService: AuthService) {
    this.authService.user.subscribe((user) => (this.user = user));
  }

  logOut() {
    this.authService.logOut();
  }
}
