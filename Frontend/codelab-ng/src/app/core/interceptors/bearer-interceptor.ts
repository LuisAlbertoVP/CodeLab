import { HttpInterceptorFn } from '@angular/common/http';
import { inject } from '@angular/core';
import { LoginService } from '../../login-component/login-service';

export const bearerInterceptor: HttpInterceptorFn = (req, next) => {
  const service = inject(LoginService);

  if (service.token) {
      const apiReq = req.clone({
        setHeaders: {
          Authorization: `Bearer ${service.token}`
        }
      });
      return next(apiReq);
    }
    return next(req);
};
