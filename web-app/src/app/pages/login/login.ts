import { Component, OnInit } from '@angular/core';
import { Auth } from '../../services/auth';
import { MatCard, MatCardHeader, MatCardTitle, MatCardContent } from '@angular/material/card';
import {
  MatFormField,
  MatFormFieldControl,
  MatLabel,
  MatError,
} from '@angular/material/form-field';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { MatInput } from '@angular/material/input';
import { MatIcon } from '@angular/material/icon';
import { MatAnchor } from '@angular/material/button';
import { IAuthToken } from '../../types/auth';
import { ToastrService } from 'ngx-toastr';
import { Route, Router } from '@angular/router';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { NgIf } from '@angular/common';

@Component({
  selector: 'app-login',
  imports: [
    MatCard,
    ReactiveFormsModule,
    MatFormField,
    MatInput,
    MatLabel,
    MatError,
    MatIcon,
    MatCardHeader,
    MatCardTitle,
    MatCardContent,
    MatAnchor,
    MatProgressSpinnerModule,
    NgIf
  ],
  templateUrl: './login.html',
  styleUrl: './login.css',
})
export class Login implements OnInit {
  constructor(
    private authService: Auth,
    private fb: FormBuilder,
    private tot: ToastrService,
    private router: Router
  ) {}
  loginForm!: FormGroup;
  isLoading: boolean = false;

  ngOnInit(): void {
    this.loginForm = this.fb.group({
      email: ['', [Validators.required, Validators.email]],
      password: ['', Validators.required],
    });

    if (this.authService.isLoggedIn) {
      this.router.navigateByUrl('/');
    }
  }

  onLogin() {
    this.isLoading = true;
    this.authService.login(this.loginForm.value.email, this.loginForm.value.password).subscribe({
      next: (resp) => {
        console.log(resp);
        this.authService.saveToken(resp);
        this.tot.success('Login Successfully!');
        if (resp.role == 'Admin') {
          this.router.navigateByUrl('/');
        } else {
          this.router.navigateByUrl('/employee-dashboard');
        }
      },
      error: (et) => {
        console.log(et);
        this.tot.error('Login Failed');
        this.isLoading=false;
      },
    });
  }
}
