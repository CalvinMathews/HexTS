import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class UserService {
  private apiUrl = 'http://localhost:5285/api/users'; // adjust if your backend runs on different port

  constructor(private http: HttpClient) {}

  // Get all users (Admin only)
  getAll(): Observable<any[]> {
    return this.http.get<any[]>(this.apiUrl);
  }

  // Get a single user by ID
  getById(id: number): Observable<any> {
    return this.http.get<any>(`${this.apiUrl}/${id}`);
  }

  // Update active/inactive status
  updateStatus(id: number, isActive: boolean): Observable<any> {
    return this.http.put(`${this.apiUrl}/${id}/status`, { isActive });
  }

  // Update role (Customer / SupportAgent / Admin)
  updateRole(id: number, role: string): Observable<any> {
    return this.http.put(`${this.apiUrl}/${id}/role`, { role });
  }

  // Delete user
  deleteUser(id: number): Observable<any> {
    return this.http.delete(`${this.apiUrl}/${id}`);
  }
}
