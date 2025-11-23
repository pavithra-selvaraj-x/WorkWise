import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { jwtDecode } from 'jwt-decode';
import { Observable } from 'rxjs';
import { environment } from '../../environments/environment';
import { LoginDto } from '../../types/user';

@Injectable({
  providedIn: 'root',
})
export class AuthService {
  firstName: string = '';
  lastName: string = '';
  email: string = '';

  constructor(private http: HttpClient) {
    this.decodeTokenDetails();
  }

  login(loginDto: LoginDto): Observable<any> {
    return this.http.post<any>(`${environment.apiUrl}/user/login`, loginDto);
  }

  decodeTokenDetails() {
    const token = localStorage.getItem('token');
    if (token) {
      const decodedToken = jwtDecode(token) as any;
      this.firstName = decodedToken.FirstName;
      this.lastName = decodedToken.LastName;
      this.email = decodedToken.email;
    }
  }
}
