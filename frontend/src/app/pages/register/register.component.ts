import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { Router, RouterLink } from '@angular/router'; // 1. Import RouterLink
import { AuthService } from '../../services/auth.service';

@Component({
  selector: 'app-register',
  standalone: true,
  imports: [CommonModule, FormsModule, RouterLink], // 2. Add RouterLink here
  templateUrl: './register.component.html',
  styleUrl: './register.component.css'
})
export class RegisterComponent {
  form: any = {
    name: null,
    email: null,
    password: null
  };
  errorMessage = '';
  successMessage = '';
showPassword = false;
  constructor(private authService: AuthService, private router: Router) { }

  onSubmit(): void {
    this.errorMessage = '';
    this.successMessage = '';

    this.authService.register(this.form).subscribe({
      next: response => {
        console.log('Registration successful:', response);
        this.successMessage = 'Registration successful! Redirecting to login...';
        
        setTimeout(() => {
          this.router.navigate(['/login']);
        }, 2000);
      },
      error: err => {
        console.error('Registration failed:', err);
        // We check err.error because the message is often nested in the error object
        this.errorMessage = err.error || 'An unknown error occurred during registration.';
      }
    });
  }
  togglePasswordVisibility(): void {
    this.showPassword = !this.showPassword;
  }
}