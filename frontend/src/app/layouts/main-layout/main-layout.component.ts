import { Component } from '@angular/core';
import { Router, RouterOutlet } from '@angular/router'; // Import RouterOutlet

@Component({
  selector: 'app-main-layout',
  standalone: true,
  imports: [RouterOutlet], // Add RouterOutlet to imports
  templateUrl: './main-layout.component.html',
  styleUrl: './main-layout.component.css'
})
export class MainLayoutComponent {
  constructor(private router: Router) {}

  logout(): void {
    // 1. Remove the token from storage
    localStorage.removeItem('authToken');
    
    // 2. Redirect the user to the login page
    this.router.navigate(['/login']);
  }
}