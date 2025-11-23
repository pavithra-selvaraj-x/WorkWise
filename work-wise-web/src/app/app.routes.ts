import { Routes } from '@angular/router';
import { WrapperComponent } from './components/wrapper/wrapper.component';
import { CreateGoalComponent } from './pages/create-goal/create-goal.component';
import { DashboardComponent } from './pages/dashboard/dashboard.component';
import { GoalDetailsComponent } from './pages/goal-details/goal-details.component';
import { GoalManagementComponent } from './pages/goal-management/goal-management.component';
import { LoginComponent } from './pages/login/login.component';
import { TaskManagementComponent } from './pages/task-management/task-management.component';

export const routes: Routes = [
  {
    path: 'login',
    component: LoginComponent,
  },
  {
    path: 'ww',
    component: WrapperComponent,
    children: [
      { path: '', redirectTo: 'dashboard', pathMatch: 'full' },
      {
        path: 'dashboard',
        component: DashboardComponent,
      },
      {
        path: 'task',
        component: TaskManagementComponent,
      },
      {
        path: 'goal',
        component: GoalManagementComponent,
      },
      {
        path: 'goal/create',
        component: CreateGoalComponent,
      },
      {
        path: 'goal/:goalId',
        component: GoalDetailsComponent,
      },
    ],
  },
  {
    path: '',
    redirectTo: 'login',
    pathMatch: 'full',
  },
];
