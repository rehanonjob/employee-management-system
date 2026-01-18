import { Component, inject, OnInit } from '@angular/core';
import { Httpp } from '../../services/httpp';
import { IEmployee } from '../../types/employee';
import { Table } from '../../components/table/table';
import { MatCardModule } from '@angular/material/card';
import { MatButton } from '@angular/material/button';
import { MatDialog } from '@angular/material/dialog';
import { EmployeeDialog } from './employee-dialog/employee-dialog';
import { ToastrService } from 'ngx-toastr';

import { FormControl, ReactiveFormsModule } from '@angular/forms';
import { MatInputModule } from '@angular/material/input';
import { MatFormFieldModule } from '@angular/material/form-field';
import { debounce, debounceTime } from 'rxjs';
import { Auth } from '../../services/auth';
import { Router } from '@angular/router';

@Component({
  selector: 'app-employee',
  imports: [
    Table,
    MatCardModule,
    MatButton,
    MatFormFieldModule,
    MatInputModule,
    ReactiveFormsModule,
  ],
  templateUrl: './employee.html',
  styleUrl: './employee.css',
})
export class Employee implements OnInit {
  constructor(
    private httpser: Httpp,
    private tot: ToastrService,
    public authSer: Auth,
    private route: Router
  ) {}

  empList: IEmployee[] = [];
  showCols = ['id', 'name', 'email', 'phone', 'Action'];
  serachControl = new FormControl('');
  filter: any = {};
  ngOnInit(): void {
    this.latestEmpData();
    this.serachControl.valueChanges.pipe(debounceTime(350)).subscribe({
      next: (resp) => {
        this.filter.search = resp;
        this.latestEmpData();
      },
    });

    if (!this.authSer.isAdmin) {
      alert('Access Denied! You are not authorized to view this page.');
      this.route.navigateByUrl('employee-dashboard');
    }
  }

  latestEmpData() {
    this.httpser.GETEMPLOYEES(this.filter).subscribe({
      next: (r) => {
        this.empList = r;
      },
      error: (e) => {
        console.log(e);
      },
    });
  }

  edit(emp: IEmployee) {
    // console.log(emp);
    var dialogstate = this.dialog.open(EmployeeDialog, {
      width: '25%',
      height:'90%',
      panelClass: 'm-auto',
      data: emp,
    });

    dialogstate.afterClosed().subscribe({
      next: (resp) => {
        this.latestEmpData();
      },
    });
  }

  deleteEMP(emp: IEmployee) {
    console.log(emp);
    if (confirm('Are you sure delete ' + emp.name))
      this.httpser.DELETEEMPLOYEE(emp.id).subscribe({
        next: (resp) => {
          this.tot.error(emp.name + ' Successfully Deleted!');
          this.latestEmpData();
        },
        error: (er) => {
          console.log(er);
        },
      });
  }

  add() {
    this.openDialog();
  }

  readonly dialog = inject(MatDialog);

  openDialog(): void {
    var ref = this.dialog.open(EmployeeDialog, {
      width: '30%',
      panelClass: 'm-auto',
    });
    ref.afterClosed().subscribe({
      next: (resp) => {
        this.latestEmpData();
      },
    });
  }
}
