import { Routes } from '@angular/router';

import { authGuard } from '../../core/guards/auth.guard';
import { roleGuard } from '../../core/guards/role.guard';

export const STUDENT_ROUTES: Routes = [
  {
    path: '',
    canActivate: [authGuard, roleGuard],
    data: {
      role: 'Student',
    },
    loadComponent: () =>
      import('./student-header/student-header.component').then((m) => m.StudentHeaderComponent),
    children: [
      {
        path: '',
        loadComponent: () =>
          import('./dashboard/student-dashboard.component').then(
            (m) => m.StudentDashboardComponent,
          ),
      },
      {
        path: 'complete-profile',
        loadComponent: () =>
          import('./profile/complete-profile/complete-profile.component').then(
            (m) => m.CompleteProfileComponent,
          ),
      },
    ],
  },
];
