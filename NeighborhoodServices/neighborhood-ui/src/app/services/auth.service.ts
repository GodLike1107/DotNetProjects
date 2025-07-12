import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { tap } from 'rxjs/operators';
import { Observable } from 'rxjs';
import { jwtDecode } from 'jwt-decode';
import { Router } from '@angular/router';

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  private apiUrl = 'https://localhost:7121/api/Auth';
  private decodedToken: any = null;

  constructor(private http: HttpClient, private router: Router) {}

  login(email: string, password: string): Observable<any> {
    return this.http.post(`${this.apiUrl}/login`, { email, password }).pipe(
      tap((res: any) => {
        localStorage.setItem('token', res.token);
        this.decodedToken = jwtDecode(res.token);
        console.log('🔓 Decoded on login:', this.decodedToken);
      })
    );
  }

  logout(): void {
    localStorage.removeItem('token');
    this.decodedToken = null;
    this.router.navigate(['/login']);
  }

  getToken(): string | null {
    return localStorage.getItem('token');
  }

  isLoggedIn(): boolean {
    const token = this.getToken();
    if (!token) return false;

    try {
      const decoded: any = jwtDecode(token);
      const now = Math.floor(Date.now() / 1000);
      return decoded['exp'] > now;
    } catch {
      return false;
    }
  }

  getUserInfo(): any {
    if (this.decodedToken) return this.decodedToken;

    const token = this.getToken();
    if (!token) return null;

    try {
      this.decodedToken = jwtDecode(token);
      console.log('🔍 Decoded in getUserInfo:', this.decodedToken);
      return this.decodedToken;
    } catch {
      return null;
    }
  }

  getRole(): string | null {
    const user = this.getUserInfo();
    const roleClaimKey = 'http://schemas.microsoft.com/ws/2008/06/identity/claims/role';
    const role = user?.[roleClaimKey] ?? null;
    console.log('👤 User Role:', role);
    return role;
  }

  getEmail(): string | null {
    const user = this.getUserInfo();
    const emailClaimKey = 'http://schemas.xmlsoap.org/ws/2005/05/identity/claims/emailaddress';
    return user?.[emailClaimKey] ?? null;
  }

  getName(): string | null {
    const user = this.getUserInfo();
    const nameClaimKey = 'http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name';
    return user?.[nameClaimKey] ?? null;
  }

  loginWithGoogle(idToken: string): Observable<any> {
    return this.http.post(`${this.apiUrl}/auth/google`, { idToken });
  }

  // ✅ NEW METHOD to return a simplified user object
  getUser(): { name: string; email: string; role: string } | null {
    const user = this.getUserInfo();
    if (!user) return null;

    return {
      name: user['http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name'],
      email: user['http://schemas.xmlsoap.org/ws/2005/05/identity/claims/emailaddress'],
      role: user['http://schemas.microsoft.com/ws/2008/06/identity/claims/role']
    };
  }
}
