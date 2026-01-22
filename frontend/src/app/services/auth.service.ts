import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  private apiUrl = 'https://localhost:7293/api/auth';

  constructor(private http: HttpClient) {}

  register(userData: any): Observable<any> {
    return this.http.post(`${this.apiUrl}/register`, userData);
  }

  login(credentials: any): Observable<any> {
    return this.http.post(`${this.apiUrl}/login`, credentials);
  }

  // ---- helpers for decoding JWT ----
  private getTokenPayload(): any | null {
    const token = localStorage.getItem('authToken');
    if (!token) return null;

    try {
      const payloadPart = token.split('.')[1];
      const json = atob(payloadPart);
      return JSON.parse(json);
    } catch (err) {
      console.error('Error decoding token', err);
      return null;
    }
  }

  getUserRole(): string | null {
    const payload = this.getTokenPayload();
    if (!payload) return null;

    // ASP.NET Core default
    const longRole = 'http://schemas.microsoft.com/ws/2008/06/identity/claims/role';

    return (
      payload[longRole] ||    // "http://schemas.microsoft.com/ws/2008/06/identity/claims/role"
      payload['role'] ||      // sometimes "role": "Customer"
      (Array.isArray(payload['roles']) ? payload['roles'][0] : null) || // sometimes ["Admin","SupportAgent"]
      null
    );
  }

  getUserId(): number | null {
  const token = localStorage.getItem('authToken');
  if (!token) return null;

  try {
    const payload = JSON.parse(atob(token.split('.')[1]));
    const nameId = 'http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier';
    const id = payload[nameId] || payload['sub'] || payload['userId'];
    return id ? Number(id) : null;
  } catch (e) {
    console.error('Error decoding token:', e);
    return null;
  }
}}

