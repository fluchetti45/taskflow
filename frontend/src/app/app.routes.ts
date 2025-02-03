import { Routes } from '@angular/router';
import { LandingComponent } from './components/landing/landing.component';
import { LoginComponent } from './components/login-form/login-form.component';
import { SignupComponent } from './components/signup-form/signup-form.component';
import { HomeComponent } from './components/home/home.component';
import { ProfileComponent } from './components/profile/profile.component';
import { RolesComponent } from './components/roles/roles.component';
import { DashboardComponent } from './components/dashboard/dashboard.component';
import { StatusesComponent } from './components/statuses/statuses.component';
import { AuthGuard } from './guards/is-logged-in.guard';
import { NotFoundComponent } from './components/not-found/not-found.component';
import { hasRoleGuard } from './guards/has-role.guard';

export const routes: Routes = [
  { path: '', component: LandingComponent },
  { path: 'login', component: LoginComponent },
  { path: 'landing', component: LandingComponent },
  { path: 'signup', component: SignupComponent },
  {
    path: 'home',
    component: HomeComponent,
    canActivate: [AuthGuard],
  },
  { path: 'profile', component: ProfileComponent, canActivate: [AuthGuard] },
  {
    path: 'roles',
    data: { role: 'Admin' },
    component: RolesComponent,
    canActivate: [hasRoleGuard],
  },
  {
    path: 'statuses',
    data: { role: 'Admin' },
    component: StatusesComponent,
    canActivate: [hasRoleGuard],
  },
  {
    path: 'dashboard',
    data: { role: 'Admin' },
    component: DashboardComponent,
    canActivate: [hasRoleGuard],
  },
  {
    path: '**',
    component: NotFoundComponent,
  },
];
