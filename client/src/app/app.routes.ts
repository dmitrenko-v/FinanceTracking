import { Routes } from "@angular/router";
import { LoginComponent } from "./components/login/login.component";
import { AuthCallbackComponent } from "./components/auth-callback/auth-callback.component";
import { ExpensesComponent } from "./components/expenses/expenses.component";
import { BudgetsComponent } from "./components/budgets/budgets.component";
import { IncomesComponent } from "./components/incomes/incomes.component";
import { GoalsComponent } from "./components/goals/goals.component";
import { RegisterComponent } from "./components/register/register.component";
import { authenticatedGuard } from "./guards/authGuard";

export const routes: Routes = [
  { path: "", redirectTo: "expenses", pathMatch: "full" },
  {
    path: "expenses",
    component: ExpensesComponent,
    canActivate: [authenticatedGuard],
  },
  {
    path: "budgets",
    component: BudgetsComponent,
    canActivate: [authenticatedGuard],
  },
  {
    path: "incomes",
    component: IncomesComponent,
    canActivate: [authenticatedGuard],
  },
  {
    path: "goals",
    component: GoalsComponent,
    canActivate: [authenticatedGuard],
  },
  { path: "signin-google", component: AuthCallbackComponent },
  { path: "login", component: LoginComponent },
  { path: "register", component: RegisterComponent },
];
