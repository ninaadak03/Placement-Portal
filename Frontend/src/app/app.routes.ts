import { Routes } from '@angular/router';

export const routes: Routes = [
  {
    path: '',
    loadChildren: () => import('./features/home/home.routes').then((m) => m.HOME_ROUTES),
  },
  // {
  //   path: 'verify-otp',
  //   loadChildren: () => import('./features/auth/otp/otp.routes').then((m) => m.OTP_ROUTES),
  // },
  // {
  //   path: 'student',
  //   loadChildren: () => import('./features/student/student.routes').then((m) => m.STUDENT_ROUTES),
  // },
  // {
  //   path: 'admin',
  //   loadChildren: () => import('./features/admin/admin.routes').then((m) => m.ADMIN_ROUTES),
  // },
  {
    path: '**',
    redirectTo: '',
  },
];
