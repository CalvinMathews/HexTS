import { Routes } from '@angular/router';

// Import all our components and the guard
import { WelcomeComponent } from './pages/welcome/welcome.component';
import { RegisterComponent } from './pages/register/register.component';
import { LoginComponent } from './pages/login/login.component';
import { DashboardComponent } from './pages/dashboard/dashboard.component';
import { MainLayoutComponent } from './layouts/main-layout/main-layout.component';
import { authGuard } from './guards/auth.guard';
import { AdminUsersComponent } from './pages/admin-users/admin-users.component';

export const routes: Routes = [
    // --- PUBLIC ROUTES (accessible to everyone) ---
    { path: 'welcome', component: WelcomeComponent },
    { path: 'login', component: LoginComponent },
    { path: 'register', component: RegisterComponent },
    { path: 'admin/users', component: AdminUsersComponent },
    
    // --- DEFAULT ROUTE ---
    // If a user visits the base URL, redirect them to the welcome page.
    { path: '', redirectTo: 'welcome', pathMatch: 'full' },
    
    // --- AUTHENTICATED ROUTES (protected by the guard) ---
    {
        path: '', // The parent path is empty
        component: MainLayoutComponent, // Use MainLayoutComponent as the wrapper/shell
        canActivate: [authGuard],      // The guard protects all child routes
        children: [
            // Any routes inside this 'children' array will be displayed
            // inside the <router-outlet> of the MainLayoutComponent.
            { path: 'dashboard', component: DashboardComponent },
            // Example of a future route:
            // { path: 'tickets/:id', component: TicketDetailComponent },
        ]
    },
    
    // --- FALLBACK ROUTE ---
    // If a user types a URL that doesn't match any of the above, redirect them to welcome.
    { path: '**', redirectTo: 'welcome' }
];