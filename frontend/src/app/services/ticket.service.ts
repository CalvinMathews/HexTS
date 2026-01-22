import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class TicketService {
  private apiUrl = 'http://localhost:5285/api/tickets';
  private adminUrl = 'http://localhost:5285/api/admin';

  constructor(private http: HttpClient) { }

  getDashboardTickets(): Observable<any[]> {
    return this.http.get<any[]>(`${this.apiUrl}/dashboard`);
  }

 
 updateTicketStatus(id: number, status: string) {
  // backend route: PUT /api/tickets/{id}/status
  return this.http.put(`${this.apiUrl}/${id}/status`, { status });
}

  getTicketById(id: number): Observable<any> {
    return this.http.get<any>(`${this.apiUrl}/${id}`);
  }

  createTicket(ticketData: any): Observable<any> {
    return this.http.post(this.apiUrl, ticketData);
  }

  addComment(ticketId: number, commentData: any): Observable<any> {
    return this.http.post(`${this.apiUrl}/${ticketId}/comments`, commentData);
  }

  closeTicket(id: number): Observable<any> {
    return this.http.put(`${this.apiUrl}/${id}/close`, null);
  }
  getAllTickets(): Observable<any[]> {
  return this.http.get<any[]>(`${this.apiUrl}`);
}
getTicketsByUser(userId: number) {
  return this.http.get<any[]>(`${this.apiUrl}/by-user/${userId}`);
}

getTicketsByAgent(agentId: number) {
  return this.http.get<any[]>(`${this.apiUrl}/by-agent/${agentId}`);
}


getAgents(): Observable<any[]> {
  return this.http.get<any[]>(`http://localhost:5285/api/admin/agents`);
}

adminUpdateTicket(id: number, payload: any): Observable<any> {
  return this.http.put(`${this.apiUrl}/${id}`, payload);
}
}