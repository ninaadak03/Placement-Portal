import { inject } from '@angular/core';
import { ActivatedRouteSnapshot, CanActivateFn, Router } from '@angular/router';

import { AuthService } from '../services/auth.service';

export const roleGuard: CanActivateFn = (route: ActivatedRouteSnapshot) => {
  const authService = inject(AuthService);
  const router = inject(Router);

  const expectedRole = route.data['role'] as string;
  const userRole = authService.getRole();

  if (authService.isLoggedIn() && userRole === expectedRole) {
    return true;
  }

  if (authService.isLoggedIn() && userRole === 'Student') {
    return router.createUrlTree(['/student']);
  }

  if (authService.isLoggedIn() && userRole === 'Admin') {
    return router.createUrlTree(['/admin']);
  }

  return router.createUrlTree(['/']);
};
