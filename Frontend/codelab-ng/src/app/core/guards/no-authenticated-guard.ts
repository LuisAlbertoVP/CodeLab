import { inject } from '@angular/core';
import { CanActivateFn, Router } from '@angular/router';
import { LoginService } from '../../login-component/login-service';

export const noAuthenticatedGuard: CanActivateFn = (route, state) => {
  const service = inject(LoginService);
  const router = inject(Router);

  if (!service.token) {
    router.navigate(['/']);
    return false;
  }
  
  return true;
};