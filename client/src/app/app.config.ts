import {
  APP_INITIALIZER,
  ApplicationConfig,
  importProvidersFrom,
  provideZoneChangeDetection,
} from "@angular/core";
import { provideRouter } from "@angular/router";

import { routes } from "./app.routes";
import {
  provideHttpClient,
  withInterceptorsFromDi,
} from "@angular/common/http";
import { JwtModule } from "@auth0/angular-jwt";
import { AuthService } from "./services/auth.service";
import { provideAnimations } from "@angular/platform-browser/animations";

export function tokenGetter() {
  return document.cookie.split("=")[1];
}

function initUser(authService: AuthService) {
  authService.setUser(tokenGetter());
}

export const appConfig: ApplicationConfig = {
  providers: [
    provideZoneChangeDetection({ eventCoalescing: true }),
    provideRouter(routes),
    provideHttpClient(withInterceptorsFromDi()),
    provideAnimations(),
    importProvidersFrom(
      JwtModule.forRoot({
        config: {
          tokenGetter,
          allowedDomains: ["localhost:5013"],
        },
      }),
    ),
    AuthService,
    {
      provide: APP_INITIALIZER,
      useFactory: initUser,
      deps: [AuthService],
    },
  ],
};
