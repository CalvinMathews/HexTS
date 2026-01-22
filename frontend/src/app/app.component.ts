import { Component } from '@angular/core';
import { RouterOutlet } from '@angular/router';

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [RouterOutlet],
  templateUrl: './app.component.html',
  styleUrl: './app.component.css'
})
export class AppComponent {
  title = 'csts-frontend';
}

// --- ADD THIS NEW, SIMPLE COMPONENT ---
@Component({
  standalone: true,
  template: '<h1>Home Page Works!</h1>' // The simplest possible HTML
})
export class HomeComponent {}