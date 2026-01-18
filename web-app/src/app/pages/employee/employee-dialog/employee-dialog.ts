import { Component, Inject, inject, Input, OnInit } from '@angular/core';
import { Httpp } from '../../../services/httpp';
import { FormBuilder, FormGroup, Validators, ReactiveFormsModule } from '@angular/forms';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatSelectModule } from '@angular/material/select';
import { MatDatepickerModule } from '@angular/material/datepicker';
import { MatNativeDateModule } from '@angular/material/core';
import { MatButtonModule } from '@angular/material/button';
import { CommonModule, NgFor } from '@angular/common';
import { MatCardModule } from '@angular/material/card';
import { MatRadioModule } from '@angular/material/radio';
import { MatIconModule } from '@angular/material/icon';
import { IDepartment } from '../../../types/department';
import { IEmployee } from '../../../types/employee';
import { ToastrService } from 'ngx-toastr';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';
import { MatDivider } from '@angular/material/divider';

@Component({
  selector: 'app-employee-dialog',
  imports: [
    CommonModule,
    ReactiveFormsModule,
    MatFormFieldModule,
    MatInputModule,
    MatSelectModule,
    MatDatepickerModule,
    MatNativeDateModule,
    MatButtonModule,
    MatCardModule,
    MatRadioModule,
    MatIconModule,
    NgFor,
    MatDivider
  ],
  templateUrl: './employee-dialog.html',
  styleUrl: './employee-dialog.css',
})
export class EmployeeDialog implements OnInit {
  @Input() employeeData!: number;

  fb = inject(FormBuilder);
  constructor(
    private httpservice: Httpp,
    private tote: ToastrService,
    private dialogRef: MatDialogRef<EmployeeDialog>,
    @Inject(MAT_DIALOG_DATA) public data: IEmployee
  ) {}
  depart: IDepartment[] = [];

  employeeForm = this.fb.group({
    name: ['', [Validators.required, Validators.minLength(2)]],
    email: ['', [Validators.required, Validators.email]],
    phone: ['', [Validators.required, Validators.pattern('^[0-9]{10}$')]],
    departmentId: [0, Validators.required],
    gender: [0, Validators.required],
    jobTitle: ['', [Validators.required]],
    joiningDate: [null, [Validators.required]],
    lastWorkingDate: [null],
    dateOfBirth: [null, [Validators.required]],
    basicSalary:[null]
  });

  ngOnInit(): void {
    // console.log(this.data.name);
    this.httpservice.getDepartment().subscribe({
      next: (result) => {
        this.depart = result;
      },
      error: (err) => {
        console.log(err);
      },
    });

    if (this.data) {
      this.employeeForm.patchValue(this.data as any);
      this.employeeForm.get('gender')?.disable();
      this.employeeForm.get('joiningDate')?.disable();
      this.employeeForm.get('dateOfBirth')?.disable();
    }
  }

  onSubmit() {
    if (!this.employeeForm) return;

    if (this.data) {
      var valuesasd: any = this.employeeForm.value;
      this.httpservice.UPDATEEMPLOYEE(this.data.id, valuesasd).subscribe({
        next: (resp) => {
          this.tote.success(valuesasd.name + ' Updated Successfully!');
          this.dialogRef.close();
        },
      });
    } else {
      var value: any = this.employeeForm.value;
      if (!value.lastWorkingDate) {
        value.lastWorkingDate = null; 
      }
      this.httpservice.ADDEMPLOYEE(value).subscribe({
        next: (res) => {
          this.tote.success(value.name + ' Added Successfully!');
          this.dialogRef.close();
        },
        error: (er) => {
          console.log(er);
        },
      });
    }
  }
}
