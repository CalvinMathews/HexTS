import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { RouterModule } from '@angular/router';
import { TicketService } from '../../services/ticket.service';
import { AuthService } from '../../services/auth.service';
import { TicketListComponent } from '../../components/ticket-list/ticket-list.component';

@Component({
  selector: 'app-dashboard',
  standalone: true,
  imports: [CommonModule, FormsModule, RouterModule, TicketListComponent],
  templateUrl: './dashboard.component.html',
  styleUrl: './dashboard.component.css'
})
export class DashboardComponent implements OnInit {
  // ---- Ticket categories (shared across roles) ----
  newTickets: any[] = [];
  openTickets: any[] = [];
  closedTickets: any[] = [];

  // ---- Agent-specific lists ----
  inProgressTickets: any[] = [];
  assignedTickets: any[] = [];

  // ---- Shared state ----
  userRole: string | null = null;
  currentUserId: number | null = null;
  isLoading = true;
  errorMessage = '';
  showCreateForm = false;
  selectedTicket: any = null;
  newCommentMessage = '';
  agentIsSaving = false;

  // ---- Admin-only ----
  agents: any[] = [];
  selectedStatus = '';
  selectedAgentId: number | null = null;
  availableStatuses = ['New', 'Assigned', 'InProgress', 'Resolved', 'Closed'];

  // ---- Agent-only (dropdown version) ----
  // we keep agent’s own selection separate from admin’s
  agentSelectedStatus = 'InProgress';

  constructor(
    private ticketService: TicketService,
    private authService: AuthService
  ) {}

  ngOnInit(): void {
    this.userRole = this.authService.getUserRole();
    // note: getUserId might not exist yet in your AuthService, so we null-fallback
    this.currentUserId = this.authService.getUserId?.() ?? null;
    this.loadTickets();

    if (this.isAdmin()) {
      this.loadAgents();
    }
  }

  // ---- Role helpers ----
  isCustomer(): boolean {
    return this.userRole === 'Customer';
  }

  isSupportAgent(): boolean {
    return this.userRole === 'SupportAgent';
  }

  isAdmin(): boolean {
    return this.userRole === 'Admin';
  }

  /**
   * For now: if you're an agent and the backend already gave you this ticket,
   * we can treat it as "yours". If later you add assignedToId to the DTO,
   * tighten this check.
   */
  isMyTicket(ticket: any): boolean {
    if (!ticket) return false;

    // if we actually have assignedToId, use it
    if (ticket.assignedToId && this.currentUserId) {
      return ticket.assignedToId === this.currentUserId;
    }

    // fallback: agent can see their own detail
    return this.isSupportAgent();
  }

  // ---- Load tickets ----
  loadTickets(): void {
    this.isLoading = true;
    this.errorMessage = '';

    if (this.isAdmin()) {
      // ADMIN: sees all tickets
      this.ticketService.getAllTickets().subscribe({
        next: data => {
          const sorted = data.sort((a, b) => b.ticketId - a.ticketId);

          // group for admin
          this.newTickets = sorted.filter(
            t => t.status === 'New' && (!t.assignedToId || t.assignedToId === 0)
          );
          this.openTickets = sorted.filter(
            t => t.status === 'Assigned' || t.status === 'InProgress'
          );
          this.closedTickets = sorted.filter(
            t => t.status === 'Resolved' || t.status === 'Closed'
          );

          this.isLoading = false;
        },
        error: err => {
          this.errorMessage = 'Failed to load tickets.';
          this.isLoading = false;
          console.error(err);
        }
      });
      return;
    }

    // CUSTOMER / AGENT DASHBOARD
    this.ticketService.getDashboardTickets().subscribe({
      next: data => {
        const sorted = data.sort((a, b) => b.ticketId - a.ticketId);

        if (this.isSupportAgent()) {
          this.groupAgentTickets(sorted);
        } else {
          this.groupCustomerTickets(sorted);
        }

        this.isLoading = false;
      },
      error: err => {
        this.errorMessage = 'Failed to load tickets.';
        this.isLoading = false;
        console.error(err);
      }
    });
  }
  agentSaveStatus(): void {
  if (!this.selectedTicket) return;

  this.agentIsSaving = true;
  this.errorMessage = '';

  this.ticketService
    .updateTicketStatus(this.selectedTicket.ticketId, this.agentSelectedStatus)
    .subscribe({
      next: () => {
        // refresh detail + lists
        this.viewTicketDetails(this.selectedTicket.ticketId);
        this.loadTickets();
        this.agentIsSaving = false;
      },
      error: err => {
        this.errorMessage = 'Failed to update ticket status.';
        console.error(err);
        this.agentIsSaving = false;
      }
    });
}

