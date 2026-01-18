import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from '../../environments/environment';
import { IAuthToken } from '../types/auth';
import { Router } from '@angular/router';
import { IEditProfile } from '../types/editprofile';

@Injectable({
  providedIn: 'root',
})
export class Auth {
  constructor(private http: HttpClient, private router: Router) {}

  login(email: string, password: string) {
    return this.http.post<IAuthToken>(environment.apiUrl + '/Auth/login', {
      email: email,
      password: password,
    });
  }

  saveToken(authtoken: IAuthToken) {
    localStorage.setItem('auth', JSON.stringify(authtoken));
    localStorage.setItem('token', authtoken.token);
  }

  get isLoggedIn() {
    return localStorage.getItem('token') ? true : false;
  }

  get isEmployee() {
    var token = JSON.parse(localStorage.getItem('auth')!);
    if (token?.role?.toLowerCase() == 'employee') return true;
    else return false;
  }

  get isAdmin() {
    var token = JSON.parse(localStorage.getItem('auth')!);
    if (token?.role?.toLowerCase() == 'admin') return true;
    else return false;
  }

  get isName() {
    var n = JSON.parse(localStorage.getItem('auth')!);
    var name = n.name;
    return name;
  }

  logout() {
    localStorage.removeItem('auth');
    localStorage.removeItem('token');
    this.router.navigateByUrl('/login');
  }

  GetProfileDetails() {
    return this.http.get<IEditProfile>(environment.apiUrl + "/Auth/profile");
  }

  UpdateProfileDetails(model: IEditProfile){
    return this.http.post(environment.apiUrl+"/Auth/profile", model);
  }
}
