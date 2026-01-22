import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { Router, RouterLink } from '@angular/router'; // Import RouterLink
import { AuthService } from '../../services/auth.service';

@Component({
  selector: 'app-login',
  standalone: true,
  imports: [CommonModule, FormsModule, RouterLink], // Add RouterLink
  templateUrl: './login.component.html',
  styleUrl: './login.component.css'
})
export class LoginComponent {
  form: any = {
    email: null,
    password: null
  };
  errorMessage = '';
  showPassword = false;


  constructor(private authService: AuthService, private router: Router) { }

  onSubmit(): void {
    this.errorMessage = '';
    this.authService.login(this.form).subscribe({
      next: data => {
        localStorage.setItem('authToken', data.token);
        this.router.navigate(['/dashboard']); // Navigate to dashboard on success
      },
      error: err => {
        this.errorMessage = err.error || 'Login failed. Please check credentials.';
      }
    });
  }
  togglePasswordVisibility(): void {
    this.showPassword = !this.showPassword;
  }
}