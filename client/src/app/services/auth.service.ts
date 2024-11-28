import { Injectable } from "@angular/core";
import { HttpClient } from "@angular/common/http";
import { BehaviorSubject } from "rxjs";
import { UserData } from "../UserData";
import { Router } from "@angular/router";
import { JwtHelperService } from "@auth0/angular-jwt";
import { LoginDto } from "../dtos/auth/loginDto";
import { RegisterDto } from "../dtos/auth/registerDto";

@Injectable({
  providedIn: "root",
})
export class AuthService {
  private userSubject = new BehaviorSubject<UserData | undefined>(undefined);
  user = this.userSubject.asObservable();
  private apiUrl = "http://localhost:5013/api/auth";

  constructor(
    private http: HttpClient,
    private router: Router,
    private jwtHelper: JwtHelperService,
  ) {}

  setUser(token: string) {
    if (!token) return;
    const decodedToken = this.jwtHelper.decodeToken(token);
    this.userSubject.next({
      userId: decodedToken.userId,
      role: decodedToken.role,
      accountType: decodedToken.accountType,
    });
  }

  credentialsLogin(userData: LoginDto) {
    return this.http.post(`${this.apiUrl}/login`, userData, {
      withCredentials: true,
    });
  }

  credentialsRegister(userData: RegisterDto) {
    return this.http.post(`${this.apiUrl}/register`, userData, {
      withCredentials: true,
    });
  }

  googleLogin(authCode: string) {
    return this.http.post(`${this.apiUrl}/google`, null, {
      params: { code: authCode },
      withCredentials: true,
    });
  }

  logOut() {
    document.cookie =
      "accessToken=;expires=Thu, 01 Jan 1970 00:00:00 UTC; domain=localhost; path=/;";
    this.userSubject.next(undefined);
    this.router.navigateByUrl("/login");
  }
}
