import { Component, EventEmitter, Output } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-ticket-form',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './ticket-form.component.html',
  styleUrl: './ticket-form.component.css'
})
export class TicketFormComponent {
  // This will emit the form data up to the parent component when submitted.
  @Output() formSubmit = new EventEmitter<any>();

  ticket: any = {
    title: null,
    description: null,
    priority: 'Medium' // Default priority
  };

  onSubmit(): void {
    this.formSubmit.emit(this.ticket);
  }
}