  private loadAgents(): void {
    this.ticketService.getAgents().subscribe({
      next: data => (this.agents = data),
      error: err => console.error('Failed to load agents', err)
    });
  }

  private groupCustomerTickets(tickets: any[]): void {
    this.newTickets = tickets.filter(t => t.status === 'New');
    this.openTickets = tickets.filter(
      t => t.status === 'Assigned' || t.status === 'InProgress'
    );
    this.closedTickets = tickets.filter(
      t => t.status === 'Resolved' || t.status === 'Closed'
    );
  }

  /**
   * Agent view:
   * backend already returns only THIS agent's tickets,
   * so we just split by status.
   */
  private groupAgentTickets(tickets: any[]): void {
    const sorted = tickets.sort((a, b) => b.ticketId - a.ticketId);

    // work currently being done
    this.inProgressTickets = sorted.filter(t => t.status === 'InProgress');

    // to-do / assigned list
    this.assignedTickets = sorted.filter(
      t => t.status === 'Assigned' || t.status === 'New'
    );
  }

  // ---- Detail view actions ----
  viewTicketDetails(ticketId: number): void {
    this.isLoading = true;
    this.errorMessage = '';
    this.ticketService.getTicketById(ticketId).subscribe({
      next: data => {
        this.selectedTicket = data;

        // admin fields
        this.selectedStatus = data.status;
        this.selectedAgentId = data.assignedToId ?? null;

        // agent dropdown should show current ticket status if it's one of the allowed ones
        if (data.status === 'InProgress' || data.status === 'Resolved') {
          this.agentSelectedStatus = data.status;
        } else {
          this.agentSelectedStatus = 'InProgress';
        }

        this.isLoading = false;
      },
      error: err => {
        this.errorMessage = 'Failed to load ticket details.';
        this.isLoading = false;
        console.error(err);
      }
    });
  }

  backToList(): void {
    this.selectedTicket = null;
  }

  // ---- Customer ----
  handleCreateTicket(formData: any): void {
    this.ticketService.createTicket(formData).subscribe({
      next: () => {
        this.showCreateForm = false;
        this.loadTickets();
      },
      error: err => {
        this.errorMessage = 'Failed to create ticket.';
        console.error(err);
      }
    });
  }

  closeTicket(): void {
    if (!this.selectedTicket) return;
    if (!confirm('Are you sure you want to close this ticket?')) return;

    this.ticketService.closeTicket(this.selectedTicket.ticketId).subscribe({
      next: () => {
        this.selectedTicket = null;
        this.loadTickets();
      },
      error: err => {
        this.errorMessage = 'Failed to close ticket.';
        console.error(err);
      }
    });
  }

  // ---- Comments ----
  handleNewComment(): void {
    if (!this.selectedTicket || !this.newCommentMessage.trim()) return;

    this.ticketService
      .addComment(this.selectedTicket.ticketId, {
        message: this.newCommentMessage
      })
      .subscribe({
        next: newComment => {
          this.selectedTicket.comments.push(newComment);
          this.newCommentMessage = '';
        },
        error: err => {
          this.errorMessage = 'Failed to post comment.';
          console.error(err);
        }
      });
  }

  // ---- Agent: dropdown change ----
  // called from (change) on the agent's select
  agentChangeStatus(): void {
    if (!this.selectedTicket) return;

    this.ticketService
      .updateTicketStatus(this.selectedTicket.ticketId, this.agentSelectedStatus)
      .subscribe({
        next: () => {
          // refresh detail + lists
          this.viewTicketDetails(this.selectedTicket.ticketId);
          this.loadTickets();
        },
        error: err => {
          this.errorMessage = 'Failed to update ticket status.';
          console.error(err);
        }
      });
  }

  // (you can keep this if somewhere else you still call it)
  setStatus(newStatus: string): void {
    if (!this.selectedTicket) return;

    this.ticketService
      .updateTicketStatus(this.selectedTicket.ticketId, newStatus)
      .subscribe({
        next: () => {
          this.viewTicketDetails(this.selectedTicket.ticketId);
          this.loadTickets();
        },
        error: err => {
          this.errorMessage = 'Failed to update ticket status.';
          console.error(err);
        }
      });
  }

  // ---- Admin ----
  adminSaveChanges(): void {
    if (!this.selectedTicket) return;

    const payload = {
      status: this.selectedStatus,
      assignedToId: this.selectedAgentId
    };

    this.ticketService
      .adminUpdateTicket(this.selectedTicket.ticketId, payload)
      .subscribe({
        next: () => {
          this.loadTickets();
        },
        error: err => {
          this.errorMessage = 'Failed to update ticket.';
          console.error(err);
        }
      });
  }
}
