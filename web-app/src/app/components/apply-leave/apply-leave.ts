import { Component, inject, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';

import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatSelectModule } from '@angular/material/select';
import { MatButtonModule } from '@angular/material/button';
import { MatDatepickerModule } from '@angular/material/datepicker';
import { MatNativeDateModule, provideNativeDateAdapter } from '@angular/material/core';

import { ToastrService } from 'ngx-toastr';
import { Leave } from '../../services/leave';
import { ILeave } from '../../types/leave';
import { MatDialogRef } from '@angular/material/dialog';
import { Router } from '@angular/router';

@Component({
  selector: 'app-apply-leave',
  standalone: true,
  imports: [
    CommonModule,
    ReactiveFormsModule,
    MatFormFieldModule,
    MatInputModule,
    MatSelectModule,
    MatButtonModule,
    MatDatepickerModule,
    MatNativeDateModule,
  ],
  providers: [provideNativeDateAdapter()],
  templateUrl: './apply-leave.html',
  styleUrl: './apply-leave.css',
})
export class ApplyLeave{
  private fb = inject(FormBuilder);
  private toastr = inject(ToastrService);
  private leaveService = inject(Leave);
  private routee = inject(Router);

  

  leaveTypes = ['Planned Leave', 'Emergency Leave'];

  minEndDate: Date | null = null;

  leaveForm = this.fb.group({
    leaveType: ['', Validators.required],
    startDate: [null as Date | null, Validators.required],
    endDate: [null as Date | null, Validators.required],
    reason: ['', Validators.required],
  });

  constructor() {
    // 🔥 This is the key logic
    this.leaveForm.get('startDate')?.valueChanges.subscribe((startDate) => {
      if (startDate) {
        this.minEndDate = startDate;
        this.leaveForm.get('endDate')?.setValue(null);
      }
    });
  }
  dialogRef = inject(MatDialogRef<ApplyLeave>);
  onSubmit() {
    if (this.leaveForm.invalid) {
      this.toastr.error('Please fill all fields');
      return;
    }

    const model: ILeave = this.leaveForm.value as ILeave;

    this.leaveService.APPLYLEAVE(model).subscribe({
      next: () => {
        this.toastr.success('Leave applied successfully');
        this.leaveForm.reset();
        this.minEndDate = null;
        this.dialogRef.close();
      },
      error: (er) => {
        this.toastr.error('Failed to apply leave');
        console.log(er);
      },
    });
  }

  refreshList() {
    this.routee.navigateByUrl('/employee-dashboard');
  }
}
