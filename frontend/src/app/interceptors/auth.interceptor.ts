import { HttpInterceptorFn } from '@angular/common/http';

export const authInterceptor: HttpInterceptorFn = (req, next) => {
  // Get the authentication token from local storage.
  const authToken = localStorage.getItem('authToken');

  // If a token exists, clone the request to add the new Authorization header.
  if (authToken) {
    const authReq = req.clone({
      setHeaders: {
        Authorization: `Bearer ${authToken}`
      }
    });
    // Pass the cloned request with the header to the next handler.
    return next(authReq);
  }

  // If no token exists, pass the original request along without modification.
  return next(req);
};