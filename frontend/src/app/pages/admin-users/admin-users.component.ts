import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { Router } from '@angular/router';
import { UserService } from '../../services/user.service';
import { AuthService } from '../../services/auth.service';
import { TicketService } from '../../services/ticket.service';

@Component({
  selector: 'app-admin-users',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './admin-users.component.html',
  styleUrl: './admin-users.component.css'
})
export class AdminUsersComponent implements OnInit {
  users: any[] = [];
  filteredUsers: any[] = [];
  selectedUser: any = null;

  searchTerm = '';
  isLoading = true;
  errorMessage = '';

  selectedRole = '';
  selectedStatus = '';
  availableRoles = ['Customer', 'SupportAgent', 'Admin'];

  // tickets for the selected user
  ticketsRaised: any[] = [];
  ticketsAssigned: any[] = [];

  constructor(
    private userService: UserService,
    private authService: AuthService,
    private ticketService: TicketService,
    private router: Router
  ) {}

  ngOnInit(): void {
    if (this.authService.getUserRole() !== 'Admin') {
      this.router.navigate(['/dashboard']);
      return;
    }
    this.loadUsers();
  }

  loadUsers(): void {
    this.isLoading = true;
    this.userService.getAll().subscribe({
      next: data => {
        this.users = data;
        this.filteredUsers = data;
        this.isLoading = false;
      },
      error: err => {
        this.errorMessage = 'Failed to load users.';
        this.isLoading = false;
        console.error(err);
      }
    });
  }

  filterUsers(): void {
    const term = this.searchTerm.toLowerCase();
    this.filteredUsers = this.users.filter(u =>
      (u.name || '').toLowerCase().includes(term) ||
      (u.email || '').toLowerCase().includes(term) ||
      (u.role || '').toLowerCase().includes(term)
    );
  }

  selectUser(user: any): void {
    this.selectedUser = user;
    this.selectedRole = user.role;
    this.selectedStatus = user.isActive ? 'Active' : 'Inactive';

    // load tickets raised by this user
    this.ticketService.getTicketsByUser(user.userId).subscribe({
      next: t => (this.ticketsRaised = t),
      error: err => {
        console.error('Failed to load tickets for user', err);
        this.ticketsRaised = [];
      }
    });

    // if support agent -> also load tickets assigned to them
    if (user.role === 'SupportAgent') {
      this.ticketService.getTicketsByAgent(user.userId).subscribe({
        next: t => (this.ticketsAssigned = t),
        error: err => {
          console.error('Failed to load tickets for agent', err);
          this.ticketsAssigned = [];
        }
      });
    } else {
      this.ticketsAssigned = [];
    }
  }

  saveRole(): void {
    if (!this.selectedUser) return;

    this.userService.updateRole(this.selectedUser.userId, this.selectedRole).subscribe({
      next: () => {
        this.selectedUser.role = this.selectedRole;
      },
      error: err => {
        this.errorMessage = 'Failed to update role.';
        console.error(err);
      }
    });
  }

  saveStatus(): void {
    if (!this.selectedUser) return;

    const isActive = this.selectedStatus === 'Active';
    this.userService.updateStatus(this.selectedUser.userId, isActive).subscribe({
      next: () => {
        this.selectedUser.isActive = isActive;
      },
      error: err => {
        this.errorMessage = 'Failed to update status.';
        console.error(err);
      }
    });
  }

  deleteUser(): void {
    if (!this.selectedUser) return;
    if (!confirm('Are you sure you want to delete this user?')) return;

    this.userService.deleteUser(this.selectedUser.userId).subscribe({
      next: () => {
        this.users = this.users.filter(u => u.userId !== this.selectedUser.userId);
        this.filteredUsers = this.filteredUsers.filter(u => u.userId !== this.selectedUser.userId);
        this.selectedUser = null;
        this.ticketsRaised = [];
        this.ticketsAssigned = [];
      },
      error: err => {
        this.errorMessage = 'Failed to delete user.';
        console.error(err);
      }
    });
  }
}
