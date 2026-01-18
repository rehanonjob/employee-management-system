import { Component, inject, OnInit } from '@angular/core';
import { MatFormField, MatFormFieldModule, MatLabel } from '@angular/material/form-field';
import { MatCardModule, MatCardTitle } from '@angular/material/card';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { Auth } from '../../services/auth';
import { CommonModule } from '@angular/common';
import { MatInputModule } from '@angular/material/input';
import { MatSelectModule } from '@angular/material/select';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { ToastrService } from 'ngx-toastr';
import { tokenHttpInterceptor } from '../../services/token-http-interceptor';
import { MatDialogRef } from '@angular/material/dialog';
import { Router } from '@angular/router';

@Component({
  selector: 'app-profile',
  imports: [
    CommonModule,
    ReactiveFormsModule,
    MatFormFieldModule,
    MatInputModule,
    MatSelectModule,
    MatButtonModule,
    MatCardModule,
    MatIconModule,
  ],
  templateUrl: './profile.html',
  styleUrl: './profile.css',
})
export class Profile implements OnInit {
  constructor(
    private authSer: Auth,
    private dialogRef: MatDialogRef<Profile>,
    private route: Router
  ) {}
  ngOnInit(): void {
    this.authSer.GetProfileDetails().subscribe({
      next: (resp) => {
        this.profileForm.patchValue(resp);
        this.imgSrc = resp.profileImage as string;
      },
      error: (err) => {
        console.log(err);
      },
    });
  }

  fb = inject(FormBuilder);
  profileForm = this.fb.group({
    name: ['', [Validators.required, Validators.minLength(3)]],
    email: [{ value: '', disabled: true }],
    phone: ['', [Validators.required, Validators.pattern(/^\d{10}$/)]],
    profileImage: [''],
    password: ['', Validators.minLength(5)],
  });
  tot = inject(ToastrService);
  onUpdate() {
    var formData = this.profileForm.getRawValue();
    this.authSer.UpdateProfileDetails(formData).subscribe((result) => {
      this.tot.success('Profile updated successfully');
      this.dialogRef.close();
      this.route.navigateByUrl('/employee-dashboard');
    });
  }

  imgSrc!: string;
  fileUpload(e: Event) {
    var target: any = e.target;
    if (target.files && target.files[0]) {
      var file = target.files[0];
      var read = new FileReader();
      read.onload = () => {
        this.imgSrc = read.result as string;
        this.profileForm.patchValue({
          profileImage: this.imgSrc,
        });
      };
      read.readAsDataURL(file);
    }
  }
}
