import { Routes } from '@angular/router';

export const OTP_ROUTES: Routes = [
  {
    path: '',
    loadComponent: () => import('./otp.component').then((m) => m.OtpComponent),
  },
];
