import { Component } from "@angular/core";

@Component({
  selector: "app-google-button",
  standalone: true,
  imports: [],
  templateUrl: "./google-button.component.html",
  styleUrl: "./google-button.component.css",
})
export class GoogleButtonComponent {
  loginWithGoogle() {
    const clientId =
      "516613164968-k0q37s0b2q995cbhv9jrqfu2v6dmsjjr.apps.googleusercontent.com";
    const redirectUri = "http://localhost:4200/signin-google";
    const scope = "openid email profile";
    window.location.href = `https://accounts.google.com/o/oauth2/v2/auth?response_type=code&client_id=${clientId}&redirect_uri=${redirectUri}&scope=${scope}`;
  }
}
