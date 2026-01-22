import { Component, Input, Output, EventEmitter } from '@angular/core';
import { CommonModule } from '@angular/common';


@Component({
  selector: 'app-ticket-list',
  standalone: true,
  imports: [CommonModule], 
  templateUrl: './ticket-list.component.html',
  styleUrl: './ticket-list.component.css'
})
export class TicketListComponent {
  @Input() tickets: any[] = [];
  
  // This will emit the ticket ID when a user clicks the "View Details" button
  @Output() ticketSelected = new EventEmitter<number>();

  selectTicket(ticketId: number): void {
    this.ticketSelected.emit(ticketId);
  }
}