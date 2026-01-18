import { Routes } from '@angular/router';
import { Home } from './pages/home/home';
import { Departments } from './pages/departments/departments';
import { Employee } from './pages/employee/employee';
import { Login } from './pages/login/login';
import { Employeedashboard } from './pages/employeedashboard/employeedashboard';
import { Profile } from './pages/profile/profile';
import { AttendenceList } from './components/attendence-list/attendence-list';
import { authGuard } from './Guard/auth.guard';
import { Payslip } from './pages/payslip/payslip';

export const routes: Routes = [
  { path: '', component: Home, canActivate: [authGuard] },
  { path: 'login', component: Login },

  { path: 'payslip/:id', component: Payslip, canActivate: [authGuard] },
  { path: 'departments', component: Departments, canActivate: [authGuard] },
  { path: 'employees', component: Employee, canActivate: [authGuard] },
  { path: 'employee-dashboard', component: Employeedashboard, canActivate: [authGuard] },
  { path: 'profile', component: Profile, canActivate: [authGuard] },
  { path: 'attendence-list', component: AttendenceList, canActivate: [authGuard] },
];
