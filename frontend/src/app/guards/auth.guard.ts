import { inject } from '@angular/core';
import { CanActivateFn, Router } from '@angular/router';

export const authGuard: CanActivateFn = (route, state) => {
  const router = inject(Router);

  // Check if the authentication token exists in local storage
  const token = localStorage.getItem('authToken');

  if (token) {
    // If a token is found, the user is considered authenticated. Allow access.
    return true;
  } else {
    // If no token is found, the user is not authenticated.
    // Redirect them to the login page.
    console.error('Auth Guard: No token found, redirecting to login.');
    router.navigate(['/login']);
    return false; // Block access to the requested route
  }
};