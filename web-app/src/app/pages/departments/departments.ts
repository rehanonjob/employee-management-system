import { HttpClient } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { Httpp } from '../../services/httpp';
import { IDepartment } from '../../types/department';
import { NgFor, NgIf } from '@angular/common';
import { MatButtonModule } from '@angular/material/button';
import { MatFormFieldModule } from '@angular/material/form-field';
import { FormsModule } from '@angular/forms';
import { ToastrService } from 'ngx-toastr';
import { Auth } from '../../services/auth';

@Component({
  selector: 'app-departments',
  imports: [NgFor, MatButtonModule, MatFormFieldModule, FormsModule],
  templateUrl: './departments.html',
  styleUrl: './departments.css',
})
export class Departments implements OnInit {
  isListOpen: boolean = true;
  departlist: IDepartment[] = [];
  deptataName: string = '';
  isUpdatebtn: boolean = false;
  upId: number = 0;

  constructor(private httpser: Httpp, private tot: ToastrService, public auth: Auth) {}

  ngOnInit(): void {
    this.getLatestData();
  }

  getLatestData() {
    this.httpser.getDepartment().subscribe({
      next: (resp) => {
        this.departlist = resp;
      },
      error: (err) => {
        console.log(err);
      },
    });
  }

  addingDepartment() {
    this.httpser.postDepartment(this.deptataName).subscribe({
      next: (resp) => {
        this.tot.success('Department Added Successfully');
        this.isListOpen = true;
        this.deptataName = ''; 
        this.getLatestData();
      },
    });
  }

  editDepartment(d: IDepartment) {
    this.deptataName = d.name;
    this.upId = d.id;
    this.isListOpen = false;
    this.isUpdatebtn = true;
  }

  updateDepartment() {
    this.httpser.updateDDepartment(this.upId, this.deptataName).subscribe({
      next: (resp) => {
        this.tot.success('Department Updated Successfully');
        this.isListOpen = true;
        this.getLatestData();
      },
    });
  }

  deleteDepartment(d: IDepartment) {
    if (confirm('Are you sure you want to delete this department?')) {
      this.httpser.DELETEDepartment(d.id).subscribe({
        next: (resp) => {
          this.tot.error('Department Deleted Successfully');
          this.getLatestData();
        },
      });
    }
  }
}
